// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResponseResults.cs" company="">
//   
// </copyright>
// <summary>
//   The response results.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExternalLinksChecker.Metadata
{
  using System;

  using ExternalLinksChecker.Utils;

  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;

  /// <summary>
  /// The response results.
  /// </summary>
  public class ResponseResults
  {
    #region Fields

    /// <summary>
    /// The field.
    /// </summary>
    private readonly Field field;

    /// <summary>
    /// The uri.
    /// </summary>
    private readonly Uri uri;

    /// <summary>
    /// The code.
    /// </summary>
    private string code;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ResponseResults"/> class.
    /// </summary>
    /// <param name="field">
    /// The field.
    /// </param>
    /// <param name="code">
    /// The code.
    /// </param>
    /// <param name="uri">
    /// The uri.
    /// </param>
    public ResponseResults(Field field, string code, Uri uri)
    {
      this.field = field;
      this.code = code;
      this.uri = uri;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// TRender report.
    /// </summary>
    /// <returns>
    /// The <see cref="string[]"/>.
    /// </returns>
    public string[] RenderReport()
    {
      string description = this.GetDescription(ref this.code);

      if (Settings.ShowAllResults || !string.IsNullOrEmpty(description))
      {
        if (string.IsNullOrEmpty(description))
        {
          description = this.code;
        }

        string[] returnedValue =
          {
            ReportUtil.ExcludeDelimiter(this.code), ReportUtil.ExcludeDelimiter(this.uri.ToString()), ReportUtil.ExcludeDelimiter(this.field.ID.ToString()), ReportUtil.ExcludeDelimiter(this.field.Item.ID.ToString()), ReportUtil.ExcludeDelimiter(this.field.Item.Language.Name), ReportUtil.ExcludeDelimiter(description)
          };

        return returnedValue;
      }

      return null;
    }

    #endregion

    #region Protected methods

    /// <summary>
    /// The exclude delimiter.
    /// </summary>
    /// <param name="input">
    /// The input.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
  

    #endregion

    #region Private methods

    /// <summary>
    /// The get description.
    /// </summary>
    /// <param name="code">
    /// The code.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    private string GetDescription(ref string code)
    {
      Database db = Settings.DefaultSettingDatabase;
      if (db != null)
      {
        Item codeItem = db.GetItem(Settings.HttpStatusCodeFolder + "/" + code);

        if (codeItem == null)
        {
          return null;
        }

        code += "(" + codeItem["code"] + ")";
        return codeItem["description"];
      }

      return null;
    }

    #endregion
  }
}