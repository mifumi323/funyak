using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsDesktopTests
{
    public partial class FormStart : Form
    {
        public FormStart()
        {
            InitializeComponent();
        }

        private void FormStart_Load(object sender, EventArgs e)
        {
            listBox1.Items.AddRange(new object[] {
                new {
                    Name = "線とキャラを表示するだけのテスト",
                    Type = typeof(FormLineCharaTest),
                },
            });
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            button1.PerformClick();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dynamic selectedItem = listBox1.SelectedItem;
            if (selectedItem == null) return;
            var type = (Type)selectedItem.Type;
            using (var form = (Form)Activator.CreateInstance(type))
            {
                Hide();
                form.ShowDialog(this);
                Show();
            }
        }
    }
}
