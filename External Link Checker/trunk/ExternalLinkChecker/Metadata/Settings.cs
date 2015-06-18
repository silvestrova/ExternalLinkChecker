// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="">
//   
// </copyright>
// <summary>
//   The settings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExternalLinksChecker.Metadata
{
  using System;
  using System.Threading;

  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.IO;

  /// <summary>
  /// The settings.
  /// </summary>
  public class Settings
  {
    #region Public properties

    /// <summary>
    /// Gets the default setting database.
    /// </summary>
    public static Database DefaultSettingDatabase
    {
      get
      {
        string setting = Sitecore.Configuration.Settings.GetSetting("DefaultSettingDatabase", "master");
        return Factory.GetDatabase(setting);
      }
    }

    /// <summary>
    /// Gets the delimiter replacer.
    /// </summary>
    public static string DelimiterReplacer
    {
      get
      {
        return Sitecore.Configuration.Settings.GetSetting("DelimiterReplacer", ".CommaDelimiter.");
      }
    }

    /// <summary>
    /// Gets the http status code folder.
    /// </summary>
    public static string HttpStatusCodeFolder
    {
      get
      {
        return Sitecore.Configuration.Settings.GetSetting("HttpStatusCodeFolder", "/sitecore/system/Modules/ExternalLinksChecker/Errors");
      }
    }

    /// <summary>
    /// Gets the http web request timeout.
    /// </summary>
    public static int HttpWebRequestTimeout
    {
      get
      {
        return Sitecore.Configuration.Settings.GetIntSetting("HttpWebRequest.Timeout", 500);
      }
    }

    /// <summary>
    /// Gets the allowed protocols.
    /// </summary>
    public static string AllowedProtocols
    {
      get
      {
        return Sitecore.Configuration.Settings.GetSetting("AllowedProtocols", "http|https");
      }
    }

    /// <summary>
    /// Gets the max thread count.
    /// </summary>
    public static int MaxThreadCount
    {
      get
      {
        return Sitecore.Configuration.Settings.GetIntSetting("CheckerMaxThreadCount", 50);
      }
    }

    /// <summary>
    /// Gets a value indicating whether show all results.
    /// </summary>
    public static bool ShowAllResults
    {
      get
      {
        return Sitecore.Configuration.Settings.GetBoolSetting("ShowAllResults", true);
      }
    }

    /// <summary>
    /// Gets the source database template id.
    /// </summary>
    public static TemplateID SourceDatabaseTemplateId
    {
      get
      {
        string setting = Sitecore.Configuration.Settings.GetSetting("SourceDatabaseTemplateID", "{D61E9003-D6BC-4EEE-9727-451FA03C5C02}");
        var id = new ID(setting);
        return new TemplateID(id);
      }
    }

    /// <summary>
    /// Gets the source databases root.
    /// </summary>
    public static string SourceDatabasesRoot
    {
      get
      {
        return Sitecore.Configuration.Settings.GetSetting("SourceDatabasesRoot", "/sitecore/system/Modules/ExternalLinksChecker/SourceDatabases");
      }
    }

    /// <summary>
    /// Gets the source root item.
    /// </summary>
    public static string SourceRootItem
    {
      get
      {
        return Sitecore.Configuration.Settings.GetSetting("SourceRootItem", "/sitecore/content");
      }
    }

    /// <summary>
    /// Gets the thread priority.
    /// </summary>
    public static ThreadPriority ThreadPriority
    {
      get
      {
        string setting = Sitecore.Configuration.Settings.GetSetting("CheckerThreadPriority", "BelowNormal");
        return (ThreadPriority)Enum.Parse(typeof(ThreadPriority), setting, true);
      }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Get report file path.
    /// </summary>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public static string GetReportFilePath()
    {
      string filePath = FileUtil.MakePath(TempFolder.Folder, "ExternalLinksCheckerResults.csv", '/');
      return FileUtil.MapPath(filePath);
    }

    #endregion
  }
}