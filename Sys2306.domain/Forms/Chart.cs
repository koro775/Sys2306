using ScottPlot;
using Sys2306.domain.Data;
using Sys2306.domain.Statics;
using Sys2306.domain.Trading.Exits;

namespace Sys2306.domain.Forms
{
      public partial class Chart : Form
      {
            private readonly Plot _plot;
            private readonly OHLC[] _ohlcs;
            private readonly double[]? _x;     //X軸
            private readonly DB _db;
            private readonly TB _tb;
            private readonly Wave _wave;
            private readonly Exit _exit;

            //コンストラクタ
            public Chart(DB db, TB tb, Wave wave, Exit exit) {

                  InitializeComponent();
                  StartPosition = FormStartPosition.CenterScreen;

                  _plot = formsPlot1.Plot;
                  _ohlcs = new OHLC[db.Len];
                  _db = db;
                  _x = db.D.X;
                  _tb = tb;
                  _wave = wave;
                  _exit = exit;
                  Draw();
            }

            private void Draw() {

                  var (X, Dt, Open, High, Low, Close, Hspace, Lspace) = _db.D;
                  var (Idx, PosNeg, TbX, TbY, StartX, StartY, EndX, EndY, SpaceNum, BrkX, BrkY) = _wave.W;
                  var (EntrySellX, EntrySellY, ExitBuyX, ExitBuyY, _) = _exit.ShortExitTable;
                  var (EntryBuyX, EntryBuyY, ExitSellX, ExitSellY, _) = _exit.LongExitTable;

                  CandleDraw();
                  WaveLineDraw();
                  ChangePointDraw();
                  SpaceNumDraw();
                  TradeDraw();


                  //目盛りのフォント設定
                  _plot.YAxis.TickLabelStyle(fontSize: 18);
                  _plot.XAxis.TickLabelStyle(fontSize: 18);


                  // 再描画
                  formsPlot1.Render();

                  //ローソク足描画-------------------------------------------------------------------
                  void CandleDraw() {

                        for (int i = 0; i < _db.Len; i++) {
                              _ohlcs[i] = new OHLC(Open[i], High[i], Low[i], Close[i], i, 1);
                        }
                        var candlePlot = _plot.AddCandlesticks(_ohlcs); // データ列
                        candlePlot.WickColor = Color.Black;          //多分ひげの色
                        candlePlot.ColorDown = Color.Black;       // 陰線の色
                        candlePlot.ColorUp = Color.White;           // 陽線の色
                  }

                  //波動線描画-------------------------------------------------------------------
                  void WaveLineDraw() {
                        double[] y = _tb.Tb.Tb;   //しょうがない,,,コピーするよ
                        Functions.Fill0(y.AsSpan(), Open[0], Close[_db.Len - 1]);  //波動線の０補間
                        _plot.AddScatterLines(_x, y, Color.Green, 2);
                  }

                  //転換点Point描画-------------------------------------------------------------------
                  void ChangePointDraw() {
                        _plot.AddScatterPoints(EndX.Select(x => Convert.ToDouble(x)).ToArray(),
                                                    EndY,
                                                    Color.GreenYellow, 10);
                  }
                  //空間TextPoint描画-------------------------------------------------------------------
                  void SpaceNumDraw() {
                        for (int i = 1; i < _wave.Len; i++) {
                              _plot.AddText(SpaceNum[i].ToString(),
                                                   TbX[i],
                                                   PosNeg[i] ? TbY[i] + 0.03 : TbY[i],
                                                   size: 18, color: Color.Blue);
                        }
                  }
                  //売買描画-------------------------------------------------------------------

                  void TradeDraw() {
                        //Sell
                        if (_exit.ShortLen != 0) {
                              for (int i = 0; i < _exit.ShortLen; i++) {
                                    var arrow = _plot.AddArrow(Convert.ToDouble(ExitBuyX[i]),
                                                  ExitBuyY[i],
                                                  Convert.ToDouble(EntrySellX[i]),
                                                  EntrySellY[i]
                                                  );
                                    arrow.Color = Color.Blue;
                                    arrow.LineWidth = 3;
                              }
                        }
                        //Buy
                        if (_exit.LongLen != 0) {
                              for (int i = 0; i < _exit.LongLen; i++) {
                                    var arrow2 = _plot.AddArrow(Convert.ToDouble(ExitSellX[i]),
                                                  ExitSellY[i],
                                                  Convert.ToDouble(EntryBuyX[i]),
                                                  EntryBuyY[i]
                                                  );
                                    arrow2.Color = Color.Red;
                                    arrow2.LineWidth = 3;
                              }
                        }
                  }
            }
      }
}
