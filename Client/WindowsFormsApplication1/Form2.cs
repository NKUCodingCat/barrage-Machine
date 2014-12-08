using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Configuration;
using System.Net.Cache;
namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        int Count = 0;
        public String FontNow = "";
        public int SizeNow = 0;
        public int SpeedNow = 0;
        public String Url = "";
        public Form2()
        {
            InitializeComponent();
            System.Drawing.Text.InstalledFontCollection fonts = new System.Drawing.Text.InstalledFontCollection();
            object[] Arr = new object[fonts.Families.Length];
            int Mark = 0;
            for (int i = 0; i < fonts.Families.Length; i++)
            {
                Arr[i] = fonts.Families[i].Name.ToString();
                if (Arr[i].Equals("SimHei") || Arr[i].Equals("黑体")) Mark = i;
            }
            this.comboBox1.Items.AddRange(Arr);
            this.comboBox1.SelectedIndex = Mark;
            object[] Arr2 = new object[29];
            for (int i = 0; i < Arr2.Length; i++)
            {
                Arr2[i] = 2 * i + 16;
            }
            this.comboBox2.Items.AddRange(Arr2);
            this.comboBox2.SelectedIndex = 7;
            FontNow = this.comboBox1.Text.ToString();
            SizeNow = int.Parse(this.comboBox2.Text.ToString());
            object[] Arr3 = new object[9];
            for (int i = 0; i < Arr3.Length; i++)
            {
                Arr3[i] = i + 1;
            }
            this.comboBox3.Items.AddRange(Arr3);
            this.comboBox3.SelectedIndex = 4;
            this.SpeedNow = int.Parse(this.comboBox3.Text.ToString());
            this.Url = ReadConfig("Url");
            this.textBox1.Text = this.Url;
        }
        private String ReadConfig(String Item)
        {
            return ConfigurationManager.AppSettings[Item];
        }
        private void WriteConfig(String Item,String Value)
        {
            string assemblyConfigFile = Assembly.GetEntryAssembly().Location;
            string appDomainConfigFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            AppSettingsSection appSettings = (AppSettingsSection)config.GetSection("appSettings");
            appSettings.Settings.Remove(Item);
            appSettings.Settings.Add(Item, Value);
            config.Save();
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            //this.Icon = new Icon("C:\\Users\\dell\\Documents\\Visual Studio 2012\\Projects\\WindowsFormsApplication1 - Fork\\WindowsFormsApplication1\\1fd9885cbf0c57276900e22f85a28a24.ico");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (Log.Lines.Length > 16)
            {
                string[] sLines = Log.Lines;
                string[] sNewLines = new string[sLines.Length - 1];
                Array.Copy(sLines, 1, sNewLines, 0, sNewLines.Length);
                Log.Lines = sNewLines;
            }
            Log.SelectionStart = Log.Text.Length;
        }
        static DateTime dt;
        public static String TimeMark()
        {
            dt = DateTime.Now;
            return dt.ToLongTimeString().ToString();
        }
        public void LogOut(String L)
        {
            Log.Text = Log.Text.Insert(Log.Text.Length, "[" + (++Count).ToString() + "]  " + TimeMark() + " ====> " + L + "\n");
        }

        public void button2_Click(object sender, EventArgs e)
        {
            FontNow = this.comboBox1.Text.ToString();
            WriteConfig("FontNow", this.comboBox1.Text.ToString());
            SizeNow = int.Parse(this.comboBox2.Text.ToString());
            WriteConfig("SizeNow", this.comboBox2.Text.ToString());
            SpeedNow = int.Parse(this.comboBox3.Text.ToString());
            WriteConfig("SpeedNow", this.comboBox3.Text.ToString());
            LogOut("Change Font, Size and Speed to (" + FontNow + ", " + SizeNow.ToString() + ", " + SpeedNow.ToString() + ")");
        }
        public event EventHandler StopRef;
        public event EventHandler StaRef;
        private void button3_Click(object sender, EventArgs e)
        {

            if (!textBox1.Text.Equals(Url))
            {
                //LogOut("Wait 5 Sec");
                if (StopRef != null) StopRef(this, new EventArgs());
                /*for (int i = 0; i < 50; i++)
                {
                    Thread.Sleep(100);
                }*/
                System.Net.WebRequest wReq;
                System.Net.WebResponse wResp;
                System.IO.Stream respStream;
                try
                {
                    wReq = System.Net.WebRequest.Create("http://" + textBox1.Text);
                    wReq.Timeout = 5000;
                    wReq.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                    wResp = wReq.GetResponse();
                    respStream = wResp.GetResponseStream();
                }
                catch (System.Exception ex)
                {
                    LogOut(ex.ToString());
                    LogOut("连接不成功，请重新输入");
                    textBox1.Text = Url;
                    if (StaRef != null) StaRef(this, new EventArgs());
                    return;
                }
                LogOut("连接成功，服务器地址已经改为" + textBox1.Text);
                Url = textBox1.Text;
                WriteConfig("Url", textBox1.Text);
                if (StaRef != null) StaRef(this, new EventArgs());
                return;
            }
            else return;
        }
        static bool isOpen = true;
        private void button4_Click(object sender, EventArgs e)
        {
            if (isOpen) { button4.Text = "开启弹幕"; isOpen = false; LogOut("Please Wait 2 sec and it will Stop"); if (StopRef != null) StopRef(this, new EventArgs()); }
            else { button4.Text = "关闭弹幕"; isOpen = true; LogOut("Please Wait 2 sec and it will Start"); if (StaRef != null) StaRef(this, new EventArgs()); }
        }
    }
}
