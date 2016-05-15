using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsDesktopTests
{
    public partial class FormGraphicsTest : Form
    {
        interface ITest
        {
            string Name { get; }
            void Start(Graphics g);
            void DoWork(Graphics g, int i);
            void End(Graphics g);
        }

        class UnscaledTest : ITest
        {
            Image source;

            public string Name { get { return "画像等倍表示"; } }

            public void Start(Graphics g)
            {
                source = Image.FromFile("icon0.gif");
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.Clear(Color.Black);
            }

            public void DoWork(Graphics g, int i)
            {
                g.DrawImage(source, i % 640, i % 480);
            }

            public void End(Graphics g)
            {
                source.Dispose();
                source = null;
            }
        }

        class TwiceScaledTest : ITest
        {
            Image source;

            public string Name { get { return "画像二倍表示"; } }

            public void Start(Graphics g)
            {
                source = Image.FromFile("icon0.gif");
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.Clear(Color.Black);
            }

            public void DoWork(Graphics g, int i)
            {
                g.DrawImage(source, i % 640, i % 480, source.Width * 2, source.Height * 2);
            }

            public void End(Graphics g)
            {
                source.Dispose();
                source = null;
            }
        }

        class HalfScaledTest : ITest
        {
            Image source;

            public string Name { get { return "画像半分表示"; } }

            public void Start(Graphics g)
            {
                source = Image.FromFile("icon0.gif");
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.Clear(Color.Black);
            }

            public void DoWork(Graphics g, int i)
            {
                g.DrawImage(source, i % 640, i % 480, source.Width / 2, source.Height / 2);
            }

            public void End(Graphics g)
            {
                source.Dispose();
                source = null;
            }
        }

        public FormGraphicsTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var item = listBox1.SelectedItem as ITest;
            if (item == null) return;
            int i = 0;
            var img = new Bitmap(640, 480);
            if (pictureBox1.Image != null)
            {
                var oldImage = pictureBox1.Image;
                pictureBox1.Image = null;
                oldImage.Dispose();
            }
            using (var g = Graphics.FromImage(img))
            {
                item.Start(g);
                for (long ticks = DateTime.Now.Ticks; DateTime.Now.AddMilliseconds(-10).Ticks < ticks;)
                {
                    item.DoWork(g, i);
                    i++;
                }
                item.End(g);
            }
            pictureBox1.Image = img;
            MessageBox.Show("描画回数：" + i + "回");
        }

        private void FormGraphicsTest_Load(object sender, EventArgs e)
        {
            listBox1.Items.AddRange(new ITest[] {
                new UnscaledTest(),
                new TwiceScaledTest(),
                new HalfScaledTest(),
            });
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            button1.PerformClick();
        }
    }
}
