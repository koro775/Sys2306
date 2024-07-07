using MathNet.Numerics.Statistics;
using Sys2306.domain.Trading.Trades;

namespace Sys2306.domain.performances;


public record PerformRec2(string シンボル, string 売買種, int スペース数, int 売買数, double 合計, double? 平均,
                                    double? 勝ち平均, double? 負け平均,
                                    double? 勝率, double? PR, double? PF,
                                    double 最大順行, double 最大逆行, double 順行平均, double 逆行平均, double 順行偏差, double 逆行偏差,
                                    double 勝ち順行平均, double 勝ち逆行平均, double 勝ち順行偏差, double 勝ち逆行偏差, double 勝ち最大逆行,
                                    double 負け順行平均, double 負け逆行平均, double 負け順行偏差, double 負け逆行偏差);

public class Performance2
{
      //インスタンス変数
      string _symbol;
      int _spaceNum;
      List<TradeRec> _shortList;
      List<TradeRec> _longList;


      //プロパティー
      public List<PerformRec2> PerformList2 { get; set; } = new();
      private int Tick { get; set; } = 100;

      //コンストラクタ
      public Performance2(string symbol, int spaceNum, Trade trade) =>
                        (_symbol, _spaceNum, _shortList, _longList) = (symbol, spaceNum, trade.ShortList, trade.LongList);

      public void Process() {

            CalcPerformance(_shortList, "Short");
            CalcPerformance(_longList, "Long");
      }

      private void CalcPerformance(List<TradeRec> list, string tradeKind) {

            (int count, double sum, int winCount, double? winSum, double? winAvg, int loseCount, double? loseSum, double? loseAvg,
            double fwMax, double adMax, double fwAvg, double adAvg, double fwStd, double adStd,
            double FwAvgWin, double AdAvegWin, double FwStdWin, double AdStdWin, double AdMaxWin,
            double FwAvgLose, double AdAvegLose, double FwStdLose, double AdStdLose) =
                              (0, 0, 0, 0, 0, 0, 0, 0, double.MinValue, double.MaxValue, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            winCount = list.Count(x => x.Win != null);
            loseCount = list.Count(x => x.Lose != null);
            (double[] fw, double[] ad) winArr = (new double[winCount], new double[winCount]);
            (double[] fw, double[] ad) loseArr = (new double[loseCount], new double[loseCount]); ;
            (int j, int k) = (0, 0);

            for (int i = 0; i < list.Count; i++) {
                  count++;
                  sum += list[i].ProfitLoss;
                  if (list[i].Win != null) {
                        winSum += list[i].Win;
                        winArr.fw[j] = list[i].Fw;
                        winArr.ad[j] = list[i].Ad;
                        j++;
                  }
                  if (list[i].Lose != null) {
                        loseSum += list[i].Lose;
                        loseArr.fw[k] = list[i].Fw;
                        loseArr.ad[k] = list[i].Ad;
                        k++;
                  }
                  fwMax = list[i].Fw > fwMax ? list[i].Fw : fwMax;
                  adMax = list[i].Ad < adMax ? list[i].Ad : adMax;
            }
            double? avg = count != 0 ? sum / count : null;
            double? winRate = count != 0 ? (double)winCount / count : null;
            winAvg = winCount != 0 ? winSum / winCount : null;
            loseAvg = loseCount != 0 ? loseSum / loseCount : null;

            double? PayoffR = winAvg / loseAvg * -1;
            double? ProfitF = loseSum != 0 ? (winSum / loseSum) * -1 : null;
            (fwAvg, fwStd) = list.Select(x => x.Fw).ToArray().MeanStandardDeviation();
            (adAvg, adStd) = list.Select(x => x.Ad).ToArray().MeanStandardDeviation();

            (FwAvgWin, FwStdWin) = winArr.fw.MeanStandardDeviation();
            (AdAvegWin, AdStdWin) = winArr.ad.MeanStandardDeviation();
            (FwAvgLose, FwStdLose) = loseArr.fw.MeanStandardDeviation();
            (AdAvegLose, AdStdLose) = loseArr.ad.MeanStandardDeviation();
            AdMaxWin = winArr.ad.Min(x => x);

            PerformList2.Add(new PerformRec2(_symbol, tradeKind, _spaceNum, count,
                                                                   Math.Round(sum * Tick, 1, MidpointRounding.AwayFromZero),
                                                                   avg != null ? Math.Round((double)avg * Tick, 1, MidpointRounding.AwayFromZero) : null,
                                                                   winAvg != null ? Math.Round((double)winAvg * Tick, 1, MidpointRounding.AwayFromZero) : null,
                                                                   loseAvg != null ? Math.Round((double)loseAvg * Tick, 1, MidpointRounding.AwayFromZero) : null,
                                                                   winRate != null ? Math.Round((double)winRate * 100, 1, MidpointRounding.AwayFromZero) : null,
                                                                   PayoffR != null ? Math.Round((double)PayoffR * 100, 1, MidpointRounding.AwayFromZero) : null,
                                                                   ProfitF != null ? Math.Round((double)ProfitF * 100, 1, MidpointRounding.AwayFromZero) : null,
                                                                   Math.Round(fwMax * Tick, 1, MidpointRounding.AwayFromZero),
                                                                   Math.Round(adMax * Tick, 1, MidpointRounding.AwayFromZero),
                                                                   Math.Round(fwAvg * Tick, 1, MidpointRounding.AwayFromZero),
                                                                   Math.Round(adAvg * Tick, 1, MidpointRounding.AwayFromZero),
                                                                   Math.Round(fwStd * Tick, 1, MidpointRounding.AwayFromZero),
                                                                   Math.Round(adStd * Tick, 1, MidpointRounding.AwayFromZero),
                                                                   Math.Round(FwAvgWin * Tick, 1, MidpointRounding.AwayFromZero),
                                                                   Math.Round(AdAvegWin * Tick, 1, MidpointRounding.AwayFromZero),
                                                                   Math.Round(FwStdWin * Tick, 1, MidpointRounding.AwayFromZero),
                                                                   Math.Round(AdStdWin * Tick, 1, MidpointRounding.AwayFromZero),
                                                                   Math.Round(AdMaxWin * Tick, 1, MidpointRounding.AwayFromZero),
                                                                   Math.Round(FwAvgLose * Tick, 1, MidpointRounding.AwayFromZero),
                                                                   Math.Round(AdAvegLose * Tick, 1, MidpointRounding.AwayFromZero),
                                                                   Math.Round(FwStdLose * Tick, 1, MidpointRounding.AwayFromZero),
                                                                   Math.Round(AdStdLose * Tick, 1, MidpointRounding.AwayFromZero)));
      }
}

