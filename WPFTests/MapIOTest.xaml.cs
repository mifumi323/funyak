using System;
using System.Collections.Generic;
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
    /// MapIOTest.xaml の相互作用ロジック
    /// </summary>
    public partial class MapIOTest : Page
    {
        ElapsedFrameCounter frameCounter = new ElapsedFrameCounter();
        Map map;
        MapView mapView;
        IInput input;
        Sprite resource;

        public MapIOTest()
        {
            InitializeComponent();

            resource = SpriteReader.Read(@"Assets\main.png");
            input = new KeyInput();

            map = new Map(100, 100);
            mapView = new MapView(map, new MapObjectViewFactory()
            {
                MainMapObjectResourceSelector = a => resource,
            })
            {
                Canvas = canvas,
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

        private void CompositionTarget_Rendering(object sender, object e)
        {
            var frames = frameCounter.GetElapsedFrames(true);
            if (frames > 0)
            {
                for (int i = 0; i < frames; i++)
                {
                    input.Update();
                    map.Update();
                }
                mapView.Update(sliderScale.Value);
            }
        }

        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }

        private void buttonFromString_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                map = MapReader.FromString(textBox.Text);
                canvas.Children.Clear();
                mapView = new MapView(map, new MapObjectViewFactory()
                {
                    MainMapObjectResourceSelector = a => resource,
                })
                {
                    Canvas = canvas,
                };
                textBlockError.Text = "";
            }
            catch (Exception ex)
            {
                textBlockError.Text = ex.Message;
            }
            e.Handled = true;
        }
    }
}
