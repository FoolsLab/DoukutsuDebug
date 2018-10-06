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
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        };
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct CSSprite
        {
            public int Existence;
            public int CollisionFlag;
            public int X;
            public int Y;
            public int VelX;
            public int VelY;
            public int State1;
            public int State2;
            public int State3;
            public int State4;
            public int SpriteType;
            public int FlagID;
            public int EventID;
            public int Image;
            public int DefeatedSE;
            public int DamagedSE;
            public int HP;
            public int Exp;
            public int DefeatedEffect;
            public int Direction;
            public int DetailsFlag;
            public RECT SrcRect;
            public int AnimTimer;
            public int AnimIndex;
            public int LifeTimer;
            public int State5;
            public int ActionState;
            public int ActionTimer;
            public RECT CollisionRect;
            public RECT ViewRect;
            public int FrameCounter;
            public int DamageCounter;
            public int DamageToTouch;
            public uint Parent;
        };
        
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct CSBulletShot
        {
            public int Collision;
            public int Type;
            public int Flags;
            public int Exist;
            public int X;
            public int Y;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public int[] Data;
            public int Direction;
            public RECT SrcRect;
            public int Timer;
            public int Unknown;
            public int TTL;
            public int Damage;
            public int hitNum;
            public RECT CollisionRect;
            public int ViewLenFront;
            public int ViewWidth;
            public int ViewLenBack;
            public int Unknown2;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class CSData
        {
            public int MyCharX;
            public int MyCharY;
            public int MyCharVelX;
            public int MyCharVelY;
            public int CameraX;
            public int CameraY;
            public int MyCharState;
            public int MyCharAir;
            public Int16 MyCharDamageTimer;
            public int BoosterFuel;
            public byte ShotConsumeTimer;
            public int Frame;
            public byte BoostInfo;
            public int MyCharCollisionState;

            public int ScriptNowLocation;
            public int pScriptStr;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string ScriptStr;

            public int EventTimer;
            public int _290Timer;

            public UInt32 holdrand;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public Int32[] EffectExist;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public Int32[] NumEffectExist;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
            public CSSprite[] SpriteDB;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public CSSprite[] MBSpriteDB;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public CSBulletShot[] BulletShotDB;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 307200)]
            public byte[] TileMapDB;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public byte[] TileTypeDB;

            public Int16 MapWidth;
            public Int16 MapHeight;
        };

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

        public Form1()
        {
            dat = new CSData();

            var frameSync = Task.Run(() =>
            {
                while(true)
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
                    if(finishedFlag)
                    {
                        return;
                    }
                }
            });
            InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
        {
        }

		private void Screen_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.Clear(Color.FromArgb(25, 33, 66));
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
            WorkRestartEvent.Set();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            frameDelay = trackBar1.Value;
        }
    }
}
