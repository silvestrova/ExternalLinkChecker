// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckingStatus.cs" company="">
//   
// </copyright>
// <summary>
//   The checking status.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExternalLinksChecker.Metadata
{
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.Runtime.CompilerServices;
  using System.Threading;

  using Sitecore.Data;
  using Sitecore.Globalization;
  using Sitecore.Jobs;

  /// <summary>
  /// The checking status.
  /// </summary>
  public class CheckingStatus
  {
    #region Fields

    /// <summary>
    /// The messages.
    /// </summary>
    private readonly StringCollection messages = new StringCollection();

    /// <summary>
    /// The wait handle.
    /// </summary>
    private readonly ManualResetEvent waitHandle = new ManualResetEvent(false);

    /// <summary>
    /// The current database.
    /// </summary>
    private Database currentDatabase;

    /// <summary>
    /// The current language.
    /// </summary>
    private Language currentLanguage;

    /// <summary>
    /// The expired.
    /// </summary>
    [CompilerGenerated]
    private bool expired;

    /// <summary>
    /// The failed.
    /// </summary>
    private bool failed;

    /// <summary>
    /// The processed info.
    /// </summary>
    private string processedInfo;

    /// <summary>
    /// The results.
    /// </summary>
    private List<ResponseResults> results = new List<ResponseResults>();

    /// <summary>
    /// The revision.
    /// </summary>
    [CompilerGenerated]
    private long revision;

    /// <summary>
    /// The state.
    /// </summary>
    private JobState state;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckingStatus"/> class.
    /// </summary>
    public CheckingStatus()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckingStatus"/> class.
    /// </summary>
    /// <param name="db">
    /// The db.
    /// </param>
    public CheckingStatus(Database db)
    {
      this.results = new List<ResponseResults>();
      this.currentDatabase = db;
    }

    #endregion

    #region Methods

    /// <summary>
    /// The set current language.
    /// </summary>
    /// <param name="language">
    /// The language.
    /// </param>
    public void SetCurrentLanguage(Language language)
    {
      this.currentLanguage = language;
      this.Revision += 1L;
    }

    /// <summary>
    /// The set detabase.
    /// </summary>
    /// <param name="db">
    /// The db.
    /// </param>
    public void SetDetabase(Database db)
    {
      this.currentDatabase = db;
      this.Revision += 1L;
    }

    /// <summary>
    /// The set expired.
    /// </summary>
    /// <param name="expired">
    /// The expired.
    /// </param>
    public void SetExpired(bool expired)
    {
      this.Expired = expired;
      this.Revision += 1L;
    }

    /// <summary>
    /// The set failed.
    /// </summary>
    /// <param name="failed">
    /// The failed.
    /// </param>
    public void SetFailed(bool failed)
    {
      this.failed = failed;
      this.Revision += 1L;
    }

    /// <summary>
    /// The set processed.
    /// </summary>
    /// <param name="value">
    /// The value.
    /// </param>
    public void SetProcessed(string value)
    {
      this.processedInfo = value;
      this.Revision += 1L;
    }

    /// <summary>
    /// The set state.
    /// </summary>
    /// <param name="state">
    /// The state.
    /// </param>
    public void SetState(JobState state)
    {
      this.state = state;
      if (this.state == JobState.Finished)
      {
        this.waitHandle.Set();
      }

      this.Revision += 1L;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the current database.
    /// </summary>
    public Database CurrentDatabase
    {
      get
      {
        return this.currentDatabase;
      }
    }

    /// <summary>
    /// Gets the current language.
    /// </summary>
    public Language CurrentLanguage
    {
      get
      {
        return this.currentLanguage;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether expired.
    /// </summary>
    public bool Expired
    {
      [CompilerGenerated]
      get
      {
        return this.expired;
      }

      [CompilerGenerated]
      protected set
      {
        this.expired = value;
      }
    }

    /// <summary>
    /// Gets a value indicating whether failed.
    /// </summary>
    public bool Failed
    {
      get
      {
        return this.failed;
      }
    }

    /// <summary>
    /// Gets a value indicating whether is done.
    /// </summary>
    public bool IsDone
    {
      get
      {
        return this.State == JobState.Finished;
      }
    }

    /// <summary>
    /// Gets the messages.
    /// </summary>
    public StringCollection Messages
    {
      get
      {
        return this.messages;
      }
    }

    /// <summary>
    /// Gets the processed.
    /// </summary>
    public string Processed
    {
      get
      {
        return this.processedInfo;
      }
    }

    /// <summary>
    /// Gets or sets the results.
    /// </summary>
    public List<ResponseResults> Results
    {
      get
      {
        return this.results;
      }

      set
      {
        this.results = value;
      }
    }

    /// <summary>
    /// Gets or sets the revision.
    /// </summary>
    public long Revision
    {
      [CompilerGenerated]
      get
      {
        return this.revision;
      }

      [CompilerGenerated]
      protected set
      {
        this.revision = value;
      }
    }

    /// <summary>
    /// Gets the state.
    /// </summary>
    public JobState State
    {
      get
      {
        return this.state;
      }
    }

    /// <summary>
    /// Gets the wait handle.
    /// </summary>
    public ManualResetEvent WaitHandle
    {
      get
      {
        return this.waitHandle;
      }
    }

    #endregion
  }
}