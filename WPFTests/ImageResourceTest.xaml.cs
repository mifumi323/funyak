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
using MifuminSoft.funyak.View.Resource;

namespace WPFTests
{
    /// <summary>
    /// ImageResourceTest.xaml の相互作用ロジック
    /// </summary>
    public partial class ImageResourceTest : Page
    {
        BitmapImageResource resource;

        public ImageResourceTest()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
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
                    },
                };
            }
            listBox.ItemsSource = resource.Chip;
            Update();
        }

        private void Update()
        {
            // TODO: 表示更新する
        }
    }
}
