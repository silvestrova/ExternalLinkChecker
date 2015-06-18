// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DownloadReport.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the DownloadReport type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExternalLinksChecker.Commands
{
  using ExternalLinksChecker.Metadata;
  using Sitecore.Diagnostics;
  using Sitecore.Shell.Framework;
  using Sitecore.Shell.Framework.Commands;

  /// <summary>
  /// The download command,is executed on the Report page if user press close button.
  /// </summary>
  public class DownloadReport : Command
  {
    /// <summary>
    /// The execute.
    /// </summary>
    /// <param name="context">
    /// The context.
    /// </param>
    public override void Execute(CommandContext context)
    {
      Assert.ArgumentNotNull(context, "context");

      string filePath = Settings.GetReportFilePath();
      if (!string.IsNullOrEmpty(filePath))
      {
        Files.Download(filePath);
      }
    }
  }
}