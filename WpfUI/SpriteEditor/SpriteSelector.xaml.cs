using System.Windows;
using System.Windows.Controls;
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
            set
            {
                SetValue(SourceProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(Sprite), typeof(SpriteSelector), new PropertyMetadata(null, new PropertyChangedCallback(OnSourceChanged)));

        public SpriteSelector()
        {
            InitializeComponent();
        }

        public void Refresh()
        {
            if (Source != null)
            {
                Source.SetToRectangle(background, "", 0, 0, 1.0);
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
