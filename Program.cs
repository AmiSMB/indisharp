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
            Application.Run(new INDIForm(new INDIClient("burnterwaffle.freemyip.com", 7624)));
        }
    }
}
