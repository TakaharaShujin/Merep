using CUETools.Codecs;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoogleSpeechLib
{
    public class VoiceRecorder
    {
        static WaveIn sourceStream = null;
        static DirectSoundOut waveOut = null;
        static WaveFileWriter waveWriter = null;
        public static string SuankiKomut = "";
        public static void BeginRecord()
        {
            //MessageBox.Show("Başlatıldı.");
            int deviceNumber = RefreshMics();

            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Wave File (*.wav)|*.wav;";

            SuankiKomut = Application.StartupPath + "\\komutlar\\kmt_" + DateTime.Now.ToShortDateString() + DateTime.Now.Millisecond + ".wav";
            save.FileName = SuankiKomut;

            sourceStream = new WaveIn();
            sourceStream.DeviceNumber = deviceNumber;
            sourceStream.WaveFormat = new WaveFormat(44100, WaveIn.GetCapabilities(deviceNumber).Channels);
            sw = new StreamWriter(Application.StartupPath + "\\bos.txt", false, ASCIIEncoding.Default);
            sourceStream.DataAvailable += new EventHandler<WaveInEventArgs>(sourceStream_DataAvailable);
            waveWriter = new WaveFileWriter(save.FileName, sourceStream.WaveFormat);
            sourceStream.StartRecording();
        }
        public static StreamWriter sw = null;
        static void sourceStream_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (waveWriter == null)
            {
                sw.Close();
                return;
            }

            foreach (var item in e.Buffer)
            {
                sw.Write(item+" ");
            }
            //waveWriter.WriteData(e.Buffer, 0, e.BytesRecorded);
            //waveWriter.Flush();
        }

        static Dictionary<string, int> Mikrofonlar = new Dictionary<string, int>();
        public static int RefreshMics()
        {
            List<WaveInCapabilities> sources = new List<WaveInCapabilities>();
            for (int i = 0; i < WaveIn.DeviceCount; i++)
                sources.Add(WaveIn.GetCapabilities(i));

            Mikrofonlar.Clear();

            foreach (var source in sources)
                Mikrofonlar.Add(source.ProductName, source.Channels);

            for (int i = 0; i < Mikrofonlar.Count; i++)
                return i;

            return -1;
        }
        public static void StopRecord()
        {
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }
            if (sourceStream != null)
            {
                sourceStream.StopRecording();
                sourceStream.Dispose();
                sourceStream = null;
            }
            if (waveWriter != null)
            {
                waveWriter.Dispose();
                waveWriter = null;
            }

            //MessageBox.Show("Durduruldu.");
        }
    }
}
