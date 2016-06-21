using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MifuminSoft.funyak.View.Resource;

namespace WPFTests
{
    /// <summary>
    /// SpriteEditorTest.xaml の相互作用ロジック
    /// </summary>
    public partial class SpriteEditorTest : Page
    {
        public SpriteEditorTest()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            chipSelector.Source = SpriteReader.Read(@"Assets\main.png");
        }

        private void textBoxSource_TextChanged(object sender, TextChangedEventArgs e)
        {
            chipSelector.Refresh();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var newKey = chipSelector.Source.Chip.Keys.Max() + "x";
            chipSelector.Source.Chip[newKey] = new SpriteChipInfo();
            chipSelector.Select(newKey);
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            SpriteWriter.WriteFileInfo(chipSelector.Source, @"Assets\main.png");
        }
    }
}
