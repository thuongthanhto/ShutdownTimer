using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShutdownTimer
{
    public partial class frmMain : Form
    {
        Process cmdProcess; // Tiến trình của cmd

        // Dùng để di chuyển form vì mặc định form không di chuyển được 
        int _togMove; // Cho biết pnlTitle có được click chuột vào hay không (1 là có, 0 là không có)
        int _mValX; // Khoảng cách chuột di chuyển theo phương x
        int _mValY; // Khoảng cách chuột di chuyển theo phương y

        public frmMain()
        {
            InitializeComponent();

            // Khởi tạo combobox.
            Dictionary<string, string> dict = new Dictionary<string, string>();
            for (int i = 30; i <= 180; i += 30)
            {
                string temp = i.ToString();
                dict.Add(temp, temp);
            }
            cboTime.DataSource = new BindingSource(dict, null);
            cboTime.DisplayMember = "Value";
            cboTime.ValueMember = "Key";
            
            // Khởi tạo process.
            cmdProcess = new Process();
        }

        private void btnTimer_Click(object sender, EventArgs e)
        {
            string value = ((KeyValuePair<string, string>)cboTime.SelectedItem).Value;
            ProcessStartInfo proc = new ProcessStartInfo("cmd", "/c shutdown -s -t " + (int.Parse(value)*60))
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            cmdProcess.StartInfo = proc;
            cmdProcess.Start();
            cmdProcess.WaitForExit();
        }

        private void btnDisTimer_Click(object sender, EventArgs e)
        {
            ProcessStartInfo proc = new ProcessStartInfo("cmd", "/c shutdown -a")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            cmdProcess.StartInfo = proc;
            cmdProcess.Start();
            cmdProcess.WaitForExit();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            cmdProcess.Close();
        }

        private void pnlTitle_MouseDown(object sender, MouseEventArgs e)
        {
            _togMove = 1;
            _mValX = e.X;
            _mValY = e.Y;
        }

        private void pnlTitle_MouseUp(object sender, MouseEventArgs e)
        {
            _togMove = 0;
        }

        private void pnlTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (_togMove == 1)
            {
                this.SetDesktopLocation(MousePosition.X - _mValX, MousePosition.Y - _mValY);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
