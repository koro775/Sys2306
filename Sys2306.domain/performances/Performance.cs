using MathNet.Numerics.Statistics;
using Sys2306.domain.Trading.Trades;

namespace Sys2306.domain.performances;

public record PerformRec(string Symbol, string TradeKind, int SpaceNum, int Count, double Sum, double? Ave,
                                    double? WinAvg, double? LoseAvg,
                                    double? WinRate, double? PayoffR, double? ProfitF,
                                    double FwMax, double AdMax, double FwAvg, double AdAveg, double FwStd, double AdStd);

public class Performance
{
      //インスタンス変数
      string _symbol;
      int _spaceNum;
      List<TradeRec> _shortList;
      List<TradeRec> _longList;

      //プロパティー
      public List<PerformRec> PerformList { get; set; } = new();

      //コンストラクタ
      public Performance(string symbol, int spaceNum, Trade trade) =>
                        (_symbol, _spaceNum, _shortList, _longList) = (symbol, spaceNum, trade.ShortList, trade.LongList);

      public void Process() {

            CalcPerformance(_shortList, "Short");
            CalcPerformance(_longList, "Long");
      }

      private void CalcPerformance(List<TradeRec> list, string tradeKind) {

            //ローカル変数初期化
            (int count, double sum, int? winCount, double? winSum, double? winAvg, int? loseCount, double? loseSum, double? loseAvg,
                              double fwMax, double adMax, double fwAvg, double adAvg, double fwStd, double adStd) =
                              (0, 0, 0, 0, 0, 0, 0, 0, double.MinValue, double.MaxValue, 0, 0, 0, 0);

            for (int i = 0; i < list.Count; i++) {
                  count++;
                  sum += list[i].ProfitLoss;
                  if (list[i].Win != null) winCount++;
                  if (list[i].Win != null) winSum += list[i].Win;
                  if (list[i].Lose != null) loseCount++;
                  if (list[i].Lose != null) loseSum += list[i].Lose;
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

            PerformList.Add(new PerformRec(_symbol, tradeKind, _spaceNum, count, sum, avg,
                                                                  winAvg, loseAvg,
                                                                  winRate, PayoffR, ProfitF,
                                                                  fwMax, adMax, fwAvg, adAvg, fwStd, adStd));
      }
}

//private void Short() {

//      (int count, double sum, int winCount, double winSum, int loseCount, double loseSum,
//       double fwMax, double adMax, double fwAvg, double adAvg, double fwStd, double adStd) =
//                                          (0, 0, 0, 0, 0, 0, double.MinValue, double.MaxValue, 0, 0, 0, 0);

//      for (int i = 0; i < _shortList.Count; i++) {
//            count++;
//            sum += _shortList[i].ProfitLoss;
//            if (_shortList[i].Win != double.NaN) winCount++;
//            if (_shortList[i].Win != double.NaN) winSum += _shortList[i].Win;
//            if (_shortList[i].Lose != double.NaN) loseCount++;
//            if (_shortList[i].Lose != double.NaN) loseSum += _shortList[i].Lose;
//            fwMax = _shortList[i].Fw > fwMax ? _shortList[i].Fw : fwMax;
//            adMax = _shortList[i].Ad < adMax ? _shortList[i].Ad : adMax;
//      }

//      var avg = count != 0 ? sum / count : double.NaN;
//      var winRate = winCount + loseCount != 0 ? winCount / (winCount + loseCount) : double.NaN;
//      var PayoffR = loseSum / loseCount != 0 ? (winSum / winCount) / (loseSum / loseCount) : double.NaN;
//      var ProfitF = loseSum != 0 ? winSum / loseSum : double.NaN;
//      (fwAvg, fwStd) = Statistics.MeanStandardDeviation(_shortList.Select(x => x.Fw).ToArray());
//      (adAvg, adStd) = Statistics.MeanStandardDeviation(_shortList.Select(x => x.Ad).ToArray());

//      PerformTable.Add(new PerformRec(_symbol, "Short", _spaceNum, count, sum, avg,
//                                                            winRate, PayoffR, ProfitF,
//                                                            fwMax, adMax, fwAvg, adAvg, fwStd, adStd));
//}

//private void Long() {

//      (int count, double sum, int winCount, double winSum, int loseCount, double loseSum,
//                        double fwMax, double adMax, double fwAvg, double adAvg, double fwStd, double adStd) =
//                        (0, 0, 0, 0, 0, 0, double.MinValue, double.MaxValue, 0, 0, 0, 0);
//      for (int i = 0; i < _longList.Count; i++) {
//            count++;
//            sum += _longList[i].ProfitLoss;
//            if (_longList[i].Win != double.NaN) winCount++;
//            if (_longList[i].Win != double.NaN) winSum += _longList[i].Win;
//            if (_longList[i].Lose != double.NaN) loseCount++;
//            if (_longList[i].Lose != double.NaN) loseSum += _longList[i].Lose;
//            fwMax = _longList[i].Fw > fwMax ? _longList[i].Fw : fwMax;
//            adMax = _longList[i].Ad < adMax ? _longList[i].Ad : adMax;
//      }
//      var avg = count != 0 ? sum / count : double.NaN;
//      var winRate = winCount + loseCount != 0 ? winCount / (winCount + loseCount) : double.NaN;
//      var PayoffR = loseSum / loseCount != 0 ? (winSum / winCount) / (loseSum / loseCount) : double.NaN;
//      var ProfitF = loseSum != 0 ? winSum / loseSum : double.NaN;
//      (fwAvg, fwStd) = Statistics.MeanStandardDeviation(_longList.Select(x => x.Fw).ToArray());
//      (adAvg, adStd) = Statistics.MeanStandardDeviation(_longList.Select(x => x.Ad).ToArray());

//      PerformTable.Add(new PerformRec(_symbol, "Long", _spaceNum, count, sum, avg,
//                                                            winRate, PayoffR, ProfitF,
//                                                            fwMax, adMax, fwAvg, adAvg, fwStd, adStd));
//}

//int WinCount, double WinSum, double LoseSum, double WinAvg, double LoseAvg);
