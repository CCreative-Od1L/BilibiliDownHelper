using BvDownkr.src;
using BvDownkr.src.Models;
using BvDownkr.src.Services;
using BvDownkr.src.ViewModels;
using BvDownkr.src.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BvDownkr;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow() {
        InitializeComponent();
        // * Load DataContext
        DataContext = new MainWindowVM();
        // * Init UI
        InitUI();
        // * OpenServices
        OpenServices();
    }
    public void InitUI() {
        // * Default Load(Search Page)
        Area2.Content = PageManager.SearchPage;
        UserInfoContent.Content = PageManager.UserInfoPage;
        DownloadTaskContent.Content = PageManager.DownloadTaskPage;
    }
    public static void OpenServices() {
        UserService.OpenService();
    }
}