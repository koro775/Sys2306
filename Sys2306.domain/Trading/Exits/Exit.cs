using Sys2306.domain.Data;
using Sys2306.domain.Trading.Entries;

namespace Sys2306.domain.Trading.Exits
{
      //ShortExitOrderKindKind
#nullable disable
      public abstract class Exit
      {
            protected readonly DB _db;
            protected readonly Wave _wave;
            protected readonly Entry _entry;

            //Exitテーブル
            public (int[] EntrySellX, double[] EntrySellY, int[] ExitBuyX, double[] ExitBuyY, OrderKind[] ShortExitOrderKind) ShortExitTable { get; set; }
            public (int[] EntryBuyX, double[] EntryBuyY, int[] ExitSellX, double[] ExitSellY, OrderKind[] LongExitOrderKind) LongExitTable { get; set; }

            public string Symbol { get; }
            public int SpaceNum { get; }
            public int ShortLen { get; }
            public int LongLen { get; }
            public OrderKind ShotEntryOrderKind { get; }
            public OrderKind LongEntryOrderKind { get; }


            //コンストラクタ
            public Exit(string symbol, int spaceNum, DB db, Wave wave, Entry entry) {

                  (Symbol, SpaceNum, _db, _wave, _entry) = (symbol, spaceNum, db, wave, entry);
                  (ShortLen, LongLen) = (entry.SellLen, entry.BuyLen);
                  ShotEntryOrderKind = LongEntryOrderKind = entry.EntryOrderKind;

                  ShortExitTable = (new int[ShortLen], new double[ShortLen], new int[ShortLen], new double[ShortLen], new OrderKind[ShortLen]);
                  LongExitTable = (new int[LongLen], new double[LongLen], new int[LongLen], new double[LongLen], new OrderKind[LongLen]);
            }

            protected abstract (int, double, OrderKind) GetShortExit(int i);
            protected abstract (int, double, OrderKind) GetLongExit(int i);

            public void SetExitPoints() {

                  var (SellX, SellY) = _entry.ShortEntry;
                  var (BuyX, BuyY) = _entry.LongEntry;
                  var (EntrySellX, EntrySellY, ExitBuyX, ExitBuyY, ShortExitOrderKind) = ShortExitTable;
                  var (EntryBuyX, EntryBuyY, ExitSellX, ExitSellY, LongExitOrder) = LongExitTable;

                  //sell
                  for (int i = 0; i < ShortLen; i++) {

                        (EntrySellX[i], EntrySellY[i]) = (SellX[i], SellY[i]);
                        (ExitBuyX[i], ExitBuyY[i], ShortExitOrderKind[i]) = GetShortExit(i);
                  }
                  //buy
                  for (int i = 0; i < LongLen; i++) {

                        (EntryBuyX[i], EntryBuyY[i]) = (BuyX[i], BuyY[i]);
                        (ExitSellX[i], ExitSellY[i], LongExitOrder[i]) = GetLongExit(i);
                  }
            }
      }

      public sealed class Exit1 : Exit
      {
            //コンストラクタ
            public Exit1(string symbol, int spaceNum, DB db, Wave wave, Entry entry) : base(symbol, spaceNum, db, wave, entry) { }

            public int ExitPeriod = 60;

            protected override (int, double, OrderKind) GetShortExit(int i) {
                  var ans = _entry.ShortEntry.SellX[i] + ExitPeriod;
                  ans = ans < _db.Len ? ans : _db.Len - 1;    //ExitがDB範囲を超えていたら最終取得データを返す
                  return (ans, _db.D.Open[ans], OrderKind.Open);
            }


            protected override (int, double, OrderKind) GetLongExit(int i) {
                  var ans = _entry.LongEntry.BuyX[i] + ExitPeriod;
                  ans = ans < _db.Len ? ans : _db.Len - 1;    //ExitがDB範囲を超えていたら最終取得データを返す
                  return (ans, _db.D.Open[ans], OrderKind.Open);
            }
      }


      public sealed class Exit2 : Exit
      {
            //コンストラクタ
            public Exit2(string symbol, int spaceNum, DB db, Wave wave, Entry entry) : base(symbol, spaceNum, db, wave, entry) { }

            public int ExitPeriod = 60;
            public double Limit = 0.086027093;

            protected override (int, double, OrderKind) GetShortExit(int i) {

                  double exitBuyY;
                  (int entryX, double entryY) = (_entry.ShortEntry.SellX[i], _entry.ShortEntry.SellY[i]);
                  int timestopX = entryX + ExitPeriod < _db.Len ? entryX + ExitPeriod : _db.Len - 1;    //ExitがDB範囲を超えていたら最終取得データを返す

                  for (int x = entryX; x < timestopX; x++) {
                        if (_db.D.Low[x] < entryY - Limit) {
                              exitBuyY = _db.D.Open[x] < entryY - Limit ? _db.D.Open[x] : entryY - Limit;
                              return (x, exitBuyY, OrderKind.Limit);
                        }
                  }
                  var exitBuyX = timestopX;
                  return (exitBuyX, _db.D.Open[exitBuyX], OrderKind.Open);
            }


            protected override (int, double, OrderKind) GetLongExit(int i) {

                  double exitSellY;
                  (int entryX, double entryY) = (_entry.LongEntry.BuyX[i], _entry.LongEntry.BuyY[i]);
                  int timestopX = entryX + ExitPeriod < _db.Len ? entryX + ExitPeriod : _db.Len - 1;    //ExitがDB範囲を超えていたら最終取得データを返す

                  for (int x = entryX; x < timestopX; x++) {
                        if (_db.D.High[x] > entryY + Limit) {
                              exitSellY = _db.D.Open[x] > entryY + Limit ? _db.D.Open[x] : entryY + Limit;
                              return (x, exitSellY, OrderKind.Limit);
                        }
                  }

                  var ExitSellX = timestopX;
                  return (ExitSellX, _db.D.Open[ExitSellX], OrderKind.Open);
            }
      }
}


