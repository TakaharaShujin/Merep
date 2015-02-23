using CUETools.Codecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoogleSpeechLib
{
    public class VoiceRecorder
    {
        [DllImport("winmm.dll")]
        private static extern int mciSendString(string MciComando, string MciRetorno, int MciRetornoLeng, int CallBack);

        string musica = "";
        public static bool BeginRecord()
        {
            try
            {
                mciSendString("open new type waveaudio alias Som", null, 0, 0);
                mciSendString("record Som", null, 0, 0);
                return true;
            }
            catch (Exception ef)
            {
                return false;
            }
        }

        public static bool StopRecord()
        {
            try
            {
                mciSendString("pause Som", null, 0, 0);
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "wave|*.wav";
                if (save.ShowDialog() == DialogResult.OK)
                {
                    mciSendString("save Som " + save.FileName, null, 0, 0);
                    mciSendString("close Som", null, 0, 0);
                    return true;
                }
                return false;
            }
            catch (Exception ef)
            {
                return false;
            }
        }
    }
}
