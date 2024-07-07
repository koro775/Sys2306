namespace Sys2306.domain.Statics
{
      public static class Functions
      {
            public static int GetRecCnt(string symbol, int cnt) {
                  //SQLでlead関数を使って最終行がnullになるから　ー１
                  //if (cnt == 0) return Dict.SymbolLen[symbol] - 1;
                  if (cnt == 0) return SymbolLen[symbol];
                  return cnt;
            }

            private static Dictionary<string, int> SymbolLen => new() {
                  ["USDJPY"] = 5609709,
                  ["GBPJPY"] = 5609410,
            };

            /// <summary>
            /// double欠損値0を穴埋める
            /// </summary>
            /// <param name="data"></param>
            public static void Fill0(Span<double> data, double start, double end) {

                  double x0 = data[0] = start;
                  data[data.Length - 1] = end;

                  (int cnt, int seqNo, double a) = (0, 0, 0);
                  for (int i = 1; i < data.Length; i++) {
                        if ((data[i]) == 0 && seqNo == 0) {
                              x0 = data[i - 1];
                              seqNo = i;
                        }
                        if (cnt > 0 && (data[i] != 0)) {
                              a = (data[i] - x0) / (cnt + 1);
                              for (int j = 0; j < cnt; j++) {
                                    data[j + seqNo] = data[j + seqNo - 1] + a;
                              }
                              cnt = 0;
                              seqNo = 0;
                              a = 0;
                        }
                        else if (seqNo > 0) cnt++;
                  }
            }

            public static double MaxSpan(Span<double> data) {
                  if (data.Length == 0) {
                        return double.NaN;
                  }

                  double num = double.NegativeInfinity;
                  for (int i = 0; i < data.Length; i++) {
                        if (data[i] > num || double.IsNaN(data[i])) {
                              num = data[i];
                        }
                  }
                  return num;
            }

            public static double MinSpan(Span<double> data) {
                  if (data.Length == 0) {
                        return double.NaN;
                  }

                  double num = double.PositiveInfinity;
                  for (int i = 0; i < data.Length; i++) {
                        if (data[i] < num || double.IsNaN(data[i])) {
                              num = data[i];
                        }
                  }
                  return num;
            }

            public static double AvgSpan(Span<double> data) {
                  if (data.Length == 0) {
                        return double.NaN;
                  }

                  double num = 0.0;
                  ulong num2 = 0uL;
                  for (int i = 0; i < data.Length; i++) {
                        num += (data[i] - num) / (double)(++num2);
                  }
                  return num;
            }

      }
}
