using BvDownkr.src.Utils;
using BvDownkr.src.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BvDownkr.src.ViewModels
{
    public class SearchPageVM {
        public static ICommand OpenWindowTest {
            get {
                return new ReplyCommand<Button>(
                    (Button? btn) => {
                        double top_p = 0d;
                        double left_p = 0d;
                        // * 屏幕缩放率
                        Graphics graphics = Graphics.FromHwnd(IntPtr.Zero);
                        var ratio = (graphics.DpiX * 1.041666667) / 100;

                        if (btn != null) {
                            System.Windows.Point point = btn.PointToScreen(new System.Windows.Point(0d, 0d));
                            top_p = point.Y / ratio + btn.ActualHeight;
                            left_p = point.X / ratio + btn.ActualWidth;
                        }
                        
                        var userWindow = new UserWindow {
                            Top = top_p,
                            Left = left_p,
                        };
                        userWindow.Left -= userWindow.Width / 2;

                        userWindow.ShowDialog();
                    }, 
                    true);
            }
            
        }
    }
}
