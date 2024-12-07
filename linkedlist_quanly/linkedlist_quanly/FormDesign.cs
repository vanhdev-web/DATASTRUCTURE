using linkedlist_quanly.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;



namespace linkedlist_quanly
{
 
    public partial class MainForm : CustomizedForm
    {
        private SocialMediaLinkedList postList;
        private string currentUser = "CurrentUser";
        private bool isProfileView = false;
        private FlowLayoutPanel postsPanel;
        private Random random = new Random();


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e); // Call the base method
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // or SmoothingMode.
        }






        public MainForm()
        {
            // Show login form first
            using (var loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() != DialogResult.OK)
                {
                    Application.Exit();
                    return;
                }
                currentUser = loginForm.LoggedInUser;
            }
            //InitializeComponent();
            postList = new SocialMediaLinkedList();
            this.StartPosition = FormStartPosition.CenterScreen; // Đặt vị trí form ở giữa màn hình
            this.FormBorderStyle = FormBorderStyle.None; // Bỏ viền form
            AddSamplePosts();
            InitializeUI();
            //Di chuyển 
            // Enable double buffering
            this.DoubleBuffered = true;
        }

        private void AddSamplePosts()
        {

            postList.AddPost(
                "Hello, world!",
                "https://example.com/hello.jpg",
                "CurrentUser",
                new DateTime(2024, 11, 20, 14, 30, 0) // 20/11/2024 14:30:00
            );

            postList.AddPost(
                "My first video",
                "https://example.com/video.mp4",
                "OtherUser",
                new DateTime(2024, 11, 21, 9, 15, 0) // 21/11/2024 09:15:00
            );

            postList.AddPost(
                "Funny GIF",
                "https://example.com/funny.gif",
                "CurrentUser",
                new DateTime(2024, 11, 21, 16, 45, 0) // 21/11/2024 16:45:00
            );

            postList.AddPost(
                "Beautiful sunset today!",
                "https://example.com/sunset.jpg",
                "OtherUser",
                new DateTime(2024, 11, 22, 10, 20, 0) // 22/11/2024 10:20:00
            );

            postList.AddPost(
                "Just finished my project!",
                null,
                "CurrentUser",
                new DateTime(2024, 11, 22, 11, 30, 0) // 22/11/2024 11:30:00
            );
            postList.AddPost(
                 "I am handsome!",
                null,
                "OthertUser",
                new DateTime(2024, 11, 21, 11, 30, 0) // 22/11/2024 11:30:00
                );
            postList.AddPost(
                 "I get into Havard!",
                null,
                "OthertUser",
                new DateTime(2024, 11, 20, 11, 30, 0) // 22/11/2024 11:30:00
                );
            postList.ShufflePosts(); ;
        }

        private string FormatTimeAgo(DateTime postTime)
        {
            TimeSpan timeDiff = DateTime.Now - postTime;

            if (timeDiff.TotalSeconds < 60)
                return $"{Math.Floor(timeDiff.TotalSeconds)} giây trước";
            if (timeDiff.TotalMinutes < 60)
                return $"{Math.Floor(timeDiff.TotalMinutes)} phút trước";
            if (timeDiff.TotalHours < 24)
                return $"{Math.Floor(timeDiff.TotalHours)} giờ trước";
            if (timeDiff.TotalDays < 7)
                return $"{Math.Floor(timeDiff.TotalDays)} ngày trước";

            return postTime.ToString("dd/MM/yyyy HH:mm:ss");
        }

        private void InitializeUI()
        {
            this.Size = new Size(320, 650);

            //Icon Social+ 
            System.Windows.Forms.Label app_name = new System.Windows.Forms.Label
            {
                Text = "facebook",
                ForeColor = Color.FromArgb(255, 1, 95, 105),
                Location = new Point(3, 7),
                Size = new Size(140, 40), // Kích thước cố định
                Font = new Font("Inter", 20, FontStyle.Bold),
            };
           

            Panel navigationPanel = new Panel
            {
                Size = new Size(320, 40),
                Location = new Point(0, 35),
                BackColor = Color.Transparent
            };

            RoundedPictureBox homeButton = new RoundedPictureBox
            {
                Location = new Point(5, 5),
                Size = new Size(100, 30),
                Image = ResizeImage(Image.FromFile("Resources/home.png"), 30, 30),
                SizeMode = PictureBoxSizeMode.CenterImage, // Adjust image to fit
                BackColor = Color.Transparent // Optional: make background transparent

            };

            // Add mouse event handlers for click behavior
            homeButton.MouseEnter += (s, e) => homeButton.BackColor = Color.FromArgb(255, 227, 229, 228); // Optional: hover effect
            homeButton.MouseLeave += (s, e) => homeButton.BackColor = Color.Transparent; // Reset hover effect*/

            RoundedPictureBox profileButton = new RoundedPictureBox
            {
                Location = new Point(110, 5),
                Size = new Size(100, 30), 
                Image = ResizeImage(Image.FromFile("Resources/profile.png"), 30, 30),
                SizeMode = PictureBoxSizeMode.CenterImage, // Adjust image to fit
                BackColor = Color.Transparent // Optional: make background transparent
            };
            // Add mouse event handlers for click behavior
            profileButton.MouseEnter += (s, e) => profileButton.BackColor = Color.FromArgb(255, 227, 229, 228); // Optional: hover effect
            profileButton.MouseLeave += (s, e) => profileButton.BackColor = Color.Transparent; // Reset hover effect*/

            RoundedPictureBox notificationButton = new RoundedPictureBox
            {
                Location = new Point(220, 5),
                Size = new Size(100, 30),
                Image = ResizeImage(Image.FromFile("Resources/bell.png"), 30, 30),
                SizeMode = PictureBoxSizeMode.CenterImage, // Adjust image to fit
                BackColor = Color.Transparent // Optional: make background transparent
            };
            notificationButton.MouseEnter += (s, e) => notificationButton.BackColor = Color.FromArgb(255, 227, 229, 228); // Optional: hover effect
            notificationButton.MouseLeave += (s, e) => notificationButton.BackColor = Color.Transparent; // Reset hover effect*/

            Panel separator0 = new Panel
            {
                Size = new Size(330, 2), // Width of the postsPanel, height of the line
                Location = new Point(0, notificationButton.Bottom),
                BackColor = Color.FromArgb(255, 227, 229, 228), // Color of the line
                Margin = new Padding(0, 0, 0, 10) // Optional margin
            };
            Panel separatorForButton = new Panel
            {
                Size = new Size(105, 5), // Width of the postsPanel, height of the line
                Location = new Point(0, notificationButton.Bottom),
                BackColor = Color.FromArgb(255, 1, 95, 105), // Color of the line
                Margin = new Padding(0, 0, 0, 10) // Optional margin
            };

            navigationPanel.Controls.AddRange(new Control[] { homeButton, profileButton, notificationButton, separator0, separatorForButton });

            Panel uploadPanel  = new Panel
            {
                Size = new Size(320, 40),
                Location = new Point(0, 80)
            };

            RoundedPictureBox uploadButton = new RoundedPictureBox
            {
                CornerRadius = 30,
                Location = new Point(55, 0),
                Size = new Size(225, 30),
                DisplayText = "Bạn đang nghĩ gì?", // Set the text to display
                TextColor = Color.Black, // Set the text color to 
                TextStartX = 11,
                TextFont = new Font("Arial", 10, FontStyle.Regular), // Set the font style
                BackColor = Color.Transparent, // Optional: make background transparent
                ShowBorder = true,
                BorderColor = Color.FromArgb(255, 227, 229, 228),
                BorderThickness = 4
            };
            uploadButton.MouseEnter += (s, e) => uploadButton.BackColor = Color.FromArgb(255, 227, 229, 228); // Optional: hover effect
            uploadButton.MouseLeave += (s, e) => uploadButton.BackColor = Color.Transparent; // Reset hover effect*/

            RoundedPictureBox uploadPicture = new RoundedPictureBox
            {
                Location = new Point(283, 0),
                Size = new Size(30, 30),
                Image = ResizeImage(Image.FromFile("Resources/photos.png"), 30, 30),
                SizeMode = PictureBoxSizeMode.CenterImage, // Adjust image to fit
                BackColor = Color.Transparent // Optional: make background transparent
            };
            uploadPicture.MouseEnter += (s, e) => uploadPicture.BackColor = Color.FromArgb(255, 227, 229, 228); // Optional: hover effect
            uploadPicture.MouseLeave += (s, e) => uploadPicture.BackColor = Color.Transparent; // Reset hover effect*/

            RoundedPictureBox uehLogo = new RoundedPictureBox
            {
                Location = new Point(9, 2),
                Size = new Size(40, 25),
                Image = ResizeImage(Image.FromFile("Resources/logo.png"), 40, 25),
                SizeMode = PictureBoxSizeMode.CenterImage, // Adjust image to fit
                BackColor = Color.Transparent // Optional: make background transparent
            };
            Panel separator1 = new Panel
            {
                Size = new Size(330, 3), // Width of the postsPanel, height of the line
                Location = new Point (0,uploadButton.Bottom + 8),
                BackColor = Color.Gray, // Color of the line
                Margin = new Padding(0, 0, 0, 10) // Optional margin
            };

            uploadPanel.Controls.AddRange(new Control[] { uploadButton, uploadPicture, uehLogo, separator1 });




            TextBox contentBox = new TextBox
            {
                Multiline = true,
                Size = new Size(400, 20),
                Location = new Point(20, 50)
            };

            Button postBtn = new Button
            {
                Text = "Đăng bài",
                Location = new Point(120, 180)
            };

            postsPanel = new FlowLayoutPanel
            {
                Size = new Size(340, 600),
                Location = new Point(0, 150),
                AutoScroll = true,
            };

            homeButton.Click += (s, e) =>
            {
                isProfileView = false;
                RefreshPosts();
            };

            profileButton.Click += (s, e) =>
            {
                isProfileView = true;
                RefreshPosts();
            };

            string selectedMediaPath = "";
            uploadPicture.Click += (s, e) =>
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Media files (*.jpg, *.gif, *.mp4)|*.jpg;*.gif;*.mp4";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        selectedMediaPath = ofd.FileName;
                    }
                }
            };

            postBtn.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(contentBox.Text))
                {
                    postList.AddPost(contentBox.Text, selectedMediaPath, currentUser);
                    RefreshPosts();
                    contentBox.Clear();
                    selectedMediaPath = "";
                }
            };

            this.Controls.AddRange(new Control[] {
                //contentBox,
                navigationPanel,
                uploadPanel,
                //postBtn,
                postsPanel,
                app_name
            });

            RefreshPosts();
        }

        private void RefreshPosts()
        {
            postsPanel.Controls.Clear();

            // Lấy danh sách bài viết tùy theo chế độ xem
            var posts = isProfileView
                 ? postList.GetUserPosts(currentUser)
                 : postList.GetAllPosts().Where(post => post.Author != currentUser).ToList();

            foreach (var post in posts)
            {
               
                Panel postPanel = new Panel
                {
                    Size = new Size(320, 350),
                    BorderStyle = BorderStyle.None,
                    Margin = new Padding(0, 0, 0, 10)
                };

                // Thông tin tác giả và thời gian
                Panel headerPanel = new Panel
                {
                    Size = new Size(300, 30),
                    Location = new Point(10, 10)
                };

                System.Windows.Forms.Label authorLabel = new System.Windows.Forms.Label
                {
                    Text = post.Author,
                    Font = new Font(this.Font, FontStyle.Bold),
                    Location = new Point(0, 5),
                    AutoSize = true
                };

                System.Windows.Forms.Label timeLabel = new System.Windows.Forms.Label
                {
                    Text = $"• {FormatTimeAgo(post.PostTime)}",
                    Font = new Font(this.Font.FontFamily, 8, FontStyle.Regular), // Change 12 to your desired font size
                    ForeColor = Color.Gray,
                    Location = new Point(authorLabel.Right, 5),
                    AutoSize = true
                };

                // Tooltip cho thời gian chính xác
                ToolTip tooltip = new ToolTip();
                tooltip.SetToolTip(timeLabel, post.PostTime.ToString("dd/MM/yyyy HH:mm:ss"));

                headerPanel.Controls.AddRange(new Control[] { authorLabel, timeLabel });

                System.Windows.Forms.Label contentLabel = new System.Windows.Forms.Label
                {
                    Text = post.Content,
                    Font = new Font(this.Font.FontFamily, 8, FontStyle.Regular), // Change 12 to your desired font size
                    Size = new Size(300, 60),
                    Location = new Point(10, 45)

                };

                if (!string.IsNullOrEmpty(post.MediaReference))
                {
                    string extension = Path.GetExtension(post.MediaReference).ToLower();
                    if (extension == ".jpg" || extension == ".gif")
                    {
                        PictureBox pictureBox = new PictureBox
                        {
                            ImageLocation = post.MediaReference,
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Size = new Size(500, 300),
                            Location = new Point(10, 110)
                        };
                        postPanel.Controls.Add(pictureBox);
                    }
                    else if (extension == ".mp4")
                    {
                        System.Windows.Forms.Label mediaLabel = new System.Windows.Forms.Label
                        {
                            Text = "Selected video: " + Path.GetFileName(post.MediaReference),
                            Location = new Point(10, 110),
                            Size = new Size(300, 20)
                        };
                        postPanel.Controls.Add(mediaLabel);
                    }
                }

                postPanel.Controls.AddRange(new Control[] {
            headerPanel,
            contentLabel
        });

                postsPanel.Controls.Add(postPanel);
                // Add a separator line after each post
                Panel separator = new Panel
                {
                    Size = new Size(330, 3), // Width of the postsPanel, height of the line
                    BackColor = Color.Gray, // Color of the line
                    Margin = new Padding(0, 0, 0, 10) // Optional margin
                };

                postsPanel.Controls.Add(separator);
            }
        }


        //Handle button's icon
        private void SetButtonImage(Button button, string imagePath)
        {
            Image originalImage = Image.FromFile(imagePath);
            button.Image = ResizeImage(originalImage, button.Width, button.Height);
        }

        private Image ResizeImage(Image img, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic; // Set high-quality interpolation
                g.DrawImage(img, 0, 0, width, height);
            }
            return resizedImage;
        }
    }
}

