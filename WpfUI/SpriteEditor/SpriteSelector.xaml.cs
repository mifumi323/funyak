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
            get { return (Sprite)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register(nameof(Source), typeof(Sprite), typeof(SpriteSelector), new PropertyMetadata(0));

        public SpriteSelector()
        {
            InitializeComponent();
        }
    }
}
