using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoLibrary;
using MediaToolkit;
using System.IO;
using System.Net;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Boolean format = true;

        // T--> MP3   F--> MP4


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Lütfen Klasör Seçiniz:" })
            {
                if(fbd.ShowDialog()==DialogResult.OK)
                {
                    GetTitle();
                    MessageBox.Show("İndirme Başladı Lütfen Bekleyiniz","Bilgi",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    var yt = YouTube.Default;
                    var video = await yt.GetVideoAsync(link.Text);
                    File.WriteAllBytes(fbd.SelectedPath + @"\" + video.FullName, await video.GetBytesAsync());

                    var inputfile = new MediaToolkit.Model.MediaFile { Filename = fbd.SelectedPath + @"\" + video.FullName };
                    var outputfile = new MediaToolkit.Model.MediaFile { Filename = $"{fbd.SelectedPath + @"\" + video.FullName}.mp3" };

                    using (var enging = new Engine())
                    {
                        enging.GetMetadata(inputfile);
                        enging.Convert(inputfile, outputfile);
                    }

                    if (format == true)
                    {
                        File.Delete(fbd.SelectedPath + @"\" + video.FullName);
                    }

                    else
                    {
                        File.Delete($"{fbd.SelectedPath + @"\" + video.FullName}.mp3");
                    }

                }
                else
                {
                    MessageBox.Show("Lütfen Dosya Yolu Seçiniz!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                progressBar1.Value = 100;
                MessageBox.Show("İndirme İşlemi Tamamlandı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void mp3_CheckedChanged(object sender, EventArgs e)
        {
            format = true;
        }

        private void mp4_CheckedChanged(object sender, EventArgs e)
        {
            format = false;
        }
        private void mp3_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void mp4_TextChanged(object sender, EventArgs e)
        {
            
        }

        void GetTitle()
        {
            WebRequest request = HttpWebRequest.Create(link.Text);
            WebResponse answer;
            answer = request.GetResponse();
            StreamReader sr = new StreamReader(answer.GetResponseStream());
            string c = sr.ReadToEnd();
            int start = c.IndexOf("<title>") + 7;
            int finish = c.Substring(start).IndexOf("</title>");
            string cInfo = c.Substring(start, finish);
            title.Text = (cInfo);
        }
        
    }
}
