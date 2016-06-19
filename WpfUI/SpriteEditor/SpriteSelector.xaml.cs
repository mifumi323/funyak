using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using MifuminSoft.funyak.View.Resource;

namespace MifuminSoft.funyak.UI.SpriteEditor
{
    /// <summary>
    /// Spriteを編集のため表示・選択します
    /// </summary>
    public partial class SpriteSelector : UserControl
    {
        public Sprite Source
        {
            get { return (Sprite)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(Sprite), typeof(SpriteSelector), new PropertyMetadata(null, new PropertyChangedCallback(OnSourceChanged)));


        public SpriteChipInfo SelectedChip
        {
            get { return (SpriteChipInfo)GetValue(SelectedChipProperty); }
            set { SetValue(SelectedChipProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedChipProperty =
            DependencyProperty.Register(nameof(SelectedChip), typeof(SpriteChipInfo), typeof(SpriteSelector), new PropertyMetadata(null, new PropertyChangedCallback(OnSourceChanged)));


        public Color ChipColor
        {
            get { return (Color)GetValue(ChipColorProperty); }
            set { SetValue(ChipColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChipColorProperty =
            DependencyProperty.Register(nameof(ChipColor), typeof(Color), typeof(SpriteSelector), new PropertyMetadata(Colors.Black, new PropertyChangedCallback(OnSourceChanged)));


        public Color SelectedChipColor
        {
            get { return (Color)GetValue(SelectedChipColorProperty); }
            set { SetValue(SelectedChipColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedChipColorProperty =
            DependencyProperty.Register(nameof(SelectedChipColor), typeof(Color), typeof(SpriteSelector), new PropertyMetadata(Colors.Red, new PropertyChangedCallback(OnSourceChanged)));


        public SpriteSelector()
        {
            InitializeComponent();
        }

        public void Refresh()
        {
            canvas.Children.RemoveRange(1, canvas.Children.Count - 1);
            var source = Source;
            if (source != null)
            {
                source.SetToRectangle(background, "", 0, 0, 1.0);
                foreach (var chip in source.Chip.Values)
                {
                    var rectangle = new Rectangle()
                    {
                        Stroke = new SolidColorBrush(chip == SelectedChip ? SelectedChipColor : ChipColor),
                        Width = chip.SourceWidth,
                        Height = chip.SourceHeight,
                    };
                    canvas.Children.Add(rectangle);
                    Canvas.SetLeft(rectangle, chip.SourceLeft);
                    Canvas.SetTop(rectangle, chip.SourceTop);
                    Panel.SetZIndex(rectangle, 1);
                }
            }
            else
            {
                background.Fill = null;
            }
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SpriteSelector)?.Refresh();
        }
    }
}
