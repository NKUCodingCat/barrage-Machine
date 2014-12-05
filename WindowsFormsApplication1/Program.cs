using System; 　  
using System.Drawing; 　  
using System.Windows.Forms; 　  
using System.Runtime.InteropServices;
namespace WindowsFormsApplication1　  
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //Application.Run(new Form2());
        }
    }
}　