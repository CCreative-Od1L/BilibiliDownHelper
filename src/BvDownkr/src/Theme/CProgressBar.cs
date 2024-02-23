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

namespace BvDownkr.src.Theme {
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:BvDownkr.src.Theme"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:BvDownkr.src.Theme;assembly=BvDownkr.src.Theme"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:CProgressBar/>
    ///
    /// </summary>
    public class CProgressBar : ProgressBar {
        static CProgressBar() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CProgressBar), new FrameworkPropertyMetadata(typeof(CProgressBar)));
        }
        public override void OnApplyTemplate() {
            base.OnApplyTemplate();
        }
        #region Dependency Properties
        // * 进度条背景色
        public static readonly DependencyProperty CpbBackgroundProperty
            = DependencyProperty.Register(nameof(CpbBackground), typeof(Brush), typeof(CProgressBar), new PropertyMetadata(Brushes.Black));
        // * 进度条前景色
        public static readonly DependencyProperty CpbForegroundProperty
            = DependencyProperty.Register(nameof(CpbForeground), typeof(Brush), typeof(CProgressBar), new PropertyMetadata(Brushes.Black));
        // * 进度条外边缘色
        public static readonly DependencyProperty CpbBorderBrushProperty
            = DependencyProperty.Register(nameof(CpbBorderBrush), typeof(Brush), typeof(CProgressBar), new PropertyMetadata(Brushes.Black));
        // * 进度条外边缘宽度
        public static readonly DependencyProperty CpbBorderThicknessProperty
            = DependencyProperty.Register(nameof(CpbBorderThickness), typeof(Thickness), typeof(CProgressBar), new PropertyMetadata(new Thickness(1)));
        // * 进度条前景边缘色
        public static readonly DependencyProperty CpbForeBorderBrushProperty
            = DependencyProperty.Register(nameof(CpbForeBorderBrush), typeof(Brush), typeof(CProgressBar), new PropertyMetadata(Brushes.Black));
        // * 进度条前景边缘宽度
        public static readonly DependencyProperty CpbForeBorderThicknessProperty
            = DependencyProperty.Register(nameof(CpbForeBorderThickness), typeof(Thickness), typeof(CProgressBar), new PropertyMetadata(new Thickness(1)));
        // * 进度条圆角
        public static readonly DependencyProperty CpbCornerRadiusProperty
            = DependencyProperty.Register(nameof(CpbCornerRadius), typeof(CornerRadius), typeof(CProgressBar), new PropertyMetadata(new CornerRadius(0)));
        #endregion

        #region Property Wrappers
        public Brush CpbBackground {
            get => (Brush)GetValue(CpbBackgroundProperty);
            set => SetValue(CpbBackgroundProperty, value);
        }
        public Brush CpbForeground {
            get => (Brush)GetValue(CpbForegroundProperty);
            set => SetValue(CpbForegroundProperty, value);
        }
        public Brush CpbBorderBrush {
            get => (Brush)GetValue(CpbBorderBrushProperty);
            set => SetValue(CpbBorderBrushProperty, value);
        }
        public Thickness CpbBorderThickness {
            get => (Thickness)GetValue(CpbBorderThicknessProperty);
            set => SetValue(CpbBorderThicknessProperty, value);
        }
        public Brush CpbForeBorderBrush {
            get => (Brush)GetValue(CpbForeBorderBrushProperty);
            set => SetValue(CpbForeBorderBrushProperty, value);
        }
        public Thickness CpbForeBorderThickness {
            get => (Thickness)GetValue(CpbForeBorderThicknessProperty);
            set => SetValue(CpbForeBorderThicknessProperty, value);
        }
        public CornerRadius CpbCornerRadius {
            get => (CornerRadius)GetValue(CpbCornerRadiusProperty);
            set => SetValue(CpbCornerRadiusProperty, value);
        }
        #endregion
    }
}
