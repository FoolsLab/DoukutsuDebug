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

	public partial class Form1 : Form
	{
        int handle;
		CSData dat;
        ManualResetEvent finishWait = new ManualResetEvent(false), WorkRestartEvent = new ManualResetEvent(false);
        bool finishedFlag = false, datLoaded = false;

        int frameDelay = 0;

        [DllImport("dddll.dll")]
		static extern UInt32 FindCaveStory();

        [DllImport("dddll.dll")]
        static extern int OpenCaveStory(UInt32 pid);

        [DllImport("dddll.dll")]
        static extern int GetTls(int handle, UInt32 pid);

        [DllImport("dddll.dll")]
		static extern void GetCSData(int handle, [Out] CSData dat, int pTls);

        [DllImport("dddll.dll")]
        static extern int isAlive(int handle);

        [DllImport("dddll.dll")]
        static extern bool WaitFrame(int handle, out UInt32 pid, out UInt32 tid, out UInt32 opt);

        [DllImport("dddll.dll")]
        static extern void ContinueFrame(UInt32 pid, UInt32 tid);

        [DllImport("dddll.dll")]
        static extern void Detach(int handle, UInt32 pid);

        private void PowerSwitch(bool state)
        {
            trackBar1.Visible = state;
            Sprite_Mode0.Visible = state;
            Sprite_Mode1.Visible = state;
            Sprite_RemoveSmoke.Visible = state;
            MyCharCamera.Visible = state;
        }

        System.Timers.Timer CSSearch;

        public Form1()
        {
            dat = new CSData();
            CSSearch = new System.Timers.Timer(1000);
            CSSearch.Elapsed += (sender, e) =>
            {
                WorkRestartEvent.Set();
            };

            var frameSync = Task.Run(() =>
            {
                while (true)
                {
                    finishWait.Reset();
                    var pid = FindCaveStory();
                    handle = OpenCaveStory(pid);

                    if (handle == 0)
                    {
                        goto wait;
                    }
                    var ptls = GetTls(handle, pid);
                    while (!finishedFlag && (isAlive(handle) != 0))
                    {
                        UInt32 tid, opt;
                        if (WaitFrame(handle, out pid, out tid, out opt))
                        {
                            GetCSData(handle, dat, ptls);
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

                    wait:
                    datLoaded = false;
                    Screen.Invalidate();
                    WorkRestartEvent.Reset();
                    finishWait.Set();
                    WorkRestartEvent.WaitOne();
                    if (finishedFlag)
                    {
                        return;
                    }
                }
            });
            CSSearch.Start();
            InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
        {
        }

		private void Screen_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.Clear(Color.FromArgb(25, 33, 66));
            PowerSwitch(datLoaded);
            if (!datLoaded)
            {
                return;
            }
            debugPresenter.Present(e.Graphics, dat);
        }

        private void Form1_FormClosed(object sender, FormClosingEventArgs e)
        {
            finishedFlag = true;
            finishWait.WaitOne();
            WorkRestartEvent.Set();
        }

        private void Sprite_Mode0_CheckedChanged(object sender, EventArgs e)
        {
            if (Sprite_Mode0.Checked)
            {
                DoukutsuRenderer.SpriteMode = 0;
            }
            Screen.Invalidate();
        }
        private void Sprite_Mode1_CheckedChanged(object sender, EventArgs e)
        {
            if(Sprite_Mode1.Checked)
            {
                DoukutsuRenderer.SpriteMode = 1;
            }
            Screen.Invalidate();
        }
        private void Sprite_RemoveSmoke_CheckedChanged(object sender, EventArgs e)
        {
            DoukutsuRenderer.SmokeRemove = Sprite_RemoveSmoke.Checked;
            Screen.Invalidate();
        }
        private void MyCharCamera_CheckedChanged(object sender, EventArgs e)
        {
            DoukutsuRenderer.camMyChar = MyCharCamera.Checked;
            Screen.Invalidate();
        }

        private void Screen_Click(object sender, EventArgs e)
        {
            //WorkRestartEvent.Set();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            frameDelay = trackBar1.Value;
        }
    }
}
