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
            // Xóa tất cả bài viết hiện tại trong panel
            postsPanel.Controls.Clear();

            // Lấy danh sách bài viết
            var posts = postList.GetAllPosts();

            // Duyệt qua danh sách bài viết để tạo giao diện
            foreach (var post in posts)
            {
                // Nếu ở chế độ Home, bỏ qua bài viết được chia sẻ (SharedBy không null)
                if (!isProfileView && post.SharedBy != null)
                {
                    continue;
                }

                // Nếu đang ở chế độ Profile, chỉ hiển thị bài viết của currentUser (bao gồm bài viết chia sẻ)
                if (isProfileView && post.Author != currentUser && post.SharedBy != currentUser)
                {
                    continue;
                }

                // Tạo giao diện bài viết
                Panel postPanel = new Panel
                {
                    Size = new Size(920, 350),
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(0, 0, 0, 10),
                    AutoScroll = true
                };

                // Tạo header (tác giả và thời gian)
                Panel headerPanel = new Panel
                {
                    Size = new Size(900, 30),
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
                    ForeColor = Color.Gray,
                    Location = new Point(authorLabel.Right + 10, 5),
                    AutoSize = true
                };

                headerPanel.Controls.AddRange(new Control[] { authorLabel, timeLabel });

                // Hiển thị nội dung bài viết
                System.Windows.Forms.Label contentLabel = new System.Windows.Forms.Label
                {
                    Text = post.SharedBy != null
                        ? $"{post.SharedBy} đã chia sẻ: {post.Content}" // Hiển thị nếu là bài viết chia sẻ
                        : post.Content,
                    Size = new Size(900, 60),
                    Location = new Point(10, 45)
                };

                // Thêm các phần tử vào postPanel
                postPanel.Controls.AddRange(new Control[] { headerPanel, contentLabel });

                // Chỉ hiển thị nút chia sẻ nếu đang ở chế độ Home
                if (!isProfileView)
                {
                    Button shareButton = new Button
                    {
                        Text = "Chia sẻ",
                        Location = new Point(10, 110),
                        Size = new Size(100, 30)
                    };
                    shareButton.Click += (s, e) =>
                    {
                        postList.AddPost(post.Content, post.MediaReference, currentUser); // Thêm bài viết chia sẻ
                        RefreshPosts(); // Làm mới danh sách bài viết
                    };
                    postPanel.Controls.Add(shareButton);
                }


                // Tạo một TextBox để bình luận dưới mỗi bài viết
                TextBox commentTextBox = new TextBox
                    {
                        Location = new Point(10, 150), // Vị trí của TextBox dưới bài viết
                        Size = new Size(300, 30),
                        MaxLength = 50, // Giới hạn số ký tự nhập
                        ForeColor = Color.Gray // Màu nhạt cho placeholder
                    };

                    // Đặt placeholder (text mặc định) cho TextBox
                    string placeholder = "Viết bình luận...(tối đa 50 ký tự)";
                    commentTextBox.Text = placeholder;

                    // Sự kiện khi người dùng bắt đầu nhập liệu
                    commentTextBox.Enter += (sender, e) =>
                    {
                        if (commentTextBox.Text == placeholder)
                        {
                            commentTextBox.Text = "";
                            commentTextBox.ForeColor = Color.Black; // Màu chữ khi người dùng nhập
                        }
                    };

                    // Sự kiện khi TextBox mất focus và không có nội dung
                    commentTextBox.Leave += (sender, e) =>
                    {
                        if (string.IsNullOrEmpty(commentTextBox.Text))
                        {
                            commentTextBox.Text = placeholder;
                            commentTextBox.ForeColor = Color.Gray; // Màu nhạt khi không có nội dung
                        }
                    };

                    // Tạo nút "Bình luận"
                    Button commentButton = new Button
                    {
                        Text = "Bình luận",
                        Location = new Point(10, 190),
                        Size = new Size(100, 30)
                    };

                // Sự kiện khi người dùng nhấn "Bình luận"
                commentButton.Click += (s, e) =>
                {
                    string commentText = commentTextBox.Text;
                    if (!string.IsNullOrEmpty(commentText) && commentText != placeholder)
                    {
                        // Add the new comment to the list
                        Comment newComment = new Comment(commentText, currentUser, DateTime.Now);
                        post.Comments.Add(newComment);

                        // Reset TextBox
                        commentTextBox.Text = placeholder; // Reset the placeholder
                        commentTextBox.ForeColor = Color.Gray;

                        // Refresh posts to display new comment
                        RefreshPosts();
                    }
                };

                // Thêm TextBox và Button vào postPanel
                postPanel.Controls.Add(commentTextBox);
                    postPanel.Controls.Add(commentButton);

                if (post.Comments == null)
                {
                    post.Comments = new List<Comment>();
                }
                System.Windows.Forms.Label postContentLabel = new System.Windows.Forms.Label
                   {
               Text = post.Content, // Lấy nội dung bài viết từ thuộc tính Content
                 Size = new Size(900, 50),
                Location = new Point(10, 10), // Nội dung bài viết nằm ở đầu panel
                    ForeColor = Color.Black
                   };
                postPanel.Controls.Add(postContentLabel);

                // Hiển thị tất cả bình luận của bài viết
                if (post.Comments != null && post.Comments.Count > 0)   
                {
                    int yOffset = 230; // Dịch chuyển vị trí hiển thị bình luận
                    foreach (var comment in post.Comments)
                    {
                        System.Windows.Forms.Label commentLabel = new System.Windows.Forms.Label
                        {
                            Text = $"{comment.Author}: {comment.Text}",
                            Location = new Point(10, yOffset),
                            Size = new Size(900, 30),
                            ForeColor = Color.Black
                        };
                        postPanel.Controls.Add(commentLabel);
                        yOffset += 35; // Tăng độ cao để hiển thị các bình luận tiếp theo
                    }
                }

                // Thêm panel bài viết vào danh sách hiển thị
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


