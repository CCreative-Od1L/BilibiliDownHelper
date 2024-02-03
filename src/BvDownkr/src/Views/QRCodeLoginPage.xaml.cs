using BvDownkr.src.Models;
using BvDownkr.src.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BvDownkr.src.Views {
    /// <summary>
    /// QRCodeLoginPage.xaml 的交互逻辑
    /// </summary>
    public partial class QRCodeLoginPage : Page {
        public QRCodeLoginPage() {
            InitializeComponent();
            var model = new QRCodeLoginModel();
            // * Load Model
            LoadModel(model);
            // * Load DataContext
            DataContext = new QRCodeLoginVM(model);
        }
        public void LoadModel(QRCodeLoginModel model) {
            model.QRcodeImage = qrcodeImage;
        }
    }
}
