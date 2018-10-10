using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace DoukutsuDebug
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class CSData
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

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct CSPossessBullet
        {
            public int type;
            public int lv;
            public int exp;
            public int shotNumMax;
            public int shotNum;
        };

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

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public CSPossessBullet[] PossessBulletDB;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 42)]
        public int[] BulletMaxExpTbl;

        public int UsingBulletIndex;

        public Int16 MyCharHP;
        public Int16 MyCharMaxHP;
    }
}
