using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace TcpClient
{
    /// <summary>
    /// Client Formular
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                var client = new System.Net.Sockets.TcpClient("localhost", 3434);

                // Translate the passed message into ASCII and store it as a Byte array.
                var message = "Hallo Message";
                var data = Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                var stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                this.textBox1.Text += $@"Sent: {message}";

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new byte[256];

                // String to store the response ASCII representation.

                // Read the first batch of the TcpServer response bytes.
                var bytes = stream.Read(data, 0, data.Length);
                var responseData = Encoding.ASCII.GetString(data, 0, bytes);
                this.textBox1.Text += $@"Received: {responseData}";

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException ex)
            {
                this.textBox1.Text += $@"ArgumentNullException: {ex}";
            }
            catch (SocketException ex)
            {
                this.textBox1.Text += $@"SocketException: {ex}";
            }
        }
    }
}
