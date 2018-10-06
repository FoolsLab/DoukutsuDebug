using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace DoukutsuDebug
{
    class debugPresenter
    {
        static Font f = new Font("MS Gothic", 16);

        public static void Present(Graphics g, Form1.CSData dat)
        {
            g.DrawString(
    String.Format(
        "Pos X:{0, 10}    =   {1}0x{2, 8:X}    =    0x2000*{3, 4} {4} 0x{5, 4:X}",
        +dat.MyCharX,
        (dat.MyCharX < 0) ? '-' : ' ',
        Math.Abs(dat.MyCharX),
        (dat.MyCharX + 0x1000) / 0x2000,
        (((dat.MyCharX + 0x1000) % 0x2000 - 0x1000) < 0) ? '-' : '+',
        Math.Abs((dat.MyCharX + 0x1000) % 0x2000 - 0x1000)),
    f, Brushes.White, new Point(0, 0));


            g.DrawString(
                String.Format(
                    "Pos Y:{0, 10}    =   {1}0x{2, 8:X}    =    0x2000*{3, 4} {4} 0x{5, 4:X}",
                    +dat.MyCharY,
                    (dat.MyCharY < 0) ? '-' : ' ',
                    Math.Abs(dat.MyCharY),
                    (dat.MyCharY + 0x1000) / 0x2000,
                    (((dat.MyCharY + 0x1000) % 0x2000 - 0x1000) < 0) ? '-' : '+',
                    Math.Abs((dat.MyCharY + 0x1000) % 0x2000 - 0x1000)),
                f, Brushes.White, new Point(0, 30));


            g.DrawString(
                String.Format(
                    "Vel X:{0, 10}    =   {1}0x{2, 8:X}",
                    dat.MyCharVelX,
                    (dat.MyCharVelX < 0) ? '-' : ' ',
                    Math.Abs(dat.MyCharVelX)),
                f, Brushes.White, new Point(0, 70));

            g.DrawString(
                String.Format(
                    "Vel Y:{0, 10}    =   {1}0x{2, 8:X}",
                    dat.MyCharVelY,
                    (dat.MyCharVelY < 0) ? '-' : ' ',
                    Math.Abs(dat.MyCharVelY)),
                f, Brushes.White, new Point(0, 100));

            {
                Point p = new Point(130, 250);
                Size sz = new Size(160, 160);

                g.DrawRectangle(
                    (dat.MyCharCollisionState & 0x100) != 0 ? Pens.Aqua : Pens.White,
                    new Rectangle(
                        p.X - sz.Width / 2, p.Y - sz.Height / 2,
                        sz.Width, sz.Height));

                {
                    Pen ColPen = new Pen(Brushes.Green, 3);
                    Pen FloPen = new Pen(Brushes.Pink, 6);
                    {
                        if ((dat.MyCharCollisionState & 0x1000) > 0)
                        {
                            g.DrawLine(
                                FloPen,
                                new Point(p.X - sz.Width / 2 - 10, p.Y - sz.Height / 2),
                                new Point(p.X - sz.Width / 2 - 10, p.Y + sz.Height / 2));
                        }
                        if ((dat.MyCharCollisionState & 0x2000) > 0)
                        {
                            g.DrawLine(
                                FloPen,
                                new Point(p.X - sz.Width / 2, p.Y - sz.Height / 2 - 10),
                                new Point(p.X + sz.Width / 2, p.Y - sz.Height / 2 - 10));
                        }
                        if ((dat.MyCharCollisionState & 0x4000) > 0)
                        {
                            g.DrawLine(
                                FloPen,
                                new Point(p.X + sz.Width / 2 + 10, p.Y - sz.Height / 2),
                                new Point(p.X + sz.Width / 2 + 10, p.Y + sz.Height / 2));
                        }
                        if ((dat.MyCharCollisionState & 0x8000) > 0)
                        {
                            g.DrawLine(
                                FloPen,
                                new Point(p.X - sz.Width / 2, p.Y + sz.Height / 2 + 10),
                                new Point(p.X + sz.Width / 2, p.Y + sz.Height / 2 + 10));
                        }
                    }
                    {
                        if ((dat.MyCharCollisionState & 1) > 0)
                        {
                            g.DrawLine(
                                ColPen,
                                new Point(p.X - sz.Width / 2, p.Y - sz.Height / 2),
                                new Point(p.X - sz.Width / 2, p.Y + sz.Height / 2));
                        }
                        if ((dat.MyCharCollisionState & 2) > 0)
                        {
                            g.DrawLine(
                                ColPen,
                                new Point(p.X - sz.Width / 2, p.Y - sz.Height / 2),
                                new Point(p.X + sz.Width / 2, p.Y - sz.Height / 2));
                        }
                        if ((dat.MyCharCollisionState & 4) > 0)
                        {
                            g.DrawLine(
                                ColPen,
                                new Point(p.X + sz.Width / 2, p.Y - sz.Height / 2),
                                new Point(p.X + sz.Width / 2, p.Y + sz.Height / 2));
                        }
                        if ((dat.MyCharCollisionState & 8) > 0)
                        {
                            g.DrawLine(
                                ColPen,
                                new Point(p.X - sz.Width / 2, p.Y + sz.Height / 2),
                                new Point(p.X + sz.Width / 2, p.Y + sz.Height / 2));
                        }
                    }
                    {
                        if ((dat.MyCharCollisionState & 0x10) > 0)
                        {
                            g.DrawLine(
                                ColPen,
                                new Point(p.X + sz.Width / 2, p.Y - sz.Height / 2),
                                new Point(p.X - sz.Width / 2, p.Y + sz.Height / 2));
                        }
                        if ((dat.MyCharCollisionState & 0x20) > 0)
                        {
                            g.DrawLine(
                                ColPen,
                                new Point(p.X - sz.Width / 2, p.Y - sz.Height / 2),
                                new Point(p.X + sz.Width / 2, p.Y + sz.Height / 2));
                        }
                    }
                    {
                        if ((dat.MyCharCollisionState & 0x10000) > 0)
                        {
                            g.DrawLine(
                                ColPen,
                                new Point(p.X - sz.Width / 2, p.Y - sz.Height / 2),
                                new Point(p.X + sz.Width / 2, p.Y));
                        }
                        if ((dat.MyCharCollisionState & 0x20000) > 0)
                        {
                            g.DrawLine(
                                ColPen,
                                new Point(p.X + sz.Width / 2, p.Y + sz.Height / 2),
                                new Point(p.X - sz.Width / 2, p.Y));
                        }
                        if ((dat.MyCharCollisionState & 0x40000) > 0)
                        {
                            g.DrawLine(
                                ColPen,
                                new Point(p.X - sz.Width / 2, p.Y + sz.Height / 2),
                                new Point(p.X + sz.Width / 2, p.Y));
                        }
                        if ((dat.MyCharCollisionState & 0x80000) > 0)
                        {
                            g.DrawLine(
                                ColPen,
                                new Point(p.X + sz.Width / 2, p.Y - sz.Height / 2),
                                new Point(p.X - sz.Width / 2, p.Y));
                        }
                    }
                }

                {

                    g.DrawLine(
                        new Pen(Brushes.Red, 4),
                        p,
                        new Point(
                            p.X + dat.MyCharVelX * sz.Width / 2 / 1536,
                            p.Y + dat.MyCharVelY * sz.Height / 2 / 1536));
                }
            }

            g.DrawString(String.Format("      Frame:{0, 10}", dat.Frame), f, Brushes.White, new Point(0, 360));
            g.DrawString(String.Format("BoosterFuel:{0, 10}", dat.BoosterFuel), f, Brushes.White, new Point(0, 400));
            g.FillRectangle(Brushes.LightGreen, 400, 405, dat.BoosterFuel * 250 / 50, 24);
            g.DrawRectangle(dat.BoostInfo != 0 ? Pens.Red : Pens.White, 400, 405, 250, 24);
            g.DrawString(String.Format("  AirRemain:{0, 10}", dat.MyCharAir), f, Brushes.White, new Point(0, 440));
            g.FillRectangle(Brushes.Aqua, 400, 445, dat.MyCharAir * 250 / 1000, 24);
            g.DrawRectangle((dat.MyCharCollisionState & 0x100) != 0 ? Pens.Red : Pens.White, 400, 445, 250, 24);
            g.DrawString(String.Format("DamageTimer:  {0, 8}", dat.MyCharDamageTimer), f, Brushes.White, new Point(0, 480));
            g.FillRectangle(Brushes.Orange, 400, 485, dat.MyCharDamageTimer * 250 / 128, 24);
            g.DrawRectangle(Pens.White, 400, 485, 250, 24);
            g.DrawString(String.Format("MyCharState:0x{0, 8:X}", dat.MyCharState), f, Brushes.White, new Point(0, 520));
            g.DrawString(String.Format("   holdrand:{0, 10}", dat.holdrand), f, Brushes.White, new Point(0, 560));

            Pen ypen = new Pen(Color.Yellow);
            Brush gbrush = new SolidBrush(Color.Green);

            g.DrawString("Sprite", f, Brushes.White, new Point(0, 610));
            g.FillRectangle(gbrush, 160, 600, 512, 50);
            for (int i = 0; i < 256; i++)
            {
                if (dat.SpriteDB[i].Existence != 0)
                {
                    g.DrawLine(ypen, 160 + 2 * i, 600, 160 + 2 * i, 625);
                }
                if (dat.SpriteDB[i + 256].Existence != 0)
                {
                    g.DrawLine(ypen, 160 + 2 * i, 625, 160 + 2 * i, 650);
                }
            }
            g.FillRectangle(gbrush, 160, 650, 32, 25);
            for (int i = 0; i < 16; i++)
            {
                if (dat.MBSpriteDB[i].Existence != 0)
                {
                    g.DrawLine(ypen, 160 + 2 * i, 650, 160 + 2 * i, 675);
                }
            }

            g.DrawString("Effect", f, Brushes.White, new Point(0, 675));
            g.FillRectangle(Brushes.Green, 160, 680, 128, 25);
            for (int i = 0; i < 64; i++)
            {
                if (dat.EffectExist[i] != 0)
                {
                    g.DrawLine(ypen, 160 + 2 * i, 680, 160 + 2 * i, 705);
                }
            }

            g.DrawString("NumEffect", f, Brushes.White, new Point(0, 705));
            g.FillRectangle(Brushes.Green, 160, 710, 32, 25);
            for (int i = 0; i < 16; i++)
            {
                if (dat.NumEffectExist[i] != 0)
                {
                    g.DrawLine(ypen, 160 + 2 * i, 710, 160 + 2 * i, 735);
                }
            }

            g.DrawString("Shot", f, Brushes.White, new Point(0, 735));
            g.FillRectangle(Brushes.Green, 160, 740, 128, 25);
            for (int i = 0; i < 64; i++)
            {
                if (dat.BulletShotDB[i].Exist != 0)
                {
                    g.DrawLine(ypen, 160 + 2 * i, 740, 160 + 2 * i, 765);
                }
            }
            g.DrawString(String.Format("Cmd:{0, 10}", dat.ScriptStr.Replace("\r", "").Replace("\n", " ")), f, Brushes.White, new Point(0, 770));
            g.DrawString(String.Format("EventTimer:{0, 4}", dat.EventTimer), f, Brushes.White, new Point(0, 810));
            g.FillRectangle(Brushes.Orange, 300, 815, Math.Min(dat.EventTimer, 16) * 250 / 16, 24);
            g.DrawRectangle(Pens.White, 300, 815, 250, 24);

            g.DrawString(
                String.Format(
                    "{0, 2}'{1:00}\"{2:00}",
                    dat._290Timer / (50 * 60),
                    (dat._290Timer / 50) % 60,
                    (dat._290Timer * 100 / 50) % 100),
                f, Brushes.White, new Point(675, 135));
            var vscr = DoukutsuRenderer.GetVScreen(dat);
            g.DrawImage(vscr, 680, 170);
            g.DrawRectangle(Pens.White, 680, 170, 320*3, 240*3);
            vscr.Dispose();
        }
    }
}
