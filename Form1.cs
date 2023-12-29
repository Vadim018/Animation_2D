using System;
using System.Drawing;
using System.Windows.Forms;
namespace lr_3
{
    public partial class Form1 : Form
    {
        private PictureBox pictureBox;
        private Timer animationTimer;
        private Random random = new Random();
        private int objectSize = 50;
        private int objectSpeed = 5;
        private int objectDirectionX = 1;
        private int objectDirectionY = 1;

        public Form1()
        {
            InitializeComponent();
            InitializePictureBox();
            InitializeMenu();
            InitializeTimer();
        }

        private void InitializePictureBox()
        {
            pictureBox = new PictureBox();
            pictureBox.Size = new Size(500, 500);
            pictureBox.BackColor = Color.White;
            pictureBox.Paint += PictureBox1_Paint;
            Controls.Add(pictureBox);
        }

        private void InitializeMenu()
        {
            MenuStrip menuStrip = new MenuStrip();
            ToolStripMenuItem imageMenu = new ToolStripMenuItem("&Image");
            imageMenu.DropDownItems.Add("New", null, NewImage);
            imageMenu.DropDownItems.Add("Open", null, OpenImage);

            ToolStripMenuItem drawMenu = new ToolStripMenuItem("Draw");
            drawMenu.DropDownItems.Add("Animal", null, DrawAnimal);
            menuStrip.Items.Add(imageMenu);
            menuStrip.Items.Add(drawMenu);

            MainMenuStrip = menuStrip;
            Controls.Add(menuStrip);
        }

        private void InitializeTimer()
        {
            animationTimer = new Timer();
            animationTimer.Interval = 20;
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();
        }

        private void NewImage(object sender, EventArgs e)
        {
            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);
            pictureBox.Invalidate();
        }

        private void OpenImage(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All files|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Image originalImage = Image.FromFile(openFileDialog.FileName);

                    float scaleWidth = (float)pictureBox.Width / originalImage.Width;
                    float scaleHeight = (float)pictureBox.Height / originalImage.Height;
                    float scale = Math.Min(scaleWidth, scaleHeight);
                    int newWidth = (int)(originalImage.Width * scale);
                    int newHeight = (int)(originalImage.Height * scale);
                    Image resizedImage = new Bitmap(newWidth, newHeight);

                    using (Graphics g = Graphics.FromImage(resizedImage))
                    {
                        g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                    }

                    int margin = 0;
                    int smallerWidth = pictureBox.Width - margin;
                    int smallerHeight = pictureBox.Height - margin;
                    Image smallerImage = new Bitmap(smallerWidth, smallerHeight);

                    using (Graphics g = Graphics.FromImage(smallerImage))
                    {
                        g.DrawImage(resizedImage, 0, 0, smallerWidth, smallerHeight);
                    }
                    pictureBox.Image = smallerImage;
                    pictureBox.Invalidate();
                }
            }
        }

        private void DrawAnimal(object sender, EventArgs e)
        {
            using (Graphics g = Graphics.FromImage(pictureBox.Image))
            {
                int x = random.Next(0, pictureBox.Width - objectSize);
                int y = random.Next(0, pictureBox.Height - objectSize);
                Color randomColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                SolidBrush brush = new SolidBrush(randomColor);
                g.FillEllipse(brush, x, y, objectSize, objectSize);
            }
            pictureBox.Invalidate();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            int newX = pictureBox.Location.X + objectSpeed * objectDirectionX;
            int newY = pictureBox.Location.Y + objectSpeed * objectDirectionY;

            if (newX < 0 || newX + pictureBox.Width > ClientSize.Width)
            {
                objectDirectionX *= -1;
            }

            if (newY < 0 || newY + pictureBox.Height > ClientSize.Height)
            {
                objectDirectionY *= -1;
            }
            pictureBox.Location = new Point(newX, newY);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.KeyCode)
            {
                case Keys.Up:
                    objectDirectionY = -1;
                    break;
                case Keys.Down:
                    objectDirectionY = 1;
                    break;
                case Keys.Left:
                    objectDirectionX = -1;
                    break;
                case Keys.Right:
                    objectDirectionX = 1;
                    break;
            }
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}


/*

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lr_3
{
    public partial class Form1 : Form
    {

        private PictureBox pictureBox;
        private Image sunImage;
        private Image moonImage;
        public Form1()
        {
            InitializeComponent();

            // Ініціалізація компонентів
            InitializeComponents();

            sunImage = DrawSun();
            moonImage = DrawMoon();
        }

        private Image DrawSun()
        {
            Bitmap sun = new Bitmap(100, 100);

            using (Graphics g = Graphics.FromImage(sun))
            {
                g.Clear(Color.Yellow); // Сонце - жовта кругла форма
            }

            return sun;
        }

        private Image DrawMoon()
        {
            Bitmap moon = new Bitmap(100, 100);

            using (Graphics g = Graphics.FromImage(moon))
            {
                g.Clear(Color.Gray); // Місяць - сірий кругла форма
            }

            return moon;
        }

        private void InitializeComponents()
        {
            // Ініціалізація PictureBox
            pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Додавання PictureBox на форму
            Controls.Add(pictureBox);

            // Ініціалізація меню
            MenuStrip menuStrip = new MenuStrip();

            // Підменю Image
            ToolStripMenuItem imageMenu = new ToolStripMenuItem("Image");
            imageMenu.DropDownItems.Add("New", null, NewImage);
            imageMenu.DropDownItems.Add("Open", null, OpenImage);

            // Підменю Draw
            ToolStripMenuItem drawMenu = new ToolStripMenuItem("Draw");
            ToolStripMenuItem animalSubMenu = new ToolStripMenuItem("Animal");
            animalSubMenu.DropDownItems.Add("День", null, DayButton_Click);
            animalSubMenu.DropDownItems.Add("Ніч", null, NightButton_Click);
            drawMenu.DropDownItems.Add(animalSubMenu);

            // Додавання підменю до меню
            menuStrip.Items.Add(imageMenu);
            menuStrip.Items.Add(drawMenu);

            // Додавання меню на форму
            Controls.Add(menuStrip);
        }


        private void DayButton_Click(object sender, EventArgs e)
        {
            // Відображення сонця
            pictureBox.Image = sunImage;
        }

        private void NightButton_Click(object sender, EventArgs e)
        {
            // Відображення місяця
            pictureBox.Image = moonImage;
        }

        private void NewImage(object sender, EventArgs e)
        {
            // Створення нового зображення (припустимо, зображення має бути розміром 100x100)
            Bitmap newImage = new Bitmap(100, 100);

            // Встановлення білого фону
            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.Clear(Color.White);
            }

            // Встановлення нового зображення в PictureBox
            pictureBox.Image = newImage;
        }

        private void OpenImage(object sender, EventArgs e)
        {
            // Відкриття діалогового вікна для вибору файлу
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All Files|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Завантаження зображення з вибраного файлу
                Image loadedImage = Image.FromFile(openFileDialog.FileName);

                // Встановлення завантаженого зображення в PictureBox
                pictureBox.Image = loadedImage;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

*/

/*
 
using System;
using System.Drawing;
using System.Windows.Forms;
namespace lr_3
{
    public partial class Form1 : Form
    {
        private Timer animationTimer;
        private Random random = new Random();
        private int objectSize = 50;
        private int objectSpeed = 5;
        private int objectDirectionX = 1;
        private int objectDirectionY = 1;

        public Form1()
        {
            InitializeComponent();
            InitializePictureBox();
            InitializeMenu();
            InitializeTimer();
        }

        private void InitializePictureBox()
        {
            if (pictureBox1 == null)
            {
                pictureBox1 = new PictureBox();
                SetPictureBoxProperties();
                Controls.Add(pictureBox1);
            }
            else
            {
                SetPictureBoxProperties();
            }
        }

        private void SetPictureBoxProperties()
        {
            pictureBox1.Size = new Size(500, 500);
            pictureBox1.BackColor = Color.White;
        }

        private void InitializeMenu()
        {
            MenuStrip menuStrip = new MenuStrip();
            ToolStripMenuItem imageMenu = new ToolStripMenuItem("&Image");
            imageMenu.DropDownItems.Add("New", null, NewImage);
            imageMenu.DropDownItems.Add("Open", null, OpenImage);

            ToolStripMenuItem drawMenu = new ToolStripMenuItem("Draw");
            drawMenu.DropDownItems.Add("Animal", null, DrawAnimal);
            menuStrip.Items.Add(imageMenu);
            menuStrip.Items.Add(drawMenu);

            MainMenuStrip = menuStrip;
            Controls.Add(menuStrip);
        }

        private void InitializeTimer()
        {
            animationTimer = new Timer();
            animationTimer.Interval = 20;
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();
        }

        private void NewImage(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Invalidate();
        }

        private void OpenImage(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All files|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Image originalImage = Image.FromFile(openFileDialog.FileName);

                    float scaleWidth = (float)pictureBox1.Width / originalImage.Width;
                    float scaleHeight = (float)pictureBox1.Height / originalImage.Height;
                    float scale = Math.Min(scaleWidth, scaleHeight);
                    int newWidth = (int)(originalImage.Width * scale);
                    int newHeight = (int)(originalImage.Height * scale);
                    Image resizedImage = new Bitmap(newWidth, newHeight);

                    using (Graphics g = Graphics.FromImage(resizedImage))
                    {
                        g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                    }

                    int margin = 0;
                    int smallerWidth = pictureBox1.Width - margin;
                    int smallerHeight = pictureBox1.Height - margin;
                    Image smallerImage = new Bitmap(smallerWidth, smallerHeight);

                    using (Graphics g = Graphics.FromImage(smallerImage))
                    {
                        g.DrawImage(resizedImage, 0, 0, smallerWidth, smallerHeight);
                    }
                    pictureBox1.Image = smallerImage;
                    pictureBox1.Invalidate();
                }
            }
        }

 private void DrawAnimal(object sender, EventArgs e)
        {
            using (Graphics g = Graphics.FromImage(pictureBox1.Image))
            {
                int x = random.Next(0, pictureBox1.Width - objectSize);
                int y = random.Next(0, pictureBox1.Height - objectSize);
                Color randomColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                SolidBrush brush = new SolidBrush(randomColor);
                g.FillEllipse(brush, x, y, objectSize, objectSize);
            }
            pictureBox1.Invalidate();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            int newX = pictureBox1.Location.X + objectSpeed * objectDirectionX;
            int newY = pictureBox1.Location.Y + objectSpeed * objectDirectionY;

            if (newX < 0  newX + pictureBox1.Width > ClientSize.Width)
            {
                objectDirectionX *= -1;
            }

            if (newY < 0  newY + pictureBox1.Height > ClientSize.Height)
            {
                objectDirectionY *= -1;
            }
            pictureBox1.Location = new Point(newX, newY);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.KeyCode)
            {
                case Keys.Up:
                    objectDirectionY = -1;
                    break;
                case Keys.Down:
                    objectDirectionY = 1;
                    break;
                case Keys.Left:
                    objectDirectionX = -1;
                    break;
                case Keys.Right:
                    objectDirectionX = 1;
                    break;
            }
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
 */