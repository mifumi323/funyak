﻿using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using MifuminSoft.funyak.Core;
using MifuminSoft.funyak.Core.MapObject;
using MifuminSoft.funyak.View;
using MifuminSoft.funyak.View.Utility;

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
        LineMapObject line, batten1, batten2;
        Point target;

        public LineMapObjectViewTest()
        {
            InitializeComponent();
            map = new Map(200, 200);
            line = new LineMapObject(50, 100, 150, 150);
            map.AddMapObject(line);
            batten1 = new LineMapObject(-10, -10, 10, 10);
            map.AddMapObject(batten1);
            batten2 = new LineMapObject(-10, 10, 10, -10);
            map.AddMapObject(batten2);
            mapView = new MapView(map)
            {
                Canvas = canvas,
                FocusTo = batten2,
            };
            target = new Point(100, 100);
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
            batten1.X1 = target.X - 10;
            batten1.Y1 = target.Y - 10;
            batten1.X2 = target.X + 10;
            batten1.Y2 = target.Y + 10;
            batten2.X1 = target.X - 10;
            batten2.Y1 = target.Y + 10;
            batten2.X2 = target.X + 10;
            batten2.Y2 = target.Y - 10;
            mapView.Update(1);
            textBlock.Text = "FPS：" + counter.Fps.ToString("0.00");
        }
    }
}
