//using Sys2306.domain.Data;
//using Sys2306.domain.Trading.Entries;

//namespace Sys2306.domain.Trading
//{
//      public record TradeRec(DateTime EntryDt, double EntryPrice, DateTime ExitDt, double ExitPrice
//                                  , TimeSpan DtSpan, double ProfitLoss, double? Win, double? Lose, double Accum);

//      public record PerformRec(string Symbol, string SellBuy, int SpaceNum, int Count, double Sum, double Ave, double Max, double Min, double WinRate, double PayoffR, double ProfitF,
//                                int WinCount, double WinSum, double LoseSum, double WinAvg, double LoseAvg);

//#nullable disable
//      public class TradeTable           //コンストラクタで一気に初期化できないからRecordではなくClassにした
//      {
//            public string Stmbol { get; set; }
//            public int SpaceNum { get; set; }
//            public List<TradeRec> SellTable { get; set; }
//            public List<TradeRec> BuyTable { get; set; }
//      }

//      public class ExitOld
//      {
//            public TradeTable _tradeTable;
//            readonly DB _db;
//            readonly Wave _wave;
//            readonly EntryOld _entry;

//            //コンストラクタ
//            public ExitOld(string symbol, int spaceNum, DB db, Wave wave, EntryOld entry) {
//                  _db = db;
//                  _wave = wave;
//                  _tradeTable = new TradeTable {
//                        Stmbol = symbol,
//                        SpaceNum = spaceNum,
//                        SellTable = new(),
//                        BuyTable = new(),
//                  };

//                  _entry = entry;
//            }

//            public TradeTable CreateTradeTable() {

//                  var (X, Dt, Open, High, Low, Close, Hspace, Lspace) = _db.D;
//                  var (Idx, PosNeg, TbX, TbY, StartX, StartY, EndX, EndY, SpaceNum, BrkX, BrkY) = _wave.W;
//                  var (SellWaveIdx, EntrySellX, EntrySellY) = _entry.SellEntry;
//                  var (BuyWaveIdx, EntryBuyX, EntryBuyY) = _entry.BuyEntry;

//                  //sell
//                  double accumSell = 0;
//                  for (int i = 0; i < _entry.SellEntry.EntrySellX.Count - 0; i++) {

//                        var entryDt = Dt[EntrySellX[i]];
//                        var entryPrice = EntrySellY[i];
//                        var exitDt = Dt[EndX[SellWaveIdx[i]]];
//                        var exitPrice = EndY[SellWaveIdx[i]];
//                        var dtSpan = exitDt.Subtract(entryDt);
//                        var profitLoss = entryPrice - exitPrice;
//                        var profit = profitLoss > 0 ? profitLoss : double.NaN;
//                        var loss = profitLoss <= 0 ? profitLoss : double.NaN;
//                        accumSell += profitLoss;

//                        _tradeTable.SellTable.Add(new TradeRec(entryDt, entryPrice, exitDt, exitPrice, dtSpan, profitLoss, profit, loss, accumSell));
//                  }
//                  //buy
//                  double accumBuy = 0;

//                  for (int i = 0; i < _entry.BuyEntry.EntryBuyX.Count; i++) {

//                        var entryDt = Dt[EntryBuyX[i]];
//                        var entryPrice = EntryBuyY[i];
//                        var exitDt = Dt[EndX[BuyWaveIdx[i]]];
//                        var exitPrice = EndY[BuyWaveIdx[i]];
//                        var dtSpan = exitDt.Subtract(entryDt);
//                        var profitLoss = exitPrice - entryPrice;
//                        var profit = profitLoss > 0 ? profitLoss : double.NaN;
//                        var loss = profitLoss <= 0 ? profitLoss : double.NaN;
//                        accumBuy += profitLoss;

//                        _tradeTable.BuyTable.Add(new TradeRec(entryDt, entryPrice, exitDt, exitPrice, dtSpan, profitLoss, profit, loss, accumBuy));
//                  }
//                  return _tradeTable;
//                  //ローカル関数
//                  //ドローダウン計算
//            }

//            public List<PerformRec> PerformTable = new();

//            public List<PerformRec> CreatePerformanceTable() {
//                  //Sell
//                  if (_tradeTable.SellTable.Count != 0) {
//                        var CountSell = _tradeTable.SellTable.Count;
//                        var winCountSell = _tradeTable.SellTable.Count(x => x.ProfitLoss > 0);
//                        var loseCountSell = _tradeTable.SellTable.Count(x => x.ProfitLoss <= 0);
//                        var winSumSell = _tradeTable.SellTable.Where(x => x.ProfitLoss > 0).Sum(x => x.ProfitLoss);
//                        var LoseSumSell = _tradeTable.SellTable.Where(x => x.ProfitLoss <= 0).Sum(x => x.ProfitLoss);
//                        var winAvgSell = winCountSell != 0 ? _tradeTable.SellTable.Where(x => x.ProfitLoss > 0).Average(x => x.ProfitLoss) : double.NaN;
//                        var LoseAvgSell = loseCountSell != 0 ? _tradeTable.SellTable.Where(x => x.ProfitLoss <= 0).Average(x => x.ProfitLoss) : double.NaN;

//                        PerformTable.Add(new PerformRec(_tradeTable.Stmbol, "Sell", _tradeTable.SpaceNum, CountSell,
//                              _tradeTable.SellTable.Sum(x => x.ProfitLoss), _tradeTable.SellTable.Average(x => x.ProfitLoss),
//                              _tradeTable.SellTable.Max(x => x.ProfitLoss), _tradeTable.SellTable.Min(x => x.ProfitLoss),
//                              winCountSell / (double)CountSell * 100,
//                              LoseAvgSell != 0 ? winAvgSell / LoseAvgSell * -1 : double.NaN,
//                              LoseSumSell != 0 ? (double)winSumSell / (double)LoseSumSell * -1 : double.NaN,
//                              winCountSell, winSumSell, LoseSumSell, winAvgSell, LoseAvgSell));
//                  }
//                  //Buy
//                  if (_tradeTable.BuyTable.Count != 0) {
//                        var CountBuy = _tradeTable.BuyTable.Count;
//                        var winCountBuy = _tradeTable.BuyTable.Count(x => x.ProfitLoss > 0);
//                        var loseCountBuy = _tradeTable.BuyTable.Count(x => x.ProfitLoss <= 0);
//                        var winSumBuy = _tradeTable.BuyTable.Where(x => x.ProfitLoss > 0).Sum(x => x.ProfitLoss);
//                        var LoseSumBuy = _tradeTable.BuyTable.Where(x => x.ProfitLoss <= 0).Sum(x => x.ProfitLoss);
//                        var winAvgBuy = winCountBuy != 0 ? _tradeTable.BuyTable.Where(x => x.ProfitLoss > 0).Average(x => x.ProfitLoss) : double.NaN;
//                        var LoseAvgBuy = loseCountBuy != 0 ? _tradeTable.BuyTable.Where(x => x.ProfitLoss <= 0).Average(x => x.ProfitLoss) : double.NaN;

//                        PerformTable.Add(new PerformRec(_tradeTable.Stmbol, "Buy", _tradeTable.SpaceNum, CountBuy,
//                              _tradeTable.BuyTable.Sum(x => x.ProfitLoss), _tradeTable.BuyTable.Average(x => x.ProfitLoss),
//                              _tradeTable.BuyTable.Max(x => x.ProfitLoss), _tradeTable.BuyTable.Min(x => x.ProfitLoss),
//                              winCountBuy / (double)CountBuy * 100,
//                              LoseAvgBuy != 0 ? winAvgBuy / LoseAvgBuy * -1 : double.NaN,
//                              LoseSumBuy != 0 ? (double)winSumBuy / (double)LoseSumBuy * -1 : double.NaN,
//                              winCountBuy, winSumBuy, LoseSumBuy, winAvgBuy, LoseAvgBuy));
//                  }
//                  return PerformTable;
//            }
//      }

//}
