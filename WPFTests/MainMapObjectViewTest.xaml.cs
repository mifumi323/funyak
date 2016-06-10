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
using MifuminSoft.funyak.Core;
using MifuminSoft.funyak.Core.Input;
using MifuminSoft.funyak.Core.MapObject;
using MifuminSoft.funyak.View;
using MifuminSoft.funyak.View.Input;
using MifuminSoft.funyak.View.MapObject;
using MifuminSoft.funyak.View.Resource;
using MifuminSoft.funyak.View.Utility;

namespace WPFTests
{
    /// <summary>
    /// MainMapObjectViewTest.xaml の相互作用ロジック
    /// </summary>
    public partial class MainMapObjectViewTest : Page
    {
        FpsCounter counter = new FpsCounter();
        Map map;
        MapView mapView;
        LineMapObject[] lines;
        MainMapObject main;
        IInput input;
        BitmapImageResource resource;

        public MainMapObjectViewTest()
        {
            InitializeComponent();
            using (var stream = new FileStream(@"Assets\main.png", FileMode.Open))
            {
                resource = new BitmapImageResource(stream, null, true)
                {
                    Chip = new Dictionary<string, ImageChipInfo>()
                    {
                        {
                            "StandF",
                            new ImageChipInfo()
                            {
                                SourceLeft = 0,
                                SourceTop = 0,
                                SourceWidth = 40,
                                SourceHeight = 40,
                                SourceOriginX = 20,
                                SourceOriginY = 20,
                                DestinationWidth = 40,
                                DestinationHeight = 40,
                            }
                        },
                        {
                            "JumpF",
                            new ImageChipInfo()
                            {
                                SourceLeft = 360,
                                SourceTop = 0,
                                SourceWidth = 40,
                                SourceHeight = 40,
                                SourceOriginX = 380,
                                SourceOriginY = 20,
                                DestinationWidth = 40,
                                DestinationHeight = 40,
                            }
                        },
                        {
                            "FallF",
                            new ImageChipInfo()
                            {
                                SourceLeft = 0,
                                SourceTop = 40,
                                SourceWidth = 40,
                                SourceHeight = 40,
                                SourceOriginX = 20,
                                SourceOriginY = 60,
                                DestinationWidth = 40,
                                DestinationHeight = 40,
                            }
                        },
                        {
                            "SitF",
                            new ImageChipInfo()
                            {
                                SourceLeft = 40,
                                SourceTop = 40,
                                SourceWidth = 40,
                                SourceHeight = 40,
                                SourceOriginX = 60,
                                SourceOriginY = 60,
                                DestinationWidth = 40,
                                DestinationHeight = 40,
                            }
                        },
                    },
                };
            }
            map = new Map(500, 500);
            map.BackgroundColor = "LightGreen";
            lines = new[] {
                new LineMapObject(0, 0, 50, 500),
                new LineMapObject(0, 500, 500, 450),
                new LineMapObject(500, 500, 450, 0),
                new LineMapObject(500, 0, 0, 50),
                new LineMapObject(200, 250, 300, 250),
                new LineMapObject(250, 200, 250, 300),
            };
            foreach (var line in lines)
            {
                map.AddMapObject(line);
            }
            main = new MainMapObject(250, 250);
            map.AddMapObject(main);
            mapView = new MapView(map, new MapObjectViewFactory()
            {
                MainMapObjectResourceSelector = a => resource,
            })
            {
                Canvas = canvas,
                FocusTo = main,
            };
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
            main.X += input.X;
            main.Y += input.Y;
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
