using Sys2306.domain.Statics;
using Sys2306.domain.Trading.Entries;

namespace Sys2306.domain.Tools
{
      public static class Functions2
      {
            //値型のコピー阻止。ref 渡しなので書き換え注意！
            public static (double Fw, double Ad) GerShortFwAd(ref OrderKind shortEntryOrder, ref OrderKind shortExitOrder,
                                                                  Span<double> Open, Span<double> High, Span<double> Low, Span<double> Close,
                                                                  ref int entrySellX, ref double entrySellY, ref int ExitBuyX, ref double ExitBuyY) {
                  ref int x = ref entrySellX;
                  ref int xx = ref ExitBuyX;
                  double max, min;
                  //(double max, double min)= GetMaxMin();

                  //ローカル関数
                        if (x == xx) {
                              max = High[x];
                              min = Low[x];
                        }
                        else if (xx - x == 1) {
                              max = Math.Max(High[x], High[x + 1]);
                              min = Math.Min(High[x], High[x + 1]);
                        }
                        else {
                              max = Functions.MaxSpan(High[(x + 1)..xx]);
                              min = Functions.MinSpan(Low[(x + 1)..xx]);
                        }

                  //Entry
                  if (shortEntryOrder == OrderKind.Open) {
                        max = High[x] > max ? High[x] : max;
                        min = Low[x] < min ? Low[x] : min;
                  }
                  else if (shortEntryOrder == OrderKind.Limit) {
                        max = High[x] > max ? High[x] : max;
                        min = Close[x] < min ? Close[x] : min;
                  }
                  else if (shortEntryOrder == OrderKind.Stop) {
                        max = Close[x] > max ? Close[x] : max;
                        min = Low[x] < min ? Low[x] : min;
                  }
                  else throw new Exception("ShortEntryエラー");

                  //Exit
                  if (shortExitOrder == OrderKind.Open) {
                        max = Open[xx] > max ? Open[xx] : max;
                        min = Open[xx] < min ? Open[xx] : min;
                  }
                  else if (shortExitOrder == OrderKind.Limit) {
                        max = ExitBuyY > max ? ExitBuyY : max;
                        min = ExitBuyY < min ? ExitBuyY : min;
                  }
                  else if (shortExitOrder == OrderKind.Stop) {
                        max = ExitBuyY > max ? ExitBuyY : max;
                        min = ExitBuyY < min ? ExitBuyY : min;
                  }
                  else throw new Exception("ShortExitエラー");

                  return (entrySellY - min, entrySellY - max);     //Adはマイナスにしてる
            }


            public static (double Fw, double Ad) GerLongFwAd(ref OrderKind longEntryOrder, ref OrderKind longExitOrder,
                                                      Span<double> Open, Span<double> High, Span<double> Low, Span<double> Close,
                                                      ref int entryBuyX, ref double entryBuyY, ref int exitSellX, ref double exitSellY) {
                  ref int x = ref entryBuyX;
                  ref int xx = ref exitSellX;
                  double max, min;

                  //ローカル関数
                  if (x == xx) {
                        max = High[x];
                        min = Low[x];
                  }
                  else if (xx - x == 1) {
                        max = Math.Max(High[x], High[x + 1]);
                        min = Math.Min(High[x], High[x + 1]);
                  }
                  else {
                        max = Functions.MaxSpan(High[(x + 1)..xx]);
                        min = Functions.MinSpan(Low[(x + 1)..xx]);
                  }

                  //double max = x == xx ? High[x] : Functions.MaxSpan(High[(x + 1)..xx]);
                  //double min = x == xx ? Low[x] : Functions.MinSpan(Low[(x + 1)..xx]);

                  //Entry
                  if (longEntryOrder == OrderKind.Open) {
                        max = High[x] > max ? High[x] : max;
                        min = Low[x] < min ? Low[x] : min;
                  }
                  else if (longEntryOrder == OrderKind.Limit) {
                        max = Close[x] > max ? Close[x] : max;
                        min = Low[x] < min ? Low[x] : min;
                  }
                  else if (longEntryOrder == OrderKind.Stop) {
                        max = High[x] > max ? High[x] : max;
                        min = Close[x] < min ? Close[x] : min;
                  }
                  else throw new Exception("LongEntryエラー");

                  //Exit
                  if (longEntryOrder == OrderKind.Open) {
                        max = Open[xx] > max ? Open[xx] : max;
                        min = Open[xx] < min ? Open[xx] : min;
                  }
                  else if (longEntryOrder == OrderKind.Limit) {
                        max = exitSellY > max ? exitSellY : max;
                        min = exitSellY < min ? exitSellY : min;
                  }
                  else if (longEntryOrder == OrderKind.Stop) {
                        max = exitSellY > max ? exitSellY : max;
                        min = exitSellY < min ? exitSellY : min;
                  }
                  else throw new Exception("LongExitエラー");

                  return (max - entryBuyY, min - entryBuyY);     //Adはマイナスにしてる
            }
      }
}