using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Der Server...
namespace TcpServer
{
    public partial class Form1 : Form
    {
        private bool serverRun = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void ShowMessageAsync(string message)
        {
            Invoke(new Action(() =>
            {
                var lineText = $"{DateTime.Now:s}: {message}\r\n";
                TextBoxOutput(this.textBox1, lineText);
            }));
        }

        private static void TextBoxOutput(TextBoxBase textbox, string output)
        {
            textbox.AppendText(output);
            textbox.SelectionStart = textbox.Text.Length;
            textbox.SelectionLength = 0;
            textbox.ScrollToCaret();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                TcpListener server = null;
                try
                {
                    server = new TcpListener(IPAddress.Parse("127.0.0.1"), 3434);
                    server.Start();
                    var bytes = new byte[256];

                    // Enter the listening loop.
                    while (this.serverRun)
                    {
                        ShowMessageAsync(@"Waiting for a connection... ");

                        // Perform a blocking call to accept requests.
                        // You could also user server.AcceptSocket() here.
                        var client = server.AcceptTcpClient();
                        ShowMessageAsync(@"Connected!");

                        // Get a stream object for reading and writing
                        var stream = client.GetStream();

                        int i;

                        // Loop to receive all the data sent by the client.
                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            // Translate data bytes to a ASCII string.
                            var data = Encoding.ASCII.GetString(bytes, 0, i);
                            ShowMessageAsync($@"Received: {data}");

                            // Process the data sent by the client.
                            data = data.ToUpper();

                            var msg = Encoding.ASCII.GetBytes(data);

                            // Send back a response.
                            stream.Write(msg, 0, msg.Length);
                            ShowMessageAsync($@"Sent: {data}");
                        }

                        // Shutdown and end connection
                        client.Close();
                    }
                }
                catch (SocketException ex)
                {
                    ShowMessageAsync($@"SocketException: {ex.Message}");
                }
                finally
                {
                    // Stop listening for new clients.
                    server?.Stop();
                }
            });








        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.serverRun = false;
        }
    }
}
