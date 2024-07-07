using Sys2306.domain.Data;
using Sys2306.domain.Trading.Entries;
using Sys2306.domain.Trading.Exits;

namespace Sys2306.domain.Tools
{
      public record DbRec(double X, DateTime Dt, double Open, double High, double Low, double Close,
                                       int Hspace, int Lspace);

      public record TbRec(double Tb, double Chg);

      public record WaveRec(int Idxx, bool PosNeg, int TbX, double TbY, int StartX, double StartY,
                          int EndX, double EndY, int SpaceNum, int BrkX, double BrkY);

      public record ShortEntryRec(int EntrySellX, double EntrySellY);
      public record LongEntryRec(int EntryBuyX, double EntryBuyY);

      public record ShortExitRec(int EntrySellX, double EntrySellY, int ExitBuyX, double ExitBuyY, OrderKind ShortExitOrderKind);
      public record LonfExitRec(int EntryBuyX, double EntryBuyY, int ExitSellX, double ExitSellY, OrderKind LongExitOrderKind);

#nullable disable
      public abstract class ListConvert
      {
            public virtual List<DbRec> DbList { get; set; }
            public virtual List<TbRec> TbList { get; set; }
            public virtual List<WaveRec> WaveList { get; set; }
            public virtual List<ShortEntryRec> ShortEntry { get; set; }
            public virtual List<LongEntryRec> LongEntry { get; set; }
            public virtual List<ShortExitRec> ShortExitList { get; set; }
            public virtual List<LonfExitRec> LongExitList { get; set; }

            public abstract void Convert();
      }

      public class ConvertDb : ListConvert
      {
            readonly DB _db;

            //コンストラクタ
            public ConvertDb(DB db) {
                  _db = db;
                  Convert();
            }
            public override List<DbRec> DbList { get; set; } = new();

            public override void Convert() {

                  var (X, Dt, Open, High, Low, Close, Hspace, Lspace) = _db.D;
                  for (int i = 0; i < _db.Len; i++) {
                        DbList.Add(new DbRec(
                                      X[i],
                                      Dt[i],
                                      Open[i],
                                      High[i],
                                      Low[i],
                                      Close[i],
                                      Hspace[i],
                                      Lspace[i]
                              ));
                  }
            }
      }

      public class ConvertTB : ListConvert
      {
            readonly TB _tb;

            //コンストラクタ
            public ConvertTB(TB tb) {
                  _tb = tb;
                  Convert();
            }
            public override List<TbRec> TbList { get; set; } = new();

            public override void Convert() {

                  var (Tb, Chg) = _tb.Tb;

                  for (int i = 0; i < _tb.Len; i++) {
                        TbList.Add(new TbRec(Tb[i], Chg[i])); ;
                  }
            }

            public class ConvertWave : ListConvert
            {
                  readonly Wave _wave;

                  //コンストラクタ
                  public ConvertWave(Wave wave) {
                        _wave = wave;
                        Convert();
                  }
                  public override List<WaveRec> WaveList { get; set; } = new();

                  public override void Convert() {

                        var (Idx, PosNeg, TbX, TbY, StartX, StartY, EndX, EndY, SpaceNum, BrkX, BrkY) = _wave.W;

                        for (int i = 0; i < _wave.Len; i++) {
                              WaveList.Add(new WaveRec(
                                          Idx[i],
                                           PosNeg[i],
                                            TbX[i],
                                            TbY[i],
                                            StartX[i],
                                            StartY[i],
                                            EndX[i],
                                            EndY[i],
                                            SpaceNum[i],
                                            BrkX[i],
                                            BrkY[i]
                                    ));
                        }
                  }
            }
      }

      public class ConvertEntry : ListConvert
      {
            readonly Entry _entry;

            //コンストラクタ
            public ConvertEntry(Entry entry) {
                  _entry = entry;
                  Convert();
            }
            public override List<ShortEntryRec> ShortEntry { get; set; } = new();
            public override List<LongEntryRec> LongEntry { get; set; } = new();

            public override void Convert() {

                  //sell
                  var (EntrySellX, EntrySellY) = _entry.ShortEntry;
                  var (EntryBuyX, EntryBuyY) = _entry.LongEntry;

                  for (int i = 0; i < EntrySellX.Count; i++) {
                        ShortEntry.Add(new ShortEntryRec(
                                      EntrySellX[i],
                                      EntrySellY[i]
                         ));
                  }

                  //buy
                  for (int i = 0; i < EntryBuyX.Count; i++) {
                        LongEntry.Add(new LongEntryRec(
                                      EntryBuyX[i],
                                      EntryBuyY[i]
                         ));
                  }

            }
      }

      public class ConvertExit : ListConvert
      {
            readonly Exit _exit;

            //コンストラクタ
            public ConvertExit(Exit exit) {
                  _exit = exit;
                  Convert();
            }

            public override List<ShortExitRec> ShortExitList { get; set; } = new();
            public override List<LonfExitRec> LongExitList { get; set; } = new();

            public override void Convert() {

                  var (EntrySellX, EntrySellY, ExitBuyX, ExitBuyY, ShortExitOrderKind) = _exit.ShortExitTable;
                  var (EntryBuyX, EntryBuyY, ExitSellX, ExitSellY, LongExitOrderKind) = _exit.LongExitTable;

                  //sell
                  for (int i = 0; i < _exit.ShortLen; i++) {
                        ShortExitList.Add(new ShortExitRec(
                                                      EntrySellX[i],
                                                      EntrySellY[i],
                                                      ExitBuyX[i],
                                                      ExitBuyY[i],
                                                      ShortExitOrderKind[i]
                                      ));
                  }

                  //buy
                  for (int i = 0; i < _exit.LongLen; i++) {
                        LongExitList.Add(new LonfExitRec(
                                                      EntryBuyX[i],
                                                      EntryBuyY[i],
                                                      ExitSellX[i],
                                                      ExitSellY[i],
                                                      LongExitOrderKind[i]
                                      ));
                  }
            }
      }
}
