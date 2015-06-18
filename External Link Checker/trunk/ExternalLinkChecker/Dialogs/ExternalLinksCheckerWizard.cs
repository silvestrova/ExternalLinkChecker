// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExternalLinksCheckerWizard.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ExternalLinksCheckerWizard type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ExternalLinksChecker.Dialogs
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics.CodeAnalysis;
  using System.IO;
  using System.Text;
  using System.Web.UI;
  using ExternalLinksChecker.Metadata;
  using ExternalLinksChecker.Process;
  using ExternalLinksChecker.Utils;

  using Sitecore;
  using Sitecore.Collections;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  using Sitecore.Diagnostics;
  using Sitecore.Extensions;
  using Sitecore.Globalization;
  using Sitecore.Jobs;
  using Sitecore.Text;
  using Sitecore.Web.UI.HtmlControls;
  using Sitecore.Web.UI.Pages;
  using Sitecore.Web.UI.Sheer;
  using Sitecore.Web.UI.WebControls;

  /// <summary>
  /// The external links checker wizard.
  /// </summary>
  public class ExternalLinksCheckerWizard : WizardForm
  {
    #region Fields

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    protected Radiogroup DatabasesPanel;

    /// <summary>
    /// The error text.
    /// </summary>
    protected Memo ErrorText;

    /// <summary>
    /// The language panel.
    /// </summary>
    protected Groupbox LanguagePanel;

    /// <summary>
    /// The select root item panel.
    /// </summary>
    protected Border SelectRootItemPanel;

    /// <summary>
    /// The source root data context.
    /// </summary>
    protected DataContext SourceRootDataContext;

    /// <summary>
    /// The status.
    /// </summary>
    protected Literal Status;

    /// <summary>
    /// The tree view.
    /// </summary>
    protected TreeviewEx TreeView;

    /// <summary>
    /// The module context database.
    /// </summary>
    private Database moduleContextDatabase;

    /// <summary>
    /// The source database.
    /// </summary>
    private Database sourceDatabase;

    #endregion

    #region LoadDialog

    /// <summary>
    /// Build languages method. Get all languages from the site
    /// </summary>
    protected virtual void BuildLanguages()
    {
      this.LanguagePanel.Controls.Clear();
      Checkbox child = null;
      LanguageCollection languages = LanguageManager.GetLanguages(Context.ContentDatabase);

      foreach (Language language in languages)
      {
        if (Sitecore.Configuration.Settings.CheckSecurityOnLanguages)
        {
          ID languageItemId = LanguageManager.GetLanguageItemId(language, Context.ContentDatabase);
          if (ItemUtil.IsNull(languageItemId))
          {
            continue;
          }

          Item item = Context.ContentDatabase.GetItem(languageItemId);
          if ((item == null) || !item.Access.CanRead())
          {
            continue;
          }
        }

        string uniqueId = "la_" + language.Name;
        child = new Checkbox
                  {
                    ID = uniqueId, 
                    Header = language.CultureInfo.DisplayName
                  };
        this.LanguagePanel.Controls.Add(child);
        this.LanguagePanel.Controls.Add(new LiteralControl("<br>"));
      }

      if ((languages.Count == 1) && (child != null))
      {
        this.LanguagePanel.Disabled = true;
        child.Attributes["checked"] = "checked";
        child.Attributes["disabled"] = "disabled";
      }
    }

    /// <summary>
    /// Build source root tree.
    /// </summary>
    protected virtual void BuildSourceRoot()
    {
      this.SourceRootDataContext.DataViewName = "Master";
      this.TreeView.ShowRoot = true;
    }

    /// <summary>
    /// Get source using selected database.
    /// </summary>
    protected virtual void BuildSources()
    {
      this.DatabasesPanel.Controls.Clear();
      Item sourceDatabasesRoot = this.moduleContextDatabase.GetItem(Metadata.Settings.SourceDatabasesRoot);
      ChildList children = sourceDatabasesRoot.Children;
      string dbName = this.moduleContextDatabase.Name;

      if (children.Count <= 0)
      {
        sourceDatabasesRoot.Add(dbName, Metadata.Settings.SourceDatabaseTemplateId);
      }

      Radiobutton child = null;
      foreach (Item item2 in children)
      {
        string str3 = ShortID.Encode(item2.ID);
        child = new Radiobutton
                  {
                    Visible = true, 
                    ID = str3, 
                    Disabled = (children.Count == 1) || !item2.Access.CanWrite(), 
                    Header = string.Format("{0} ({1}:{2})", item2.DisplayName, item2["DatabaseName"], item2.Fields["Root"]), 
                    Value = str3, 
                    Name = "SourceDatabase"
                  };
        this.DatabasesPanel.Controls.Add(child);
        this.DatabasesPanel.Controls.Add(new LiteralControl("<br>"));
      }

      if (children.Count == 1)
      {
        this.DatabasesPanel.Disabled = true;
        if (child != null)
        {
          child.Checked = true;
        }
      }
    }

    /// <summary>
    /// The on load.
    /// </summary>
    /// <param name="e">
    /// The e.
    /// </param>
    protected override void OnLoad(EventArgs e)
    {
      Assert.ArgumentNotNull(e, "e");
      base.OnLoad(e);
      if (!Context.ClientPage.IsEvent)
      {
        this.moduleContextDatabase = Metadata.Settings.DefaultSettingDatabase;

        this.BuildSources();
        this.BuildLanguages();
        this.BuildSourceRoot();
      }
    }

    #endregion

    #region Protected properties

    /// <summary>
    /// Gets or sets the job handle.
    /// </summary>
    protected string JobHandle
    {
      get
      {
        return StringUtil.GetString(this.ServerProperties["JobHandle"]);
      }

      set
      {
        Assert.ArgumentNotNullOrEmpty(value, "value");
        this.ServerProperties["JobHandle"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the source root item.
    /// </summary>
    protected Item SourceRootItem { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// The check process status. Shows the latest page when the checking process is finished
    /// </summary>
    /// <exception cref="Exception">
    /// </exception>
    public void CheckStatus()
    {
      Handle handle = Handle.Parse(this.JobHandle);
      if (!handle.IsLocal)
      {
        SheerResponse.Timer("CheckStatus", 500);
      }
      else
      {
        CheckingStatus status = ExternalLinksCheckerManager.GetStatus(handle);
        if (status == null)
        {
          throw new Exception("The checking process was unexpectedly interrupted.");
        }

        if (status.Failed)
        {
          this.Active = "Retry";
          this.NextButton.Disabled = true;
          this.BackButton.Disabled = false;
          this.CancelButton.Disabled = false;
          this.ErrorText.Value = StringUtil.StringCollectionToString(status.Messages);
        }
        else
        {
          string process;
          if (status.State == JobState.Running)
          {
            object[] objArray =
              {
                Translate.Text("Database:"), status.CurrentDatabase.NullOr(db => db.Name), "</br>", Translate.Text("Language:"), " ", status.CurrentLanguage.NullOr(lang => lang.CultureInfo.DisplayName), "<br/><br/>", Translate.Text("Processed:"), " ", status.Processed
              };
            process = string.Concat(objArray);
          }
          else if (status.State == JobState.Initializing)
          {
            process = Translate.Text("Initializing.");
          }
          else
          {
            process = Translate.Text("Queued.");
          }

          if (status.IsDone)
          {
            this.Status.Text = Translate.Text("Links processed: {0}.", new object[]
                                                                         {
                                                                           status.Results.Count
                                                                         });
            this.Active = "LastPage";
            this.BackButton.Disabled = true;
          
            ReportUtil.CreateTempTable(status.Results);
          }
          else
          {
            SheerResponse.SetInnerHtml("ProcessView", process);
            SheerResponse.Timer("CheckStatus", 500);
          }
        }
      }
    }

    #endregion

    #region Protected methods

    /// <summary>
    /// The active page changed.
    /// </summary>
    /// <param name="page">
    /// The page.
    /// </param>
    /// <param name="oldPage">
    /// The old page.
    /// </param>
    protected override void ActivePageChanged(string page, string oldPage)
    {
      Assert.ArgumentNotNull(page, "page");
      Assert.ArgumentNotNull(oldPage, "oldPage");
      this.NextButton.Header = Translate.Text("Next >");
      if (page == "SelectRootItem")
      {
        this.NextButton.Header = Translate.Text("Process") + " >";
      }

      base.ActivePageChanged(page, oldPage);
      if (page == "Process")
      {
        this.NextButton.Disabled = true;
        this.BackButton.Disabled = true;
        this.CancelButton.Disabled = true;
        SheerResponse.Timer("StartProcessing", 10);
      }
    }

    /// <summary>
    /// The active page changing.
    /// </summary>
    /// <param name="page">
    /// The page.
    /// </param>
    /// <param name="newpage">
    /// The newpage.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    protected override bool ActivePageChanging(string page, ref string newpage)
    {
      Assert.ArgumentNotNull(page, "page");
      Assert.ArgumentNotNull(newpage, "newpage");
      if (page == "Retry")
      {
        newpage = "SelectRootItem";
      }

      if (newpage == "SelectLanguage")
      {
        if (GetSource() == null)
        {
          SheerResponse.Alert(Translate.Text("You must pick Source Database."), new string[0]);
          return false;
        }
      }

      if (newpage == "SelectRootItem")
      {
        this.UpdateTreeView();
        if (GetLanguage() == null || GetLanguage().Count == 0)
        {
          SheerResponse.Alert(Translate.Text("You must pick at least one language to publish."), new string[0]);
          return false;
        }
      }

      return base.ActivePageChanging(page, ref newpage);
    }

    /// <summary>
    /// Create temporary table as csv file.
    /// </summary>
    /// <param name="results">
    /// The results.
    /// </param>
 

    /// <summary>
    /// The show result.
    /// </summary>
    protected void ShowResult()
    {
      var uri = new UrlString("/sitecore modules/Shell/ExternalLinksChecker/Report.aspx");
      uri.Append("db", GetSourceDatabase().Name);
      Sitecore.Context.ClientPage.ClientResponse.ShowModalDialog(uri.ToString(), "900px", "800px", string.Empty, false);
    }

    /// <summary>
    /// The start processing.
    /// </summary>
    protected void StartProcessing()
    {
      List<Language> languages = GetLanguage();
      Database sourceDb = GetSourceDatabase();
      Item selectedRoot = this.GetSourceRootItem();
      Item root = selectedRoot ?? sourceDb.GetItem("/sitecore/content/", languages[0]);
      Handle handle = ExternalLinksCheckerManager.CheckItems(root, languages);
      this.JobHandle = handle.ToString();
      SheerResponse.Timer("CheckStatus", 500);
    }

    #endregion

    /// <summary>
    /// The get language.
    /// </summary>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    private static List<Language> GetLanguage()
    {
      var list = new List<Language>();
      foreach (string str in Context.ClientPage.ClientRequest.Form.Keys)
      {
        if ((str != null) && str.StartsWith("la_", StringComparison.InvariantCulture))
        {
          list.Add(Language.Parse(str.Substring(3)));
        }
      }

      return list;
    }

    /// <summary>
    /// The get source.
    /// </summary>
    /// <returns>
    /// The <see cref="Item"/>.
    /// </returns>
    private static Item GetSource()
    {
      string value = Context.ClientPage.ClientRequest.Form["SourceDatabase"];
      if (!string.IsNullOrEmpty(value))
      {
        string id = ShortID.Decode(value);
        Item item = Metadata.Settings.DefaultSettingDatabase.Items[id];
        Assert.IsNotNull(item, typeof(Item), "Source not found.", new object[0]);
        return item;
      }

      return null;
    }

    /// <summary>
    /// Get selected source database.
    /// </summary>
    /// <returns>
    /// The <see cref="Database"/>.
    /// </returns>
    private static Database GetSourceDatabase()
    {
      Item item = GetSource();
      Assert.IsNotNull(item, "SourceItem");
      string name = item["DatabaseName"];
      Database database = Factory.GetDatabase(name);
      Assert.IsNotNull(database, typeof(Database), Translate.Text("Database \"{0}\" not found."), new object[]
                                                                                                    {
                                                                                                      name
                                                                                                    });

      return database;
    }

    /// <summary>
    /// The get source root item.
    /// </summary>
    /// <returns>
    /// The <see cref="Item"/>.
    /// </returns>
    protected Item GetSourceRootItem()
    {
      Item selectionItem = this.TreeView.GetSelectionItem();
      if (selectionItem != null)
      {
        return selectionItem;
      }

      return null;
    }



    #region Private methods 
    /// <summary>
    /// The update source tree view.
    /// </summary>
    private void UpdateTreeView()
    {
      this.sourceDatabase = GetSourceDatabase();
      Item source = GetSource();

      string root = (source != null && !string.IsNullOrEmpty(source["Root"])) ? source["Root"] : Metadata.Settings.SourceRootItem;
      this.SourceRootDataContext.Root = root;

      Item folder = this.sourceDatabase.GetItem(root);
      if (folder.GetChildren().Count > 0)
      {
        this.SourceRootDataContext.Folder = folder.GetChildren()[0].Paths.Path;
      }

      this.SourceRootDataContext.Parameters = "databasename=" + this.sourceDatabase.Name;

      this.TreeView.RefreshRoot();
    }

    #endregion
  }
}