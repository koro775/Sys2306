using System.Diagnostics;
using Helpers2306;
using Sys2306.domain.Data;
using Sys2306.domain.Forms;
using Sys2306.domain.performances;
using Sys2306.domain.Tools;
using Sys2306.domain.Trading.Entries;
using Sys2306.domain.Trading.Exits;
using Sys2306.domain.Trading.Trades;
using static Sys2306.domain.Tools.ConvertTB;

namespace Sys2306.domain;

public partial class MainForm : Form
{
      private readonly Stopwatch _sw = new();

      int _num;
      string _symbol = "GBPJPY";
      int _recNo;
      int _spaceNo;

      public MainForm() {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
      }

      private void RunButton_Click(object sender, EventArgs e) {

            _num = ExeCombBox.SelectedIndex;
            _symbol = SymbolComboBox.Text;
            _recNo = System.Convert.ToInt32(RecNoComboBox.Text);
            _spaceNo = System.Convert.ToInt32(SpaceNumComboBox.SelectedItem);

            _sw.Start();
            switch (_num) {
                  case 0: WaveGraphExe(_symbol, _spaceNo, _recNo); break;
                  //case 1: TradeTable(_symbol, _spaceNo, _recNo); break;
                  case 2: ConvertExe(_symbol, _spaceNo, _recNo); break;
            }

            StopTimer();
      }

      private void StopTimer() {
            _sw.Stop();
            this.TimeLabel.Text = _sw.Elapsed.ToString();
            _sw.Reset();
      }

      //Chart描画(0)
      private void WaveGraphExe(string symbol, int spaceNum, int topNum) {

            var db = new DB(symbol, topNum);
            db.Read();

            var tb = new TB(spaceNum, db);
            tb.SetTbData();

            var wave = new Wave(db, tb);
            wave.Process();

            Entry entry = Entry.CreateEntry(EntryKind.No3, db, wave);
            entry.SetEntryPoints();

            Exit exit = new Exit2(symbol, spaceNum, db, wave, entry);
            exit.SetExitPoints();

            using var f = new Chart(db, tb, wave, exit);
            f.ShowDialog();
      }

      //Entry(1)
      //private void TradeTable(string symbol, int spaceNum, int topNum) {

      //      List<PerformRec> aaa = new();

      //      var db = new DB(symbol, topNum);
      //      db.Read();

      //      for (int i = 3; i < 4; i++) {

      //            //var tb = new TB(spaceNum, db);
      //            var tb = new TB(i, db);
      //            tb.SetTbData();

      //            var wave = new Wave(db, tb);
      //            wave.Process();

      //            //Entry entry = Factory.CreateEntry(Method.No1, db, wave);
      //            Entry entry = Entry.CreateEntry(EntryKind.No3, db, wave);
      //            entry.SetEntryPoints();

      //            //Trade.Trade t = new(symbol, spaceNum, db, wave, entry);
      //            //Trade.Trade t = new(symbol, i, db, wave, entry);
      //            //t.CreateTradeTable();
      //            //aaa.AddRange(t.CreatePerformanceTable());


      //      }
      //      //TradeTable output = t.CreateTradeTable();
      //      //_ = output;
      //      //List<PerformRec> per = t.PerformTable;
      //      //_ = per;

      //      RecordNumlabel.Text = db.Len.ToString();

      //}

      //Listに変換 (2)
      private void ConvertExe(string symbol, int spaceNum, int topNum) {

            var db = new DB(symbol, topNum);
            db.Read();

            var tb = new TB(spaceNum, db);
            tb.SetTbData();

            var wave = new Wave(db, tb);
            wave.Process();

            Entry entry = Entry.CreateEntry(EntryKind.No3, db, wave);
            entry.SetEntryPoints();

            Exit exit = new Exit2(symbol, spaceNum, db, wave, entry);
            exit.SetExitPoints();
            //_ = exit.ShortExitTable;

            Trade trade = new(db, exit);
            trade.Process();
            _ = trade.ShortList;

            Performance performance = new(symbol, spaceNum, trade);
            Performance2 performance2 = new(symbol, spaceNum, trade);
            performance.Process();
            performance2.Process();
            _ = performance.PerformList;
            _ = performance2.PerformList2;

            //-------------------------------------------------------------------------------------------------
            //CSVへの書き込み
            bool writeFlg = false;
            if (writeFlg) {
                  //Db List
                  var dd = new ConvertDb(db).DbList;
                  CsvHelpers.Write(dd, @"Db.csv");

                  //TopBottom Lsit
                  _ = new ConvertTB(tb).TbList;

                  //Wave Lsit
                  _ = new ConvertWave(wave).WaveList;

                  //Entry Lsit
                  ListConvert en = new ConvertEntry(entry);
                  _ = en.ShortEntry;
                  _ = en.LongEntry;

                  //Exit Lsit
                  ListConvert ex = new ConvertExit(exit);
                  var sss = ex.ShortExitList;
                  var lll = ex.LongExitList;

                  _ = performance.PerformList;
                  _ = performance2.PerformList2;

                  //Exit Lsit
                  //CsvHelpers.Write(sss, @"ShortExit.csv");
                  //CsvHelpers.Write(lll, @"LongExit.csv");

                  //Trade Lsit
                  CsvHelpers.Write(trade.ShortList, @"ShortTable.csv");
                  CsvHelpers.Write(trade.LongList, @"LongTable.csv");

                  //PerformList
                  CsvHelpers.Write(performance.PerformList, @"PerformTable.csv");
                  CsvHelpers.Write(performance2.PerformList2, @"PerformTable2.csv");
            }
      }

      private void MainForm_Load(object sender, EventArgs e) {

            //ExeCombBox設定
            string[] arr = Enumerable.Range(0, 10).Select(x => x.ToString()).ToArray();
            ExeCombBox.Items.AddRange(arr);
            ExeCombBox.SelectedItem = "2";      //Defalt値

            //SymbolCombo設定
            SymbolComboBox.Items.Add("USDJPY");
            SymbolComboBox.Items.Add("GBPJPY");
            SymbolComboBox.SelectedItem = "GBPJPY"; //Defalt値

            //Space数Combo設定
            string[] arr2 = Enumerable.Range(1, 1440).Select(x => x.ToString()).ToArray();
            SpaceNumComboBox.Items.AddRange(arr2);
            SpaceNumComboBox.SelectedItem = "5";      //Defalt値

            //RecNoCombo設定
            RecNoComboBox.Items.Add("0");            //全レコード
            RecNoComboBox.Items.Add("3000");
            RecNoComboBox.Items.Add("10000");
            RecNoComboBox.SelectedItem = "3000"; //Defalt値
      }
}
