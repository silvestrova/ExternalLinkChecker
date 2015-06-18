// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckLinkField.cs" company="">
//   
// </copyright>
// <summary>
//   The check link field.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExternalLinksChecker.TypesForChecking
{
  using System;
  using System.Collections.Generic;

  using ExternalLinksChecker.Metadata;
  using ExternalLinksChecker.Utils;

  using Sitecore.Data.Fields;
  using Sitecore.Diagnostics;

  /// <summary>
  /// The check link field.
  /// </summary>
  public class CheckLinkField : ITypeForChecking
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
      LinkField link = field;
      if (!(link.IsInternal || link.IsMediaLink))
      {
        string url = link.Url;
        if (!string.IsNullOrEmpty(url))
        {
          string code = RequestUtil.GetResponseCode(url);
          if (code != null)
          {
            list.Add(new ResponseResults(field, code, new Uri(url)));
          }
        }
      }

      return list;
    }

    #endregion
  }
}