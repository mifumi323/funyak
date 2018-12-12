using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using MifuminSoft.funyak.Input;
using MifuminSoft.funyak.View.Input;

namespace WPFTests
{
    /// <summary>
    /// KeyInputTest.xaml の相互作用ロジック
    /// </summary>
    public partial class KeyInputTest : Page
    {
        KeyInput keyInput;

        class StatusControls
        {
            public Rectangle pushed;
            public Rectangle pressed;
            public Rectangle released;
        }
        Dictionary<int, StatusControls> statusControls;
        Brush brushOn, brushOff;

        public KeyInputTest()
        {
            InitializeComponent();
            statusControls = new Dictionary<int, StatusControls>();
            int row = 1;
            foreach (int key in Enum.GetValues(typeof(Keys)))
            {
                keyTable.RowDefinitions.Add(new RowDefinition());
                var sc = statusControls[key] = new StatusControls()
                {
                    pushed = new Rectangle(),
                    pressed = new Rectangle(),
                    released = new Rectangle(),
                };
                var label = new TextBlock()
                {
                    Text = Enum.GetName(typeof(Keys), key),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                keyTable.Children.Add(label);
                Grid.SetRow(label, row);
                Grid.SetColumn(label, 0);
                keyTable.Children.Add(sc.pushed);
                Grid.SetRow(sc.pushed, row);
                Grid.SetColumn(sc.pushed, 1);
                keyTable.Children.Add(sc.pressed);
                Grid.SetRow(sc.pressed, row);
                Grid.SetColumn(sc.pressed, 2);
                keyTable.Children.Add(sc.released);
                Grid.SetRow(sc.released, row);
                Grid.SetColumn(sc.released, 3);
                row++;
            }
            keyInput = new KeyInput();
            brushOn = new SolidColorBrush(Colors.Red);
            brushOff = new SolidColorBrush(Colors.Transparent);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering += CompositionTarget_Rendering;
            direction.Focus();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e) => CompositionTarget.Rendering -= CompositionTarget_Rendering;

        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e) => Keyboard.ClearFocus();

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            keyInput.Update();
            direction.Margin = new Thickness(keyInput.X * 96, keyInput.Y * 96, 0, 0);
            foreach (int key in Enum.GetValues(typeof(Keys)))
            {
                var sc = statusControls[key];
                sc.pushed.Fill = keyInput.IsPushed((Keys)key) ? brushOn : brushOff;
                sc.pressed.Fill = keyInput.IsPressed((Keys)key) ? brushOn : brushOff;
                sc.released.Fill = keyInput.IsReleased((Keys)key) ? brushOn : brushOff;
            }
        }
    }
}
