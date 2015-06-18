// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportUtil.cs" company="">
//   
// </copyright>
// <summary>
//   The report util.
// </summary>
// --------------------------------------------------------------------------------------------------------------------



namespace ExternalLinksChecker.Utils
{
  using System.Collections.Generic;
  using System.IO;
  using System.Text;
  using ExternalLinksChecker.Metadata;

  /// <summary>
  /// The report util class allowed to create report file, read data.
  /// </summary>
  public static class ReportUtil
  {
    /// <summary>
    /// The create temp table.
    /// </summary>
    /// <param name="results">
    /// The results.
    /// </param>
    private static readonly string[] header =
      {
        "response code", "uri", "field id", "item id", "language", "description"
      };

    /// <summary>
    /// Create temp table and add header.
    /// </summary>
    /// <param name="results">
    /// The results.
    /// </param>
    public static void CreateTempTable(List<ResponseResults> results)
    {
      string path = Metadata.Settings.GetReportFilePath();
      FileStream tempFile = null;
      CreateFile(ref path, ref tempFile, 1);

      const string Delimiter = ",";

      int length = results.Count;

      var sb = new StringBuilder();
      sb.AppendLine(string.Join(Delimiter, header));
      for (int index = 0; index < length; index++)
      {
        if (results[index] != null)
        {
          string[] res = results[index].RenderReport();
          if (res != null)
          {
            sb.AppendLine(string.Join(Delimiter, res));
          }
        }
      }

      File.WriteAllText(path, sb.ToString());
    }

    /// <summary>
    /// Include delimiter to the text in case to avoid comma using.
    /// </summary>
    /// <param name="input">
    /// The input.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public static string IncludeDelimiter(string input)
    {
      return input.Replace(Settings.DelimiterReplacer, ",");
    }

    /// <summary>
    /// Exclude delimiter from the text to be able to display commas.
    /// </summary>
    /// <param name="input">
    /// The input.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public static string ExcludeDelimiter(string input)
    {
      return input.Replace(",", Settings.DelimiterReplacer);
    }

    /// <summary>
    /// The get table reader without header.
    /// </summary>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    public static List<string[]> GetTableReader()
    {
      List<string[]> tableRows = new List<string[]>();
      string filePath = Metadata.Settings.GetReportFilePath();
      const char Delimiter = ',';
      string line;
      var reader = new StreamReader(File.OpenRead(filePath));
      while ((line = reader.ReadLine()) != null)
      {
        tableRows.Add(line.Split(Delimiter));
      }

      tableRows.RemoveAt(0);
      reader.Close();
      return tableRows;
    }

    /// <summary>
    /// Create file by path.
    /// </summary>
    /// <param name="path">
    /// The path.
    /// </param>
    /// <param name="tempFile">
    /// The temp file.
    /// </param>
    /// <param name="index">
    /// The index.
    /// </param>
    private static void CreateFile(ref string path, ref FileStream tempFile, int index)
    {
      try
      {
        tempFile = File.Create(path);
        tempFile.Close();
      }
      catch (IOException)
      {
        path += index++;
        CreateFile(ref path, ref tempFile, index);
      }
    }
  }
}