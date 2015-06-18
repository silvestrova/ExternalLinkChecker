// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportData.cs" company="">
//   
// </copyright>
// <summary>
//   The report data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExternalLinksChecker.Metadata
{
  using System.Linq;
  using System.Web;

  using ExternalLinksChecker.Utils;

  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Globalization;
  using Sitecore.Web;

  /// <summary>
  /// The report data.
  /// </summary>
  public class ReportData
  {
    #region Fields

    /// <summary>
    /// The code.
    /// </summary>
    private readonly string code;

    /// <summary>
    /// The description.
    /// </summary>
    private readonly string description;

    /// <summary>
    /// The field.
    /// </summary>
    private readonly Field field;

    /// <summary>
    /// The link.
    /// </summary>
    private readonly string link;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ReportData"/> class.
    /// </summary>
    /// <param name="reportData">
    /// The report data.
    /// </param>
    /// <param name="currentDatabase">
    /// The current database.
    /// </param>
    public ReportData(string[] reportData, Database currentDatabase)
    {
      for (int i = 0; i < reportData.Count(); i++)
      {
        reportData[i] = ReportUtil.IncludeDelimiter(reportData[i]);
      }

      Language la;
      if (reportData[0] != null)
      {
        this.code = reportData[0];
      }

      if (reportData[1] != null)
      {
        this.link = reportData[1];
      }

      if (reportData[2] != null && reportData[3] != null && reportData[4] != null && Language.TryParse(reportData[4], out la))
      {
        Item contentItem = currentDatabase.GetItem(reportData[3], la);
        this.field = contentItem.Fields[new ID(reportData[2])];
      }

      if (reportData[5] != null)
      {
        this.description = reportData[5];
      }
    }

    #endregion

    #region Protected methods

    /// <summary>
    /// The include delimiter.
    /// </summary>
    /// <param name="input">
    /// The input.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
   

    #endregion

    #region Properties

    /// <summary>
    /// Gets the code.
    /// </summary>
    public string Code
    {
      get
      {
        return this.code;
      }
    }

    /// <summary>
    /// Gets the content item.
    /// </summary>
    public string ContentItem
    {
      get
      {
        return this.field.Item.Paths.Path;
      }
    }

    /// <summary>
    /// Gets the description.
    /// </summary>
    public string Description
    {
      get
      {
        return ReportUtil.IncludeDelimiter(this.description);
      }
    }

    /// <summary>
    /// Gets the field html.
    /// </summary>
    public string FieldHtml
    {
      get
      {
        return HttpUtility.HtmlEncode(this.field.GetValue(true));
      }
    }

    /// <summary>
    /// Gets the field name.
    /// </summary>
    public string FieldName
    {
      get
      {
        return this.field.Name + " (" + this.field.TypeKey + ")";
      }
    }

    /// <summary>
    /// Gets the item link.
    /// </summary>
    public string ItemLink
    {
      get
      {
        string link = string.Format("http://" + WebUtil.GetHostName() + "/sitecore/shell/sitecore/content/Applications/Content Editor.aspx?id={0}&la={1}&fo={0}", this.field.Item.ID.ToGuid(), this.field.Item.Language);
        return link;
      }
    }

    /// <summary>
    /// Gets the link text.
    /// </summary>
    public string LinkText
    {
      get
      {
        return this.link;
      }
    }

    #endregion
  }
}