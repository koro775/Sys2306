using Sys2306.domain.Data;

namespace Sys2306.domain.Trading.Entries
{
      //var (WaveIdx,EntryBuyX,EntryBuyY) =_e.BuyEntry
      //var (WaveIdx,EntrySellX,EntrySellY) =_e.SellEntry
      public abstract class EntryOld
      {
            public (List<int> BuyWaveIdx, List<int> EntryBuyX, List<double> EntryBuyY) BuyEntry = (new List<int>(), new List<int>(), new List<double>());
            public (List<int> SellWaveIdx, List<int> EntrySellX, List<double> EntrySellY) SellEntry = (new List<int>(), new List<int>(), new List<double>());

            protected DB _db;
            protected Wave _wave;

            //コンストラクタ
            public EntryOld(DB db, Wave wave) {
                  _db = db;
                  _wave = wave;
            }

            public void SetEntryPoints() {
                  int ii = 0;                               //Entryのカウント
                  for (var i = 0; i < _wave.Len; i++) {
                        if (SellJudge(i)) SetXY(i, false);  //false = 売り
                        if (BuyJudge(i)) SetXY(i, true);  //true  = 買い
                        ii++;
                  }
            }
            protected abstract bool BuyJudge(int i);
            protected abstract bool SellJudge(int i);
            protected abstract void SetXY(int i, bool posneg);
      }

      public sealed class Entry1 : EntryOld
      {
            public Entry1(DB db, Wave wave) : base(db, wave) { }
            //var(PosNeg, TbX, TbY, StartX, StartY, EndX, EndY, SpaceNum, BrkX, BrkY) = _wave.W;

            protected override bool SellJudge(int i) {
                  var (Idx, PosNeg, _, TbY, _, StartY, _, _, SpaceNum, _, _) = _wave.W;
                  return 5 <= i &&
                             PosNeg[i] == true &&
                             SpaceNum[i - 2] < 100 &&
                             TbY[i - 2] > TbY[i] &&                        //前高値を抜かない場合
                             TbY[i - 3] < TbY[i - 1] &&
                             TbY[i - 4] < TbY[i - 1] &&
                             TbY[i - 4] < TbY[i - 2] &&
                             TbY[i - 5] < TbY[i - 3];
            }

            protected override bool BuyJudge(int i) {
                  var (Idx, PosNeg, _, TbY, _, StartY, _, _, SpaceNum, _, _) = _wave.W;
                  return 5 <= i &&
                             PosNeg[i] == false &&
                             SpaceNum[i - 2] < 100 &&
                             TbY[i - 2] < TbY[i] &&                        //前安値を抜かない場合
                             TbY[i - 3] > TbY[i - 1] &&
                             TbY[i - 4] > TbY[i - 1] &&
                             TbY[i - 4] > TbY[i - 2] &&
                             TbY[i - 5] > TbY[i - 3];

            }


            protected override void SetXY(int i, bool posneg) {

                  if (!posneg) {
                        //if (D.Close[W.EndX[i - 1]] > W.TbY[i - 2]) {
                        SellEntry.SellWaveIdx.Add(i);
                        SellEntry.EntrySellX.Add(_wave.W.StartX[i]);
                        SellEntry.EntrySellY.Add(_wave.W.StartY[i]);
                        //}
                  }
                  if (posneg) {
                        //if (D.Close[W.EndX[i - 1]] < W.TbY[i - 2]) {
                        BuyEntry.BuyWaveIdx.Add(i);                     //WaveテーブルのIndex
                        BuyEntry.EntryBuyX.Add(_wave.W.StartX[i]);
                        BuyEntry.EntryBuyY.Add(_wave.W.StartY[i]);
                        //}
                  }

            }
      }

      //Gap逆張り
      public sealed class Entry2 : EntryOld
      {
            public Entry2(DB db, Wave wave) : base(db, wave) { }
            //var(PosNeg, TbX, TbY, StartX, StartY, EndX, EndY, SpaceNum, BrkX, BrkY) = _wave.W;

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
                                    SellEntry.SellWaveIdx.Add(i);
                                    SellEntry.EntrySellX.Add(j);
                                    SellEntry.EntrySellY.Add(TbY[i - 2]);
                                    break;
                              }
                        }
                  }
                  if (posneg) {
                        for (int j = StartX[i]; j < EndX[i]; j++) {
                              if (Low[j] <= TbY[i - 2]) {
                                    BuyEntry.BuyWaveIdx.Add(i);                     //WaveテーブルのIndex
                                    BuyEntry.EntryBuyX.Add(j);
                                    BuyEntry.EntryBuyY.Add(TbY[i - 2]);
                                    break;
                              }
                        }
                  }
            }
      }
}
