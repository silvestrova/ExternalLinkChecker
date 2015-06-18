// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITypeForChecking.cs" company="">
//   
// </copyright>
// <summary>
//   The TypeForChecking interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExternalLinksChecker.TypesForChecking
{
  using System.Collections.Generic;

  using ExternalLinksChecker.Metadata;

  using Sitecore.Data.Fields;

  /// <summary>
  /// The TypeForChecking interface.
  /// </summary>
  internal interface ITypeForChecking
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
    List<ResponseResults> CheckFieldValue(Field field);

    #endregion
  }
}