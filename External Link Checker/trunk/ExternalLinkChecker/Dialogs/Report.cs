// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Report.cs" company="">
//   
// </copyright>
// <summary>
//   The report.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ExternalLinksChecker.Dialogs
{
  using System;
  using System.Collections.Generic;
  using System.Web;

  using ComponentArt.Web.UI;

  using ExternalLinksChecker.Metadata;
  using ExternalLinksChecker.Utils;

  using Sitecore;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Shell.Framework.Commands;
  using Sitecore.Web.UI.Grids;
  using Sitecore.Web.UI.Sheer;

  /// <summary>
  ///   The report.
  /// </summary>
  public class Report : ClientPage, IHasCommandContext
  {
    #region Fields

    /// <summary>
    ///   The results.
    /// </summary>
    protected Grid Results;

    #endregion

    #region Public methods

    /// <summary>
    /// The on filter.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="oArgs">
    /// The o args.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// </exception>
    public void OnFilter(object sender, GridFilterCommandEventArgs oArgs)
    {
      if (oArgs == null)
      {
        throw new ArgumentNullException("oArgs");
      }

      this.Results.Filter = oArgs.FilterExpression;
    }

    #endregion

    public override void Dispatch(string command)
    {

      if (command.StartsWith("Search"))
      {
        int paramsStart = command.IndexOf("(");
        if (paramsStart > -1)
        {
          string args = command.Substring(paramsStart + 2);
          string searchCode = args.Split('\'')[0];
          this.Search(searchCode);
        }
      }
      else
      {
        base.Dispatch(command);
      }
    }

    #region Protected methods

    /// <summary>
    /// The on load.
    /// </summary>
    /// <param name="e">
    /// The e.
    /// </param>
    protected override void OnLoad(EventArgs e)
    {
      Assert.ArgumentNotNull(e, "e");
     // var test = Sitecore.Shell.Framework.Commands.CommandManager.GetCommand("Search");
     // if (string.IsNullOrEmpty(test.Name))
      //{
        base.OnLoad(e);
        if (!this.IsPostBack)
        {
          IEnumerable<ReportData> managedResults = this.GetManagedResults();
          if (this.Results != null)
          {
            ComponentArtGridHandler<ReportData>.Manage(this.Results, new GridSource<ReportData>(managedResults), true);
          }
       // }
      }
    }

    #endregion

    #region Private methods

    /// <summary>
    ///   The get command context.
    /// </summary>
    /// <returns>
    ///   The <see cref="CommandContext" />.
    /// </returns>
    CommandContext IHasCommandContext.GetCommandContext()
    {
      Database db = Database.GetDatabase("core");
      if (db != null)
      {
        Item itemNotNull = Client.GetItemNotNull("/sitecore/content/Applications/ExternalLinksChecker/Report/Ribbon", Client.CoreDatabase);

        var context = new CommandContext(itemNotNull.GetChildren().ToArray())
                        {
                          RibbonSourceUri = itemNotNull.Uri, 
                          IsContextMenu = true
                        };
        return context;
      }

      return new CommandContext();
    }

    /// <summary>
    ///   The get managed results.
    /// </summary>
    /// <returns>
    ///   The <see cref="IEnumerable" />.
    /// </returns>
    private IEnumerable<ReportData> GetManagedResults()
    {
      Database db = Factory.GetDatabase(HttpContext.Current.Request.QueryString["db"]);
      var results = new List<ReportData>();
      List<string[]> tableRows = ReportUtil.GetTableReader();
      foreach (var arr in tableRows)
      {
        results.Add(new ReportData(arr, db));
      }

      return results;
    }

    public void Search(string searchCode)
    {
      SheerResponse.Eval("search('" + searchCode + "','"+searchCode+"')");
    }
    #endregion
  }
}