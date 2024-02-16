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
    ///     xmlns:local="clr-namespace:BvDownkr.src.Theme"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:local="clr-namespace:BvDownkr.src.Theme;assembly=BvDownkr.src.Theme"
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
    ///     <local:ImageButton/>
    ///
    /// </summary>
    public class ImageButton : Button {
        static ImageButton() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }
        public override void OnApplyTemplate() {
            base.OnApplyTemplate();
            MouseOverBackground ??= Background;
            MouseDownBackground ??= MouseOverBackground;

            MouseOverBorderBrush ??= BorderBrush;
            MouseDownBorderBrush ??= MouseOverBorderBrush;

            MouseOverForeground ??= Foreground;
            MouseDownForeground ??= MouseOverForeground;
        }

        #region Dependency Properties  

        /// <summary>  
        /// 鼠标移上去的背景颜色  
        /// </summary>  
        public static readonly DependencyProperty MouseOverBackgroundProperty
            = DependencyProperty.Register("MouseOverBackground", typeof(Brush), typeof(ImageButton));

        /// <summary>  
        /// 鼠标按下去的背景颜色  
        /// </summary>  
        public static readonly DependencyProperty MouseDownBackgroundProperty
            = DependencyProperty.Register("MouseDownBackground", typeof(Brush), typeof(ImageButton));

        /// <summary>  
        /// 鼠标移上去的字体颜色  
        /// </summary>  
        public static readonly DependencyProperty MouseOverForegroundProperty
            = DependencyProperty.Register("MouseOverForeground", typeof(Brush), typeof(ImageButton), new PropertyMetadata(null, null));

        /// <summary>  
        /// 鼠标按下去的字体颜色  
        /// </summary>  
        public static readonly DependencyProperty MouseDownForegroundProperty
            = DependencyProperty.Register("MouseDownForeground", typeof(Brush), typeof(ImageButton), new PropertyMetadata(null, null));

        /// <summary>  
        /// 鼠标移上去的边框颜色  
        /// </summary>  
        public static readonly DependencyProperty MouseOverBorderBrushProperty
            = DependencyProperty.Register("MouseOverBorderBrush", typeof(Brush), typeof(ImageButton), new PropertyMetadata(null, null));

        /// <summary>  
        /// 鼠标按下去的边框颜色  
        /// </summary>  
        public static readonly DependencyProperty MouseDownBorderBrushProperty
            = DependencyProperty.Register("MouseDownBorderBrush", typeof(Brush), typeof(ImageButton), new PropertyMetadata(null, null));

        /// <summary>  
        /// 圆角  
        /// </summary>  
        public static readonly DependencyProperty CornerRadiusProperty
            = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ImageButton), null);

        //图标  
        public static readonly DependencyProperty IconProperty
            = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(ImageButton), null);

        //鼠标移上去的图标图标  
        public static readonly DependencyProperty IconMouseOverProperty
            = DependencyProperty.Register("IconMouseOver", typeof(ImageSource), typeof(ImageButton), null);

        //鼠标按下去的图标图标  
        public static readonly DependencyProperty IconPressProperty
            = DependencyProperty.Register("IconPress", typeof(ImageSource), typeof(ImageButton), null);

        //图标高度  
        public static readonly DependencyProperty IconHeightProperty
            = DependencyProperty.Register("IconHeight", typeof(double), typeof(ImageButton), new PropertyMetadata(24.0, null));

        //图标宽度  
        public static readonly DependencyProperty IconWidthProperty
            = DependencyProperty.Register("IconWidth", typeof(double), typeof(ImageButton), new PropertyMetadata(24.0, null));

        //图标和内容的对齐方式  
        public static readonly DependencyProperty IconContentOrientationProperty
            = DependencyProperty.Register("IconContentOrientation", typeof(Orientation), typeof(ImageButton), new PropertyMetadata(Orientation.Horizontal, null));

        //图标和内容的距离  
        public static readonly DependencyProperty IconContentMarginProperty
            = DependencyProperty.Register("IconContentMargin", typeof(Thickness), typeof(ImageButton), new PropertyMetadata(new Thickness(0, 0, 0, 0), null));

        #endregion

        #region Property Wrappers
        public Brush MouseOverBackground {
            get => (Brush)GetValue(MouseOverBackgroundProperty);
            set => SetValue(MouseOverBackgroundProperty, value);
        }
        public Brush MouseDownBackground {
            get => (Brush)GetValue(MouseDownBackgroundProperty);
            set => SetValue(MouseDownBackgroundProperty, value);
        }
        public Brush MouseOverForeground {
            get => (Brush)GetValue(MouseOverForegroundProperty);
            set => SetValue(MouseOverForegroundProperty, value);
        }
        public Brush MouseDownForeground {
            get => (Brush)GetValue(MouseDownForegroundProperty);
            set => SetValue(MouseDownForegroundProperty, value);
        }
        public Brush MouseOverBorderBrush {
            get => (Brush)GetValue(MouseOverBorderBrushProperty);
            set => SetValue(MouseOverBorderBrushProperty, value);
        }
        public Brush MouseDownBorderBrush {
            get => (Brush)GetValue(MouseDownBorderBrushProperty);
            set => SetValue(MouseDownBorderBrushProperty, value);
        }
        public CornerRadius CornerRadius {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public ImageSource Icon {
            get => (ImageSource)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        public ImageSource IconMouseOver {
            get => (ImageSource)GetValue(IconMouseOverProperty);
            set => SetValue(IconMouseOverProperty, value);
        }
        public ImageSource IconPress {
            get => (ImageSource)GetValue(IconPressProperty);
            set => SetValue(IconPressProperty, value);
        }
        public double IconHeight {
            get => (double)GetValue(IconHeightProperty);
            set => SetValue(IconHeightProperty, value);
        }
        public double IconWidth {
            get => (double)GetValue(IconWidthProperty);
            set => SetValue(IconWidthProperty, value);
        }
        public Orientation IconContentOrientation {
            get => (Orientation)GetValue(IconContentOrientationProperty);
            set => SetValue(IconContentOrientationProperty, value);
        }
        public Thickness IconContentMargin {
            get => (Thickness)GetValue(IconContentMarginProperty);
            set => SetValue(IconContentMarginProperty, value);
        }
        #endregion
    }
}
