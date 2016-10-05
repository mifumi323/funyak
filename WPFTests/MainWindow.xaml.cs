using System;
using System.Windows;
using System.Windows.Controls;

namespace WPFTests
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            comboBox.ItemsSource = new object[] {
                new {
                    Name = "通常の単体テスト",
                    Type = typeof(NormalUnitTest),
                },
                new {
                    Name = "マップ入出力",
                    Type = typeof(MapIOTest),
                },
                new {
                    Name = "SripteEditor",
                    Type = typeof(SpriteEditorTest),
                },
                new {
                    Name = "MainMapObject",
                    Type = typeof(MainMapObjectViewTest),
                },
                new {
                    Name = "画像チップ表示",
                    Type = typeof(ImageResourceTest),
                },
                new {
                    Name = "画像情報",
                    Type = typeof(ImageMagickTest),
                },
                new {
                    Name = "LineMapObject",
                    Type = typeof(LineMapObjectViewTest),
                },
                new {
                    Name = "キー入力",
                    Type = typeof(KeyInputTest),
                },
                new {
                    Name = "グラフィック",
                    Type = typeof(RectangleTest),
                },
            };
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic selectedItem = comboBox.SelectedItem;
            if (selectedItem == null) return;
            var type = (Type)selectedItem.Type;
            var page = (Page)Activator.CreateInstance(type);
            frame.Navigate(page);
        }
    }
}
