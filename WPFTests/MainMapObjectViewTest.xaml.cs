using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using MifuminSoft.funyak;
using MifuminSoft.funyak.Data;
using MifuminSoft.funyak.Input;
using MifuminSoft.funyak.MapObject;
using MifuminSoft.funyak.View;
using MifuminSoft.funyak.View.Input;
using MifuminSoft.funyak.View.MapObject;
using MifuminSoft.funyak.View.Resource;
using MifuminSoft.funyak.View.Timing;

namespace WPFTests
{
    /// <summary>
    /// MainMapObjectViewTest.xaml の相互作用ロジック
    /// </summary>
    public partial class MainMapObjectViewTest : Page
    {
        FpsCounter drawFpsCounter = new FpsCounter();
        FpsCounter gameFpsCounter = new FpsCounter();
        ElapsedFrameCounter frameCounter = new ElapsedFrameCounter();
        Map map;
        MapView mapView;
        MainMapObject main;
        IInput input;
        Sprite resource;

        public MainMapObjectViewTest()
        {
            InitializeComponent();

            resource = SpriteReader.Read(@"Assets\main.png");
            input = new KeyInput();

            Reset();
        }

        private void Reset()
        {
            map = MapReader.FromString(File.ReadAllText(@"Assets\mainmapobjectviewtest.json"), new MapReaderOption()
            {
                Input = input,
            });
            main = (map.FindMapObject("main") ?? map.GetMapObjects().FirstOrDefault()) as MainMapObject;
            canvas.Children.Clear();
            mapView = new MapView(map, new MapObjectViewFactory()
            {
                MainMapObjectResourceSelector = a => resource,
            })
            {
                Canvas = canvas,
                FocusTo = main,
            };
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
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
            var frames = frameCounter.GetElapsedFrames(true);
            if (frames > 0)
            {
                drawFpsCounter.Step();
                map.Wind = sliderWind.Value;
                for (int i = 0; i < frames; i++)
                {
                    gameFpsCounter.Step();
                    input.Update();
                    map.Update();
                }
                mapView.Update(sliderScale.Value);
                var message = new StringBuilder();
                message.AppendLine("拡大率：" + sliderScale.Value);
                message.AppendLine("風：" + sliderWind.Value);
                message.AppendLine("描画FPS：" + drawFpsCounter.Fps);
                message.AppendLine("処理FPS：" + gameFpsCounter.Fps);
                message.AppendLine("X：" + main.X);
                message.AppendLine("Y：" + main.Y);
                message.AppendLine("速度X：" + main.VelocityX);
                message.AppendLine("速度Y：" + main.VelocityY);
                message.AppendLine("角度：" + main.Angle);
                message.AppendLine("角速度：" + main.AngularVelocity);
                message.AppendLine("表示オブジェクト：");
                foreach (var child in canvas.Children)
                {
                    message.AppendLine(child.ToString());
                }
                textBox.Text = message.ToString();
            }
        }

        private void buttonReset_Click(object sender, RoutedEventArgs e)
        {
            Reset();
            Keyboard.ClearFocus();
        }

        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }
    }
}
