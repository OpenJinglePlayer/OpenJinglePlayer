// Source: https://github.com/Vocaluxe/Vocaluxe

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using OpenJinglePlayer.Lib.Draw;

namespace OpenJinglePlayer
{
    static class CDraw
    {
        private static IDraw _Draw = null;
        
        public static void InitDraw()
        {
            
            try
            {
                _Draw = new CDirect3D();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + " - Error in initializing of Direct3D. Please check if " +
                    "your DirectX redistributables and graphic card drivers are up to date. You can " +
                    "download the DirectX runtimes at http://www.microsoft.com/download/en/details.aspx?id=8109",
                    "OpenJinglePlayer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                Environment.Exit(Environment.ExitCode);
            }
                    
            _Draw.Init();
        }

        public static void BeforeDraw()
        {
            _Draw.BeforeDraw();
        }

        public static void AfterDraw()
        {
            _Draw.AfterDraw();
        }

        public static bool Unload()
        {
            return _Draw.Unload();
        }

        public static void Hide()
        {
            _Draw.HideScreen();
        }

        public static void Show()
        {
            _Draw.ShowScreen();
        }

        public static void EnterFullScreen()
        {
            _Draw.EnterFullScreen();
        }

        public static void LeaveFullScreen()
        {
            _Draw.LeaveFullScreen();
        }

        public static int GetScreenWidth()
        {
            return _Draw.GetScreenWidth();
        }

        public static int GetScreenHeight()
        {
            return _Draw.GetScreenHeight();
        }

        public static void DrawLine(int a, int r, int g, int b, int w, int x1, int y1, int x2, int y2)
        {
            _Draw.DrawLine(a, r, g, b, w, x1, y1, x2, y2);
        }

        public static void DrawRect(SColorF Color, SRectF Rect, float Thickness)
        {
            if (Thickness <= 0f)
                return;

            _Draw.DrawColor(Color, new SRectF(Rect.X - Thickness / 2, Rect.Y - Thickness / 2, Rect.W + Thickness, Thickness, Rect.Z));
            _Draw.DrawColor(Color, new SRectF(Rect.X - Thickness / 2, Rect.Y + Rect.H - Thickness / 2, Rect.W + Thickness, Thickness, Rect.Z));
            _Draw.DrawColor(Color, new SRectF(Rect.X - Thickness / 2, Rect.Y - Thickness / 2, Thickness, Rect.H + Thickness, Rect.Z));
            _Draw.DrawColor(Color, new SRectF(Rect.X + Rect.W - Thickness / 2, Rect.Y - Thickness / 2, Thickness, Rect.H + Thickness, Rect.Z));
        }

        public static void DrawColor(SColorF color, SRectF rect)
        {
            _Draw.DrawColor(color, rect);
        }

        public static void ClearScreen()
        {
            _Draw.ClearScreen();
        }

        public static STexture CopyScreen()
        {
            return _Draw.CopyScreen();
        }

        public static void CopyScreen(ref STexture Texture)
        {
            _Draw.CopyScreen(ref Texture);
        }

        public static void MakeScreenShot()
        {
            _Draw.MakeScreenShot();
        }
        

        public static STexture AddTexture(Bitmap Bitmap)
        {
            return _Draw.AddTexture(Bitmap);
        }

        public static STexture AddTexture(string TexturePath)
        {
            return _Draw.AddTexture(TexturePath);
        }

        public static STexture AddTexture(string TexturePath, int MaxSize)
        {
            if (MaxSize == 0)
                return _Draw.AddTexture(TexturePath);

            if (!System.IO.File.Exists(TexturePath))
                return new STexture(-1);

            Bitmap origin = new Bitmap(TexturePath);
            int w = MaxSize;
            int h = MaxSize;

            if (origin.Width >= origin.Height && origin.Width > w)
                h = (int)Math.Round((float)w / origin.Width * origin.Height);
            else if (origin.Height > origin.Width && origin.Height > h)
                w = (int)Math.Round((float)h / origin.Height * origin.Width);

            Bitmap bmp = new Bitmap(origin, w, h);
            STexture tex = _Draw.AddTexture(bmp);
            bmp.Dispose();
            return tex;
        }

        public static STexture AddTexture(int W, int H, IntPtr Data)
        {
            return _Draw.AddTexture(W, H, Data);
        }

        public static STexture AddTexture(int W, int H, ref byte[] Data)
        {
            return _Draw.AddTexture(W, H, ref Data);
        }

        public static STexture QuequeTexture(int W, int H, ref byte[] Data)
        {
            return _Draw.QuequeTexture(W, H, ref Data);
        }

        public static bool UpdateTexture(ref STexture Texture, ref byte[] Data)
        {
            return _Draw.UpdateTexture(ref Texture, ref Data);
        }

        public static bool UpdateTexture(ref STexture Texture, IntPtr Data)
        {
            return _Draw.UpdateTexture(ref Texture, Data);
        }

        public static void RemoveTexture(ref STexture Texture)
        {
            _Draw.RemoveTexture(ref Texture);
        }

        public static void DrawTexture(STexture Texture)
        {
            _Draw.DrawTexture(Texture);
        }

        public static void DrawTexture(STexture Texture, SRectF rect)
        {
            _Draw.DrawTexture(Texture, rect);
        }

        public static void DrawTexture(STexture Texture, SRectF rect, SColorF color)
        {
            _Draw.DrawTexture(Texture, rect, color);
        }

        public static void DrawTexture(STexture Texture, SRectF rect, SColorF color, SRectF bounds)
        {
            _Draw.DrawTexture(Texture, rect, color, bounds);
        }

        public static void DrawTexture(STexture Texture, SRectF rect, SColorF color,bool mirrored)
        {
            _Draw.DrawTexture(Texture, rect, color, mirrored);
        }

        public static void DrawTexture(STexture Texture, SRectF rect, SColorF color, SRectF bounds, bool mirrored)
        {
            _Draw.DrawTexture(Texture, rect, color, bounds, mirrored);
        }

        public static void DrawTexture(STexture Texture, SRectF rect, SColorF color, float begin, float end)
        {
            _Draw.DrawTexture(Texture, rect, color, begin, end);
        }

        public static void DrawTextureReflection(STexture Texture, SRectF rect, SColorF color, SRectF bounds, float space, float height)
        {
            _Draw.DrawTextureReflection(Texture, rect, color, bounds, space, height);
        }

        public static int TextureCount()
        {
            return _Draw.TextureCount();
        }
    }
}
