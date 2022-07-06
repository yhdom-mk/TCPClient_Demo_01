using SuperSimpleTcp;
using System.Text;

namespace TCPServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SimpleTcpServer server;

        private void Form1_Load(object sender, EventArgs e)
        {
            btnSend.Enabled = false;
            server = new SimpleTcpServer(txtIP.Text);
            server.Events.ClientConnected += Events_ClientConnected;
            server.Events.ClientDisconnected += Events_ClientDisconnected;
            server.Events.DataReceived += Events_DataReceived;
        }

        private void Events_ClientDisconnected(object? sender, ConnectionEventArgs e)
        {
            txtInfo.Text += $"{e.IpPort} disconnected.{Environment.NewLine}";
            listClientIP.Items.Remove(e.IpPort);
        }

        private void Events_ClientConnected(object? sender, ConnectionEventArgs e)
        {
            txtInfo.Text += $"{e.IpPort} connected.{Environment.NewLine}";
            listClientIP.Items.Add(e.IpPort);
        }

        private void Events_DataReceived(object? sender, DataReceivedEventArgs e)
        {
            txtInfo.Text += $"{e.IpPort}: {Encoding.UTF8.GetString(e.Data)}{Environment.NewLine}";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            server.Start();
            txtInfo.Text += $"Starting...{Environment.NewLine}";
            btnStart.Enabled = false;
            btnSend.Enabled = true;
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (server.IsListening)
            {
                if(!string.IsNullOrEmpty(txtMessage.Text) && listClientIP.SelectedItem != null) //check message & select client ip from listbox
                {
                    server.Send(listClientIP.SelectedItem.ToString(), txtMessage.Text);
                    txtInfo.Text += $"Server: {txtMessage.Text}{Environment.NewLine}";
                    txtMessage.Text = string.Empty;
                }
            }
        }

    }
}