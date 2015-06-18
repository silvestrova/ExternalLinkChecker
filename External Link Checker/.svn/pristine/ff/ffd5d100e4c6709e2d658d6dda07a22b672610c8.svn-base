// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExternalLinksCheckerManager.cs" company="">
//   
// </copyright>
// <summary>
//   The external links checker manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExternalLinksChecker.Process
{
  using System;
  using System.Collections.Generic;

  using ExternalLinksChecker.Metadata;

  using Sitecore;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  using Sitecore.Diagnostics;
  using Sitecore.Globalization;
  using Sitecore.Jobs;

  /// <summary>
  /// The external links checker manager.
  /// </summary>
  public class ExternalLinksCheckerManager
  {
    #region Public methods

    /// <summary>
    /// Check items under the root Item.
    /// </summary>
    /// <param name="rootItem">
    /// The root item.
    /// </param>
    /// <param name="languages">
    /// The languages.
    /// </param>
    /// <returns>
    /// The <see cref="Handle"/>.
    /// </returns>
    public static Handle CheckItems(Item rootItem, List<Language> languages)
    {
      Assert.ArgumentNotNull(rootItem, "_root Item");
      var options = new LinksOptions(rootItem)
                      {
                        Languages = languages
                      };
      return new ExternalLinksCheckerManager().CheckLinks(options);
    }

    /// <summary>
    /// Get handle status.
    /// </summary>
    /// <param name="handle">
    /// The handle.
    /// </param>
    /// <returns>
    /// The <see cref="CheckingStatus"/>.
    /// </returns>
    public static CheckingStatus GetStatus(Handle handle)
    {
      Assert.ArgumentNotNull(handle, "handle");
      Error.AssertObject(handle, "handle");
      Job job = JobManager.GetJob(handle);
      if (job != null)
      {
        return job.Options.CustomData as CheckingStatus;
      }

      return null;
    }

    /// <summary>
    /// The check links handler. Start checking job
    /// </summary>
    /// <param name="options">
    /// The options.
    /// </param>
    /// <returns>
    /// The <see cref="Handle"/>.
    /// </returns>
    public virtual Handle CheckLinks(LinksOptions options)
    {
      Assert.ArgumentNotNull(options, "options");
      var status = new CheckingStatus();
      var options2 = new JobOptions("CheckLinks", "ExternalLinksCheckerManager", "shell", new ExternalLinksCheckerManager(), "CheckingProcess", new object[]
                                                                                                                                                  {
                                                                                                                                                    options, status
                                                                                                                                                  })
                       {
                         ContextUser = Context.User, 
                         AfterLife = TimeSpan.FromMilliseconds(1000), 
                         Priority = Settings.ThreadPriority
                       };
      Job job = JobManager.Start(options2);
      job.Options.CustomData = status;
      return job.Handle;
    }

    #endregion

    #region Protected methods

    /// <summary>
    /// The checking process.
    /// </summary>
    /// <param name="options">
    /// The options.
    /// </param>
    /// <param name="status">
    /// The status.
    /// </param>
    protected virtual void CheckingProcess(LinksOptions options, CheckingStatus status)
    {
      // string processed = string.Empty;
      bool failed = false;
      try
      {
        status.SetState(JobState.Running);
        status.SetDetabase(options.Root.Database);
        for (int i = 0; i < options.Languages.Count; i++)
        {
          status.SetCurrentLanguage(options.Languages[i]);
          options.Root = ItemManager.GetItem(options.Root.ID, options.Languages[i], Sitecore.Data.Version.Latest, options.Root.Database);
          Job job = new Checker(options).CheckAsync();
          while (!job.WaitHandle.WaitOne(200, false))
          {
            status.SetProcessed(job.Status.Messages[0]);
          }

          status.SetProcessed(job.Status.Messages[0]);
          this.CopyMessages(job, status);
          if (job.Status.Failed)
          {
            failed = true;
            break;
          }

          /*
            processed = status.Processed;
*/
        }
      }
      catch (Exception exception)
      {
        failed = true;
        status.Messages.Add(exception.ToString());
      }

      this.Finish(failed, ref status);
    }

    /// <summary>
    /// The finish.
    /// </summary>
    /// <param name="failed">
    /// The failed.
    /// </param>
    /// <param name="status">
    /// The status.
    /// </param>
    protected void Finish(bool failed, ref CheckingStatus status)
    {
      status.SetFailed(failed);
      status.SetState(JobState.Finished);
    }

    #endregion

    #region Private methods

    /// <summary>
    /// The copy messages.
    /// </summary>
    /// <param name="job">
    /// The job.
    /// </param>
    /// <param name="status">
    /// The status.
    /// </param>
    private void CopyMessages(Job job, CheckingStatus status)
    {
      Assert.ArgumentNotNull(job, "job");
      Assert.ArgumentNotNull(status, "status");
      foreach (string str in job.Status.Messages)
      {
        status.Messages.Add(str);
      }

      var checkingStatus = job.Options.CustomData as CheckingStatus;
      if (checkingStatus != null)
      {
        foreach (ResponseResults result in checkingStatus.Results)
        {
          status.Results.Add(result);
        }
      }
    }

    #endregion
  }
}