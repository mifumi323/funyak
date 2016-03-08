using System;
using System.Drawing;
using System.Windows.Forms;
using MifuminSoft.funyak.Core;
using MifuminSoft.funyak.Core.MapObject;
using MifuminSoft.funyak.View;

namespace WindowsDesktopTests
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            var map = new Map(200, 200);
            var main = new MainMapObject(100, 50);
            var line = new LineMapObject(50, 150, 150, 150);
            map.AddMapObject(main);
            map.AddMapObject(line);
            var img = new Bitmap((int)map.Width, (int)map.Height);
            using (var mapView = new MapView(map))
            {
                using (var graphics = Graphics.FromImage(img))
                {
                    mapView.Display(graphics);
                }
            }
            pictureBox1.Image = img;
        }
    }
}
