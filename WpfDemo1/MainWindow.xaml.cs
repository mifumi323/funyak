using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using MifuminSoft.funyak;
using MifuminSoft.funyak.Data;
using MifuminSoft.funyak.Input;
using MifuminSoft.funyak.MapEnvironment;
using MifuminSoft.funyak.MapObject;
using MifuminSoft.funyak.View;
using MifuminSoft.funyak.View.Input;
using MifuminSoft.funyak.View.MapObject;
using MifuminSoft.funyak.View.Resource;
using MifuminSoft.funyak.View.Timing;

namespace WpfDemo1
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        ElapsedFrameCounter frameCounter = new ElapsedFrameCounter();
        Map map = null;
        MapView mapView = null;
        IInput input;
        Sprite resource;
        IMapObject mainObject = null;
        AreaEnvironment goalArea = null;
        bool goaled = false;

        public MainWindow()
        {
            InitializeComponent();

            resource = SpriteReader.Read(@"Assets\main.png");
            input = new KeyInput();

            ReadMap(File.ReadAllText(@"Assets\demo1map.json"));
        }

        private void ReadMap(string data)
        {
            map = MapReader.FromString(data, new MapReaderOption()
            {
                Input = input,
            });
            mainObject = map.FindMapObject("main") ?? map.GetMapObjects().FirstOrDefault();
            goalArea = map.FindAreaEnvironment("goal");
            canvas.Children.Clear();
            mapView = new MapView(map, new MapObjectViewFactory()
            {
                MainMapObjectResourceSelector = a => resource,
            })
            {
                Canvas = canvas,
                FocusTo = mainObject
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
        }

        private void CompositionTarget_Rendering(object sender, object e)
        {
            var frames = frameCounter.GetElapsedFrames(true);
            if (frames > 0 && map != null)
            {
                for (int i = 0; i < frames; i++)
                {
                    input.Update();
                    map.Update();

                    if (!goaled)
                    {
                        if (map.GetEnvironment(mainObject.X, mainObject.Y) == goalArea)
                        {
                            goaled = true;
                            label.Visibility = Visibility.Visible;
                        }
                    }
                }
                mapView.Update(1.0);
            }
        }
    }
}
