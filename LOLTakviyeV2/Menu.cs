using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Net.Http;
using System.Windows.Forms;

namespace loltakviyev2
{
  public class Menu : Form
  {
    private LCUListener listener = new LCUListener();
    private IContainer components = (IContainer) null;
        private Panel HeadUstPanel;
        private Label website;
        private Label MinimizeTxtBtn;
        private Label KapatTxtBtn;
        private Label label1;
        private Button button1;

    public Menu()
    {
      this.InitializeComponent();
      this.listener.StartListening();
    }

    private void Form1_Load(object sender, EventArgs e) => this.FormClosing += new FormClosingEventHandler(this.Form1_FormClosing);

    private void pictureBox1_Click(object sender, EventArgs e)
    {
    }

    private async void button1_Click(object sender, EventArgs e)
    {
      if (this.listener.GetGatheredLCUs().Count == 0)
      {
        int num = (int) MessageBox.Show("Lütfen önce LOL'u açın!");
      }
      else
      {
        foreach (LCUClient gatheredLcU in this.listener.GetGatheredLCUs())
        {
          LCUClient LCU = gatheredLcU;
          HttpResponseMessage httpResponseMessage = await LCU.HttpPostForm("/lol-login/v1/session/invoke", (IEnumerable<KeyValuePair<string, string>>) new List<KeyValuePair<string, string>>()
          {
            new KeyValuePair<string, string>("destination", "lcdsServiceProxy"),
            new KeyValuePair<string, string>("method", "call"),
            new KeyValuePair<string, string>("args", "[\"\",\"teambuilder-draft\",\"activateBattleBoostV1\",\"\"]")
          });
          LCU = (LCUClient) null;
        }
      }
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e) => this.listener.StopListening();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Menu));
            this.button1 = new System.Windows.Forms.Button();
            this.HeadUstPanel = new System.Windows.Forms.Panel();
            this.website = new System.Windows.Forms.Label();
            this.MinimizeTxtBtn = new System.Windows.Forms.Label();
            this.KapatTxtBtn = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.HeadUstPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(34)))), ((int)(((byte)(37)))));
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Consolas", 15F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(197)))), ((int)(((byte)(114)))));
            this.button1.Location = new System.Drawing.Point(12, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 40);
            this.button1.TabIndex = 0;
            this.button1.Text = "Takviye";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // HeadUstPanel
            // 
            this.HeadUstPanel.BackgroundImage = global::Properties.Resources.Top;
            this.HeadUstPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.HeadUstPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.HeadUstPanel.Controls.Add(this.website);
            this.HeadUstPanel.Controls.Add(this.MinimizeTxtBtn);
            this.HeadUstPanel.Controls.Add(this.KapatTxtBtn);
            this.HeadUstPanel.Controls.Add(this.label1);
            this.HeadUstPanel.Location = new System.Drawing.Point(0, 0);
            this.HeadUstPanel.Name = "HeadUstPanel";
            this.HeadUstPanel.Size = new System.Drawing.Size(156, 20);
            this.HeadUstPanel.TabIndex = 1;
            this.HeadUstPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HeadUstPanel_MouseDown);
            this.HeadUstPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HeadUstPanel_MouseMove);
            this.HeadUstPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.HeadUstPanel_MouseUp);
            // 
            // website
            // 
            this.website.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.website.AutoSize = true;
            this.website.BackColor = System.Drawing.Color.Transparent;
            this.website.Cursor = System.Windows.Forms.Cursors.Hand;
            this.website.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.website.ForeColor = System.Drawing.Color.Snow;
            this.website.Location = new System.Drawing.Point(104, 3);
            this.website.Name = "website";
            this.website.Size = new System.Drawing.Size(13, 13);
            this.website.TabIndex = 8;
            this.website.Text = "?";
            this.website.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.website.Click += new System.EventHandler(this.website_Click);
            // 
            // MinimizeTxtBtn
            // 
            this.MinimizeTxtBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MinimizeTxtBtn.AutoSize = true;
            this.MinimizeTxtBtn.BackColor = System.Drawing.Color.Transparent;
            this.MinimizeTxtBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MinimizeTxtBtn.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimizeTxtBtn.ForeColor = System.Drawing.Color.Snow;
            this.MinimizeTxtBtn.Location = new System.Drawing.Point(121, 2);
            this.MinimizeTxtBtn.Name = "MinimizeTxtBtn";
            this.MinimizeTxtBtn.Size = new System.Drawing.Size(14, 15);
            this.MinimizeTxtBtn.TabIndex = 7;
            this.MinimizeTxtBtn.Text = "-";
            this.MinimizeTxtBtn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.MinimizeTxtBtn.Click += new System.EventHandler(this.MinimizeTxtBtn_Click);
            // 
            // KapatTxtBtn
            // 
            this.KapatTxtBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.KapatTxtBtn.AutoSize = true;
            this.KapatTxtBtn.BackColor = System.Drawing.Color.Transparent;
            this.KapatTxtBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.KapatTxtBtn.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KapatTxtBtn.ForeColor = System.Drawing.Color.Crimson;
            this.KapatTxtBtn.Location = new System.Drawing.Point(139, 2);
            this.KapatTxtBtn.Name = "KapatTxtBtn";
            this.KapatTxtBtn.Size = new System.Drawing.Size(14, 15);
            this.KapatTxtBtn.TabIndex = 6;
            this.KapatTxtBtn.Text = "X";
            this.KapatTxtBtn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.KapatTxtBtn.Click += new System.EventHandler(this.KapatTxtBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.label1.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(2, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Takviye V2";
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
            this.label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label1_MouseMove);
            this.label1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label1_MouseUp);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.BackgroundImage = global::Properties.Resources.Arkaplan;
            this.ClientSize = new System.Drawing.Size(157, 78);
            this.Controls.Add(this.HeadUstPanel);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Menu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lol - Takviye Boost V2";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.HeadUstPanel.ResumeLayout(false);
            this.HeadUstPanel.PerformLayout();
            this.ResumeLayout(false);

    }

        #region HeadUstPanel
        int Move;
        int Mouse_X;
        int Mouse_Y;


        private void HeadUstPanel_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
        }

        private void HeadUstPanel_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
        }

        private void HeadUstPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }
        #endregion

        #region HeadUstPanel-Txt
        private void label1_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }
        #endregion

        #region HeadUstPanel-Btn
        private void KapatTxtBtn_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        private void MinimizeTxtBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void website_Click(object sender, EventArgs e)
        {
            Process.Start("https://yasinbalcik.com");
        }

        #endregion

    }
}
