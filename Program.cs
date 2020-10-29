using System;
using System.Windows.Forms;
using INDI.Forms;

namespace INDI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //INDIChooser chooser = new INDIChooser("127.0.0.1", 7624);
            //Application.Run(chooser);
            Application.Run(new INDIForm(new INDIClient("127.0.0.1",7624), ""));
        }
    }
}
