using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        int Count = 0;
        public Form2()
        {
            InitializeComponent();
        }
        private void Form2_Load(object sender, EventArgs e)
        {

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
            Log.Text = Log.Text.Insert(Log.Text.Length, "["+(++Count).ToString()+"]  " + TimeMark() + " ====> " + L + "\n");
        }
    }
}
