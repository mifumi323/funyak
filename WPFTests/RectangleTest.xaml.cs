using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ImageMagick;
using Microsoft.Win32;
using MifuminSoft.funyak.View.Utility;

namespace WPFTests
{
    /// <summary>
    /// RectangleTest.xaml の相互作用ロジック
    /// </summary>
    public partial class RectangleTest : Page
    {
        const int vw = 1920 / 32, vh = 1080 / 32;
        Rectangle[,] rectMap = new Rectangle[vw, vh];
        Rectangle rectMain = null;
        BitmapSource imageMap = null;
        FpsCounter counter = new FpsCounter();

        double vx = 0, vy = 0, vr = 0, vs = 1;

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (MagickImage image = new MagickImage("Assets/Ice.bmp"))
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog()
            {
                Filter = "PNGファイル|*.png",
                DefaultExt = "png",
            };
            if (dlg.ShowDialog() != true) return;

            var canvas = this.canvas;
            var size = new Size(canvas.ActualWidth, canvas.ActualHeight);
            canvas.Measure(size);
            canvas.Arrange(new Rect(size));

            var renderBitmap = new RenderTargetBitmap((int)size.Width,
                                                      (int)size.Height,
                                                      96.0d,
                                                      96.0d,
                                                      PixelFormats.Pbgra32);
            renderBitmap.Render(canvas);

            string path = dlg.FileName;
            using (var os = new FileStream(path, FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(os);
            }
        }

        public RectangleTest()
        {
            InitializeComponent();
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
