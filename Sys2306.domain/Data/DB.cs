using Helper03.SqlServerHelpers;
using Microsoft.Data.SqlClient;
using Sys2306.domain.Statics;

namespace Sys2306.domain.Data
{
      public class DB
      {
            //var(X, Dt, Open, High, Low, Close, Hspace, Lspace) = _db.D;

            readonly string _sql;

            //コンストラクタ
            public DB(string symbol, int len) {
                  _sql = GetSelectSql(symbol, len);
                  Len = Functions.GetRecCnt(symbol, len);
                  D = (new double[Len], new DateTime[Len], new double[Len], new double[Len], new double[Len], new double[Len], new int[Len], new int[Len]/*, new double[Len], new double[Len]*/);
            }

            public int Len { get; set; }
            public static bool IsRead { get; set; } = false;


            //取り込みデータ
            public (double[] X, DateTime[] Dt, double[] Open, double[] High, double[] Low, double[] Close,
                              int[] Hspace, int[] Lspace) D { get; set; }

            private static string GetSelectSql(string symbol, int cnt) {
                  string param1 = cnt == 0 ? string.Empty : $"top {cnt}";
                  return @$"
                   SELECT {param1} 
                      日時,始値,[高値],[安値],終値,hSpace,lSpace
                   FROM {symbol} 
                  order by 日時
                  ";
            }

            public void Read() {

                  int i = 0;
                  void action(SqlDataReader reader) {
                        D.X[i] = Convert.ToDouble(i);
                        D.Dt[i] = Convert.ToDateTime(reader["日時"]);
                        D.Open[i] = (double)Convert.ToDecimal(reader["始値"]);
                        D.High[i] = (double)Convert.ToDecimal(reader["高値"]);
                        D.Low[i] = (double)Convert.ToDecimal(reader["安値"]);
                        D.Close[i] = (double)Convert.ToDecimal(reader["終値"]);
                        D.Hspace[i] = Convert.ToInt32(reader["HSpace"]);
                        D.Lspace[i] = Convert.ToInt32(reader["LSpace"]);

                        //転換の計算だよ
                        //tb.Calc(i, D.High[i], D.Low[i], D.Close[i], D.Hspace[i], D.Lspace[i]);
                        i++;
                  }
                  //DB読込
                  ReaderHelper.Query(_sql, action, Cn.Koro14T2);
                  IsRead = true;
            }
      }
}
