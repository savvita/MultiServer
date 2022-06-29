using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.View
{
    public partial class ChatForm : Form
    {
        private Client client = new Client("127.0.0.1", 8008);

        public ChatForm()
        {
            InitializeComponent();

            connectButton.Focus();
            connectButton.Select();
        }

        public ChatForm(string ip, int port) : this()
        {
            client = new Client(ip, port);
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            client.ConnectToServer();

            string response;

            if (client.IsConnected)
            {
                connectButton.Enabled = false;
                disconnectButton.Enabled = true;
                sendButton.Enabled = true;
                uploadButton.Enabled = true;
                messageTextBox.Enabled = true;
                messageTextBox.Focus();

                response = "Connected to the server";
            }
            else
            {
                response = "Cannot connect to the server";
            }

            AddText(response);
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            string result = Disconnect();

            AddText(result);
        }

        private string Disconnect()
        {
            client.Disconnect();

            connectButton.Enabled = true;
            disconnectButton.Enabled = false;
            sendButton.Enabled = false;
            uploadButton.Enabled = false;
            messageTextBox.Enabled = false;

            connectButton.Focus();

            return "Disconnected";
        }

        private void AddText(string text) => this.outTextBox.Text += text + Environment.NewLine;

        private void sendButton_Click(object sender, EventArgs e)
        {
            string response;

            if (client.IsConnected)
            {
                response = client.SendMessage(messageTextBox.Text);
                messageTextBox.Clear();
                messageTextBox.Focus();

                if (!client.IsSocketAvailable)
                {
                    string result = Disconnect();
                    response += Environment.NewLine + result; ;
                }
            }
            else
            {
                response = "You are not connected to the server";
            }

            AddText(response);
        }

        private void uploadButton_Click(object sender, EventArgs e)
        {
            string response = String.Empty;

            if (client.IsConnected)
            {
                OpenFileDialog dlg = new OpenFileDialog();

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    response = client.Upload(dlg.FileName);
                }
            }
            else
            {
                response = "You are not connected to the server";
            }

            AddText(response);
        }
    }
}
