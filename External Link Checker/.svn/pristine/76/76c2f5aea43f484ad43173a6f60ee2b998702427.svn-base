// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Close.cs" company="">
//   
// </copyright>
// <summary>
//   The close command,is executed on the Report page if user press close button.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExternalLinksChecker.Commands
{
  using Sitecore.Diagnostics;
  using Sitecore.Shell.Framework.Commands;
  using Sitecore.Web.UI.Sheer;

  /// <summary>
  /// The close command,is executed on the Report page if user press close button.
  /// </summary>
  public class Close : Command
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
      SheerResponse.CloseWindow();
    }
  }
}