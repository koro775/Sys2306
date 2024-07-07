//using Sys2306.domain.Data;
//using Sys2306.domain.Tools;
//using Sys2306.domain.Trading.Entries;
//using Sys2306.domain.Trading.Exits;

//namespace Sys2306.domain.Trading.Trades
//{
//      public record TradeRec(DateTime EntryDt, DateTime ExitDt, TimeSpan DtSpan,
//                                          //double EntryPrice, double ExitPrice,
//                                          double ProfitLoss, double Win, double Lose,
//                                          double Fw, double Ad);

//      public class Trade
//      {
//            public List<TradeRec> ShortList = new();
//            public List<TradeRec> LongList = new();
//            protected readonly DB _db;
//            protected readonly Exit _exit;
//            protected OrderKind _shortEntryOrderKind;
//            protected OrderKind _longEntryOrderKind;

//            //コンストラクタ
//            public Trade(DB db, Exit exit) {
//                  _db = db;
//                  _exit = exit;
//                  (_shortEntryOrderKind, _longEntryOrderKind) = (exit.ShotEntryOrderKind, exit.LongEntryOrderKind);
//            }

//            public void Process() {

//                  Short();
//                  Long();
//            }

//            private void Short() {

//                  var (X, Dt, Open, High, Low, Close, Hspace, Lspace) = _db.D;
//                  var (EntrySellX, EntrySellY, ExitBuyX, ExitBuyY, ShortExitOrderKind) = _exit.ShortExitTable;

//                  DateTime entryDt, exitDt;
//                  TimeSpan span;
//                  double profitLoss, win, lose, fw, ad;

//                  for (int i = 0; i < _exit.ShortLen; i++) {
//                        entryDt = Dt[EntrySellX[i]];
//                        exitDt = Dt[ExitBuyX[i]];
//                        span = exitDt - entryDt;
//                        profitLoss = EntrySellY[i] - ExitBuyY[i];
//                        win = profitLoss > 0 ? profitLoss : double.NaN;
//                        lose = profitLoss <= 0 ? profitLoss : double.NaN;
//                        (fw, ad) = Functions2.GerShortFwAd(ref _shortEntryOrderKind, ref ShortExitOrderKind[i],
//                                                                                    Open.AsSpan(), High.AsSpan(), Low.AsSpan(), Close.AsSpan(),
//                                                                                    ref EntrySellX[i], ref EntrySellY[i], ref ExitBuyX[i], ref ExitBuyY[i]);

//                        ShortList.Add(new TradeRec(entryDt, exitDt, span, profitLoss, win, lose, fw, ad));
//                  }
//            }

//            private void Long() {

//                  var (X, Dt, Open, High, Low, Close, Hspace, Lspace) = _db.D;
//                  var (EntryBuyX, EntryBuyY, ExitSellX, ExitSellY, LongExitOrder) = _exit.LongExitTable;

//                  DateTime entryDt, exitDt;
//                  TimeSpan span;
//                  double profitLoss, win, lose, fw, ad;

//                  for (int i = 0; i < _exit.LongLen; i++) {
//                        entryDt = Dt[EntryBuyX[i]];
//                        exitDt = Dt[ExitSellX[i]];
//                        span = exitDt - entryDt;
//                        profitLoss = ExitSellY[i] - EntryBuyY[i];
//                        win = profitLoss > 0 ? profitLoss : double.NaN;
//                        lose = profitLoss <= 0 ? profitLoss : double.NaN;
//                        (fw, ad) = Functions2.GerLongFwAd(ref _longEntryOrderKind, ref LongExitOrder[i],
//                                                                                     Open.AsSpan(), High.AsSpan(), Low.AsSpan(), Close.AsSpan(),
//                                                                                    ref EntryBuyX[i], ref EntryBuyY[i], ref ExitSellX[i], ref ExitSellY[i]);

//                        LongList.Add(new TradeRec(entryDt, exitDt, span, profitLoss, win, lose, fw, ad));
//                  }
//            }
//      }
//}
