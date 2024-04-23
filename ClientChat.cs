using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Transitions;
using SaaUI;

namespace Chess_ClientChat_NET
{
    public partial class ClientChat : Form
    {
        public ClientChat()
        {
            InitializeComponent();
        }
        // Calculate the percentage of the width of given width
        private int percentageOfWidth(int percentage, int width = 0)
        {
            if (width == 0)
            {
                width = this.Width;
            }
            return (percentage * width) / 100;
        }
        // Calculate the percentage of the height of given height
        private int percentageOfHeight(int percentage, int height = 0)
        {
            if (height == 0)
            {
                height = this.Height;
            }
            return (percentage * height) / 100;
        }

        // Variables
        int navBarX = 0;
        int navBarY = 0;
        int chatBoxX = 0;
        int chatBoxY = 0;
        string serverIP = "127.0.0.1";
        int serverPort = 12345;

        private void ClientChat_Load(object sender, EventArgs e)
        {
            // Get navbar location and chatbox location
            navBarX = pnl_navBar.Location.X;
            navBarY = pnl_navBar.Location.Y;
            chatBoxX = pnl_chatBox.Location.X;
            chatBoxY = pnl_chatBox.Location.Y;

            // Set z-index of nav bar to 2 and chat box to 1
            
            
            pnl_navBar.BringToFront();
            pnl_navBar.BringToFront();
        }

        private void btn_toggleNavBar_Click(object sender, EventArgs e)
        {
            Console.WriteLine(pnl_navBar.Location.X.ToString(), pnl_navBar.Location.Y.ToString());
            // Set nav bar position to (594, 0) if current position is (838, 0)
            if (pnl_navBar.Location.X == navBarX)
            {
                //pnl_navBar.Location = new Point(chatBoxX, 0);
                Transition.run(pnl_navBar, "Left", chatBoxX, new TransitionType_EaseInEaseOut(500));
            }
            else
            {
                //pnl_navBar.Location = new Point(navBarX, 0);
                Transition.run(pnl_navBar, "Left", navBarX, new TransitionType_EaseInEaseOut(500));
            }
        }

        private void txtBox_chatInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Show the message in the chat box if the user press Enter key
            if (e.KeyChar == (char)Keys.Enter)
            {
                btn_sendChat_Click(sender, e);
                e.Handled = true; // Prevents the "ding" sound
            }
        }
        // Measure the width of the string
        private int testMeasureWidth(string message, Font font)
        {
            Graphics Graphics = CreateGraphics();
            int res =  (int)Math.Round(Graphics.MeasureString(message, font).Width);
            Graphics.Dispose();
            return res;
        }
        // Get the maximum string of the given width
        private string getMaxStringOfWidth(string message, Font font, int maxWidth)
        {
            // Use Graphics.MeasureString to measure the width of the string and cut it off if it exceeds the maxWidth
            Graphics Graphics = CreateGraphics();
            string res = "";
            int width = 0;
            foreach (char c in message)
            {
                width += (int)Math.Round(Graphics.MeasureString(c.ToString(), font).Width);
                if (width >= maxWidth)
                {
                    break;
                }
                res += c;
            }
            Graphics.Dispose();
            return res;
        }

        private string getMaxString(string message, int maxWidth, Font font)
        {
            string result = "";
            string currentLine = "";
            string[] words = message.Split(' ');
            foreach (var word in words)
            {
                if (testMeasureWidth(currentLine + " " + word, font) > maxWidth)
                {
                    result += currentLine + "\n";
                    currentLine = "";
                }

                if (testMeasureWidth(word, font) > maxWidth)
                {
                    string remainingWord = word;
                    while (remainingWord.Length > 0)
                    {
                        string subWord = getMaxStringOfWidth(remainingWord, font, maxWidth);
                        result += subWord + "\n";
                        remainingWord = remainingWord.Substring(subWord.Length).TrimStart();
                    }
                }
                else
                {
                    currentLine += " " + word;
                }
            }

            result += currentLine.TrimStart();
            return result;
        }

        private SaaChatBubble GenerateChatBubble(string message, PeakPositions peakPosition, bool profile = false, Color foreColor = default(Color) , Color msgBackground = default(Color))
        {
            // Initialize chatBubble
            SaaChatBubble chatBubble = new SaaChatBubble
            {
                BackColor = Color.Transparent,
                Body = "",
                PeakPosition = peakPosition,
                Profile = profile,
                ProfileSize = new Size(25, 25),
                AutoScroll = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ImageBackColor = Color.FromArgb(217, 217, 217),
                ImageBorderThickness = 0,
                MsgBackground = msgBackground,
                ForeColor = foreColor
            };
            // Set font
            Font font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
            chatBubble.Font = font;

            //Console.WriteLine(getMaxString(message, chatPnl_chatBox.Width - 15, font));

            // Set maximum width of chatBubble and text alignment
            int maxWidth = 0;
            if (peakPosition == PeakPositions.TopLeft) { 
                maxWidth = chatPnl_chatBox.Width - chatBubble.ProfileSize.Width - 30 - 50;
                chatBubble.TextAlign = ContentAlignment.TopLeft;
            }
            else if (peakPosition == PeakPositions.TopRight) { 
                maxWidth = chatPnl_chatBox.Width - 30;
                chatBubble.TextAlign = ContentAlignment.TopRight;
            }

            // Split message into words
            string result = "";
            string currentLine = "";
            string[] words = message.Split(' ');
            foreach (var word in words)
            {
                // Debugging
                Console.WriteLine("Width: " + testMeasureWidth(currentLine + " " + word, font).ToString() + " MaxWidth: " + maxWidth.ToString() + " Word: " + word);
                Console.WriteLine("CurrentLine: " + currentLine + " Word: " + word);
                // If the width of the current line + word exceeds the maxWidth, add the current line to the result and reset the current line
                if (testMeasureWidth(currentLine + " " + word, font) >= maxWidth)
                {
                    result += currentLine + "\n";
                    currentLine = "";
                }
                // If the width of the word exceeds the maxWidth, split the word into multiple lines
                if (testMeasureWidth(word, font) >= maxWidth)
                {
                    // If result just contains newline, ignore it
                    if (result.Length > 0 && result[result.Length - 1] == '\n')
                    {
                        result = result.Substring(0, result.Length - 1);
                    }
                    string remainingWord = word;
                    while (remainingWord.Length > 0)
                    {
                        string subWord = getMaxStringOfWidth(remainingWord, font, maxWidth);
                        result += subWord + "\n";
                        Console.WriteLine(subWord);
                        remainingWord = remainingWord.Substring(subWord.Length).TrimStart();
                    }
                }
                // Add the word to the current line
                else
                {
                    currentLine += " " + word;
                }
                // Trim the current line
                currentLine = currentLine.TrimStart();
            }

            // Add the current line to the result
            result += currentLine.TrimStart();

            chatBubble.Body = result;

            Console.WriteLine(chatBubble.Body);

         
            // Set the size of the chatBubble
            chatBubble.MinimumSize = new Size(chatPnl_chatBox.Width - 15, chatBubble.MeasureHeight());
            chatBubble.Size = new Size(chatPnl_chatBox.Width - 15, chatBubble.MeasureHeight());
            // Set the padding of the chatBubble
            if (peakPosition == PeakPositions.TopRight || peakPosition == PeakPositions.BottomRight)
            {
                chatBubble.Padding = new Padding(30, 0, 0, 0);
            }
            else if (peakPosition == PeakPositions.TopLeft || peakPosition == PeakPositions.BottomLeft)
                chatBubble.Padding = new Padding(0, 0, 30, 0);

            Console.WriteLine(chatBubble.MeasureHeight());
            Console.WriteLine(chatBubble.MeasureWidth());

            return chatBubble;
        }   


        private void btn_sendChat_Click(object sender, EventArgs e)
        {
            try
            {
                //// Parse ip to IPAddress
                //IPAddress ipaddress = IPAddress.Parse(serverIP);
                //// Create a new instance of the TCPClient class
                //TcpClient client = new TcpClient();
                //// Connect to the server
                //client.Connect(ipaddress, serverPort);
                //// Check if the connection was successful
                //if (!client.Connected)
                //{
                //    MessageBox.Show("Failed to connect to the server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                //// Create a new instance of the NetworkStream class
                //NetworkStream stream = client.GetStream();
                //// Convert the message to bytes
                //byte[] message = Encoding.UTF8.GetBytes(txtBox_chatInput.Text);
                //// Send the message to the server
                //stream.Write(message, 0, message.Length);
                //// Close the stream
                //stream.Close();
                //// Close the client
                //client.Close();

                // AddMessage to chatbox
                SaaChatBubble chatBubble = GenerateChatBubble(
                    txtBox_chatInput.Text, 
                    PeakPositions.TopRight, 
                    false, 
                    Color.Black,
                    Color.FromArgb(217, 217, 217));

                chatPnl_chatBox.AddMessage(chatBubble);
                
                // Clear the input TextBox
                txtBox_chatInput.Text = "";
            }
            catch (SocketException ex)
            {
                MessageBox.Show("Error: Could not connect to the server. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_otherChat_Click(object sender, EventArgs e)
        {
            SaaChatBubble chatBubble  = GenerateChatBubble(
                txt_otherChat.Text, 
                PeakPositions.TopLeft, 
                true, 
                Color.Black,
                Color.FromArgb(217, 217, 217));

            chatPnl_chatBox.AddMessage(chatBubble);
        }

        private void txtBox_chatInput_TextChanged(object sender, EventArgs e)
        {
            //// Show the message in the chat box if the user press Enter key
            //if (e.KeyChar == (char)Keys.Enter)
            //{
            //    btn_sendChat_Click(sender, e);
            //    e.Handled = true; // Prevents the "ding" sound
            //}
            //Console.WriteLine("Typed");
        }

        private void txtBox_chatInput_Enter(object sender, EventArgs e)
        {
            Console.WriteLine("Entered");
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
