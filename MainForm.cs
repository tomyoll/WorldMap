using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;



namespace WorldMap
{
    public partial class MainForm : Form
    {
        //Строка подключения к БД
        public static string ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source = Database\\Country.mdb";
        private OleDbConnection myConnection;

        public static Bitmap MapBitmap = new Bitmap("img\\BigMap.png");
        public static Bitmap MapBitmapScale = new Bitmap("img\\BigMap.png");

        //Создание элементов управления
        public Graphics pctMap;
        public Color backColor;
        public string hex;
        int x;
        int y;
        bool CheckClick = false;
        OleDbDataReader reader;
        Panel MenuPanel = new Panel();
        Button BtnPreview = new Button();
        Button BtnExit = new Button();
        Button BtnGame = new Button();
        MenuStrip ToolBar = new MenuStrip();
        ToolStripMenuItem ReferenceTool = new ToolStripMenuItem();
        ToolStripMenuItem ScaleTool = new ToolStripMenuItem();
        ToolStripMenuItem UnScaleTool = new ToolStripMenuItem();
        ToolStripMenuItem HotKeysTool = new ToolStripMenuItem();
        ToolStripMenuItem AboutAppTool = new ToolStripMenuItem();
        ToolStripMenuItem ExitTool = new ToolStripMenuItem();
        ToolStripMenuItem MainMenuTool = new ToolStripMenuItem();
        TextBox TxtBRandomCountry = new TextBox();
        GroupBox DescriptBox = new GroupBox();
        TextBox CountryName = new TextBox();
        TextBox CountryDescript = new TextBox();
        PictureBox FlagIcon = new PictureBox();


        public MainForm()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.Selectable, false);
            myConnection = new OleDbConnection(ConnectionString);
            myConnection.Open();
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint, true);
            UpdateStyles();
            
        }
        //Масштабирование
        double scale = 1.0;

        private void MapPicBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)//scroll
            {
                if (scale < 2)
                {
                    scale += 1;
                    if (scale == 2)
                    {
                        {
                            MapBitmap = MapBitmapScale;
                            MapPicBox.Width = MapBitmap.Width * 2;
                            MapPicBox.Height = MapBitmap.Height * 2;
                            MapPicBox.Refresh();
                            MainPanel.Width = 1920;
                            MainPanel.Height = 1019;
                        }
                    }


                }

            }
            else if (e.Delta < 0)//unscroll
            {
                if (scale > 1)
                {
                    scale -= 1;
                    if (scale == 1)
                    {
                        
                        MapBitmap = MapBitmapScale;
                        MapPicBox.Size = ClientSize;
                        MapPicBox.Refresh();
                    }

                }

            }
        }



        public void MapPicBox_Click(object sender, EventArgs e)
        {
            //получение цвета нажатого пикселя
            MouseEventArgs rato = e as MouseEventArgs;
            x = rato.X * MapBitmap.Width / MapPicBox.ClientSize.Width;
            y = rato.Y * MapBitmap.Height / MapPicBox.ClientSize.Height;
            backColor = MapBitmap.GetPixel(x, y);
            hex = backColor.R.ToString("X2") + backColor.G.ToString("X2") + backColor.B.ToString("X2");

            //Запрос к базе данных
            string query = "SELECT  id, NameUK, NameEN, Descript, flag FROM CountryData WHERE Color='#" + hex.ToString() + "'";
            OleDbCommand command = new OleDbCommand(query, myConnection);

            reader = command.ExecuteReader();
            while (reader.Read())
            {
                // элементы массива [] - это значения столбцов из запроса SELECT
                CountryName.Text = reader[1].ToString() + ", " + '\r' + '\n' + reader[2].ToString();
                CountryDescript.Text = reader[3].ToString();
                FlagIcon.Image = Image.FromFile(reader[4].ToString());
            }

            CheckClick = !CheckClick;
            if (CheckClick)
            {
                if (((backColor.R == 0) && (backColor.G == 0) && (backColor.B == 0)) || ((backColor.R == 176) && (backColor.G == 244) && (backColor.B == 254)))
                {

                }

                else
                {
                    changeColor(MapBitmap, backColor, Color.White);
                    changeColor(MapBitmapScale, backColor, Color.White);
                    MapPicBox.Refresh();
                     System.Media.SoundPlayer right = new System.Media.SoundPlayer(@"sound\true.wav");
                    right.Play();
                    MapPicBox.Refresh();
                }
            }
            else
            {
                MapBitmap = new Bitmap(@"img\BigMap.png");
                MapBitmapScale = new Bitmap(@"img\BigMap.png");
                MapPicBox.Refresh();
            }
            MapPicBox.Refresh();
        }

        //Горячие клавиши
        private void MainMenu_KeyDown(object sender, KeyEventArgs e)
        { 
            if (e.KeyCode == Keys.Escape)
            {
                MapBitmap = new Bitmap(@"img\BigMap.png");
                MapBitmapScale = new Bitmap(@"img\BigMap.png");
                MapPicBox.Refresh();
                MapPicBox.MouseWheel -= MapPicBox_MouseWheel;
                MenuPanel.Visible = true;
                DescriptBox.Visible = false;
                TxtBRandomCountry.Visible = false;
                Timer.Enabled = false;
                BtnGame.Click -= Game_MapClick;
            }

            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.E)
            {
                Application.Exit();
            }
        }

        public void MainForm_Load(object sender, EventArgs e)
        { 
            /////Главное меню/////

            MenuPanel.Location = new Point(0, 0);
            MenuPanel.Name = "menu";
            MenuPanel.Size = ClientSize;
            MenuPanel.BackgroundImage = Image.FromFile("img\\BigMapBlure.jpg");
            MapPicBox.Controls.Add(MenuPanel);

            BtnGame.Location = new Point(860, 301);
            BtnGame.Text = "Гра";
            BtnGame.Size = new Size(200, 100);
            BtnGame.Click += new EventHandler(preview_Click);
            BtnGame.Font = new Font("Comic Sans MS", 16F, System.Drawing.FontStyle.Bold);
            BtnGame.Click += new EventHandler(BtnGame_Click);
            BtnGame.Cursor = System.Windows.Forms.Cursors.Hand;
            MenuPanel.Controls.Add(BtnGame);

            BtnPreview.Location = new Point(860, 411);
            BtnPreview.Text = "Вільний режим";
            BtnPreview.Size = new Size(200, 100);
            BtnPreview.Click += new EventHandler(preview_Click);
            BtnPreview.Font = new Font("Comic Sans MS", 16F, System.Drawing.FontStyle.Bold);
            BtnPreview.Cursor = System.Windows.Forms.Cursors.Hand;
            MenuPanel.Controls.Add(BtnPreview);

            BtnExit.Location = new Point(860, 521);
            BtnExit.Text = "Вихід";
            BtnExit.Size = new Size(200, 100);
            BtnExit.Click += new EventHandler(btnExit_Click);
            BtnExit.Font = new Font("Comic Sans MS", 16F, System.Drawing.FontStyle.Bold);
            BtnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            MenuPanel.Controls.Add(BtnExit);

            /////GrupBox с описанием страны/////

            //groupbox
            DescriptBox.AutoSize = true;
            DescriptBox.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            DescriptBox.BackColor = SystemColors.InactiveCaption;
            DescriptBox.Location = new Point(ClientRectangle.X, ClientRectangle.Height - 210);
            DescriptBox.Name = "DescriptBox";
            DescriptBox.Size = new Size(280, 220);
            DescriptBox.Parent = this;
            DescriptBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Controls.Add(DescriptBox);
            DescriptBox.BringToFront();

            //textbox
            CountryName.BackColor = SystemColors.InactiveCaption;
            CountryName.BorderStyle = BorderStyle.None;
            CountryName.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            CountryName.Location = new Point(6, 19);
            CountryName.Multiline = true;
            CountryName.Name = "CountryName";
            CountryName.Size = new Size(147, 65);
            CountryName.ReadOnly = true;
            DescriptBox.Controls.Add(CountryName);

            //texbox
            CountryDescript.BackColor = SystemColors.InactiveCaption;
            CountryDescript.BorderStyle = BorderStyle.None;
            CountryDescript.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));
            CountryDescript.Location = new Point(0, 93);
            CountryDescript.Multiline = true;
            CountryDescript.Name = "CountryDescript";
            CountryDescript.ScrollBars = ScrollBars.Vertical;
            CountryDescript.Size = new Size(273, 126);
            CountryDescript.ReadOnly = true;
            DescriptBox.Controls.Add(CountryDescript);

            //picturebox
            FlagIcon.Location = new Point(144, 19);
            FlagIcon.Name = "FlagIcon";
            FlagIcon.Size = new Size(123, 68);
            DescriptBox.Controls.Add(FlagIcon);

            /////Другое/////

            //Textbox с рандомной страной
            TxtBRandomCountry.BackColor = SystemColors.InactiveCaption;
            TxtBRandomCountry.BorderStyle = BorderStyle.None;
            TxtBRandomCountry.Font = new Font("Comic Sans MS", 17.0F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(204)));
            TxtBRandomCountry.Location = new Point(ClientRectangle.X, ClientRectangle.Height - 320);
            TxtBRandomCountry.Multiline = true;
            TxtBRandomCountry.Name = "RandomCountry";
            TxtBRandomCountry.Size = new Size(279, 110);
            TxtBRandomCountry.Parent = this;
            TxtBRandomCountry.ReadOnly = true;
            Controls.Add(TxtBRandomCountry);
            TxtBRandomCountry.BringToFront();

            //Панель инструментов
            ToolBar.Location = new System.Drawing.Point(0, 0);
            ToolBar.Items.AddRange(new ToolStripItem[] { ReferenceTool });
            ToolBar.Items.AddRange(new ToolStripItem[] { ScaleTool });
            ToolBar.Items.AddRange(new ToolStripItem[] { UnScaleTool });
            ToolBar.Size = new System.Drawing.Size(1908, 24);
            ToolBar.BackColor = Color.FromArgb(236, 236, 240);
            ToolBar.Parent = this;
            ToolBar.ShowItemToolTips = true;
            Controls.Add(ToolBar);
            ToolBar.BringToFront();

            //Справка
            ReferenceTool.AutoSize = true;
            ReferenceTool.DropDownItems.AddRange(new ToolStripItem[] { HotKeysTool });
            ReferenceTool.DropDownItems.AddRange(new ToolStripItem[] { AboutAppTool });
            ReferenceTool.Text = "Довідка";

            ScaleTool.Image = Image.FromFile("img\\Scale.png");
            ScaleTool.Click += new EventHandler(ScaleTool_Click);
            ScaleTool.AutoSize = true;
            ScaleTool.ToolTipText = "Збільшити масштаб";

            UnScaleTool.Image = Image.FromFile("img\\UnScale.png");
            UnScaleTool.Click += new EventHandler(UnScaleTool_Click);
            UnScaleTool.AutoSize = true;
            UnScaleTool.ToolTipText = "Зменшити масштаб";

            //Про горячие клавиши
            HotKeysTool.AutoSize = true;
            HotKeysTool.DropDownItems.AddRange(new ToolStripItem[] {MainMenuTool});
            HotKeysTool.DropDownItems.AddRange(new ToolStripItem[] {ExitTool});
            HotKeysTool.Text = "Гарячі клавіші";

            //Про додаток
            AboutAppTool.AutoSize = true;
            AboutAppTool.Text = "Про додаток";
            AboutAppTool.Click += new EventHandler(AboutApp_Click);

            //Горячие клавиши
            MainMenuTool.Text = "Esc - Вихід у головне меню";
            ExitTool.Text = "Control + E - закрити додаток";

            MapPicBox.Click += MapPicBox_Click;
            AutoSize = true;
            MapPicBox.Size = ClientSize;
            MainPanel.AutoScroll = true;
            MapPicBox.MouseWheel += MapPicBox_MouseWheel;
            MapPicBox.MouseWheel -= MapPicBox_MouseWheel;
            MapPicBox.Paint -= MapPicBox_Paint;

            MainPanel.Width = 1920;
            MainPanel.Height = 1080;
            MapPicBox.Size = ClientSize;
            FlagIcon.SizeMode = PictureBoxSizeMode.Zoom;
            DescriptBox.Visible = false;
            TxtBRandomCountry.Visible = false;
            MapPicBox.Click += Game_MapClick;
        }


        public void MapPicBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(MapBitmap, 0, 0, MapPicBox.Width, MapPicBox.Height);
        }

        //MapPicBox - пикчербокс, в котором рисуется битмап

        private void preview_Click(object sender, EventArgs e)
        {
            MenuPanel.Visible = false;
            MapPicBox.MouseWheel += MapPicBox_MouseWheel;
            MapPicBox.Paint += MapPicBox_Paint;
            MapPicBox.Click += Game_MapClick;
            MapPicBox.Click -= Game_MapClick;
            MapBitmap = new Bitmap(@"img\BigMap.png");
            MapBitmapScale = new Bitmap(@"img\BigMap.png");
            MapPicBox.Refresh();
            Timer.Enabled = false;
            DescriptBox.Visible = true;
            TxtBRandomCountry.Visible = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AboutApp_Click(object sender, EventArgs e)
        {
            Reference newForm = new Reference();
            newForm.Show();
        }

        private void ScaleTool_Click(object sender, EventArgs e)
        {
            MapBitmap = MapBitmapScale;
            MapPicBox.Width = MapBitmap.Width * 2;
            MapPicBox.Height = MapBitmap.Height * 2;
            MainPanel.Width = 1920;
            MainPanel.Height = 1019;
            MapPicBox.Refresh();
        }

        private void UnScaleTool_Click(object sender, EventArgs e)
        {
            MapBitmap = MapBitmapScale;
            MapPicBox.Size = ClientSize;
            MapPicBox.Refresh();
        }

        //Быстрый аналог SetPixel
        private void changeColor(Bitmap bmp, Color targetColor, Color newColor)
        {
            // Блокировка битов.  
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            // Получение адресса первой строки.
            IntPtr ptr = bmpData.Scan0;

            // Объявление массива для хранения байтов битмапа.
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            // Коипрование RGB значений в массив.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int counter = 0; counter < rgbValues.Length; counter += 4)
            {
                if (rgbValues[counter + 2] == targetColor.R && rgbValues[counter + 1] == targetColor.G && rgbValues[counter] == targetColor.B)
                {
                    rgbValues[counter] = newColor.B;
                    rgbValues[counter + 1] = newColor.G;
                    rgbValues[counter + 2] = newColor.R;
                    rgbValues[counter + 3] = newColor.A;
                }
            }

            // Коипирование RGB значений обратно в bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Разблокировка битов.
            bmp.UnlockBits(bmpData);

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            myConnection.Close();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.Enabled = true;
            //Запрос на выборку рандомной страны из БД
            string GameQuery = "SELECT top 1 id, Color, NameUK, NameEN, Descript, flag FROM CountryData ORDER BY Rnd (-10000000 * TimeValue (Now ()) * [id])";
            OleDbCommand command = new OleDbCommand(GameQuery, myConnection);
            reader = command.ExecuteReader();

            if (reader.HasRows && reader.Read())
            {
                TxtBRandomCountry.Text = reader[2].ToString();
                CountryName.Text = reader[2].ToString() + ", " + '\r' + '\n' + reader[3].ToString();
                CountryDescript.Text = reader[4].ToString();
                FlagIcon.Image = Image.FromFile(reader[5].ToString());
            }
        }

        //Кнопка начала игры
        public void BtnGame_Click(object sender, EventArgs e)
        {
            Timer.Enabled = true;
            //Отключаю функцию, первую функцию, которая закрашивает страну
            //В не игровом режиме
            MapPicBox.Click -= MapPicBox_Click;
            this.Enabled = false;
            MapBitmap = new Bitmap(@"img\BigMap.png");
            MapBitmapScale = new Bitmap(@"img\BigMap.png");
            MapPicBox.Refresh();
            MenuPanel.Visible = false;
            TxtBRandomCountry.Visible = true;
        }

        //Закрашиваю страну
        private void Game_MapClick(object sender, EventArgs e)
        {
            MouseEventArgs rato = e as MouseEventArgs;
            x = rato.X * MapBitmap.Width / MapPicBox.ClientSize.Width;
            y = rato.Y * MapBitmap.Height / MapPicBox.ClientSize.Height;
            backColor = MapBitmap.GetPixel(x, y);
            hex = backColor.R.ToString("X2") + backColor.G.ToString("X2") + backColor.B.ToString("X2");


          
            CheckClick = !CheckClick;
            if (CheckClick)
            {
                if (backColor == ColorTranslator.FromHtml(reader[1].ToString()))
                {
                    changeColor(MapBitmap, backColor, Color.FromArgb(176, 244, 254));
                    changeColor(MapBitmapScale, backColor, Color.FromArgb(176, 244, 254));
                    MapPicBox.Refresh();
                    System.Media.SoundPlayer right = new System.Media.SoundPlayer(@"sound\true.wav");
                    right.Play();
                }
            }
            else
            {
                System.Media.SoundPlayer wrong = new System.Media.SoundPlayer(@"sound\false.wav");
                wrong.Play();
            }
        }
    }
}