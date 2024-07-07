namespace Sys2306.domain
{
      partial class MainForm
      {
            /// <summary>
            ///  Required designer variable.
            /// </summary>
            private System.ComponentModel.IContainer components = null;

            /// <summary>
            ///  Clean up any resources being used.
            /// </summary>
            /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
            protected override void Dispose(bool disposing) {
                  if (disposing && (components != null)) {
                        components.Dispose();
                  }
                  base.Dispose(disposing);
            }

            #region Windows Form Designer generated code

            /// <summary>
            ///  Required method for Designer support - do not modify
            ///  the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent() {
                  RunButton = new Button();
                  SymbolComboBox = new ComboBox();
                  RecNoComboBox = new ComboBox();
                  SpaceNumComboBox = new ComboBox();
                  label1 = new Label();
                  label2 = new Label();
                  label3 = new Label();
                  TimeLabel = new Label();
                  RecordNumlabel = new Label();
                  ExeCombBox = new ComboBox();
                  label4 = new Label();
                  SuspendLayout();
                  // 
                  // RunButton
                  // 
                  RunButton.Location = new Point(31, 31);
                  RunButton.Name = "RunButton";
                  RunButton.Size = new Size(146, 51);
                  RunButton.TabIndex = 0;
                  RunButton.Text = "Run";
                  RunButton.UseVisualStyleBackColor = true;
                  RunButton.Click += RunButton_Click;
                  // 
                  // SymbolComboBox
                  // 
                  SymbolComboBox.FormattingEnabled = true;
                  SymbolComboBox.Location = new Point(324, 78);
                  SymbolComboBox.Name = "SymbolComboBox";
                  SymbolComboBox.Size = new Size(121, 23);
                  SymbolComboBox.TabIndex = 1;
                  // 
                  // RecNoComboBox
                  // 
                  RecNoComboBox.FormattingEnabled = true;
                  RecNoComboBox.Location = new Point(324, 197);
                  RecNoComboBox.Name = "RecNoComboBox";
                  RecNoComboBox.Size = new Size(121, 23);
                  RecNoComboBox.TabIndex = 2;
                  // 
                  // SpaceNumComboBox
                  // 
                  SpaceNumComboBox.FormattingEnabled = true;
                  SpaceNumComboBox.Location = new Point(324, 135);
                  SpaceNumComboBox.Name = "SpaceNumComboBox";
                  SpaceNumComboBox.Size = new Size(121, 23);
                  SpaceNumComboBox.TabIndex = 3;
                  // 
                  // label1
                  // 
                  label1.AutoSize = true;
                  label1.Location = new Point(216, 86);
                  label1.Name = "label1";
                  label1.Size = new Size(46, 15);
                  label1.TabIndex = 4;
                  label1.Text = "Symbol";
                  // 
                  // label2
                  // 
                  label2.AutoSize = true;
                  label2.Location = new Point(216, 143);
                  label2.Name = "label2";
                  label2.Size = new Size(50, 15);
                  label2.TabIndex = 5;
                  label2.Text = "Space数";
                  // 
                  // label3
                  // 
                  label3.AutoSize = true;
                  label3.Location = new Point(216, 205);
                  label3.Name = "label3";
                  label3.Size = new Size(76, 15);
                  label3.TabIndex = 6;
                  label3.Text = "取得レコード数";
                  // 
                  // TimeLabel
                  // 
                  TimeLabel.AutoSize = true;
                  TimeLabel.Font = new Font("メイリオ", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
                  TimeLabel.Location = new Point(31, 108);
                  TimeLabel.Name = "TimeLabel";
                  TimeLabel.Size = new Size(69, 31);
                  TimeLabel.TabIndex = 7;
                  TimeLabel.Text = "Time";
                  // 
                  // RecordNumlabel
                  // 
                  RecordNumlabel.AutoSize = true;
                  RecordNumlabel.Font = new Font("メイリオ", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
                  RecordNumlabel.Location = new Point(31, 158);
                  RecordNumlabel.Name = "RecordNumlabel";
                  RecordNumlabel.Size = new Size(113, 31);
                  RecordNumlabel.TabIndex = 8;
                  RecordNumlabel.Text = "Record数";
                  // 
                  // ExeCombBox
                  // 
                  ExeCombBox.FormattingEnabled = true;
                  ExeCombBox.Location = new Point(324, 31);
                  ExeCombBox.Name = "ExeCombBox";
                  ExeCombBox.Size = new Size(121, 23);
                  ExeCombBox.TabIndex = 9;
                  // 
                  // label4
                  // 
                  label4.AutoSize = true;
                  label4.Location = new Point(216, 39);
                  label4.Name = "label4";
                  label4.Size = new Size(47, 15);
                  label4.TabIndex = 10;
                  label4.Text = "処理No";
                  // 
                  // MainForm
                  // 
                  AutoScaleDimensions = new SizeF(7F, 15F);
                  AutoScaleMode = AutoScaleMode.Font;
                  ClientSize = new Size(495, 450);
                  Controls.Add(label4);
                  Controls.Add(ExeCombBox);
                  Controls.Add(RecordNumlabel);
                  Controls.Add(TimeLabel);
                  Controls.Add(label3);
                  Controls.Add(label2);
                  Controls.Add(label1);
                  Controls.Add(SpaceNumComboBox);
                  Controls.Add(RecNoComboBox);
                  Controls.Add(SymbolComboBox);
                  Controls.Add(RunButton);
                  Name = "MainForm";
                  Text = "MainForm";
                  Load += MainForm_Load;
                  ResumeLayout(false);
                  PerformLayout();
            }

            #endregion

            private Button RunButton;
            private ComboBox SymbolComboBox;
            private ComboBox RecNoComboBox;
            private ComboBox SpaceNumComboBox;
            private Label label1;
            private Label label2;
            private Label label3;
            private Label TimeLabel;
            private Label RecordNumlabel;
            private ComboBox ExeCombBox;
            private Label label4;
      }
}