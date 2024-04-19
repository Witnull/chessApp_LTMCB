using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using Transitions;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using System.Security.Cryptography;
using System.Windows.Markup;
using Newtonsoft.Json;
using System.Drawing.Text;
using System.Windows.Controls;

namespace ChessApp
{
    public partial class starterScreeen : Form
    {
        int label1Top = 0;
        int button1Top = 0;
        int button2Top = 0;
        int button3Top = 0;
        int button4Top = 0;
        int button5Top = 0;
        int button6Top = 0;

        bool isButton1ClickActive = false;
        bool isButton2ClickActive = false;
        int selectedGameMode = 0;

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "RZxEKkX6ffq8XZgw9p0jbPYhqLYXQOeH1FIcmGIa",
            BasePath = "https://chess-database-a25f7-default-rtdb.asia-southeast1.firebasedatabase.app/"
        };
        IFirebaseClient client;
        public starterScreeen()
        {
            InitializeComponent();
            button1.Top = 230;
            button2.Top = 290;

            button4.Top = 500;
            button5.Top = 500;
            button6.Top = 500;

            label1.Top = 100;
            button1Top = button1.Top;
            button2Top = button2.Top;
            button3Top = button3.Top;


            button4Top = 170;
            button5Top = 240;
            button6Top = 310;
            
            label1Top = label1.Top;
            button3.Top = 350;
            button3.Visible = false;

            panel1.Top = 180;
            panel2.Top = 130;
            panel1.Height = 0;
            panel2.Height = 0;
            panel1.Visible = false;
            panel2.Visible = false;
            showLogin();
            hideMode();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            isButton1ClickActive = false;

            panel1.Visible = false;
            panel2.Visible = true;

            var t = new Transition(new TransitionType_EaseInEaseOut(300));
            t.add(button1, "Top", 150);
            t.add(button2, "Top", 350);
            t.add(label1, "Top", 40);
            t.add(panel2, "Height", 200);
            t.run();
            button1.Visible = false;
            button3.Visible = true;


            if (isButton2ClickActive)
            {
                Console.WriteLine("Register begin");
                if (ReUsernameBox.Text == "" | RePasswordBox.Text == "" | RePasswordBox2.Text == "")
                {
                    string g = string.Empty;
                    if (ReUsernameBox.Text == "") g = "Usernamr";
                    else if (RePasswordBox.Text == "") g = "Password";
                    else if (RePasswordBox2.Text == "") g = "Confirm password";
                    MessageBox.Show("Không được bỏ trống [ " + g + " ]", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                    return;
                }
                else
                {
                    if (RePasswordBox.Text != RePasswordBox2.Text) return;
                    User new_user = new User
                    {
                        ID = EncodeSha256(ReUsernameBox.Text),
                        UserName = ReUsernameBox.Text,
                        Password = RePasswordBox.Text,
                    };
                    SetResponse server_response = await client.SetTaskAsync("User/" + EncodeSha256(ReUsernameBox.Text), new_user);
                    if (server_response != null)
                    {
                        MessageBox.Show("Dữ liệu đã được thêm vào Firebase", "Success", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        UsernameBox.Text = PasswordBox.Text = string.Empty;
                    }
                    else
                    {
                        MessageBox.Show("Lỗi kết nối", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            else
            {
                isButton2ClickActive = true;
            }    

        }

        private void button1_Click(object sender, EventArgs e)
        {
            isButton2ClickActive = false;

            panel1.Visible = true;
            panel2.Visible = false;

            var t = new Transition(new TransitionType_EaseInEaseOut(300));
            t.add(button2, "Top", 500);
            t.add(button1, "Top", 350);
            t.add(label1, "Top", 40);
            t.add(panel1, "Height", 150);
            t.run();

            button2.Visible = false;
            button3.Visible = true;

            if (isButton1ClickActive)
            {
                Console.WriteLine("Login begin");
                if (UsernameBox.Text == "" | PasswordBox.Text == "") return;
                else
                {
                    var searching_user = client.Get(@"User/" + EncodeSha256(UsernameBox.Text));
                    var find_user = JsonConvert.DeserializeObject<User>(searching_user.Body.ToString());
                    if (find_user != null)
                    {
                        MessageBox.Show("Đăng nhập thành công", "Success", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        UsernameBox.Text = PasswordBox.Text = string.Empty;
                        //initGame();
                        selectMode();
                    }
                    else
                    {
                        MessageBox.Show("Đăng nhập thất bại. Vui lòng thử lại", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                        UsernameBox.Text = PasswordBox.Text = string.Empty;
                        return;
                    }
                }
            }
            else
            {
                isButton1ClickActive = true;
            }
          

        }

        private void button3_Click(object sender, EventArgs e)
        {
            isButton1ClickActive = false;
            isButton2ClickActive = false;

            showLogin();
            hideMode();

            var t = new Transition(new TransitionType_EaseInEaseOut(400));
            t.add(button1, "Top", button1Top);
            t.add(button2, "Top", button2Top);
            t.add(label1, "Top", label1Top);
            t.run();

            panel1.Height = 0;
            panel2.Height = 0;
            button3.Visible = false;
            panel1.Visible = false;
            panel2.Visible = false;
        }

        private void showMode()
        {
            button4.Visible = true;
            button5.Visible = true;
            button6.Visible = true;

            var t = new Transition(new TransitionType_EaseInEaseOut(400));
            t.add(button4, "Top", button4Top);
            t.add(button5, "Top", button5Top);
            t.add(button6, "Top", button6Top);
            t.run();

        }

        private void hideMode()
        {
            button4.Visible = false;
            button5.Visible = false;
            button6.Visible = false;

            button4.Top = 500;
            button5.Top = 500;
            button6.Top = 500;

        }
        private void showLogin()
        {
            button1.Visible = true;
            button2.Visible = true;
        }

        private void hideLogin()
        {
            button1.Visible = false;
            button2.Visible = false;

            panel1.Visible = false;
            panel2.Visible = false;
        }

        private void selectMode() /// select game mode screen
        {
            //init 
            hideLogin();
            showMode();

            button3.Visible = true;


            var t = new Transition(new TransitionType_EaseInEaseOut(400));


            t.run();


        }

 

        private void initGame(int mode = 0)
        {
            if(mode == 0)
            {
                Console.WriteLine("Game is starting");
                gamePlayervsPlayer game = new gamePlayervsPlayer();
                game.Show();
            }
            else if(mode == 1)
            {
                Console.WriteLine("Game is starting");
                gameScreen game = new gameScreen();
                game.Show();
            }
            else if(mode == 2)
            {
               Console.WriteLine("Game is starting");
               MessageBox.Show("Game is not available yet", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }

            this.Hide();
        }


        private string EncodeSha256(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] inputHashByte = SHA256.Create().ComputeHash(inputBytes);
            string result = BitConverter.ToString(inputHashByte).Replace("-", string.Empty).ToLower();
            return result;
        }

        private void starterScreeen_Load(object sender, EventArgs e)
        {
            client = new FireSharp.FirebaseClient(config);
            if (client == null) throw new Exception("Unable to connect to Firebase database");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            selectedGameMode = 0;
            initGame(selectedGameMode);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            selectedGameMode = 1;
            initGame(selectedGameMode);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            selectedGameMode = 2;
            initGame(selectedGameMode);
        }

        
    }
}
