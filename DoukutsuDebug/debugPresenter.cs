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

        static string GetBulletName(int type)
        {
            string[] tbl = {
                "--",
                "Sn",
                "PS",
                "FB",
                "MG",
                "Ms",
                "Ms",
                "Bb",
                "?",
                "Bd",
                "SM",
                "SM",
                "Nm",
                "Sp"
            };
            return tbl[type];
        }

        public static void Present(Graphics g, CSData dat)
        {
            g.DrawString(
                String.Format(
                    "Pos X:{0, 10}    =   0x2000*{3, 4} {4} 0x{5, 4:X}",
                    +dat.MyCharX,
                    (dat.MyCharX < 0) ? '-' : ' ',
                    Math.Abs(dat.MyCharX),
                    (dat.MyCharX + 0x1000) / 0x2000,
                    (((dat.MyCharX + 0x1000) % 0x2000 - 0x1000) < 0) ? '-' : '+',
                    Math.Abs((dat.MyCharX + 0x1000) % 0x2000 - 0x1000)),
                f, Brushes.White, new Point(0, 0));


            g.DrawString(
                String.Format(
                    "Pos Y:{0, 10}    =   0x2000*{3, 4} {4} 0x{5, 4:X}",
                    +dat.MyCharY,
                    (dat.MyCharY < 0) ? '-' : ' ',
                    Math.Abs(dat.MyCharY),
                    (dat.MyCharY + 0x1000) / 0x2000,
                    (((dat.MyCharY + 0x1000) % 0x2000 - 0x1000) < 0) ? '-' : '+',
                    Math.Abs((dat.MyCharY + 0x1000) % 0x2000 - 0x1000)),
                f, Brushes.White, new Point(0, 30));


            g.DrawString(
                String.Format(
                    "Vel X:{0, 10}",
                    dat.MyCharVelX,
                    (dat.MyCharVelX < 0) ? '-' : ' ',
                    Math.Abs(dat.MyCharVelX)),
                f, Brushes.White, new Point(0, 70));

            g.DrawString(
                String.Format(
                    "Vel Y:{0, 10}",
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
                    Pen ColPen = new Pen(Brushes.LawnGreen, 3);
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

            g.FillRectangle(Brushes.DarkGreen, 200, 401, dat.BoosterFuel * 250 / 50, 28);
            g.DrawRectangle(dat.BoostInfo != 0 ? Pens.Red : Pens.White, 200, 401, 250, 28);
            g.DrawString(String.Format("BoosterFuel:{0, 10}", dat.BoosterFuel), f, Brushes.White, new Point(0, 400));

            g.FillRectangle(Brushes.DarkSlateBlue, 200, 441, dat.MyCharAir * 250 / 1000, 28);
            g.DrawRectangle((dat.MyCharCollisionState & 0x100) != 0 ? Pens.Red : Pens.White, 200, 441, 250, 28);
            g.DrawString(String.Format("  AirRemain:{0, 10}", dat.MyCharAir), f, Brushes.White, new Point(0, 440));

            g.FillRectangle(Brushes.DarkSalmon, 200, 481, dat.MyCharDamageTimer * 250 / 128, 28);
            g.DrawRectangle(Pens.White, 200, 481, 250, 28);
            g.DrawString(String.Format("DamageTimer:  {0, 8}", dat.MyCharDamageTimer), f, Brushes.White, new Point(0, 480));

            g.DrawString(String.Format("MyCharState:0x{0, 8:X}", dat.MyCharState), f, Brushes.White, new Point(0, 520));
            g.DrawString(String.Format("   holdrand:{0, 10}", dat.holdrand), f, Brushes.White, new Point(0, 560));

            Pen ypen = new Pen(Color.Yellow);
            Brush gbrush = new SolidBrush(Color.Green);

            g.DrawString("   Sprite", f, Brushes.White, new Point(0, 600));
            g.FillRectangle(gbrush, 160, 600, 256, 100);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 128; j++)
                {
                    var n = 128 * i + j;
                    if (dat.SpriteDB[n].Existence != 0)
                    {
                        g.DrawLine(ypen, 160 + 2 * j, 600 + i * 25, 160 + 2 * j, 600 + i * 25 + 25);
                    }
                }
            }
            g.FillRectangle(gbrush, 160, 702, 32, 25);
            for (int i = 0; i < 16; i++)
            {
                if (dat.MBSpriteDB[i].Existence != 0)
                {
                    g.DrawLine(ypen, 160 + 2 * i, 702, 160 + 2 * i, 727);
                }
            }

            g.DrawString("   Effect", f, Brushes.White, new Point(0, 730));
            g.FillRectangle(Brushes.Green, 160, 732, 128, 25);
            for (int i = 0; i < 64; i++)
            {
                if (dat.EffectExist[i] != 0)
                {
                    g.DrawLine(ypen, 160 + 2 * i, 732, 160 + 2 * i, 732+25);
                }
            }

            g.DrawString("NumEffect", f, Brushes.White, new Point(0, 760));
            g.FillRectangle(Brushes.Green, 160, 762, 32, 25);
            for (int i = 0; i < 16; i++)
            {
                if (dat.NumEffectExist[i] != 0)
                {
                    g.DrawLine(ypen, 160 + 2 * i, 762, 160 + 2 * i, 762+25);
                }
            }

            g.DrawString("     Shot", f, Brushes.White, new Point(0, 790));
            g.FillRectangle(Brushes.Green, 160, 792, 128, 25);
            for (int i = 0; i < 64; i++)
            {
                if (dat.BulletShotDB[i].Exist != 0)
                {
                    g.DrawLine(ypen, 160 + 2 * i, 792, 160 + 2 * i, 792+25);
                }
            }
            g.DrawString(String.Format("Cmd:{0, 10}", dat.ScriptStr.Replace("\r", "").Replace("\n", " ")), f, Brushes.White, new Point(0, 820));

            g.FillRectangle(Brushes.DarkSalmon, 200, 861, Math.Min(dat.EventTimer, 16) * 250 / 16, 28);
            g.DrawRectangle(Pens.White, 200, 861, 250, 28);
            g.DrawString(String.Format(" EventTimer:{0, 4}", dat.EventTimer), f, Brushes.White, new Point(0, 860));


            g.DrawString(
                String.Format(
                    "{0, 2}'{1:00}\"{2:00}",
                    dat._290Timer / (50 * 60),
                    (dat._290Timer / 50) % 60,
                    (dat._290Timer * 100 / 50) % 100),
                f, Brushes.White, new Point(325, 210));
            g.DrawString(
                String.Format(
                    "{0}/{1}",
                    dat.MyCharHP,
                    dat.MyCharMaxHP),
                f, Brushes.White, new Point(325, 240));

            var vscr = DoukutsuRenderer.GetVScreen(dat);
            g.DrawImage(vscr, 470, 210);
            g.DrawRectangle(Pens.White, 470, 210, 320 * 3, 240 * 3);
            
            g.FillRectangle(Brushes.DarkRed,
                470 + (dat.UsingBulletIndex % 2) * 480,
                88 + (dat.UsingBulletIndex / 2) * 28,
                480,
                30);
            for (int i = 0; i < 8; i++)
            {
                var trueType = dat.PossessBulletDB[i].type * 3 + dat.PossessBulletDB[i].lv - 1;

                g.DrawString(
                    String.Format(
                        "{0, 2} Lv{1} Exp{2, 3}/{3, 3} {4, 3}/{5, 3}",
                        GetBulletName(dat.PossessBulletDB[i].type),
                        dat.PossessBulletDB[i].lv,
                        dat.PossessBulletDB[i].exp,
                        trueType < 0 ? "-" : dat.BulletMaxExpTbl[trueType].ToString(),
                        dat.PossessBulletDB[i].shotNum,
                        dat.PossessBulletDB[i].shotNumMax),
                    f, Brushes.White, new PointF(470 + (i % 2) * 480, 85 + (i / 2) * 28));
            }

            vscr.Dispose();
        }
    }
}
