using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace UI_Prototype
{
    public partial class Form1 : Form
    {
        bool matchButtonActive = false;
        RoundedButton splitWallButton = new RoundedButton();
        RoundedButton assignButton = new RoundedButton();
        RoundedButton createMaterialButton = new RoundedButton();

        public Form1()
        {

            InitializeComponent();

            //code from https://www.youtube.com/watch?v=wp8j02G3MyM

            Application.DoEvents();
            RoundedButton myButton = new RoundedButton();
            myButton.Text = "Import";
            EventHandler myEventHandler = new EventHandler(myButton_Click);
            myButton.Click += myEventHandler;
            myButton.Location = new System.Drawing.Point(300, 80);
            System.Drawing.Color color_import_button = System.Drawing.ColorTranslator.FromHtml("#0078D7");
            myButton.BackColor = color_import_button;
            myButton.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            myButton.Size = new System.Drawing.Size(400, 35);
            myButton.Anchor = (AnchorStyles.Right | AnchorStyles.Top);
            this.Controls.Add(myButton);
        }

        private void myButton_Click(Object sender, EventArgs e)
        {

            // change button size
            Button button = sender as Button;
            button.Size = new System.Drawing.Size(200, 35);

            //grid view
            DataGridView grid = new DataGridView();
            grid.Location = new System.Drawing.Point(300, 300);
            grid.ColumnCount = 1;
            grid.Columns[0].Name = "Wall Elements";

            List<string> wallIDs = new List<string>();

            // open file
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "CSV|*.csv", ValidateNames = true })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    String path = ofd.FileName.ToString();
                    string[] lines = System.IO.File.ReadAllLines(path);
                    
                    for (int i=1; i < lines.Length; i++)
                    {
                        string[] fields = lines[i].Split(';');
                        wallIDs.Add(fields[0]);
                    }
                }
            }

            // group labels in panel
            FlowLayoutPanel panelIDs = new FlowLayoutPanel();
            panelIDs.FlowDirection = FlowDirection.TopDown;
            panelIDs.Size = new System.Drawing.Size(300, 300);
            panelIDs.Location = new System.Drawing.Point(300, 200);
            List<Label> labels = new List<Label>();

            foreach (string id in wallIDs)
            {
                labels.Add(new Label());
            }

            MessageBox.Show(labels.Count.ToString());

            for (int i=0; i < labels.Count; i++)
            {
                labels[i].Text = wallIDs[i];
                labels[i].AutoSize = true;
                labels[i].Parent = panelIDs;

                Label seperatorLabel = new Label();
                seperatorLabel.AutoSize = false;
                seperatorLabel.Height = 2;
                seperatorLabel.BorderStyle = BorderStyle.Fixed3D;

                seperatorLabel.Parent = panelIDs;
            }

            this.Controls.Add(panelIDs);

            // add match material button
            place_match_button();

        }

        private void place_match_button()
        {
            RoundedButton matchButton = new RoundedButton();
            matchButton.Text = "Match Materials";
            EventHandler myEventHandler = new EventHandler(match_button_Click);
            matchButton.Click += myEventHandler;
            matchButton.Location = new System.Drawing.Point(550, 80);
            System.Drawing.Color color_import_button = System.Drawing.ColorTranslator.FromHtml("#D16900");
            matchButton.BackColor = color_import_button;
            matchButton.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            matchButton.Size = new System.Drawing.Size(200, 35);
            matchButton.Anchor = (AnchorStyles.Right | AnchorStyles.Top);
            this.Controls.Add(matchButton);
        }

        private void match_button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (this.matchButtonActive)
            {
                // hide sub button
                this.assignButton.Hide();
                this.splitWallButton.Hide();
                this.createMaterialButton.Hide();

                this.matchButtonActive = false;
            }
            else
            {
                place_sub_buttons();
                this.matchButtonActive = true;
            }
            
            
        }

        private void place_sub_buttons()
        {
            System.Drawing.Color color_sub_button = System.Drawing.ColorTranslator.FromHtml("#9C9E9F");
            // split wall button
            this.splitWallButton = new RoundedButton();
            splitWallButton.Text = "Split Wall";
            EventHandler splitEventHandler = new EventHandler(myButton_Click);
            splitWallButton.Click += splitEventHandler;
            splitWallButton.Location = new System.Drawing.Point(70, 140);
            splitWallButton.BackColor = color_sub_button;
            splitWallButton.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            splitWallButton.Size = new System.Drawing.Size(200, 35);
            splitWallButton.Anchor = (AnchorStyles.Right | AnchorStyles.Top);
            this.Controls.Add(splitWallButton);

            // assign manually button
            this.assignButton = new RoundedButton();
            assignButton.Text = "Assign Manually";
            EventHandler assignEventHandler = new EventHandler(myButton_Click);
            assignButton.Click += assignEventHandler;
            assignButton.Location = new System.Drawing.Point(300, 140);
            assignButton.BackColor = color_sub_button;
            assignButton.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            assignButton.Size = new System.Drawing.Size(200, 35);
            assignButton.Anchor = (AnchorStyles.Right | AnchorStyles.Top);
            this.Controls.Add(assignButton);

            // create material
            this.createMaterialButton = new RoundedButton();
            createMaterialButton.Text = "Create Material";
            EventHandler createEventHandler = new EventHandler(myButton_Click);
            createMaterialButton.Click += createEventHandler;
            createMaterialButton.Location = new System.Drawing.Point(550, 140);
            createMaterialButton.BackColor = color_sub_button;
            createMaterialButton.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            createMaterialButton.Size = new System.Drawing.Size(200, 35);
            createMaterialButton.Anchor = (AnchorStyles.Right | AnchorStyles.Top);
            this.Controls.Add(createMaterialButton);
        }

        private void Search_Enter(object sender, EventArgs e)
        {
            if (textbox_search.Text == "Search")
            {
                textbox_search.Text = "";
                textbox_search.ForeColor = Color.Black;
            }
        }

        private void Search_Leave(object sender, EventArgs e)
        {
            if (textbox_search.Text == "")
            {
                textbox_search.Text = "Search";
                textbox_search.ForeColor = Color.Silver;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }

    class RoundedButton : Button
    {
        // code from https://www.youtube.com/watch?v=wp8j02G3MyM
        GraphicsPath GetRoundPath(RectangleF rect, int radius)
        {
            float r2 = radius / 2f;
            GraphicsPath graphicsPath = new GraphicsPath();

            // round each corner
            graphicsPath.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            graphicsPath.AddLine(rect.X + r2, rect.Y, rect.Width - r2, rect.Y);

            graphicsPath.AddArc(rect.X + rect.Width - radius, rect.Y, radius, radius, 270, 90);
            graphicsPath.AddLine(rect.Width, rect.Y + r2, rect.Width, rect.Height - r2);

            graphicsPath.AddArc(rect.X + rect.Width - radius, rect.Y + rect.Height - radius,
                radius, radius, 0, 90);
            graphicsPath.AddLine(rect.Width - r2, rect.Height, rect.X + r2, rect.Height);

            graphicsPath.AddArc(rect.X, rect.Y +rect.Height - radius, radius, radius, 90, 90);
            graphicsPath.AddLine(rect.X, rect.Height - r2, rect.X, rect.Y +r2);

            graphicsPath.CloseFigure();
            return graphicsPath;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            RectangleF Rect = new RectangleF(0, 0, this.Width, this.Height);
            GraphicsPath GraphPath = GetRoundPath(Rect, 40);

            this.Region = new Region(GraphPath);
            using (Pen pen = new Pen(Color.CadetBlue, 1.75f))
            {
                pen.Alignment = PenAlignment.Inset;
                pevent.Graphics.DrawPath(pen, GraphPath);
            }
        }
    }


}
