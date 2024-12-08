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
                "OtherUser",
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
                "OtherUser",
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
                "OtherUser",
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

            Panel uploadPanel = new Panel
            {
                Size = new Size(320, 40),
                Location = new Point(0, 85)
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
                Location = new Point(0, uploadButton.Bottom + 8),
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
                viewMode = ViewMode.Default; // Set default view mode
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
            var posts = postList.GetAllPosts();

            foreach (var post in posts)
            {
                // Kiểm tra điều kiện hiển thị
                if (isProfileView)
                {
                    // Trong trang cá nhân: hiển thị bài viết của user và bài user đã share
                    if (post.Author != currentUser && post.SharedBy != currentUser)
                        continue;
                }
                else
                {
                    // Trong trang chủ: chỉ hiển thị bài viết gốc (không phải bài share)
                    if (post.SharedBy != null)
                        continue;
                }

                Panel postPanel = new Panel
                {
                    Size = new Size(920, 350),
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(0, 0, 0, 10),
                    AutoScroll = true
                };

                // Header panel với thông tin tác giả
                Panel headerPanel = new Panel
                {
                    Size = new Size(900, 60),
                    Location = new Point(10, 10),
                    BackColor = Color.Transparent
                };

                int yOffset = 5;

                RoundedPictureBox userAvatar = new RoundedPictureBox
                {
                    Size = new Size(30, 30),
                    Location = new Point(0, yOffset),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BackColor = Color.Transparent
                };

                // Load avatar của người đăng bài
                string avatarPath = new UserManager().GetUserAvatar(post.Author);
                try
                {
                    userAvatar.Image = Image.FromFile(avatarPath);
                }
                catch
                {
                    userAvatar.Image = Image.FromFile("Resources/default-avatar.png");
                }

                // Nếu là bài share, hiển thị thông tin người share trước
                if (post.SharedBy != null)
                {
                    System.Windows.Forms.Label shareInfoLabel = new System.Windows.Forms.Label
                    {
                        Text = $"{post.SharedBy} đã chia sẻ",
                        Font = new Font(this.Font.FontFamily, 10, FontStyle.Regular),
                        Location = new Point(0, yOffset),
                        ForeColor = Color.Gray,
                        AutoSize = true
                    };

                    System.Windows.Forms.Label shareTimeLabel = new System.Windows.Forms.Label
                    {
                        Text = $"• {FormatTimeAgo(post.PostTime)}",
                        ForeColor = Color.Gray,
                        Location = new Point(shareInfoLabel.Right + 10, yOffset),
                        Font = new Font(this.Font.FontFamily, 10, FontStyle.Regular),
                        AutoSize = true
                    };

                    headerPanel.Controls.AddRange(new Control[] { shareInfoLabel, shareTimeLabel });
                    yOffset += 20;
                }

                // Thông tin bài viết gốc
                System.Windows.Forms.Label authorLabel = new System.Windows.Forms.Label
                {
                    Text = post.Author,
                    Font = new Font(this.Font.FontFamily, 11, FontStyle.Bold),
                    Location = new Point(userAvatar.Right + 10, yOffset + 5),
                    AutoSize = true
                };

                System.Windows.Forms.Label timeLabel = new System.Windows.Forms.Label
                {
                    Text = post.SharedBy != null
                    ? $"• Đã đăng {FormatTimeAgo(post.OriginalPost?.PostTime ?? post.PostTime)}"
                    : $"• {FormatTimeAgo(post.PostTime)}",
                    ForeColor = Color.Gray,
                    Location = new Point(authorLabel.Right + 10, yOffset + 5),
                    Font = new Font(this.Font.FontFamily, 11, FontStyle.Regular),
                    AutoSize = true
                };

                headerPanel.Controls.AddRange(new Control[] { userAvatar, authorLabel, timeLabel });

                // Content
                System.Windows.Forms.Label contentLabel = new System.Windows.Forms.Label
                {
                    Text = post.Content,
                    Size = new Size(900, 60),
                    Location = new Point(10, headerPanel.Bottom + 5)
                };

                postPanel.Controls.AddRange(new Control[] { headerPanel, contentLabel });

                // Panel chứa các nút action
                Panel actionPanel = new Panel
                {
                    Size = new Size(900, 40),
                    Location = new Point(10, contentLabel.Bottom + 5),
                    BackColor = Color.Transparent
                };

                // Thêm nút share nếu đang ở trang chủ và không phải bài viết của chính mình
                if (!isProfileView && post.Author != currentUser)
                {
                    Button shareButton = new Button
                    {
                        Text = "Chia sẻ",
                        Location = new Point(0, 5),
                        Size = new Size(100, 30)
                    };

                    shareButton.Click += (s, e) =>
                    {
                        postList.SharePost(post, currentUser);
                        RefreshPosts();
                    };

                    actionPanel.Controls.Add(shareButton);
                }

                postPanel.Controls.Add(actionPanel);

                // Panel chứa phần comments và input
                Panel commentSection = new Panel
                {
                    Size = new Size(900, 200),
                    Location = new Point(10, actionPanel.Bottom + 5),
                    BackColor = Color.Transparent,
                    AutoScroll = true
                };

                // TextBox để viết comment
                TextBox commentTextBox = new TextBox
                {
                    Location = new Point(0, 0),
                    Size = new Size(300, 30),
                    MaxLength = 50,
                    ForeColor = Color.Gray
                };

                string placeholder = "Viết bình luận...(tối đa 50 ký tự)";
                commentTextBox.Text = placeholder;

                commentTextBox.Enter += (sender, e) =>
                {
                    if (commentTextBox.Text == placeholder)
                    {
                        commentTextBox.Text = "";
                        commentTextBox.ForeColor = Color.Black;
                    }
                };

                commentTextBox.Leave += (sender, e) =>
                {
                    if (string.IsNullOrEmpty(commentTextBox.Text))
                    {
                        commentTextBox.Text = placeholder;
                        commentTextBox.ForeColor = Color.Gray;
                    }
                };

                // Nút comment
                Button commentButton = new Button
                {
                    Text = "Bình luận",
                    Location = new Point(310, 0),
                    Size = new Size(100, 30)
                };

                commentButton.Click += (s, e) =>
                {
                    string commentText = commentTextBox.Text;
                    if (!string.IsNullOrEmpty(commentText) && commentText != placeholder)
                    {
                        Comment newComment = new Comment(commentText, currentUser, DateTime.Now);
                        post.Comments.Add(newComment);
                        commentTextBox.Text = placeholder;
                        commentTextBox.ForeColor = Color.Gray;
                        RefreshPosts();
                    }
                };

                // Thêm controls comment vào section
                commentSection.Controls.AddRange(new Control[] { commentTextBox, commentButton });

                // Hiển thị comments hiện có
                if (post.Comments != null && post.Comments.Count > 0)
                {
                    int commentY = commentButton.Bottom + 10;
                    foreach (var comment in post.Comments)
                    {
                        System.Windows.Forms.Label commentLabel = new System.Windows.Forms.Label
                        {
                            Text = $"{comment.Author}: {comment.Text}",
                            Location = new Point(0, commentY),
                            Size = new Size(880, 30),
                            Font = new Font(this.Font.FontFamily, 10),
                            ForeColor = Color.Black
                        };

                        System.Windows.Forms.Label commentTimeLabel = new System.Windows.Forms.Label
                        {
                            Text = $"• {FormatTimeAgo(comment.PostTime)}",
                            Location = new Point(commentLabel.Right - 150, commentY),
                            AutoSize = true,
                            Font = new Font(this.Font.FontFamily, 9),
                            ForeColor = Color.Gray
                        };

                        commentSection.Controls.AddRange(new Control[] { commentLabel, commentTimeLabel });
                        commentY += 35;
                    }

                    // Điều chỉnh chiều cao của comment section
                    commentSection.Height = commentY + 10;
                }

                postPanel.Controls.Add(commentSection);

                // Điều chỉnh chiều cao của postPanel
                postPanel.Size = new Size(postPanel.Width, commentSection.Bottom + 20);

                // Thêm postPanel vào danh sách hiển thị
                postsPanel.Controls.Add(postPanel);
            }
        }




        public enum ViewMode
        {
            Default,
            Filter,
            Profile
        }

        public ViewMode viewMode = ViewMode.Default;


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


