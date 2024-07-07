namespace Sys2306.domain.Forms
{
      partial class Chart
      {
            /// <summary>
            /// Required designer variable.
            /// </summary>
            private System.ComponentModel.IContainer components = null;

            /// <summary>
            /// Clean up any resources being used.
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
            /// Required method for Designer support - do not modify
            /// the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent() {
                  panel1 = new Panel();
                  formsPlot1 = new ScottPlot.FormsPlot();
                  panel1.SuspendLayout();
                  SuspendLayout();
                  // 
                  // panel1
                  // 
                  panel1.Controls.Add(formsPlot1);
                  panel1.Location = new Point(16, 56);
                  panel1.Name = "panel1";
                  panel1.Size = new Size(2854, 1418);
                  panel1.TabIndex = 0;
                  // 
                  // formsPlot1
                  // 
                  formsPlot1.Dock = DockStyle.Fill;
                  formsPlot1.Location = new Point(0, 0);
                  formsPlot1.Margin = new Padding(5, 4, 5, 4);
                  formsPlot1.Name = "formsPlot1";
                  formsPlot1.Size = new Size(2854, 1418);
                  formsPlot1.TabIndex = 0;
                  // 
                  // Chart
                  // 
                  AutoScaleDimensions = new SizeF(8F, 20F);
                  AutoScaleMode = AutoScaleMode.Font;
                  ClientSize = new Size(2851, 1476);
                  Controls.Add(panel1);
                  Name = "Chart";
                  Text = "Chart";
                  panel1.ResumeLayout(false);
                  ResumeLayout(false);
            }

            #endregion

            private Panel panel1;
            private ScottPlot.FormsPlot formsPlot1;
      }
}