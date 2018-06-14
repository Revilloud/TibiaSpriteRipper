using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace TibiaSpriteSheetExtractor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }


        private void print (string msg) { MessageBox.Show(msg); }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            print("DONE.");
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!Directory.Exists(Application.StartupPath + "/download/"))
            {
                Directory.CreateDirectory(Application.StartupPath + "/download/");
            }
            string[] lines = File.ReadAllLines(Application.StartupPath + "/spritesheets.txt");
            List<string> list = lines.ToList<string>();

            foreach (string line in list.ToArray())
                if (!line.Contains("<url>")) { list.Remove(line); }

            list.RemoveAt(0);
            list.ForEach(delegate (string line)
            {
                string garage = line.Remove(0, 7);
                garage = garage.Remove(garage.Length - 6, 6);
                WebClient cliente = new WebClient();
                
                cliente.DownloadFile(garage, Application.StartupPath + "/download/" + list.IndexOf(line) + ".png");
                int i = (list.IndexOf(line) * 100) / list.Count;
                backgroundWorker1.ReportProgress(i);
            });
            backgroundWorker1.ReportProgress(100);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }
    }
}
