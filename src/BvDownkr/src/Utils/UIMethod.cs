using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace BvDownkr.src.Utils {
    internal class UIMethod {
        public static double GetScreenScale() {
            Graphics graphics = Graphics.FromHwnd(IntPtr.Zero);
            return (graphics.DpiX * 1.041666667) / 100;
        }
        public static System.Windows.Point GetMousePosition() {
            return Mouse.GetPosition(Application.Current.MainWindow);
        }
        public static bool IsMouseInUI<T>(T uiElement, System.Windows.Point mousePos) where T : FrameworkElement {
            // * 主窗口 为坐标系
            Window window = Window.GetWindow(uiElement);
            var uiElementPoint = uiElement.TransformToAncestor(window).Transform(new System.Windows.Point(0d, 0d));
            if (mousePos.X < uiElementPoint.X || 
                mousePos.X > uiElementPoint.X + uiElement.Width ||
                mousePos.Y < uiElementPoint.Y || 
                mousePos.Y > uiElementPoint.Y + uiElement.Height) { return false; }
            else { return true; }
        }
        public static BitmapSource GetBitmapSource(byte[] rawData) {
            Bitmap? bitmap = null;
            using (MemoryStream ms = new(rawData)) {
                bitmap = Image.FromStream(ms) as Bitmap;
            }

            IntPtr hBitmap = bitmap!.GetHbitmap();
            return Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
