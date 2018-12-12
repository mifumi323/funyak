using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MifuminSoft.funyak.View.Resource;

namespace WPFTests
{
    /// <summary>
    /// ImageResourceTest.xaml の相互作用ロジック
    /// </summary>
    public partial class ImageResourceTest : Page
    {
        Sprite resource;
        Point point = new Point(0, 0);

        public ImageResourceTest() => InitializeComponent();

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            point.X = Math.Round(canvas.ActualWidth / 2);
            point.Y = Math.Round(canvas.ActualHeight / 2);

            resource = SpriteReader.Read(@"Assets\main.png");

            listBox.ItemsSource = resource.Chip;
            listBox.SelectedItem = resource.Chip.First();
        }

        private void Update()
        {
            if (listBox.SelectedItem == null) return;
            var selected = (KeyValuePair<string, SpriteChipInfo>)listBox.SelectedItem;
            var key = selected.Key;
            var scale = sliderScale.Value;
            var angle = sliderRotate.Value;
            resource.SetToRectangle(target, key, 0, point.X, point.Y, scale, angle);
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => Update();

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => Update();

        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            point = e.GetPosition(canvas);
            Update();
        }
    }
}
