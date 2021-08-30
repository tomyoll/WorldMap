using System;
using System.Drawing;
using System.Windows.Forms;

namespace WorldMap
{
    public partial class Reference : Form
    {
        PictureBox AppIco = new PictureBox();
        PictureBox AutorIco = new PictureBox();
        Label VerLabel = new Label();
        Label HomeLabel = new Label();
        Label SignaLabel = new Label();
        LinkLabel SiteLink = new LinkLabel();
        Button CloseBtn = new Button();
        public Reference()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            AppIco.Location = new Point(12, 12);
            AppIco.Size = new Size(138, 119);
            AppIco.TabStop = false;
            AppIco.Image = Image.FromFile("img\\AppIcon.png");
            AppIco.SizeMode = PictureBoxSizeMode.Zoom;
            Controls.Add(AppIco);

            AutorIco.Location = new Point(288, 12);
            AutorIco.Size = new Size(138, 119);
            AutorIco.TabStop = false;
            AutorIco.Image = Image.FromFile("img\\AutorPicture.png");
            AutorIco.SizeMode = PictureBoxSizeMode.Zoom;
            Controls.Add(AutorIco);

            VerLabel.AutoSize = true;
            VerLabel.Location = new System.Drawing.Point(169, 36);
            VerLabel.Text = "WorldMap v.1.0";
            Controls.Add(VerLabel);

            HomeLabel.AutoSize = true;
            HomeLabel.Location = new System.Drawing.Point(12, 144);
            HomeLabel.Size = new System.Drawing.Size(38, 13);
            HomeLabel.Text = "Home:";
            Controls.Add(HomeLabel);

            SiteLink.AutoSize = true;
            SiteLink.Location = new System.Drawing.Point(47, 144);
            SiteLink.Size = new System.Drawing.Size(136, 13);
            SiteLink.Text = "www.college.uzhnu.edu.ua";
            SiteLink.VisitedLinkColor = System.Drawing.Color.Blue;
            SiteLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(SiteLink_LinkClicked);
            Controls.Add(SiteLink);

            SignaLabel.AutoSize = true;
            SignaLabel.ForeColor = System.Drawing.Color.Gray;
            SignaLabel.Location = new System.Drawing.Point(265, 144);
            SignaLabel.Size = new System.Drawing.Size(161, 13);
            SignaLabel.Text = "Made by TomYoll with great pain";
            Controls.Add(SignaLabel);

            CloseBtn.Location = new System.Drawing.Point(177, 329);
            CloseBtn.Size = new System.Drawing.Size(75, 23);
            CloseBtn.Text = "Ок";
            CloseBtn.UseVisualStyleBackColor = true;
            CloseBtn.Click += new EventHandler(CloseBtn_Click);
            CloseBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            Controls.Add(CloseBtn);


        }

        private void SiteLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Specify that the link was visited.
            this.SiteLink.LinkVisited = true;

            // Navigate to a URL.
            System.Diagnostics.Process.Start(@"www.college.uzhnu.edu.ua\");


        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
