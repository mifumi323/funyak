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
                    Name = "キー入力のテスト",
                    Type = typeof(KeyInputTest),
                },
                new {
                    Name = "グラフィックのテスト",
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
