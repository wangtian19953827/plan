using System;
using System.Windows.Forms;
using ProductInventoryOptimizer.UI;

namespace ProductInventoryOptimizer
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"程序发生错误:\n{ex.Message}\n\n详细信息:\n{ex.StackTrace}",
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
