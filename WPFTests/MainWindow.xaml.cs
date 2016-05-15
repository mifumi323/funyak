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
using ImageMagick;
using MifuminSoft.funyak.View.Utility;

namespace WPFTests
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        const int vw = 1920 / 32, vh = 1080 / 32;
        Rectangle[,] rectMap = new Rectangle[vw, vh];
        Rectangle rectMain = null;
        BitmapSource imageMap = null;
        FpsCounter counter = new FpsCounter();

        double vx = 0, vy = 0, vr = 0, vs = 1;

        public MainWindow()
        {
            InitializeComponent();
            using (MagickImage image = new MagickImage("Ice.bmp"))
            {
                imageMap = image.ToBitmapSource();
                imageMap.Freeze();
            }
            var random = new Random();
            for (int x = 0; x < vw; x++)
            {
                for (int y = 0; y < vh; y++)
                {
                    var brush = new ImageBrush(imageMap);
                    brush.TileMode = TileMode.None;
                    brush.Stretch = Stretch.Fill;
                    brush.Viewbox = new Rect(random.Next(5) * 0.2, 0, 0.2, 1);
                    rectMap[x, y] = new Rectangle()
                    {
                        Width = 32 * vs,
                        Height = 32 * vs,
                        Fill = brush,
                    };
                    canvas.Children.Add(rectMap[x, y]);
                }
            }
            rectMain = new Rectangle()
            {
                Width = 32 * vs,
                Height = 32 * vs,
                Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
            };
            canvas.Children.Add(rectMain);
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void CompositionTarget_Rendering(object sender, object e)
        {
            for (int x = 0; x < vw; x++)
            {
                for (int y = 0; y < vh; y++)
                {
                    Canvas.SetLeft(rectMap[x, y], (x * 32 - vx) * vs);
                    Canvas.SetTop(rectMap[x, y], (y * 32 - vy) * vs);
                }
            }
            Canvas.SetLeft(rectMain, (vw * 16 - 16 - (vw - 1) * 16 * Math.Cos(vr) - vx) * vs);
            Canvas.SetTop(rectMain, (vh * 16 - 16 - (vh - 1) * 16 * Math.Sin(vr) - vy) * vs);
            vr += 0.01;
            counter.Step();
            textBlock.Text = "FPS：" + counter.Fps.ToString("0.00");
        }
    }
}
