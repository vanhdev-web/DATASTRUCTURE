using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;




namespace linkedlist_quanly
{
    public class PostManager    //thêm tính năng quản lý post ở đây
    {
        private const string POSTS_FILE = "posts.txt";
        private string filePath;

        public PostManager()
        {
            string debugPath = Path.GetDirectoryName(Application.ExecutablePath);
            filePath = Path.Combine(debugPath, POSTS_FILE);

            // Create the file if it doesn't exist
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
        }

        public void SavePost(Post post)
        {
            string postLine = $"{post.Author}|{post.Content}|{post.MediaReference}|{post.PostTime:yyyy-MM-dd HH:mm:ss}\n";
            File.AppendAllText(filePath, postLine);
        }

        public List<PostData> LoadAllPosts()
        {
            var posts = new List<PostData>();

            if (!File.Exists(filePath))
                return posts;

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                if (parts.Length >= 4)
                {
                    posts.Add(new PostData
                    {
                        Author = parts[0],
                        Content = parts[1],
                        MediaReference = parts[2] == "null" ? null : parts[2],
                        PostTime = DateTime.Parse(parts[3])
                    });
                }
            }

            return posts;
        }

        public void InitializeDefaultPosts(string username)    //default post khi tạo một tài khoản mới
        {
            var posts = LoadAllPosts();
            if (!posts.Any(p => p.Author == username))
            {
                var defaultPosts = new List<Post>
                {
                    new Post("Hello! This is my first post!", null, username),
                    new Post("Just joined this amazing platform!", null, username)
                };

                foreach (var post in defaultPosts)
                {
                    SavePost(post);
                }
            }
        }
    }

    public class PostData        //quan ly post
    {
        public string Content { get; set; }
        public string MediaReference { get; set; }
        public DateTime PostTime { get; set; }
        public string Author { get; set; }
    }
    public class User        //quan ly user
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string AvatarPath { get; set; } // Thêm đường dẫn ảnh đại diện
    }
    public class UserManager  //quản lý tài khoản của người dùng
    {
        private const string USER_FILE = "users.txt";
        private string filePath;

        public UserManager()
        {
            string debugPath = Path.GetDirectoryName(Application.ExecutablePath);
            filePath = Path.Combine(debugPath, USER_FILE);

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
        }

        public bool RegisterUser(string username, string password)
        {
            try
            {
                // Check if username already exists
                if (File.Exists(filePath))
                {
                    var users = File.ReadAllLines(filePath);
                    if (users.Any(u => u.Split('|')[0] == username))
                    {
                        return false;
                    }
                }

                // Save user with default avatar path
                string defaultAvatarPath = "Resources/default-avatar.png";
                File.AppendAllText(filePath, $"{username}|{password}|{defaultAvatarPath}\n");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ValidateUser(string username, string password)    //kiểm tra, xác thực tài khoản mới tạo
        {
            try
            {
                if (!File.Exists(filePath)) return false;

                var users = File.ReadAllLines(filePath);
                return users.Any(u =>
                {
                    var parts = u.Split('|');
                    return parts[0] == username && parts[1] == password;
                });
            }
            catch (Exception)
            {
                return false;
            }
        }
        public string GetUserAvatar(string username)    //đặt default avatar cho người dùng
        {
            try
            {
                if (!File.Exists(filePath)) return null;

                var users = File.ReadAllLines(filePath);
                var userLine = users.FirstOrDefault(u => u.Split('|')[0] == username);
                if (userLine != null)
                {
                    var parts = userLine.Split('|');
                    return parts.Length >= 3 ? parts[2] : "Resources/default-avatar.png";
                }
            }
            catch (Exception) { }

            return "Resources/default-avatar.png";
        }

        public bool UpdateUserAvatar(string username, string newAvatarPath)    //update avatar
        {
            try
            {
                if (!File.Exists(filePath)) return false;

                var users = File.ReadAllLines(filePath).ToList();
                for (int i = 0; i < users.Count; i++)
                {
                    var parts = users[i].Split('|');
                    if (parts[0] == username)
                    {
                        // Update avatar path while keeping username and password
                        users[i] = $"{parts[0]}|{parts[1]}|{newAvatarPath}";
                        File.WriteAllLines(filePath, users);
                        return true;
                    }
                }
            }
            catch (Exception) { }

            return false;
        }
    }
    public class LoginForm : CustomizedForm   //tạo form đăng nhập 
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnRegister;
        private PictureBox picAvatar;
        private UserManager userManager;

        public string LoggedInUser { get; private set; }

        //Round form
        private void RoundedForm_Load(object sender, EventArgs e)
        {
            int radius = 60; // Bán kính bo tròn
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(0, 0, radius, radius, 180, 90); // Góc trên bên trái
            path.AddArc(this.Width - radius, 0, radius, radius, 270, 90); // Góc trên bên phải
            path.AddArc(this.Width - radius, this.Height - radius, radius, radius, 0, 90); // Góc dưới bên phải
            path.AddArc(0, this.Height - radius, radius, radius, 90, 90); // Góc dưới bên trái
            path.CloseFigure();
            this.Region = new Region(path);
        }

        public LoginForm()
        {
            userManager = new UserManager();
            InitializeComponents();
            this.MaximizeBox = false;
            this.Text = "Đăng nhập";
            //this.Load += MainForm_Load;
            this.StartPosition = FormStartPosition.CenterScreen; // Đặt vị trí form ở giữa màn hình
            this.FormBorderStyle = FormBorderStyle.None; // Bỏ viền form
            this.Load += new EventHandler(RoundedForm_Load);            //Di chuyển 
            // Subscribe to the MouseDown, MouseMove, and MouseUp events
            this.MouseDown += new MouseEventHandler(Form1_MouseDown);
            this.MouseMove += new MouseEventHandler(Form1_MouseMove);
            this.MouseUp += new MouseEventHandler(Form1_MouseUp);
            // Enable double buffering
            this.DoubleBuffered = true;
        }

        private void InitializeComponents()
        {
            this.Size = new Size(320, 650);

            picAvatar = new PictureBox
            {
                Size = new Size(80, 80),
                Location = new Point(110, 10),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = Image.FromFile("Resources/default-avatar.png"),
                BorderStyle = BorderStyle.FixedSingle
            };


            txtUsername = new TextBox
            {
                Location = new Point(20, 300),
                Size = new Size(280, 50)
            };


            txtPassword = new TextBox
            {
                Location = new Point(20, 260),
                Size = new Size(280, 20),
                PasswordChar = '•'
            };

            btnLogin = new Button
            {
                Text = "Đăng nhập",
                Location = new Point(150, 90),
                Size = new Size(150, 30)
            };
            btnLogin.Click += BtnLogin_Click;

            btnRegister = new Button
            {
                Text = "Đăng ký",
                Location = new Point(120, 130),
                Size = new Size(100, 30)
            };
            btnRegister.Click += BtnRegister_Click;

            this.Controls.AddRange(new Control[] {
                 txtUsername,
                txtPassword,
                btnLogin, btnRegister, 
                picAvatar 
            });

        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            if (userManager.ValidateUser(txtUsername.Text, txtPassword.Text))
            {
                LoggedInUser = txtUsername.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!");
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            using (var registerForm = new RegisterForm())
            {
                registerForm.ShowDialog();
            }
        }

        private void TxtUsername_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                string avatarPath = userManager.GetUserAvatar(txtUsername.Text);
                try
                {
                    picAvatar.Image = Image.FromFile(avatarPath);
                }
                catch
                {
                    picAvatar.Image = Image.FromFile("Resources/default-avatar.png");
                }
            }
        }
        //Xử lý phần di chuyển 
        // Mouse down event to start dragging the form
        // Import necessary functions from user32.dll
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0x00A1;
        private const int HTCAPTION = 0x0002;

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        // Optionally, you can handle the MouseMove event if you want to do something while dragging
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            // You can add any code here if you want to do something while dragging
        }

        // Optionally, you can handle the MouseUp event if you want to do something after dragging
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            // You can add any code here if you want to do something after dragging
        }

        // Make sure to subscribe to the MouseDown event in the designer or constructor
        private void Form1_Load(object sender, EventArgs e)
        {
            this.MouseDown += new MouseEventHandler(Form1_MouseDown);
            this.MouseMove += new MouseEventHandler(Form1_MouseMove);
            this.MouseUp += new MouseEventHandler(Form1_MouseUp);
        }
    }

    public class RegisterForm : Form                //tạo form cho đăng ký
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;
        private Button btnRegister;
        private UserManager userManager;

        public RegisterForm()
        {
            userManager = new UserManager();
            InitializeComponents();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.Text = "Đăng ký";
        }

        private void InitializeComponents()
        {
            this.Size = new Size(300, 250);

            System.Windows.Forms.Label lblUsername = new System.Windows.Forms.Label
            {
                Text = "Tên đăng nhập:",
                Location = new Point(20, 20),
                Size = new Size(100, 20)
            };

            txtUsername = new TextBox
            {
                Location = new Point(120, 20),
                Size = new Size(150, 20)
            };

            System.Windows.Forms.Label lblPassword = new System.Windows.Forms.Label
            {
                Text = "Mật khẩu:",
                Location = new Point(20, 50),
                Size = new Size(100, 20)
            };

            txtPassword = new TextBox
            {
                Location = new Point(120, 50),
                Size = new Size(150, 20),
                PasswordChar = '•'
            };

            System.Windows.Forms.Label lblConfirmPassword = new System.Windows.Forms.Label
            {
                Text = "Xác nhận MK:",
                Location = new Point(20, 80),
                Size = new Size(100, 20)
            };

            txtConfirmPassword = new TextBox
            {
                Location = new Point(120, 80),
                Size = new Size(150, 20),
                PasswordChar = '•'
            };

            btnRegister = new Button
            {
                Text = "Đăng ký",
                Location = new Point(120, 120),
                Size = new Size(100, 30)
            };
            btnRegister.Click += BtnRegister_Click;

            this.Controls.AddRange(new Control[] {
                lblUsername, txtUsername,
                lblPassword, txtPassword,
                lblConfirmPassword, txtConfirmPassword,
                btnRegister
            });
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            if (txtPassword.Text.Length < 8)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 8 ký tự!");
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!");
                return;
            }

            if (userManager.RegisterUser(txtUsername.Text, txtPassword.Text))
            {
                MessageBox.Show("Đăng ký thành công!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại!");
            }
        }
    }
    public class Post
    {
        public string Content { get; set; }
        public string MediaReference { get; set; }
        public DateTime PostTime { get; set; }
        public string Author { get; set; }
        public Post Next { get; set; }

        public Post(string content, string mediaReference, string author)
        {
            Content = content;
            MediaReference = mediaReference;
            PostTime = DateTime.Now;
            Author = author;
            Next = null;
        }


        public Post(string content, string mediaReference, string author, DateTime postTime)
        {
            Content = content;
            MediaReference = mediaReference;
            PostTime = postTime;
            Author = author;
            Next = null;
        }
    }

    public class SocialMediaLinkedList
    {
        private Post Head;
        private PostManager postManager;

        public SocialMediaLinkedList()
        {
            postManager = new PostManager();
            LoadPostsFromStorage();
        }

        private void LoadPostsFromStorage()
        {
            var posts = postManager.LoadAllPosts();
            Head = null;

            // Convert PostData objects to linked list
            foreach (var post in posts.OrderByDescending(p => p.PostTime))
            {
                AddPost(post.Content, post.MediaReference, post.Author, post.PostTime);
            }
        }

        public void AddPost(string content, string mediaReference, string author)
        {
            Post newPost = new Post(content, mediaReference, author);
            AddPostToList(newPost);
            postManager.SavePost(newPost);
        }

        public void AddPost(string content, string mediaReference, string author, DateTime postTime)
        {
            Post newPost = new Post(content, mediaReference, author, postTime);
            AddPostToList(newPost);
        }

        private void AddPostToList(Post newPost)
        {
            if (Head == null)
            {
                Head = newPost;
                return;
            }

            if (newPost.PostTime >= Head.PostTime)
            {
                newPost.Next = Head;
                Head = newPost;
                return;
            }

            Post current = Head;
            while (current.Next != null && current.Next.PostTime > newPost.PostTime)
            {
                current = current.Next;
            }
            newPost.Next = current.Next;
            current.Next = newPost;
        }

        public List<Post> GetAllPosts()
        {
            List<Post> posts = new List<Post>();
            Post current = Head;
            while (current != null)
            {
                posts.Add(current);
                current = current.Next;
            }
            return posts;
        }
        public void ShufflePosts()
        {
            if (Head == null || Head.Next == null)
            {
                return; // Danh sách rỗng hoặc chỉ có một phần tử
            }

            // Chuyển danh sách liên kết thành danh sách mảng
            List<Post> posts = GetAllPosts();

            // Trộn ngẫu nhiên danh sách mảng
            Random random = new Random();
            for (int i = posts.Count - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                (posts[i], posts[j]) = (posts[j], posts[i]); // Hoán đổi
            }

            // Tái tạo danh sách liên kết
            Head = posts[0];
            Post current = Head;
            for (int i = 1; i < posts.Count; i++)
            {
                current.Next = posts[i];
                current = current.Next;
            }
            current.Next = null; // Đảm bảo nút cuối cùng không trỏ tới đâu
        }


        public List<Post> GetUserPosts(string author)
        {
            List<Post> userPosts = new List<Post>();
            Post current = Head;

            while (current != null)
            {
                if (current.Author == author)
                {
                    userPosts.Add(current);
                }
                current = current.Next;
            }

            // Sắp xếp danh sách theo thời gian tăng dần
            userPosts.Sort((p1, p2) => p2.PostTime.CompareTo(p1.PostTime));

            return userPosts;
        }

        public void DeletePost(DateTime postTime)
        {
            Post current = Head;
            Post previous = null;

            while (current != null && current.PostTime != postTime)
            {
                previous = current;
                current = current.Next;
            }

            if (current != null)
            {
                if (previous == null)
                {
                    Head = current.Next;
                }
                else
                {
                    previous.Next = current.Next;
                }
            }
        }

    }
}