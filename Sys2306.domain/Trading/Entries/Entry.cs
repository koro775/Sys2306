using Sys2306.domain.Data;

namespace Sys2306.domain.Trading.Entries
{
      public enum EntryKind
      {
            No1,
            No2,
            No3,
            No4,
      }

      public enum OrderKind
      {
            Open,
            Limit,
            Stop,
      }

      public abstract class Entry
      {
            protected DB _db;
            protected Wave _wave;

            //コンストラクタ
            public Entry(DB db, Wave wave) {
                  _db = db;
                  _wave = wave;
            }

            public (List<int> SellX, List<double> SellY) ShortEntry = (new List<int>(), new List<double>());
            public (List<int> BuyX, List<double> BuyY) LongEntry = (new List<int>(), new List<double>());
            public int SellLen { get; set; }
            public int BuyLen { get; set; }
            public abstract OrderKind EntryOrderKind { get; }

            //Factory
            public static Entry CreateEntry(EntryKind m, DB db, Wave wave) {
                  if (m == EntryKind.No3) return new Entry3(db, wave);
                  //else if (m == Method.No1) return new EntryX();
                  return new Entry4(db, wave);
            }

            //テンプレート
            public void SetEntryPoints() {

                  for (var i = 0; i < _wave.Len; i++) {
                        if (SellJudge(i)) SetXY(i, false);  //false = 売り
                        if (BuyJudge(i)) SetXY(i, true);  //true  = 買い
                  }

                  SellLen = ShortEntry.SellX.Count;
                  BuyLen = LongEntry.BuyX.Count;
            }

            protected abstract bool BuyJudge(int i);
            protected abstract bool SellJudge(int i);
            protected abstract void SetXY(int i, bool posneg);
      }

      public sealed class Entry3 : Entry
      {
            //コンストラクタ
            public Entry3(DB db, Wave wave) : base(db, wave) { }

            public override OrderKind EntryOrderKind => OrderKind.Open;

            protected override bool SellJudge(int i) {
                  var (Idx, PosNeg, _, TbY, _, StartY, _, _, SpaceNum, _, _) = _wave.W;
                  return 5 <= i &&
                             PosNeg[i] == true &&
                             //SpaceNum[i - 2] < 100 &&
                             //TbY[i - 2] > TbY[i] &&                        //前高値を抜かない場合
                             TbY[i - 3] < TbY[i - 1] &&
                             TbY[i - 4] < TbY[i - 1] &&
                             TbY[i - 4] < TbY[i - 2] &&
                             TbY[i - 5] < TbY[i - 3];
            }

            protected override bool BuyJudge(int i) {
                  var (Idx, PosNeg, _, TbY, _, StartY, _, _, SpaceNum, _, _) = _wave.W;
                  return 5 <= i &&
                             PosNeg[i] == false &&
                             //SpaceNum[i - 2] < 100 &&
                             //TbY[i - 2] < TbY[i] &&                        //前安値を抜かない場合
                             TbY[i - 3] > TbY[i - 1] &&
                             TbY[i - 4] > TbY[i - 1] &&
                             TbY[i - 4] > TbY[i - 2] &&
                             TbY[i - 5] > TbY[i - 3];

            }

            protected override void SetXY(int i, bool posneg) {

                  if (!posneg) {
                        ShortEntry.SellX.Add(_wave.W.StartX[i]);
                        ShortEntry.SellY.Add(_wave.W.StartY[i]);
                  }

                  if (posneg) {
                        LongEntry.BuyX.Add(_wave.W.StartX[i]);
                        LongEntry.BuyY.Add(_wave.W.StartY[i]);
                  }
            }
      }

      //Gap逆張り
      public sealed class Entry4 : Entry
      {
            //コンストラクタ
            public Entry4(DB db, Wave wave) : base(db, wave) { }

            public override OrderKind EntryOrderKind => OrderKind.Limit;

            protected override bool SellJudge(int i) {

                  var (Idx, PosNeg, _, TbY, _, _, _, _, _, _, _) = _wave.W;
                  return 4 <= i &&
                             PosNeg[i] == true &&
                             TbY[i - 4] < TbY[i - 1] &&
                             TbY[i - 2] <= TbY[i - 0];
            }

            protected override bool BuyJudge(int i) {
                  var (Idx, PosNeg, _, TbY, _, _, _, _, _, _, _) = _wave.W;
                  return 4 <= i &&
                             PosNeg[i] == false &&
                             TbY[i - 4] > TbY[i - 1] &&
                             TbY[i - 2] >= TbY[i - 0];
            }

            protected override void SetXY(int i, bool posneg) {

                  var (X, Dt, Open, High, Low, Close, Hspace, Lspace) = _db.D;
                  var (Idx, PosNeg, TbX, TbY, StartX, StartY, EndX, EndY, SpaceNum, BrkX, BrkY) = _wave.W;

                  if (!posneg) {
                        for (int j = StartX[i]; j < EndX[i]; j++) {
                              if (High[j] >= TbY[i - 2]) {
                                    ShortEntry.SellX.Add(j);
                                    ShortEntry.SellY.Add(TbY[i - 2]);
                                    break;
                              }
                        }
                  }
                  if (posneg) {
                        for (int j = StartX[i]; j < EndX[i]; j++) {
                              if (Low[j] <= TbY[i - 2]) {
                                    LongEntry.BuyX.Add(j);
                                    LongEntry.BuyY.Add(TbY[i - 2]);
                                    break;
                              }
                        }
                  }
            }
      }
}
