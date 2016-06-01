using System.Text;
using System.Windows;
using System.Windows.Controls;
using ImageMagick;

namespace WPFTests
{
    /// <summary>
    /// ImageMagickTest.xaml の相互作用ロジック
    /// </summary>
    public partial class ImageMagickTest : Page
    {
        public ImageMagickTest()
        {
            InitializeComponent();
        }

        private void textBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var data = (string[])e.Data.GetData(DataFormats.FileDrop);
                using (var image = new MagickImage(data[0]))
                {
                    var props = typeof(MagickImage).GetProperties();
                    var sb = new StringBuilder();
                    foreach (var p in props)
                    {
                        sb.AppendLine(string.Format("{0}:{1}", p.Name, p.GetValue(image)));
                    }
                    textBox.Text = sb.ToString();
                }
                e.Handled = true;
            }
        }

        private void textBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
        }
    }
}
