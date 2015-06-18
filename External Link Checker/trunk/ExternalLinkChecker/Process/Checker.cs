// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Checker.cs" company="">
//   
// </copyright>
// <summary>
//   The checker.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExternalLinksChecker.Process
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;

  using ExternalLinksChecker.Managers;
  using ExternalLinksChecker.Metadata;
  using ExternalLinksChecker.Utils;

  using Sitecore;
  using Sitecore.Collections;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Jobs;

  /// <summary>
  /// The checker.
  /// </summary>
  public class Checker
  {
    #region Fields

    /// <summary>
    /// The links options.
    /// </summary>
    private readonly LinksOptions linksOptions;

    /// <summary>
    /// The locks.
    /// </summary>
    private readonly List<string> locks;

    /// <summary>
    /// The is finished.
    /// </summary>
    private bool isFinished;

    /// <summary>
    /// The results.
    /// </summary>
    private Results results;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Checker"/> class.
    /// </summary>
    /// <param name="linksOptions">
    /// The links options.
    /// </param>
    public Checker(LinksOptions linksOptions)
    {
      this.locks = new List<string>();
      Assert.ArgumentNotNull(linksOptions, "_linksOptions");
      this.linksOptions = linksOptions;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// The get job name.
    /// </summary>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public virtual string GetJobName()
    {
      return "Check from " + this.linksOptions.Root.Database;
    }

    #endregion

    #region Protected methods

    /// <summary>
    /// The get lock.
    /// </summary>
    /// <returns>
    /// The <see cref="object"/>.
    /// </returns>
    protected virtual object GetLock()
    {
      string jobName = this.GetJobName();
      lock (this.locks)
      {
        foreach (string str2 in this.locks)
        {
          if (str2 == jobName)
          {
            return str2;
          }
        }

        this.locks.Add(jobName);
        return jobName;
      }
    }

    #endregion

    #region Check Job

    #region Public methods

    /// <summary>
    /// The check.
    /// </summary>
    public virtual void Check()
    {
      object obj3;
      Monitor.Enter(obj3 = this.GetLock());
      try
      {
        var releaseFields = new List<Field>();
        var controlList = new ItemList
                            {
                              this.linksOptions.Root
                            };
        controlList.AddRange(this.linksOptions.Root.Axes.GetDescendants());
        this.isFinished = true;
        Context.Job.Status.Messages[0] = "Start looking over items..";

        foreach (Item descendant in controlList)
        {
          FieldCollection fields = descendant.Fields;
          fields.ReadAll();

          foreach (Field field in fields)
          {
            if (LinkTypesManager.Types.Keys.Contains(field.TypeKey) && !string.IsNullOrEmpty(field.GetValue(true)))
            {
              releaseFields.Add(field);
            }
          }
        }

        Context.Job.Status.Messages[0] = "Finished looking over items.";
        if (releaseFields.Count > 0)
        {
          this.isFinished = false;
        }

        this.results = new Results(releaseFields.Count);
        this.results.Finished += this.Finish;

        Context.Job.Status.Messages[0] = "Requesting process..";

        this.WorkInThread(releaseFields);

        Context.Job.Status.Messages[0] = "Waiting for all results..";
        do
        {
          Thread.Sleep(40);
          if (this.isFinished)
          {
            break;
          }
        }
        while (true);

        Context.Job.Status.Messages[0] = "Finishing process..";
        
        var checkingStatus = Context.Job.Options.CustomData as CheckingStatus;
        if (checkingStatus != null)
        {
          checkingStatus.Results = this.results.GetResults();
        }
      }
      catch (Exception exception)
      {
        Context.Job.Status.Failed = true;
        Context.Job.Status.Messages.Add(exception.Message);
        throw;
      }
      finally
      {
        Monitor.Exit(obj3);
      }
    }

    /// <summary>
    /// Check items async.
    /// </summary>
    /// <returns>
    /// The <see cref="Job"/>.
    /// </returns>
    public virtual Job CheckAsync()
    {
      Log.Info("ExternalLinksChecker starts CheckAsync", this);
      var options = new JobOptions(this.GetJobName(), "ExternalLinksChecker", "shell", this, "Check")
                      {
                        ContextUser = Context.User, 
                        AfterLife = TimeSpan.FromMinutes(1.0), 
                        AtomicExecution = true, 
                        CustomData = new CheckingStatus()
                      };
      Job job = Context.Job;
      if (job != null)
      {
        options.ClientLanguage = job.Options.ClientLanguage;
      }

      return JobManager.Start(options);
    }

    /// <summary>
    /// Add work in thread.
    /// </summary>
    /// <param name="fields">
    /// The fields.
    /// </param>
    public void WorkInThread(List<Field> fields)
    {
      int maxTread = Settings.MaxThreadCount;

      using (var pool = new TasksPool(maxTread))
      {
        for (int i = 0; i < fields.Count; ++i)
        {
          Field f = fields[i];
          Context.Job.Status.Processed++;
          pool.QueueTask(() => this.results.AddRange(LinkTypesManager.CheckField(f)));
        }
      }
    }

    #endregion

    #region Protected methods

    /// <summary>
    /// on finish.
    /// </summary>
    protected void Finish()
    {
      this.isFinished = true;
    }

    #endregion

    #endregion
  }

  /// <summary>
  /// The results.
  /// </summary>
  public class Results : IEnumerable<ResponseResults>
  {
    #region Delegates

    /// <summary>
    /// The finished.
    /// </summary>
    public event Continue Finished;

    #endregion

    #region Fields

    /// <summary>
    /// items to be processed.
    /// </summary>
    private readonly List<ResponseResults> mItems;

    /// <summary>
    /// The free index became 0, when all items are processed.
    /// </summary>
    private int freeIndex;

    #endregion

    #region Delegates

    /// <summary>
    /// The continue.
    /// </summary>
    public delegate void Continue();

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Results"/> class.
    /// </summary>
    /// <param name="count">
    /// The count.
    /// </param>
    public Results(int count)
    {
      this.mItems = new List<ResponseResults>();
      this.freeIndex = count;
    }

    #endregion

    #region Public properties

    /// <summary>
    /// Gets or sets the free index.
    /// </summary>
    public int FreeIndex
    {
      get
      {
        return this.freeIndex;
      }

      set
      {
        this.freeIndex = value;
        Console.WriteLine(this.freeIndex);
      }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// The add.
    /// </summary>
    /// <param name="item">
    /// The item.
    /// </param>
    public void Add(ResponseResults item)
    {
      this.mItems.Add(item);
    }

    /// <summary>
    /// The add range.
    /// </summary>
    /// <param name="item">
    /// The item.
    /// </param>
    public void AddRange(List<ResponseResults> item)
    {
      this.mItems.AddRange(item);
      this.freeIndex--;

      if (this.freeIndex == 0)
      {
        this.OnFinished();
      }
    }

    /// <summary>
    /// The get results.
    /// </summary>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    public List<ResponseResults> GetResults()
    {
      return this.mItems;
    }

    #endregion

    #region IEnumerable<T> Members

    /// <summary>
    /// The get enumerator.
    /// </summary>
    /// <returns>
    /// The <see cref="IEnumerator"/>.
    /// </returns>
    public IEnumerator<ResponseResults> GetEnumerator()
    {
      foreach (ResponseResults t in this.mItems)
      {
        if (t == null)
        {
          break;
        }

        yield return t;
      }
    }

    #endregion

    #region IEnumerable Members

    /// <summary>
    /// The get enumerator.
    /// </summary>
    /// <returns>
    /// The <see cref="IEnumerator"/>.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    #endregion

    #region Protected methods

    /// <summary>
    /// The on finished.
    /// </summary>
    protected virtual void OnFinished()
    {
      if (this.Finished != null)
      {
        this.Finished();
      }
    }

    #endregion
  }
}