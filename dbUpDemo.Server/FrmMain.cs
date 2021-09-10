using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using dbUpDemo.Update;
using dbUpDemo.UIExtensions;

namespace dbUpDemo.Server
{
    public partial class FrmMain : Form
    {
        private readonly string _connectionString;

        public FrmMain()
        {
            InitializeComponent();

            _connectionString = ConfigurationManager.ConnectionStrings[0].ConnectionString;
        }

        private void btAtualizar_Click(object sender, EventArgs e)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = "dbUpDemo.Update.exe";
                process.StartInfo.Arguments = _connectionString;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;

                process.OutputDataReceived += SaidaAtualizacao;
                process.Start();

                process.BeginOutputReadLine();
            }
        }

        delegate void SafeTextChange();
        private void AtualizarTexto(string texto)
        {
            if (tbLog.InvokeRequired)
            {
                SafeTextChange textChange = new SafeTextChange(() => AtualizarTexto(texto));
                Invoke(textChange);
            }
            else
            {
                tbLog.Text += $"{texto}\n";
            }
        }

        private void SaidaAtualizacao(object sendingProcess,
            DataReceivedEventArgs outLine)
        {
            if (!string.IsNullOrEmpty(outLine.Data))
            {
                AtualizarTexto(outLine.Data);
            }
        }
    }
}
