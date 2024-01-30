using BvDownkr.src.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BvDownkr.src.ViewModels
{
    public class MainWindowVM {
        public static ICommand CloseProgram {
            get {
                return new ReplyCommand(
                    () => {
                        Application.Current.Shutdown();
                    }, true);            
            }
        }
    }
}
