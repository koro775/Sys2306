using System.Globalization;
using System.Text;
using CsvHelper;

namespace Helpers2306
{
      public static class CsvHelpers
      {

            /// <summary>
            /// Tはユーザー定義クラスorレコードでないとダメ
            /// 第二引数pathは省略可（デフォルトは OutputCsv.csv）
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="list"></param>
            /// <param name="path"></param>
            public static void Write<T>(List<T> list, string path) where T : class {
                  Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                  using var writer = new StreamWriter(path, false, Encoding.GetEncoding("Shift_JIS"));
                  using var csv = new CsvWriter(writer, new CultureInfo("ja-JP", false));
                  csv.WriteRecords(list);
            }
      }
}
