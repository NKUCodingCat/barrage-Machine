using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections;
using System.IO;
using System.Web.Script.Serialization;
struct Time_and_String
{
    public int Num;
    public double Time;
    public String Content;
}

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        
        
        public Form1()
        {
            InitializeComponent();
            F2.LogOut("弹幕姬初始化完成");
        }
        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern long GetWindowLong(IntPtr hwnd, int nIndex);
        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern long SetWindowLong(IntPtr hwnd, int nIndex, long dwNewLong);
        [DllImport("user32", EntryPoint = "SetLayeredWindowAttributes")]
        public static extern int SetLayeredWindowAttributes(IntPtr Handle, int crKey, byte bAlpha, int dwFlags);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr parent, IntPtr childAfter, string className, string windowText);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);
        const int GWL_EXSTYLE = -20;
        const int WS_EX_TRANSPARENT = 0x20;
        const int WS_EX_LAYERED = 0x80000;
        const int LWA_ALPHA = 2;
        System.Timers.Timer Timers_Timer = new System.Timers.Timer();
        System.Timers.Timer Timers_Timer2 = new System.Timers.Timer();
        ArrayList Screen = new ArrayList();
        Rectangle ScreenArea = new Rectangle();
        double Time_Max = double.MinValue;
        public static bool isChanged = false;
        public static int Scr_len = 0;
        static Form2 F2 = new Form2();
        public static string GetUrltoHtml(string Url, string type)
        {
            try
            {
                System.Net.WebRequest wReq = System.Net.WebRequest.Create(Url);
                wReq.Timeout = 5000;
                // Get the response instance.
                System.Net.WebResponse wResp = wReq.GetResponse();
                System.IO.Stream respStream = wResp.GetResponseStream();
                // Dim reader As StreamReader = New StreamReader(respStream)
                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding(type)))
                {
                   
                    return reader.ReadToEnd();
                }
            }
            catch (System.Exception ex)
            {
                F2.LogOut("网络异常"+ex.ToString());
                return "";
            }
        }
        public static Dictionary<string, object> JsonToDictionary(string jsonData)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                return jss.Deserialize<Dictionary<string, object>>(jsonData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static ArrayList Content_Make()
        {
            String JsonText = GetUrltoHtml("http://302.nktwclick.sinaapp.com/tojson", "utf-8");
            Dictionary<string, object> dic = JsonToDictionary(JsonText);
            ArrayList Content = new ArrayList();
            foreach (KeyValuePair<string, object> item in dic)
            {
                Time_and_String Temp = new Time_and_String();
                Dictionary<string, object> Con = (Dictionary<string, object>)item.Value;
                Temp.Num = int.Parse(item.Key);
                Temp.Time = double.Parse(Con["Time"].ToString());
                Temp.Content = Con["Content"].ToString();
                Content.Add(Temp);
            }
            F2.LogOut("网络正常"+((Content.Count != 0)?(" 服务器中弹幕数 : "+Content.Count.ToString()):("")));
            return Content;
        }
        void Content_Refersh(object sender, System.Timers.ElapsedEventArgs e)
        {
            
            //ArrayList Scr_Temp = new ArrayList();
            double Max = double.MinValue;
            double Time_min = double.MaxValue;
            ArrayList Content = Content_Make();
            foreach (Time_and_String con in Content)
            {
                if (con.Time < Time_min) Time_min = con.Time;
                if (con.Time > Max) Max = con.Time;
            }
            double Tm = Time_Max;
            Time_Max = Max;
            int Cou = 0;
            foreach (Time_and_String con in Content)
            {
                if (con.Time > Tm)
                {
                    Label Temp = new Label();
                    Temp.AutoSize = true;
                    Temp.Font = new System.Drawing.Font("黑体", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    Temp.Location = new System.Drawing.Point(ScreenArea.Width + (int)((con.Time-Time_min)*10), 35);
                    Temp.Name = "label1";
                    Temp.ForeColor = Color.FromArgb(255, 255, 255);
                    Temp.TabIndex = con.Num + 1;
                    Temp.Text = con.Content;
                    Temp.Visible = true;
                    Screen.Add(Temp);
                    if (InvokeRequired)
                    {
                        this.Invoke(this.AddControl, Temp);
                    }
                    Cou++;
                }
            }
            Time_Max = Max;
            if (Cou > 0)
            {
                F2.LogOut("弹幕新增 ： " + Cou.ToString());
            }
        }
        void Clr(Control c)
        {
            Label Now = (Label)c;
            for (int j = 0; j < Screen.Count; j++)
            {
                Label Prev = (Label)Screen[j];
                if ((((Now.Location.X < Prev.Location.X + Prev.Width) && (Now.Location.X > Prev.Location.X)) || ((Now.Location.X + Now.Width < Prev.Location.X + Prev.Width) && (Now.Location.X + Now.Width > Prev.Location.X))) && (Prev.Location.Y == Now.Location.Y))
                {
                    Now.Location = new System.Drawing.Point(Now.Location.X, Now.Location.Y + Prev.Height + 5);
                }
            }
        }
        void Screen_Refersh(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.TopMost = true;
            foreach (Label i in Screen)
            {
                if (i.Location.X + i.Width < 0)
                {
                    if (InvokeRequired)
                    {
                        this.Invoke(this.RemoveControl, i);
                    }
                    Screen.Remove(i);
                }
                else
                {
                    i.Location = new System.Drawing.Point(i.Location.X - 5, i.Location.Y);
                }
            }
        }
        void Form1_FormClosed(object sender, EventArgs e)
        {
            Application.Exit();
        }  
        public delegate void AddControlEventHandler(Control c);
        public AddControlEventHandler AddControl;
        public delegate void RemoveControlEventHandler(Control c);
        public RemoveControlEventHandler RemoveControl;

        private void Form1_Load(object sender, EventArgs e)
        {
            F2.Show();
            F2.Activate();
            this.AddControl = delegate(Control c)
            {
                this.Controls.Add(c);
                Clr(c);
            };
            this.RemoveControl = delegate(Control c)
            {
                this.Controls.Remove(c);
            };
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            SetWindowLong(Handle, GWL_EXSTYLE, GetWindowLong(Handle, GWL_EXSTYLE) | WS_EX_TRANSPARENT | WS_EX_LAYERED);
            this.BackColor = Color.Black;
            this.TransparencyKey = Color.Black;
            Timers_Timer.Interval = 20;
            Timers_Timer.Enabled = true;
            Timers_Timer.Elapsed += new System.Timers.ElapsedEventHandler(Screen_Refersh);
            Timers_Timer2.Interval = 2000;
            Timers_Timer2.Enabled = true;
            Timers_Timer2.Elapsed += new System.Timers.ElapsedEventHandler(Content_Refersh);
            ScreenArea = System.Windows.Forms.Screen.GetWorkingArea(this);
            F2.FormClosed += new FormClosedEventHandler(Form1_FormClosed);
            this.FormClosed += new FormClosedEventHandler(Form1_FormClosed);
        }
    }
}
