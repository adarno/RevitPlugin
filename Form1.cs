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

            List<string> wallIDs = new List<string>();
            List<string> wallData = new List<string>();

            // open file
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "CSV|*.csv", ValidateNames = true })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    String path = ofd.FileName.ToString();
                    string[] lines = System.IO.File.ReadAllLines(path);
                    
                    for (int i=1; i < lines.Length; i++) //start at 1 because first line in csv is column names
                    {
                        string[] fields = lines[i].Split(';');
                        wallIDs.Add(fields[0]);
                    }

                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] fields = lines[i].Split(';');
                        List<string> fieldsList = new List<string>(fields);

                        fieldsList.RemoveAt(0);

                        StringBuilder stringBuilder = new StringBuilder();
                        foreach (string field in fieldsList)
                        {
                            stringBuilder.Append(field);
                        }

                        wallData.Add(stringBuilder.ToString());
                    }
                }
            }

            place_gridPanels(wallIDs, wallData);
           

            // add match material button
            place_match_button();

        }

        private void place_gridPanels(List<string> wallIDs, List<string> wallData)
        {
            // create parent panel for elementPanel and dataPanel
            FlowLayoutPanel parentPanel = new FlowLayoutPanel();
            parentPanel.Size = new System.Drawing.Size(1000, 500);
            parentPanel.Location = new System.Drawing.Point(70, 150);
            parentPanel.FlowDirection = FlowDirection.LeftToRight;


            // panel for element IDs
            FlowLayoutPanel panelIDs = new FlowLayoutPanel();
            panelIDs.Name = "elementsPanel";
            panelIDs.FlowDirection = FlowDirection.TopDown;
            panelIDs.Size = new System.Drawing.Size(100, 300);
            panelIDs.Location = new System.Drawing.Point(70, 200);
            List<Label> labels = new List<Label>();

            // add name of column
            labels.Add(new Label());
            labels[0].Text = "Wall Elements";
            labels[0].Size = new System.Drawing.Size(100, 25);
            labels[0].Parent = panelIDs;

            foreach (string id in wallIDs)
            {
                labels.Add(new Label());
            }

            //MessageBox.Show(labels.Count.ToString());

            for (int i = 1; i < labels.Count; i++)
            {
                labels[i].Text = wallIDs[i - 1];
                labels[i].Size = new System.Drawing.Size(100, 25);
                labels[i].Padding = new System.Windows.Forms.Padding(5);
                labels[i].Parent = panelIDs;

                Label separatorLabel = new Label();
                separatorLabel.AutoSize = false;
                separatorLabel.Height = 2;
                separatorLabel.BorderStyle = BorderStyle.Fixed3D;

                separatorLabel.Parent = panelIDs;
            }

            //this.Controls.Add(panelIDs);

            // panel for wall data
            FlowLayoutPanel panelData = new FlowLayoutPanel();
            panelData.Name = "dataPanel";
            panelData.FlowDirection = FlowDirection.TopDown;
            panelData.Size = new System.Drawing.Size(300, 300);
            panelData.Location = new System.Drawing.Point(300, 200);
            List<Label> dataLabels = new List<Label>();

            // add name of column
            dataLabels.Add(new Label());
            dataLabels[0].Text = "Material";
            dataLabels[0].Size = new System.Drawing.Size(100, 25);
            dataLabels[0].Parent = panelData;

            foreach (string id in wallIDs)
            {
                dataLabels.Add(new Label());
            }

            //MessageBox.Show(labels.Count.ToString());

            for (int i = 1; i < labels.Count; i++)
            {
                dataLabels[i].Text = wallData[i - 1];
                dataLabels[i].Size = new System.Drawing.Size(300, 50);
                dataLabels[i].Padding = new System.Windows.Forms.Padding(5);
                dataLabels[i].Parent = panelData;

                Label separatorLabel = new Label();
                separatorLabel.AutoSize = false;
                separatorLabel.Height = 2;
                separatorLabel.BorderStyle = BorderStyle.Fixed3D;

                separatorLabel.Parent = panelData;
            }

            //this.Controls.Add(panelIDs);
            //this.Controls.Add(panelData);
            
            panelIDs.Parent = parentPanel;
            panelData.Parent = parentPanel;

            this.Controls.Add(parentPanel);

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
            // get instance of panel
            FlowLayoutPanel panel = this.Controls.Find("elementPanel", true).FirstOrDefault() as FlowLayoutPanel;

            Button button = sender as Button;
            if (this.matchButtonActive)
            {
                // hide sub button
                this.assignButton.Hide();
                this.splitWallButton.Hide();
                this.createMaterialButton.Hide();

                // change location of grid panel
                //panel.Location = new System.Drawing.Point(70, 150);

                this.matchButtonActive = false;
            }
            else
            {
                // change location of grid panel
                //panel.Location = new System.Drawing.Point(70, 300);

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

            // assign manual button
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
