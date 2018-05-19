using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace DoukutsuDebug
{
	[StructLayout(LayoutKind.Sequential)]
	struct CSData
	{
		public int MyCharX;
		public int MyCharY;
		public int MyCharVelX;
		public int MyCharVelY;
		public int Frame;
	};

	public partial class Form1 : Form
	{
		int handle;
		CSData dat;
		Font f;

		System.Threading.Timer timer;

		[DllImport("dddll.dll")]
		static extern int FindCaveStory();

		[DllImport("dddll.dll")]
		static extern void GetCSData(int handle, ref CSData dat);

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			handle = FindCaveStory();
			if(handle == 0)
			{
				MessageBox.Show("洞窟物語が起動されていません");
				Application.Exit();
			}

			dat = new CSData();

			f = new Font("MS Gothic", 16);

			TimerCallback callback = state =>
			{
				Screen.Invalidate();
			};

			timer = new System.Threading.Timer(callback, null, 100, 10);
		}

		private void Screen_Paint(object sender, PaintEventArgs e)
		{
			GetCSData(handle, ref dat);
			e.Graphics.Clear(Color.FromArgb(25, 33, 66));
			e.Graphics.DrawString(String.Format("Pos X:{0, 10}", dat.MyCharX), f, Brushes.White, new Point(0, 0));
			e.Graphics.DrawString(String.Format("Pos Y:{0, 10}", dat.MyCharY), f, Brushes.White, new Point(0, 30));
			e.Graphics.DrawString(String.Format("Vel X:{0, 10}", dat.MyCharVelX), f, Brushes.White, new Point(0, 70));
			e.Graphics.DrawString(String.Format("Vel Y:{0, 10}", dat.MyCharVelY), f, Brushes.White, new Point(0, 100));

			{
				Point p = new Point(130, 250);
				Size sz = new Size(160, 160);
				
				e.Graphics.DrawRectangle(
					new Pen(Brushes.White),
					new Rectangle(p.X - sz.Width / 2, p.Y - sz.Height / 2, sz.Width, sz.Height));

				e.Graphics.DrawLine(
					new Pen(Brushes.Red),
					p,
					new Point(p.X + dat.MyCharVelX * sz.Width / 2 / 1536, p.Y + dat.MyCharVelY * sz.Height / 2 / 1536));

			}

			e.Graphics.DrawString(String.Format("Frame:{0, 10}", dat.Frame), f, Brushes.White, new Point(0, 360));
		}
	}
}
