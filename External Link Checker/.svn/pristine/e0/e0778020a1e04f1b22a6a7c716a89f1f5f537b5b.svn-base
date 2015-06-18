// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestUtil.cs" company="">
//   
// </copyright>
// <summary>
//   The request util.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExternalLinksChecker.Utils
{
  using System;
  using System.Globalization;
  using System.Linq;
  using System.Net;

  using ExternalLinksChecker.Metadata;

  /// <summary>
  /// The request util.
  /// </summary>
  public static class RequestUtil
  {
    #region Public methods

    /// <summary>
    /// Get response code.
    /// </summary>
    /// <param name="uri">
    /// The uri.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public static string GetResponseCode(Uri uri)
    {
      return GetResponseCode(uri.ToString());
    }

    /// <summary>
    /// Get response code by string url
    /// </summary>
    /// <param name="url">
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public static string GetResponseCode(string url)
    {
      // Creates an HttpWebRequest for the specified URL. 
      Uri uri;
      bool allowedprotocol = Settings.AllowedProtocols.Split('|').Any(protocol => url.StartsWith(protocol + "://"));
      if (Uri.TryCreate(url, UriKind.Absolute, out uri) && allowedprotocol)
      {
        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
        myHttpWebRequest.Timeout = Settings.HttpWebRequestTimeout;
        myHttpWebRequest.AllowAutoRedirect = false;

        try
        {
          myHttpWebRequest.Method = "GET";
          var code = (int)((HttpWebResponse)myHttpWebRequest.GetResponse()).StatusCode;
          string response = code.ToString(CultureInfo.InvariantCulture);
          return response;
        }
        catch (TimeoutException)
        {
          var code = (int)HttpStatusCode.RequestTimeout;
          return code.ToString(CultureInfo.InvariantCulture);
        }
        catch (Exception)
        {
          var code = (int)HttpStatusCode.ExpectationFailed;
          return code.ToString(CultureInfo.InvariantCulture);
        }
      }

      return null;
    }

    #endregion
  }
}