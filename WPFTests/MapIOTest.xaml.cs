using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MifuminSoft.funyak;
using MifuminSoft.funyak.Data;
using MifuminSoft.funyak.Input;
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
        Map map = null;
        MapView mapView = null;
        IInput input;
        Sprite resource;

        public MapIOTest()
        {
            InitializeComponent();

            resource = SpriteReader.Read(@"Assets\main.png");
            input = new KeyInput();

            textBox.Text = File.ReadAllText(@"Assets\mapiotest.json");
            ReadMapFromTextBox();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e) => CompositionTarget.Rendering -= CompositionTarget_Rendering;

        private void Page_Loaded(object sender, RoutedEventArgs e) => CompositionTarget.Rendering += CompositionTarget_Rendering;

        private void CompositionTarget_Rendering(object sender, object e)
        {
            var frames = frameCounter.GetElapsedFrames(true);
            if (frames > 0 && map != null)
            {
                for (int i = 0; i < frames; i++)
                {
                    input.Update();
                    map.Update();
                }
                mapView.Update(sliderScale.Value);
            }
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e) => Keyboard.ClearFocus();

        private void ButtonFromString_Click(object sender, RoutedEventArgs e)
        {
            ReadMapFromTextBox();
            Keyboard.ClearFocus();
            e.Handled = true;
        }

        private void ReadMapFromTextBox()
        {
            try
            {
                map = MapReader.FromString(textBox.Text, new MapReaderOption()
                {
                    Input = input,
                });
                canvas.Children.Clear();
                mapView = new MapView(map, new MapObjectViewFactory()
                {
                    MainMapObjectResourceSelector = a => resource,
                })
                {
                    Canvas = canvas,
                    FocusTo = map.FindMapObject("main") ?? map.GetMapObjects().FirstOrDefault()
                };
                textBlockError.Text = "";
            }
            catch (Exception ex)
            {
                textBlockError.Text = ex.Message;
            }
        }

        private void TextBlockError_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }
    }
}
