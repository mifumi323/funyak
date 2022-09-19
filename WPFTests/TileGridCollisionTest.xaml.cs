using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;
using MifuminSoft.funyak;
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
    /// TileGridCollisionTest.xaml の相互作用ロジック
    /// </summary>
    public partial class TileGridCollisionTest : Page
    {
        private readonly FpsCounter counter = new FpsCounter();
        private readonly Map map;
        private readonly MapView mapView;
        private readonly TileGridMapObject tileGridMapObject;
        private readonly TileChip[] tiles;
        private readonly LineMapObject batten1;
        private readonly LineMapObject batten2;
        private Point target;
        private readonly IInput input;

        public TileGridCollisionTest()
        {
            InitializeComponent();
            map = new Map(500, 500)
            {
                BackgroundColor = "LightGreen"
            };
            tileGridMapObject = new TileGridMapObject(100.0, 100.0, 4, 4)
            {
            };
            var tileResource = SpriteReader.Read(@"Assets\Walls.png");
            tiles = Enumerable.Range(0, 16)
                .Select(i => new TileChip()
                {
                    Resource = new TileChipResource(tileResource, i.ToString("x")),
                    HitUpper = (i & 2) != 0,
                    HitBelow = (i & 8) != 0,
                    HitLeft = (i & 1) != 0,
                    HitRight = (i & 4) != 0,
                    Friction = 1.0,
                }).ToArray();
            for (var x = 0; x < tileGridMapObject.TileCountX; x++)
            {
                for (var y = 0; y < tileGridMapObject.TileCountY; y++)
                {
                    tileGridMapObject[x, y] = tiles[(y * tileGridMapObject.TileCountX + x) % 16];
                }
            }
            map.AddMapObject(tileGridMapObject);
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

        private void Page_Unloaded(object sender, RoutedEventArgs e) => CompositionTarget.Rendering -= CompositionTarget_Rendering;

        private void Page_Loaded(object sender, RoutedEventArgs e) => CompositionTarget.Rendering += CompositionTarget_Rendering;

        private void Button_Click(object sender, RoutedEventArgs e)
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
            using var os = new FileStream(path, FileMode.Create);
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
            encoder.Save(os);
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e) => Keyboard.ClearFocus();

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
