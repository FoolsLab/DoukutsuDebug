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
			this.Screen = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.Screen)).BeginInit();
			this.SuspendLayout();
			// 
			// Screen
			// 
			this.Screen.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Screen.Location = new System.Drawing.Point(0, 0);
			this.Screen.Name = "Screen";
			this.Screen.Size = new System.Drawing.Size(618, 424);
			this.Screen.TabIndex = 0;
			this.Screen.TabStop = false;
			this.Screen.Paint += new System.Windows.Forms.PaintEventHandler(this.Screen_Paint);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(618, 424);
			this.Controls.Add(this.Screen);
			this.Name = "Form1";
			this.Text = "DoukutsuDebug v0.1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.Screen)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox Screen;
	}
}

