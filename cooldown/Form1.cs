using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Utilities;

namespace cooldown
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        globalKeyboardHook gkh = new globalKeyboardHook();
        int Timer = 0;
        DateTime timestamp;
        bool start = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetupWindowStyle();

            timestamp = DateTime.Now;

            RegisterEvents();
        }

        private void SetupWindowStyle()
        {
            this.BackColor = Color.Wheat;
            this.AllowTransparency = true;
            this.TransparencyKey = Color.Wheat;
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;

            int initialStyle = GetWindowLong(this.Handle, -20);
            SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);
            this.TopMost = true;
        }

        private void RegisterEvents()
        {
            gkh.HookedKeys.Add(Keys.LShiftKey);
            gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
            Application.Idle += new EventHandler(uptime);
        }

        private void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.LShiftKey)
            {
                StartOrIncrementTimer();
            }

            //set key event to unhandle so other applications can handle it
            e.Handled = false;
        }

        private void StartOrIncrementTimer()
        {
            if (Timer <= 0)
            {
                Timer = 8000;
                this.pictureBox1.BackColor = Color.Red;
                timestamp = DateTime.Now;
            }
            else
            {
                Timer += 1750;
                timestamp = DateTime.Now;
            }
        }

        private void uptime(object sender, EventArgs e)
        {
            DecremenTimer();
            DisplayTimer();

            Thread.Sleep(50);
            timestamp = DateTime.Now;
        }

        private void DisplayTimer()
        {
            label1.Text = Timer.ToString();
            if (Timer <= 0)
            {
                this.pictureBox1.BackColor = Color.Wheat;
            }
        }

        private void DecremenTimer()
        {
            Timer -= (int)(DateTime.Now - timestamp).TotalMilliseconds;
        }
    }
}
