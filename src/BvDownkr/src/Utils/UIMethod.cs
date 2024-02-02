using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BvDownkr.src.Utils {
    internal class UIMethod {
        public static double GetScreenScale() {
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromHwnd(IntPtr.Zero);
            return (graphics.DpiX * 1.041666667) / 100;
        }
        public static Point GetMousePosition() {
            return Mouse.GetPosition(Application.Current.MainWindow);
        }
        public static bool IsMouseInUI<T>(T uiElement, Point mousePos) where T : FrameworkElement {
            // * 主窗口 为坐标系
            Window window = Window.GetWindow(uiElement);
            var uiElementPoint = uiElement.TransformToAncestor(window).Transform(new Point(0d, 0d));
            if (mousePos.X < uiElementPoint.X || 
                mousePos.X > uiElementPoint.X + uiElement.Width ||
                mousePos.Y < uiElementPoint.Y || 
                mousePos.Y > uiElementPoint.Y + uiElement.Height) { return false; }
            else { return true; }
        }
    }
}
