// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Panels.CodePanel.cs" company="">
//   
// </copyright>
// <summary>
//   The code panel.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace ExternalLinksChecker.Dialogs
{
  using System.IO;
  using System.Web.UI;
  using ExternalLinksChecker.Metadata;
  using Sitecore;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Resources;
  using Sitecore.Security.Accounts;
  using Sitecore.Shell.Applications.ContentManager.Panels;
  using Sitecore.Web;
  using Sitecore.Web.UI;
  using Sitecore.Web.UI.HtmlControls;

  /// <summary>
  /// The response codes panel.
  /// </summary>
  internal class CodePanel : DropDownPanel
  {
    // Methods

    /// <summary>
    /// Initializes a new instance of the <see cref="CodePanel"/> class.
    /// </summary>
    public CodePanel()
    {
      string str;
      AccountType type;
      Id = "CodePanel";
      Gallery = "Gallery.LinksChecker";
      Width = 250;
    }

 
    /// <summary>
    /// Render panel.
    /// </summary>
    /// <param name="output">
    /// The output.
    /// </param>
    
    protected override void RenderPanel(HtmlTextWriter output)
    {
      HtmlTextWriter output2 = new HtmlTextWriter(new StringWriter());
      Assert.ArgumentNotNull(output, "output");
      Database db = Settings.DefaultSettingDatabase;
      RenderCodes(output, "Show All", string.Empty);
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
      }
    }

    /// <summary>
    /// Render label.
    /// </summary>
    /// <param name="output">
    /// The output.
    /// </param>
    /// <param name="html">
    /// The html.
    /// </param>
    
    private void RenderLabel(HtmlTextWriter output, string html)
    {
      Assert.ArgumentNotNull(output, "output");
      Assert.ArgumentNotNull(html, "html");
      Tag tag = new Tag("span", html);
      tag["class"] = "scRibbonToolbarSmallButtonLabel header";
      if (base.Width > 0)
      {
        tag["style"] = "width: " + (base.Width - 0x2d) + "px";
      }

      tag.ToString(output);
    }

    /// <summary>
    /// Render response codes.
    /// </summary>
    /// <param name="output">
    /// The output.
    /// </param>
    /// <param name="code">
    /// The code.
    /// </param>
    /// <param name="searchCode">
    /// The search code.
    /// </param>
    
    private void RenderCodes(HtmlTextWriter output, string code, string searchCode)
    {
      Assert.ArgumentNotNull(output, "output");
      Assert.ArgumentNotNull(code, "code");
      string str = WebUtil.HtmlEncode(string.Concat(new object[]
                                                      {
                                                        "javascript:search('" + searchCode + "','" + code + "');"
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
  }
}