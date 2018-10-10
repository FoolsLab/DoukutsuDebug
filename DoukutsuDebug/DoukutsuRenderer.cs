using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DoukutsuDebug
{
    class DoukutsuRenderer
    {
        const int scale = 3;
        public static int SpriteMode = 0;
        public static bool SmokeRemove = false;
        public static bool camMyChar = false;

        static Pen SpriteToPen_View(CSData.CSSprite sprite)
        {
            if(SmokeRemove && sprite.SpriteType == 4)
            {
                return new Pen(Color.Transparent);
            }
            switch(SpriteMode)
            {
                case 1:
                    return new Pen(Color.Transparent);
                default:
                    return new Pen(Color.Green);
            }
        }
        static Pen SpriteToPen_Collision(CSData.CSSprite sprite)
        {
            if (SmokeRemove && sprite.SpriteType == 4)
            {
                return new Pen(Color.Transparent);
            }
            switch (SpriteMode)
            {
                case 1:
                    if((sprite.DetailsFlag & (0x2000 | 0x0100)) > 0)
                    {
                        return new Pen(Color.MediumPurple);
                    }
                    else if(sprite.DamageToTouch > 0)
                    {
                        return new Pen(
                            (sprite.DetailsFlag & (0x0001 | 0x0040)) != 0 ? Color.Red : Color.Red,
                            (sprite.DetailsFlag & (0x0001 | 0x0040)) != 0 ? 3 : 1); ;
                    }
                    else if((sprite.DetailsFlag & (0x0001 | 0x0040)) != 0)
                    {
                        return new Pen(Color.Orange);
                    }
                    else
                    {
                        return new Pen(Color.Purple);
                    }
                default:
                    return new Pen(Color.Red);
            }
        }

        static void DrawTiles(Graphics g, CSData dat, int camx, int camy)
        {
            int x0 = ((camx >> 9) + 8) >> 4;
            int y0 = ((camy >> 9) + 8) >> 4;
            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 21; x++)
                {
                    int index = (y0 + y) * dat.MapWidth + (x0 + x);
                    if (index < 0)
                    {
                        continue;
                    }
                    int type = dat.TileTypeDB[dat.TileMapDB[index]];

                    if ((type & 0x20) != 0 && type != 0x62)
                    {
                        g.DrawRectangle(
                            Pens.DarkSlateBlue,
                            toScreen((x0 + x) * 0x2000 - camx - 0x0A00),
                            toScreen((y0 + y) * 0x2000 - camy - 0x0A00),
                            toScreen(0xA00 * 2),
                            toScreen(0xA00));
                    }
                    int type_sub = type & ~0x20;
                    switch (type_sub)
                    {
                        case 0x05:
                        case 0x41:
                        case 0x43:
                        case 0x46:
                            g.DrawPolygon(type == 0x43 ? Pens.GhostWhite : Pens.Gray,
                                new PointF[]{
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx - 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy - 0x0800)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx - 0x0A00),
                                    toScreen((y0 + y) * 0x2000 - camy - 0x1000)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx + 0x0A00),
                                    toScreen((y0 + y) * 0x2000 - camy - 0x1000)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx + 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy - 0x0800)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx + 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy + 0x0800)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx + 0x0A00),
                                    toScreen((y0 + y) * 0x2000 - camy + 0x1000)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx - 0x0A00),
                                    toScreen((y0 + y) * 0x2000 - camy + 0x1000)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx - 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy + 0x0800))
                                });
                            break;
                        case 0x42:
                            if ((type & 0x20) != 0)
                            {
                                g.DrawRectangle(
                                    Pens.MediumVioletRed,
                                    toScreen((x0 + x) * 0x2000 - camx - 0x0800),
                                    toScreen((y0 + y) * 0x2000 - camy - 0x0600),
                                    toScreen(0x800 * 2),
                                    toScreen(0x600 * 2));
                            }
                            else
                            {
                                g.DrawRectangle(
                                    Pens.DarkRed,
                                    toScreen((x0 + x) * 0x2000 - camx - 0x0800),
                                    toScreen((y0 + y) * 0x2000 - camy - 0x0600),
                                    toScreen(0x800 * 2),
                                    toScreen(0x600 * 2));
                            }
                            break;
                        case 0x50:
                            g.DrawPolygon(Pens.Gray,
                                new PointF[]{
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx - 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy - 0x1000)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx + 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy - 0x1000)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx + 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx - 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy + 0x1000))
                                });
                            break;
                        case 0x51:
                            g.DrawPolygon(Pens.Gray,
                                new PointF[]{
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx - 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy - 0x1000)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx + 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy - 0x1000)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx - 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy))
                                });
                            break;
                        case 0x52:
                            g.DrawPolygon(Pens.Gray,
                                new PointF[]{
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx - 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy - 0x1000)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx + 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy - 0x1000)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx + 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy))
                                });
                            break;
                        case 0x53:
                            g.DrawPolygon(Pens.Gray,
                                new PointF[]{
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx - 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy - 0x1000)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx + 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy - 0x1000)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx + 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy + 0x1000)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx - 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy))
                                });
                            break;
                        case 0x54:
                            g.DrawPolygon(Pens.Gray,
                                new PointF[]{
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx - 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy - 0x1000)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx + 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx + 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy + 0x1000)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx - 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy + 0x1000))
                                });
                            break;
                        case 0x55:
                            g.DrawPolygon(Pens.Gray,
                                new PointF[]{
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx - 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx + 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy + 0x1000)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx - 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy + 0x1000))
                                });
                            break;
                        case 0x56:
                            g.DrawPolygon(Pens.Gray,
                                new PointF[]{
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx + 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx + 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy + 0x1000)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx - 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy + 0x1000))
                                });
                            break;
                        case 0x57:
                            g.DrawPolygon(Pens.Gray,
                                new PointF[]{
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx - 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx + 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy - 0x1000)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx + 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy + 0x1000)),
                                new PointF(
                                    toScreen((x0 + x) * 0x2000 - camx - 0x1000),
                                    toScreen((y0 + y) * 0x2000 - camy + 0x1000))
                                });
                            break;
                        default:
                            break;
                    }

                }
            }

        }
        static void DrawOneSprite(Graphics g, CSData.CSSprite sp, int camx, int camy)
        {
            if (sp.Existence == 0)
            {
                return;
            }
            g.DrawRectangle(SpriteToPen_View(sp),
                toScreen(sp.X - sp.ViewRect.left - camx),
                toScreen(sp.Y - sp.ViewRect.top - camy),
                toScreen(sp.ViewRect.left + sp.ViewRect.right),
                toScreen(sp.ViewRect.top + sp.ViewRect.bottom));
            g.DrawRectangle(SpriteToPen_Collision(sp),
                toScreen(sp.X - sp.CollisionRect.left - camx),
                toScreen(sp.Y - sp.CollisionRect.top - camy),
                toScreen(sp.CollisionRect.left + sp.CollisionRect.right),
                toScreen(sp.CollisionRect.top + sp.CollisionRect.bottom));
        }
        static void DrawSprites(Graphics g, CSData dat, int camx, int camy)
        {
            for (int i = 0; i < 512; i++)
            {
                DrawOneSprite(g, dat.SpriteDB[i], camx, camy);
            }
            for (int i = 0; i < 16; i++)
            {
                DrawOneSprite(g, dat.MBSpriteDB[i], camx, camy);
            }
        }
        static void DrawBullet(Graphics g, CSData dat, int camx, int camy)
        {
            for (int i = 0; i < 64; i++)
            {
                var bs = dat.BulletShotDB[i];
                if (bs.Exist == 0)
                {
                    continue;
                }
                g.DrawRectangle(Pens.Yellow,
                    toScreen(bs.X - bs.CollisionRect.left - camx),
                    toScreen(bs.Y - bs.CollisionRect.top - camy),
                    toScreen(bs.CollisionRect.left + bs.CollisionRect.right),
                    toScreen(bs.CollisionRect.top + bs.CollisionRect.bottom));
            }
        }
        static void DrawMyChar(Graphics g, CSData dat, int camx, int camy)
        {

            g.DrawRectangle(Pens.Cyan,
                toScreen(dat.MyCharX - 0xA00 - camx),
                toScreen(dat.MyCharY - 0x1000 - camy),
                toScreen(0xA00 * 2),
                toScreen(0x1000 * 2));
            g.DrawRectangle(Pens.Cyan,
                toScreen(dat.MyCharX - 0x400 - camx),
                toScreen(dat.MyCharY - 0x400 - camy),
                toScreen(0x400 * 2),
                toScreen(0x400 * 2));
            g.DrawRectangle(Pens.Cyan,
                toScreen(dat.MyCharX - 0x800 - camx),
                toScreen(dat.MyCharY - 0x800 - camy),
                toScreen(0x800 * 2),
                toScreen(0x800 * 2));
            g.DrawLine(Pens.Cyan,
                toScreen(dat.MyCharX - camx),
                toScreen(dat.MyCharY - 0x1000 - camy),
                toScreen(dat.MyCharX - camx),
                toScreen(dat.MyCharY + 0x1000 - camy));
            g.DrawLine(Pens.Cyan,
                toScreen(dat.MyCharX - 0x0A00 - camx),
                toScreen(dat.MyCharY - camy),
                toScreen(dat.MyCharX + 0x0A00 - camx),
                toScreen(dat.MyCharY - camy));
        }
        static void GetCamXY(CSData dat, out int camx, out int camy)
        {
            camx = camMyChar ? dat.MyCharX - (0x2000 * 20 / 2) : dat.CameraX;
            camy = camMyChar ? dat.MyCharY - (0x2000 * 15 / 2) : dat.CameraY;

            if (camMyChar)
            {
                camx = Math.Max(camx, 0);
                camx = Math.Min(camx, (dat.MapWidth << 13) - (0x2000 * 21));

                camy = Math.Max(camy, 0);
                camy = Math.Min(camy, (dat.MapHeight << 13) - (0x2000 * 16));
            }
        }

        static int toScreen(int p)
        {
            return (p * scale) >> 9;
        }
        public static Image GetVScreen(CSData dat)
        {
            Bitmap vscr = new Bitmap(320*scale, 240*scale);
            Graphics g = Graphics.FromImage(vscr);

            int camx, camy;

            GetCamXY(dat, out camx, out camy);
            DrawTiles(g, dat, camx, camy);
            DrawSprites(g, dat, camx, camy);
            DrawBullet(g, dat, camx, camy);
            DrawMyChar(g, dat, camx, camy);
            g.Dispose();
            return vscr;
        }
    }
}
