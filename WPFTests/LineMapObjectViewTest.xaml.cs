using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using MifuminSoft.funyak.Core;
using MifuminSoft.funyak.Core.Input;
using MifuminSoft.funyak.Core.MapObject;
using MifuminSoft.funyak.View;
using MifuminSoft.funyak.View.Input;
using MifuminSoft.funyak.View.Timing;

namespace WPFTests
{
    /// <summary>
    /// LineMapObjectViewTest.xaml の相互作用ロジック
    /// </summary>
    public partial class LineMapObjectViewTest : Page
    {
        FpsCounter counter = new FpsCounter();
        Map map;
        MapView mapView;
        LineMapObject[] lines;
        LineMapObject batten1, batten2;
        Point target;
        IInput input;

        public LineMapObjectViewTest()
        {
            InitializeComponent();
            map = new Map(500, 500);
            map.BackgroundColor = "LightGreen";
            lines = new[] {
                new LineMapObject(0, 0, 50, 500) { Color = "Black" },
                new LineMapObject(0, 500, 500, 450) { Color = "Black" },
                new LineMapObject(500, 500, 450, 0) { Color = "Black" },
                new LineMapObject(500, 0, 0, 50) { Color = "Black" },
                new LineMapObject(200, 250, 300, 250) { Color = "Gray" },
                new LineMapObject(250, 200, 250, 300) { Color = "Gray" },
            };
            foreach (var line in lines)
            {
                map.AddMapObject(line);
            }
            batten1 = new LineMapObject(-10, -10, 10, 10) { Color = "Red" };
            map.AddMapObject(batten1);
            batten2 = new LineMapObject(-10, 10, 10, -10) { Color = "Red" };
            map.AddMapObject(batten2);
            mapView = new MapView(map)
            {
                Canvas = canvas,
                FocusTo = batten2,
            };
            target = new Point(100, 100);
            input = new KeyInput();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
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

        private void CompositionTarget_Rendering(object sender, object e)
        {
            counter.Step();
            input.Update();
            target.X += input.X;
            target.Y += input.Y;
            batten1.X1 = target.X - 10;
            batten1.Y1 = target.Y - 10;
            batten1.X2 = target.X + 10;
            batten1.Y2 = target.Y + 10;
            batten2.X1 = target.X - 10;
            batten2.Y1 = target.Y + 10;
            batten2.X2 = target.X + 10;
            batten2.Y2 = target.Y - 10;
            mapView.Update(slider.Value);
            var children = new StringBuilder();
            foreach (var child in canvas.Children)
            {
                children.AppendLine(child.ToString());
            }
            textBox.Text = string.Format("拡大率：{0}\nFPS：{1:0.00}\n表示オブジェクト：\n{2}", slider.Value, counter.Fps, children);
        }
    }
}
