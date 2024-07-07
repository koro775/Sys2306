namespace Sys2306.domain.Data
{
      //var(PosNeg, TbX, TbY, StartX, StartY, EndX, EndY, SpaceNum, BrkX, BrkY) = _wave.W;
#nullable disable
      public class Wave
      {
            private readonly DB _db;
            private readonly TB _tb;
            private List<int> _tbXList;

            //コンストラクター
            public Wave(DB db, TB tb) {

                  _db = db;
                  _tb = tb;
                  Len = tb.Tb.Tb.Count(x => x != 0); ;
                  W = (new int[Len], new bool[Len], new int[Len], new double[Len], new int[Len], new double[Len], new int[Len], new double[Len],
                          new int[Len], new int[Len], new double[Len]);
            }

            //Wavデータテーブル
            public (int[] Idx, bool[] PosNeg, int[] TbX, double[] TbY, int[] StartX, double[] StartY, int[] EndX, double[] EndY,
                               int[] SpaceNum, int[] BrkX, double[] BrkY) W { get; set; }

            public int Len { get; }


            public void Process() {
                  SetWav();
                  SetSpace(W.TbY.AsSpan(), W.SpaceNum.AsSpan());
                  _tbXList = W.TbX.ToList();
            }

            public int GetWaveIdx(int dbX) => _tbXList.FindIndex(x => x > dbX);

            private void SetWav() {

                  int tbIdx = 0;
                  int chgIdx = 0;

                  var (_, _, Open, High, Low, _, _, _) = _db.D;
                  var (Tb, Chg) = _tb.Tb;

                  for (var i = 0; i < _db.Len; i++) {

                        //TBの０以外（配列に値が入っていれば）のインデックスとプライスを配列代入
                        if (Tb[i] > 0f) {
                              W.Idx[tbIdx] = tbIdx;     //0スタート
                              W.TbX[tbIdx] = i;
                              W.TbY[tbIdx] = Tb[i];
                              tbIdx++;
                        }
                        //Chgの０以外（配列に値が入っていれば）のインデックスと
                        //転換時の陽線なら高値、陰線なら安値を配列代入
                        if (Chg[i] > 0f) {

                              W.StartX[chgIdx] = chgIdx != 0 ? W.EndX[chgIdx - 1] + 1 : 0;
                              W.StartY[chgIdx] = chgIdx != 0 ? Open[W.StartX[chgIdx]] : Open[0];
                              W.EndX[chgIdx] = i;
                              W.EndY[chgIdx] = Chg[i];
                              W.PosNeg[chgIdx] = W.TbY[chgIdx] > W.EndY[chgIdx];

                              //W.BrkX[chgIdx] /  W.BrkY[chgIdx]を求める***********************
                              if (chgIdx >= 2) {
                                    //陽線で前Topを上抜いたら
                                    if (W.PosNeg[chgIdx] && W.TbY[chgIdx - 2] < W.TbY[chgIdx]) {
                                          //Dataテーブル検索
                                          var highidx = -1;   //もしもに備えて-1で初期化
                                          for (int j = W.StartX[chgIdx]; j < W.EndX[chgIdx]; j++) {
                                                if (High[j] > W.TbY[chgIdx - 2]) {
                                                      highidx = j;
                                                      break;
                                                }
                                          }
                                          W.BrkX[chgIdx] = highidx;
                                          W.BrkY[chgIdx] = W.TbY[chgIdx - 2];
                                    }
                                    //陰線で前Bottomを下抜いたら
                                    if (!W.PosNeg[chgIdx] && W.TbY[chgIdx - 2] > W.TbY[chgIdx]) {
                                          //Dataテーブル検索
                                          var lowidx = -1;   //もしもに備えて-1で初期化
                                          for (int j = W.StartX[chgIdx]; j < W.EndX[chgIdx]; j++) {
                                                if (Low[j] < W.TbY[chgIdx - 2]) {
                                                      lowidx = j;
                                                      break;
                                                }
                                          }
                                          W.BrkX[chgIdx] = lowidx;
                                          W.BrkY[chgIdx] = W.TbY[chgIdx - 2];
                                    }
                              }
                              //*************************************************************
                              chgIdx++;
                        }
                  }
            }

            private void SetSpace(Span<double> tby, Span<int> spaceNum) {

                  for (int k = 1; k < Len; k++) {           //K=1に注意

                        //上昇波動
                        if (tby[k - 1] < tby[k]) {
                              spaceNum[k] = (int)Math.Floor(HSpaceCount(k, tby) / 2);
                        }
                        //下降波動
                        if (tby[k - 1] > tby[k]) {
                              spaceNum[k] = (int)Math.Floor(LSpaceCount(k, tby) / 2);
                        }
                  }

                  //--------Method
                  double HSpaceCount(int seq, Span<double> h) {     //暗黙にint -> doubleに変換

                        for (int i = seq, ii = 0; i >= 0; i--, ii++) {
                              if (i != 0 && h[i - 1] >= h[seq]) return ii;
                        }
                        return seq;
                  }

                  double LSpaceCount(int seq, Span<double> l) {
                        for (int i = seq, ii = 0; i >= 0; i--, ii++) {
                              if (i != 0 && l[i - 1] <= l[seq]) return ii;
                        }
                        return seq;
                  }
            }
      }
}
