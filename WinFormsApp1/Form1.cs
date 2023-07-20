using System.Diagnostics;
using Windows.Graphics.Capture;
using WindowsCapture;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.DoubleBuffer |

      ControlStyles.UserPaint |
      ControlStyles.AllPaintingInWmPaint,
      true);
            this.UpdateStyles();


        }

        WindowsCaptureSession? _captureSession;

        public void InitWindowsCaptureSession()
        {
            _captureSession = new WindowsCaptureSession(this.Handle, new WindowsCaptureSessionOptions()
            {
                MinFrameInterval = 0,
                IsManual = true,
            });
        }

        private void CaptureSession_OnFrameArrived(Direct3D11CaptureFrame frame, Action nextFrame)
        {
            //Size
            pictureBox1.Image = frame.ToBitmap();

            nextFrame();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            _captureSession?.StopCapture();
            InitWindowsCaptureSession();
            var monis = MonitorEnumerationHelper.GetMonitors();
            var a = monis.FirstOrDefault(monis => monis.IsPrimary = true);
            _captureSession.OnFrameArrived += CaptureSession_OnFrameArrived;
            _captureSession.StartCaptureMonitor(a.Hmon);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _captureSession?.StopCapture();
            InitWindowsCaptureSession();
            var processesWithWindows = from p in Process.GetProcesses()
                                       where !string.IsNullOrWhiteSpace(p.MainWindowTitle) && WindowEnumerationHelper.IsWindowValidForCapture(p.MainWindowHandle)
                                       select p;
            _captureSession.OnFrameArrived += CaptureSession_OnFrameArrived;
            _captureSession.StartCaptureWindows(processesWithWindows.First().MainWindowHandle);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _captureSession?.StopCapture();
            InitWindowsCaptureSession();
            _captureSession.OnFrameArrived += CaptureSession_OnFrameArrived;
            _captureSession.PickAndCapture();

        }
    }
}