// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkTypesManager.cs" company="">
//   
// </copyright>
// <summary>
//   The link types manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExternalLinksChecker.Managers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Runtime.Remoting;
  using System.Xml;

  using ExternalLinksChecker.Metadata;
  using ExternalLinksChecker.TypesForChecking;

  using Sitecore.Data.Fields;
  using Sitecore.Xml;

  /// <summary>
  /// The link types manager.
  /// </summary>
  public static class LinkTypesManager
  {
    #region Fields

    /// <summary>
    /// The types.
    /// </summary>
    private static Dictionary<string, ITypeForChecking> types;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes static members of the <see cref="LinkTypesManager"/> class.
    /// </summary>
    static LinkTypesManager()
    {
      GenerateTypesForChecking();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Check field's value using an appropriate processor.
    /// </summary>
    /// <param name="field">
    /// The field.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    public static List<ResponseResults> CheckField(Field field)
    {
      return types[field.TypeKey].CheckFieldValue(field);
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Generate types for checking. Types source - config file
    /// </summary>
    private static void GenerateTypesForChecking()
    {
      types = new Dictionary<string, ITypeForChecking>();
      Dictionary<string, string> checkFieldTypes = Factory.GetTypes();
      if (checkFieldTypes != null)
      {
        foreach (string typekey in checkFieldTypes.Keys)
        {
          types.Add(typekey, Factory.GenerateType(checkFieldTypes[typekey]));
        }
      }
    }

    #endregion

    #region Factory

    /// <summary>
    /// The factory.
    /// </summary>
    internal class Factory
    {
      /// <summary>
      /// Generate type.
      /// </summary>
      /// <param name="type">
      /// The type name.
      /// </param>
      /// <returns>
      /// The <see cref="ITypeForChecking"/>.
      /// </returns>
      internal static ITypeForChecking GenerateType(string type)
      {
        type.Remove(' ');
        string[] parsedType = type.Split(',');
        if (parsedType.Count() < 2)
        {
          return null;
        }

        ObjectHandle t = Activator.CreateInstance(parsedType[1], parsedType[0]);
        return (ITypeForChecking)t.Unwrap();
      }

      /// <summary>
      /// The get types.
      /// </summary>
      /// <returns>
      /// The <see cref="Dictionary"/>.
      /// </returns>
      internal static Dictionary<string, string> GetTypes()
      {
        var types = new Dictionary<string, string>();
        foreach (XmlNode node in Sitecore.Configuration.Factory.GetConfigNodes("FieldCheckTypes/fieldType"))
        {
          types.Add(XmlUtil.GetAttribute("fieldTypeName", node).ToLowerInvariant(), XmlUtil.GetAttribute("type", node));
        }

        return types;
      }
    }

    #endregion

    /// <summary>
    /// Gets the types.
    /// </summary>
    internal static Dictionary<string, ITypeForChecking> Types
    {
      get
      {
        return types;
      }
    }
  }
}