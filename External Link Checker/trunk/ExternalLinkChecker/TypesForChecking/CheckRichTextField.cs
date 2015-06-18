// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckRichTextField.cs" company="">
//   
// </copyright>
// <summary>
//   The check rich text field.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExternalLinksChecker.TypesForChecking
{
  using System;
  using System.Collections.Generic;

  using ExternalLinksChecker.Metadata;
  using ExternalLinksChecker.Utils;

  using HtmlAgilityPack;

  using Sitecore.Data.Fields;
  using Sitecore.Diagnostics;

  /// <summary>
  /// The check rich text field.
  /// </summary>
  public class CheckRichTextField : ITypeForChecking
  {
    #region Public methods

    /// <summary>
    /// The check field value.
    /// </summary>
    /// <param name="field">
    /// The field.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    public List<ResponseResults> CheckFieldValue(Field field)
    {
      Assert.ArgumentNotNull(field, "Field");
      var list = new List<ResponseResults>();
      string innerText = field.GetValue(true);
      var document = new HtmlDocument();
      document.LoadHtml(innerText);
      HtmlNodeCollection collection = document.DocumentNode.SelectNodes("//a");
      if (collection != null)
      {
        foreach (HtmlNode link in collection)
        {
          string target = (link.Attributes["href"] != null) ? link.Attributes["href"].Value : string.Empty;
          if (!string.IsNullOrEmpty(target))
          {
            string code = RequestUtil.GetResponseCode(target);
            if (code != null)
            {
              list.Add(new ResponseResults(field, code, new Uri(target)));
            }
          }
        }
      }

      return list;
    }

    #endregion
  }
}