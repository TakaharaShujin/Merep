using GoogleSpeech;
using GoogleSpeechLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;

namespace SesliKomut1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        public String AppTitle
        {
            get { return "Google Voice Search Test v2.0"; }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        string sFilePath = "";
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Open Wave File";
            openFileDialog1.Filter = "Wave files (*.wav)|*.wav";
            openFileDialog1.InitialDirectory = Application.StartupPath + "\\komutlar\\";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            sFilePath = openFileDialog1.FileName;
            if (sFilePath == "")
            {
                return;
            }

            txtPath.Text = sFilePath;
            SendFileRun(sFilePath);
            sFilePath = "";
        }
        private void SendFile(String filePath)
        {
            try
            {
                ReportOnProgress(10, "İstek başladı!");
                string responseFromServer = GoogleVoice.GoogleSpeechRequest(filePath, "tmp.flac");
                responseFromServer = responseFromServer
                    .Replace("{\"result\":[]}\n{\"result\":[", "")
                    .Replace("],\"result_index\":0}", "");

                var table = JsonConvert.DeserializeObject<Results>(responseFromServer).Alternatives;
                txtLog.Clear();
                //AddLog(table.Rows[0].ItemArray[0].ToString());

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    var item = table.Rows[i];
                    if (item.ItemArray.Count() > 1)
                    {
                        if (item.ItemArray[1].ToString() != "")
                        {
                            AddLog(item.ItemArray[0].ToString());
                            //AddLog(item.ItemArray[0].ToString() + "--" + item.ItemArray[1].ToString() + System.Environment.NewLine);
                            return;
                        }
                    }
                    else
                    {
                        AddLog(item.ItemArray[0].ToString() + System.Environment.NewLine);
                    }
                }

                ReportOnProgress(100, "Sorgu başarıyla tamamlandı");
            }
            catch (Exception e)
            {
                ReportOnProgress(100, "İstek başarısız! Hata Sebebi : " + e.Message);
                AddLog(e.ToString());
            }

        }

        private void SendFileRun(String filePath)
        {
            DisableStartButtons();
            DoWorkDelegate worker = new DoWorkDelegate(SendFile);
            worker.BeginInvoke(filePath, new AsyncCallback(DoWorkComplete), worker);
        }
        private void DisableStartButtons()
        {
            btnBrowse.Enabled = false;
            btnSearch.Enabled = false;
        }
        private delegate void DoWorkDelegate(String filePath);
        private void DoWorkComplete(IAsyncResult workID)
        {
            EnableStartButtons();
            DoWorkDelegate worker = workID.AsyncState as DoWorkDelegate;
            worker.EndInvoke(workID);
        }
        private void EnableStartButtons()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(EnableStartButtons));
            }
            else
            {
                btnBrowse.Enabled = true;
                btnSearch.Enabled = true;
            }

        }

        private delegate void ReportOnProgressDelegate(int progress, string msg);
        private void ReportOnProgress(int progress, string msg)
        {
            if (InvokeRequired)
            {
                Invoke(new ReportOnProgressDelegate(ReportOnProgress),
                  new object[] { progress, msg });
                return;
                ;
            }
            this.Text = this.Text + " —  " + msg;
        }

        private delegate void AddDelegate(String log);
        private void AddLog(string log)
        {
            if (InvokeRequired)
            {
                Invoke(new AddDelegate(AddLog), new object[] { log });
                return;
            }
            txtLog.Text += log;
        }


        public class Results
        {
            [JsonProperty(PropertyName = "alternative")]
            public System.Data.DataTable Alternatives { get; set; }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (sFilePath == "")
                SendFileRun(VoiceRecorder.SuankiKomut);
            else
                SendFileRun(sFilePath);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.BackColor == Color.Black)//pasif
            {
                button1.BackColor = Color.Red;

                VoiceRecorder.BeginRecord();

            }
            else
            {
                button1.BackColor = Color.Black;

                VoiceRecorder.StopRecord();
            }

        }
    }
}
