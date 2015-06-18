namespace ExternalLinksChecker.Dialogs
{
  using System;
  using System.IO;
  using System.Web.UI;
  using ExternalLinksChecker.Metadata;
  using Sitecore;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Resources;
  using Sitecore.Security;
  using Sitecore.Security.Accounts;
  using Sitecore.Shell.Applications.ContentManager.Galleries;
  using Sitecore.Shell.Applications.ContentManager.Panels;
  using Sitecore.Web;
  using Sitecore.Web.UI;
  using Sitecore.Web.UI.HtmlControls;
  using Sitecore.Web.UI.Sheer;

  public class GalleryCodesForm : GalleryForm
  {
    protected Scrollbox Options;

    public override void HandleMessage(Message message)
    {
      Assert.ArgumentNotNull(message, "message");
      if (message.Name.StartsWith("checker"))
      {
        string command = message.ToString().Substring("checker".Length + 1).Replace('"', '\'');

        System.Web.UI.Control c = Context.Page.Page.Parent;
      
        SheerResponse.Eval("scForm.getParentForm().invoke(\"" + StringUtil.EscapeBackslash(command) + "\")");
        SheerResponse.Eval("scForm.getParentForm().Content.closeGallery(scForm.browser.getFrameElement().id)");
        message.CancelBubble = true;
        message.CancelDispatch = true;
      }
    }

    protected override void OnLoad(EventArgs e)
    {
      Assert.ArgumentNotNull(e, "e");
      base.OnLoad(e);
      if (!Context.ClientPage.IsEvent)
      {
        this.RenderRecentCodes();
      }
    }
    
    private void RenderRecentCodes()
    {

      HtmlTextWriter output = new HtmlTextWriter(new StringWriter());
      Database db = Settings.DefaultSettingDatabase;
      if (db != null)
      {
        Item codesRoot = db.GetItem(Settings.HttpStatusCodeFolder);
        if (codesRoot != null)
        {
          foreach (Item code in codesRoot.GetChildren())
          {
            string searchCode = code.DisplayName + "(" + code["code"] + ")";
            RenderCodes(output, searchCode, searchCode);
          }
        }
        this.Options.Controls.Add(new LiteralControl(output.InnerWriter.ToString()));
      }
    }

    private void RenderCodes(HtmlTextWriter output, string code, string searchCode)
    {
      Assert.ArgumentNotNull(output, "output");
      Assert.ArgumentNotNull(code, "code");
      string str = WebUtil.HtmlEncode(string.Concat(new object[]
                                                      {
                                                        "javascript:scForm.postEvent(this,event,'checker:Search(\"" + searchCode + "\")');"
                                                      }));
      string str2 = "scRibbonToolbarSmallButton";
      output.Write("<a id=\"" + code + "\" href=\"" + str + "\" class=\"" + str2 + "\" title=\"" + code + "\">");
      output.Write("<span class=\"scRibbonToolbarSmallButtonPrefix header\">");
      output.Write("</span>");
      this.RenderLabel(output, new ImageBuilder
                                 {
                                   Src = Images.GetThemedImageSource("Network/24x24/earth_preferences.png", ImageDimension.id16x16), 
                                   Class = "scRibbonToolbarSmallButtonIcon", 
                                   Alt = code
                                 } + StringUtil.Clip(code, 50, true));
      output.Write("</a>");
    }
    private void RenderLabel(HtmlTextWriter output, string html)
    {
      Assert.ArgumentNotNull(output, "output");
      Assert.ArgumentNotNull(html, "html");
      Tag tag = new Tag("span", html);
      tag["class"] = "scRibbonToolbarSmallButtonLabel header";
      tag.ToString(output);
    }
  }
}

  