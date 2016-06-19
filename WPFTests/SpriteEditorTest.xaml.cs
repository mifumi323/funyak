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
            spriteSelector.Source = SpriteReader.Read(@"Assets\main.png");
        }
    }
}
