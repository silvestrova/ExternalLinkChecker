// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExternalLinkscheckerCommand.cs" company="">
//   
// </copyright>
// <summary>
//   The start external links checker.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExternalLinksChecker.Commands
{
  using Sitecore;
  using Sitecore.Diagnostics;
  using Sitecore.Shell.Framework.Commands;
  using Sitecore.Text;
  using Sitecore.Web.UI.Sheer;

  /// <summary>
  /// The start external links checker.
  /// </summary>
  public class StartExternalLinksChecker : Command
  {
    #region Public methods

    /// <summary>
    /// The execute.
    /// </summary>
    /// <param name="context">
    /// The context.
    /// </param>
    public override void Execute(CommandContext context)
    {
      Assert.ArgumentNotNull(context, "context");
      var args = new ClientPipelineArgs();
      Context.ClientPage.Start(this, "Run", args);
    }

    #endregion

    #region Protected methods

    /// <summary>
    /// The run.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    protected virtual void Run(ClientPipelineArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      if (!args.IsPostBack)
      {
        Context.SetActiveSite("shell");
        SheerResponse.ShowModalDialog(new UrlString(UIUtil.GetUri("control:ExternalLinksChecker.Check")).ToString(), "540px", "590px", string.Empty, true);
        args.WaitForPostBack();
      }
    }

    #endregion
  }
}