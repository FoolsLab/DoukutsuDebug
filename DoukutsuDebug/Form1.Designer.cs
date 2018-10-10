namespace DoukutsuDebug
{
	partial class Form1
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Screen = new System.Windows.Forms.PictureBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.Sprite_Mode0 = new System.Windows.Forms.RadioButton();
            this.Sprite_Mode1 = new System.Windows.Forms.RadioButton();
            this.Sprite_RemoveSmoke = new System.Windows.Forms.CheckBox();
            this.MyCharCamera = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.Screen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // Screen
            // 
            this.Screen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Screen.Location = new System.Drawing.Point(0, 0);
            this.Screen.Name = "Screen";
            this.Screen.Size = new System.Drawing.Size(1448, 995);
            this.Screen.TabIndex = 0;
            this.Screen.TabStop = false;
            this.Screen.Click += new System.EventHandler(this.Screen_Click);
            this.Screen.Paint += new System.Windows.Forms.PaintEventHandler(this.Screen_Paint);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(248, 287);
            this.trackBar1.Maximum = 1000;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(208, 69);
            this.trackBar1.SmallChange = 20;
            this.trackBar1.TabIndex = 1;
            this.trackBar1.Visible = false;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // Sprite_Mode0
            // 
            this.Sprite_Mode0.AutoSize = true;
            this.Sprite_Mode0.Checked = true;
            this.Sprite_Mode0.Location = new System.Drawing.Point(728, 949);
            this.Sprite_Mode0.Name = "Sprite_Mode0";
            this.Sprite_Mode0.Size = new System.Drawing.Size(82, 22);
            this.Sprite_Mode0.TabIndex = 2;
            this.Sprite_Mode0.TabStop = true;
            this.Sprite_Mode0.Text = "Mode0";
            this.Sprite_Mode0.UseVisualStyleBackColor = true;
            this.Sprite_Mode0.Visible = false;
            this.Sprite_Mode0.CheckedChanged += new System.EventHandler(this.Sprite_Mode0_CheckedChanged);
            // 
            // Sprite_Mode1
            // 
            this.Sprite_Mode1.AutoSize = true;
            this.Sprite_Mode1.Location = new System.Drawing.Point(833, 949);
            this.Sprite_Mode1.Name = "Sprite_Mode1";
            this.Sprite_Mode1.Size = new System.Drawing.Size(82, 22);
            this.Sprite_Mode1.TabIndex = 3;
            this.Sprite_Mode1.Text = "Mode1";
            this.Sprite_Mode1.UseVisualStyleBackColor = true;
            this.Sprite_Mode1.Visible = false;
            this.Sprite_Mode1.CheckedChanged += new System.EventHandler(this.Sprite_Mode1_CheckedChanged);
            // 
            // Sprite_RemoveSmoke
            // 
            this.Sprite_RemoveSmoke.AutoSize = true;
            this.Sprite_RemoveSmoke.Location = new System.Drawing.Point(936, 949);
            this.Sprite_RemoveSmoke.Name = "Sprite_RemoveSmoke";
            this.Sprite_RemoveSmoke.Size = new System.Drawing.Size(144, 22);
            this.Sprite_RemoveSmoke.TabIndex = 4;
            this.Sprite_RemoveSmoke.Text = "RemoveSmoke";
            this.Sprite_RemoveSmoke.UseVisualStyleBackColor = true;
            this.Sprite_RemoveSmoke.Visible = false;
            this.Sprite_RemoveSmoke.CheckedChanged += new System.EventHandler(this.Sprite_RemoveSmoke_CheckedChanged);
            // 
            // MyCharCamera
            // 
            this.MyCharCamera.AutoSize = true;
            this.MyCharCamera.Location = new System.Drawing.Point(1099, 949);
            this.MyCharCamera.Name = "MyCharCamera";
            this.MyCharCamera.Size = new System.Drawing.Size(150, 22);
            this.MyCharCamera.TabIndex = 5;
            this.MyCharCamera.Text = "MyCharCamera";
            this.MyCharCamera.UseVisualStyleBackColor = true;
            this.MyCharCamera.Visible = false;
            this.MyCharCamera.CheckedChanged += new System.EventHandler(this.MyCharCamera_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1448, 995);
            this.Controls.Add(this.MyCharCamera);
            this.Controls.Add(this.Sprite_RemoveSmoke);
            this.Controls.Add(this.Sprite_Mode1);
            this.Controls.Add(this.Sprite_Mode0);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.Screen);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "DoukutsuDebug v1.4";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Screen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox Screen;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.RadioButton Sprite_Mode0;
        private System.Windows.Forms.RadioButton Sprite_Mode1;
        private System.Windows.Forms.CheckBox Sprite_RemoveSmoke;
        private System.Windows.Forms.CheckBox MyCharCamera;
    }
}

