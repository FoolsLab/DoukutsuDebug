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
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	struct CSData
	{
		public int MyCharX;
		public int MyCharY;
		public int MyCharVelX;
		public int MyCharVelY;
        public int MyCharState;
        public byte ShotConsumeTimer;
        public int Frame;
        public byte BoostInfo;
        public int MyCharCollisionState;

        public int ScriptNowLocation;
        public int pScriptStr;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string ScriptStr;

        public UInt32 holdrand;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        public byte[] spriteExist;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] spriteMBExist;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public Int32[] EffectExist;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public Int32[] NumEffectExist;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public Int32[] BulletShotExist;
    };

	public partial class Form1 : Form
	{
		int handle;
		CSData dat;
		Font f;
        ManualResetEvent formLoadWait = new ManualResetEvent(false), finishWait = new ManualResetEvent(false);
        bool finishedFlag = false, datLoaded = false;

        int frameDelay = 0;

        [DllImport("dddll.dll")]
		static extern UInt32 FindCaveStory();

        [DllImport("dddll.dll")]
        static extern int OpenCaveStory(UInt32 pid);

        [DllImport("dddll.dll")]
        static extern int GetTls(int handle, UInt32 pid);

        [DllImport("dddll.dll")]
		static extern void GetCSData(int handle, out CSData dat, int pTls);

        [DllImport("dddll.dll")]
        static extern bool WaitFrame(int handle, out UInt32 pid, out UInt32 tid, out UInt32 opt);

        [DllImport("dddll.dll")]
        static extern void ContinueFrame(UInt32 pid, UInt32 tid);

        [DllImport("dddll.dll")]
        static extern void Detach(int handle, UInt32 pid);

        public Form1()
        {
            var okFlag = 0;
            var setupWait = new ManualResetEvent(false);

            var frameSync = Task.Run(() =>
            {
                var pid = FindCaveStory();
                handle = OpenCaveStory(pid);
                if (handle == 0)
                {
                    okFlag = -1;
                    setupWait.Set();
                    return;
                }
                else
                {
                    okFlag = 1;
                    setupWait.Set();
                }
                var ptls = GetTls(handle, pid);
                formLoadWait.WaitOne();
                while (!finishedFlag)
                {
                    UInt32 tid, opt;
                    if(WaitFrame(handle, out pid, out tid, out opt))
                    {
                        GetCSData(handle, out dat, ptls);
                        datLoaded = true;
                        if (frameDelay > 0)
                        {
                            Thread.Sleep(frameDelay);
                        }
                        ContinueFrame(pid, tid);
                        Screen.Invalidate();
                    }
                    else
                    {
                        continue;
                    }
                }
                Detach(handle, pid);
                finishWait.Set();
            });
            setupWait.WaitOne();
            if(okFlag == -1)
            {
                throw new Exception("洞窟物語のオープンに失敗");
            }
            InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
        {
            dat = new CSData();

            f = new Font("MS Gothic", 16);
            formLoadWait.Set();
        }

		private void Screen_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.Clear(Color.FromArgb(25, 33, 66));
            if(!datLoaded)
            {
                return;
            }
            e.Graphics.DrawString(
                String.Format(
                    "Pos X:{0, 10}    =   {1}0x{2, 8:X}    =    0x2000*{3, 4} {4} 0x{5, 4:X}",
                    +dat.MyCharX,
                    (dat.MyCharX < 0) ? '-' : ' ',
                    Math.Abs(dat.MyCharX),
                    (dat.MyCharX + 0x1000) / 0x2000,
                    (((dat.MyCharX + 0x1000) % 0x2000 - 0x1000) < 0) ? '-' : '+',
                    Math.Abs((dat.MyCharX + 0x1000) % 0x2000 - 0x1000)),
                f, Brushes.White, new Point(0, 0));


            e.Graphics.DrawString(
                String.Format(
                    "Pos Y:{0, 10}    =   {1}0x{2, 8:X}    =    0x2000*{3, 4} {4} 0x{5, 4:X}",
                    +dat.MyCharY,
                    (dat.MyCharY < 0) ? '-' : ' ',
                    Math.Abs(dat.MyCharY),
                    (dat.MyCharY + 0x1000) / 0x2000,
                    (((dat.MyCharY + 0x1000) % 0x2000 - 0x1000) < 0) ? '-' : '+',
                    Math.Abs((dat.MyCharY + 0x1000) % 0x2000 - 0x1000)),
                f, Brushes.White, new Point(0, 30));


            e.Graphics.DrawString(
                String.Format(
                    "Vel X:{0, 10}    =   {1}0x{2, 8:X}",
                    dat.MyCharVelX,
                    (dat.MyCharVelX < 0) ? '-' : ' ',
                    Math.Abs(dat.MyCharVelX)),
                f, Brushes.White, new Point(0, 70));

            e.Graphics.DrawString(
                String.Format(
                    "Vel Y:{0, 10}    =   {1}0x{2, 8:X}",
                    dat.MyCharVelY,
                    (dat.MyCharVelY < 0) ? '-' : ' ',
                    Math.Abs(dat.MyCharVelY)),
                f, Brushes.White, new Point(0, 100));

            {
                Point p = new Point(130, 250);
                Size sz = new Size(160, 160);
                
                e.Graphics.DrawRectangle(
                    new Pen(Brushes.White, 2),
                    new Rectangle(
                        p.X - sz.Width / 2, p.Y - sz.Height / 2,
                        sz.Width, sz.Height));

                { 
                    Pen ColPen = new Pen(Brushes.Green, 3);
                    Pen FloPen = new Pen(Brushes.Pink, 6);
                    {
                        if ((dat.MyCharCollisionState & 0x1000) > 0)
                        {
                            e.Graphics.DrawLine(
                                FloPen,
                                new Point(p.X - sz.Width / 2 - 10, p.Y - sz.Height / 2),
                                new Point(p.X - sz.Width / 2 - 10, p.Y + sz.Height / 2));
                        }
                        if ((dat.MyCharCollisionState & 0x2000) > 0)
                        {
                            e.Graphics.DrawLine(
                                FloPen,
                                new Point(p.X - sz.Width / 2, p.Y - sz.Height / 2 - 10),
                                new Point(p.X + sz.Width / 2, p.Y - sz.Height / 2 - 10));
                        }
                        if ((dat.MyCharCollisionState & 0x4000) > 0)
                        {
                            e.Graphics.DrawLine(
                                FloPen,
                                new Point(p.X + sz.Width / 2 + 10, p.Y - sz.Height / 2),
                                new Point(p.X + sz.Width / 2 + 10, p.Y + sz.Height / 2));
                        }
                        if ((dat.MyCharCollisionState & 0x8000) > 0)
                        {
                            e.Graphics.DrawLine(
                                FloPen,
                                new Point(p.X - sz.Width / 2, p.Y + sz.Height / 2 + 10),
                                new Point(p.X + sz.Width / 2, p.Y + sz.Height / 2 + 10));
                        }
                    }
                    {
                        if ((dat.MyCharCollisionState & 1) > 0)
                        {
                            e.Graphics.DrawLine(
                                ColPen,
                                new Point(p.X - sz.Width / 2, p.Y - sz.Height / 2),
                                new Point(p.X - sz.Width / 2, p.Y + sz.Height / 2));
                        }
                        if ((dat.MyCharCollisionState & 2) > 0)
                        {
                            e.Graphics.DrawLine(
                                ColPen,
                                new Point(p.X - sz.Width / 2, p.Y - sz.Height / 2),
                                new Point(p.X + sz.Width / 2, p.Y - sz.Height / 2));
                        }
                        if ((dat.MyCharCollisionState & 4) > 0)
                        {
                            e.Graphics.DrawLine(
                                ColPen,
                                new Point(p.X + sz.Width / 2, p.Y - sz.Height / 2),
                                new Point(p.X + sz.Width / 2, p.Y + sz.Height / 2));
                        }
                        if ((dat.MyCharCollisionState & 8) > 0)
                        {
                            e.Graphics.DrawLine(
                                ColPen,
                                new Point(p.X - sz.Width / 2, p.Y + sz.Height / 2),
                                new Point(p.X + sz.Width / 2, p.Y + sz.Height / 2));
                        }
                    }
                    {
                        if ((dat.MyCharCollisionState & 0x10) > 0)
                        {
                            e.Graphics.DrawLine(
                                ColPen,
                                new Point(p.X + sz.Width / 2, p.Y - sz.Height / 2),
                                new Point(p.X - sz.Width / 2, p.Y + sz.Height / 2));
                        }
                        if ((dat.MyCharCollisionState & 0x20) > 0)
                        {
                            e.Graphics.DrawLine(
                                ColPen,
                                new Point(p.X - sz.Width / 2, p.Y - sz.Height / 2),
                                new Point(p.X + sz.Width / 2, p.Y + sz.Height / 2));
                        }
                    }
                    {
                        if ((dat.MyCharCollisionState & 0x10000) > 0)
                        {
                            e.Graphics.DrawLine(
                                ColPen,
                                new Point(p.X - sz.Width / 2, p.Y - sz.Height / 2),
                                new Point(p.X + sz.Width / 2, p.Y));
                        }
                        if ((dat.MyCharCollisionState & 0x20000) > 0)
                        {
                            e.Graphics.DrawLine(
                                ColPen,
                                new Point(p.X + sz.Width / 2, p.Y + sz.Height / 2),
                                new Point(p.X - sz.Width / 2, p.Y));
                        }
                        if ((dat.MyCharCollisionState & 0x40000) > 0)
                        {
                            e.Graphics.DrawLine(
                                ColPen,
                                new Point(p.X - sz.Width / 2, p.Y + sz.Height / 2),
                                new Point(p.X + sz.Width / 2, p.Y));
                        }
                        if ((dat.MyCharCollisionState & 0x80000) > 0)
                        {
                            e.Graphics.DrawLine(
                                ColPen,
                                new Point(p.X + sz.Width / 2, p.Y - sz.Height / 2),
                                new Point(p.X - sz.Width / 2, p.Y));
                        }
                    }
                }

                {

                    e.Graphics.DrawLine(
                        new Pen(Brushes.Red, 4),
                        p,
                        new Point(
                            p.X + dat.MyCharVelX * sz.Width / 2 / 1536,
                            p.Y + dat.MyCharVelY * sz.Height / 2 / 1536));
                }
            }

            e.Graphics.DrawString(String.Format("      Frame:{0, 10}", dat.Frame), f, Brushes.White, new Point(0, 360));
            e.Graphics.DrawString(String.Format(" BoostState:{0, 10}", dat.BoostInfo), f, Brushes.White, new Point(0, 400));
            e.Graphics.DrawString(String.Format("  Collision:0x{0, 8:X}", dat.MyCharCollisionState), f, Brushes.White, new Point(0, 440));
            e.Graphics.DrawString(String.Format("  ShotTimer:  {0, 8}", dat.ShotConsumeTimer), f, Brushes.White, new Point(0, 480));
            e.Graphics.DrawString(String.Format("MyCharState:0x{0, 8:X}", dat.MyCharState), f, Brushes.White, new Point(0, 520));
            e.Graphics.DrawString(String.Format("   holdrand:{0, 10}", dat.holdrand), f, Brushes.White, new Point(0, 560));

            Pen ypen = new Pen(Color.Yellow);
            Brush gbrush = new SolidBrush(Color.Green);

            e.Graphics.DrawString("Sprite", f, Brushes.White, new Point(0, 610));
            e.Graphics.FillRectangle(gbrush, 160, 600, 512, 50);
            for (int i = 0; i < 256; i++)
            {
                if(dat.spriteExist[i] != 0)
                {
                    e.Graphics.DrawLine(ypen, 160 + 2*i, 600, 160 + 2*i, 625);
                }
                if (dat.spriteExist[i + 256] != 0)
                {
                    e.Graphics.DrawLine(ypen, 160 + 2*i, 625, 160 + 2*i, 650);
                }
            }
            e.Graphics.FillRectangle(gbrush, 160, 650, 32, 25);
            for (int i = 0; i < 16; i++)
            {
                if (dat.spriteMBExist[i] != 0)
                {
                    e.Graphics.DrawLine(ypen, 160 + 2 * i, 650, 160 + 2 * i, 675);
                }
            }

            e.Graphics.DrawString("Effect", f, Brushes.White, new Point(0, 675));
            e.Graphics.FillRectangle(Brushes.Green, 160, 680, 128, 25);
            for (int i = 0; i < 64; i++)
            {
                if (dat.EffectExist[i] != 0)
                {
                    e.Graphics.DrawLine(ypen, 160 + 2 * i, 680, 160 + 2 * i, 705);
                }
            }

            e.Graphics.DrawString("NumEffect", f, Brushes.White, new Point(0, 705));
            e.Graphics.FillRectangle(Brushes.Green, 160, 710, 32, 25);
            for (int i = 0; i < 16; i++)
            {
                if (dat.NumEffectExist[i] != 0)
                {
                    e.Graphics.DrawLine(ypen, 160 + 2 * i, 710, 160 + 2 * i, 735);
                }
            }

            e.Graphics.DrawString("Shot", f, Brushes.White, new Point(0, 735));
            e.Graphics.FillRectangle(Brushes.Green, 160, 740, 128, 25);
            for (int i = 0; i < 64; i++)
            {
                if (dat.BulletShotExist[i] != 0)
                {
                    e.Graphics.DrawLine(ypen, 160 + 2 * i, 740, 160 + 2 * i, 765);
                }
            }
            e.Graphics.DrawString(String.Format("Cmd:{0, 10}", dat.ScriptStr.Replace("\r", "").Replace("\n", " ")), f, Brushes.White, new Point(0, 770));
        }

        private void Form1_FormClosed(object sender, FormClosingEventArgs e)
        {
            finishedFlag = true;
            finishWait.WaitOne();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            frameDelay = trackBar1.Value;
        }
    }
}
