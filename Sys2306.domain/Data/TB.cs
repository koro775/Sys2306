namespace Sys2306.domain.Data

{
      public sealed class TB
      //  var (Tb, Chg)=_tb.Tb;
      {
            //コンストラクター
            public TB(int spaceNum, DB db) {
                  SpaceNum = spaceNum;
                  Tb = (new double[db.Len], new double[db.Len]);
                  Db = db;
                  Len = db.Len;
            }

            //TopBottomデータ
            public (double[] Tb, double[] Chg) Tb { get; set; }
            public int Len { get; set; }

            internal DB Db { get; set; }
            internal (int id, double price, bool flg) Top = (0, double.MinValue, false);                  // flg : Topが高値を更新した場合true
            internal (int id, double price, bool flg) Btm = (0, double.MaxValue, false);                 // flg : Btmが安値を更新した場合true
            internal (int id, double h, double l, double c, int hspace, int lspace) rec;                    //  DBからの読込record
            internal int SpaceNum;
            IStateTB _state = NonStateTB.Instance;

            public void SetTbData() {

                  var (_, _, _, High, Low, Close, Hspace, Lspace) = Db.D;

                  for (int i = 0; i < Db.Len; i++) {
                        Calc(i, High[i], Low[i], Close[i], Hspace[i], Lspace[i]);
                  }
            }

            private void Calc(int id, double h, double l, double c, int hspace, int lspace) {
                  rec = (id, h, l, c, hspace, lspace);

                  if (Top.price <= rec.h) Top = (rec.id, rec.h, true);
                  else Top = (Top.id, Top.price, false);

                  if (Btm.price >= rec.l) Btm = (rec.id, rec.l, true);
                  else Btm = (Btm.id, Btm.price, false);

                  _state.Command(this);
            }
            internal void ChangeState(IStateTB state) {
                  _state = state;
            }
      }

      public interface IStateTB
      {
            void Command(TB ct);
      }

      internal class NonStateTB : IStateTB
      {
            //シングルトン
            private NonStateTB() { }
            internal static NonStateTB Instance { get; } = new NonStateTB();

            public void Command(TB ct) {

                  var (_, _, _, High, Low, _, _, _) = ct.Db.D;
                  var (Tb, Chg) = ct.Tb;

                  //陽転
                  if (!ct.Btm.flg && ct.rec.hspace >= ct.SpaceNum) {

                        //Btm書き込み
                        Tb[ct.Btm.id] = ct.Btm.price;
                        //転換高値書き込み
                        Chg[ct.rec.id] = High[ct.rec.id];

                        //陽転処理
                        ct.Top = (ct.rec.id, ct.rec.h, true);
                        ct.ChangeState(PosStateTB.Instance);
                  }

                  //陰転
                  if (!ct.Top.flg && ct.rec.lspace >= ct.SpaceNum) {

                        //Top書き込み
                        Tb[ct.Top.id] = ct.Top.price;

                        //転換安値書き込み
                        Chg[ct.rec.id] = Low[ct.rec.id];

                        //陰転処理
                        ct.Btm = (ct.rec.id, ct.rec.l, true);
                        ct.ChangeState(NegStateTB.Instance);
                  }
            }
      }

      public class PosStateTB : IStateTB
      {
            //シングルトン
            private PosStateTB() { }
            internal static PosStateTB Instance { get; } = new PosStateTB();

            public void Command(TB ct) {

                  var (_, _, _, High, Low, _, _, _) = ct.Db.D;
                  var (Tb, Chg) = ct.Tb;

                  //陰転
                  if (!ct.Top.flg && ct.rec.lspace >= ct.SpaceNum) {

                        //Top書き込み
                        Tb[ct.Top.id] = ct.Top.price;

                        //転換安値書き込み
                        Chg[ct.rec.id] = Low[ct.rec.id];

                        //陰転処理
                        ct.Btm = (ct.rec.id, ct.rec.l, true);
                        ct.ChangeState(NegStateTB.Instance);
                  }
            }
      }

      public class NegStateTB : IStateTB
      {
            private NegStateTB() { }
            internal static NegStateTB Instance { get; } = new NegStateTB();
            public void Command(TB ct) {

                  var (_, _, _, High, Low, _, _, _) = ct.Db.D;
                  var (Tb, Chg) = ct.Tb;

                  //陽転
                  if (!ct.Btm.flg && ct.rec.hspace >= ct.SpaceNum) {

                        //Btm書き込み
                        Tb[ct.Btm.id] = ct.Btm.price;

                        //転換高値書き込み
                        Chg[ct.rec.id] = High[ct.rec.id];

                        //陽転処理
                        ct.Top = (ct.rec.id, ct.rec.h, true);
                        ct.ChangeState(PosStateTB.Instance);
                  }
            }
      }
}
