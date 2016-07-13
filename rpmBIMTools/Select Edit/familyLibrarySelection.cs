using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

using Autodesk.Revit;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace rpmBIMTools
{
    public partial class familyLibrarySelection : System.Windows.Forms.Form
    {
        public TreeNode cachedNode;
        public DirectoryInfo familyDirectory = new DirectoryInfo(@"C:\rpmBIM\TempFamilies\");
        private System.Timers.Timer ClickTimer;
        private int ClickCounter;

        public familyLibrarySelection()
        {
            // Set Dialog Size
            ClickTimer = new System.Timers.Timer(SystemInformation.DoubleClickTime);
            ClickTimer.Elapsed += new System.Timers.ElapsedEventHandler(EvaluateClicks);
            InitializeComponent();
            this.Size = new Size(Properties.Settings.Default.familyLibrary_width, Properties.Settings.Default.familyLibrary_height);
        }

        private void familyLibrarySelection_Load(object sender, EventArgs e)
        {
            // Add family tree service directories
            buildFamilyTree();
            // Set CachedNode and select
            cachedNode = FamilyTree.GetAllNodes().FirstOrDefault(tn => tn.FullPath == Properties.Settings.Default.familyLibrary_savedDirectory);
            if (cachedNode != null)FamilyTree.SelectedNode = cachedNode;
            FamilyTree.Select();
        }

        private void EvaluateClicks(object source, System.Timers.ElapsedEventArgs e)
        {
            ClickTimer.Stop();
            ClickCounter = 0;
        }

        private void familyLibrarySelection_SizeChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.familyLibrary_height = Size.Height;
            Properties.Settings.Default.familyLibrary_width = Size.Width;
            Properties.Settings.Default.Save();
        }

        private void familySelected(object sender, EventArgs e)
        {
            TreeNode tn = FamilyTree.SelectedNode;
            if (tn != null && tn.Level == 2)
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        public void FamilyTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode senderNode = FamilyTree.SelectedNode;
            if (senderNode == null)
            {
                familyView.Controls.Clear(); // Remove any controls on the familyView
                return;
            }
            insertButton.Enabled = senderNode.Level == 2;
            // Save FamilyLibrary Properties
            Properties.Settings.Default.familyLibrary_savedDirectory = senderNode.FullPath;
            Properties.Settings.Default.Save();

            if (senderNode.Level == 0)
            {
                // Show Service Directory View
            }
            else
            {
                // Show Family Directory View
                TreeNode dirNode = senderNode.Level == 1 ? senderNode : senderNode.Parent;
                if (cachedNode != dirNode || familyView.Controls.Count == 0) { // Updates Directory View if required
                    familyView.Controls.Clear(); // Remove any controls on the familyView
                    // Calculate image size
                    int count = dirNode.GetNodeCount(false);
                    //int size = count < 5 ? 203 :
                    //    count < 10 ? 135 :
                    //    count < 17 ? 101 :
                    //    count < 26 ? 81 :
                    //    count < 37 ? 68 : 58;
                    int size = 102;
                    int gap = 2;
                    int xPos = 0;
                    int yPos = 0;
                    foreach (TreeNode familyFile in dirNode.Nodes)
                    {
                        // Create PictureBox per Family Found
                        PictureBox pBox = new FamilyPictureBox();
                        pBox.Left = xPos;
                        pBox.Top = yPos;
                        pBox.BackColor = System.Drawing.Color.White;
                        pBox.Name = familyFile.FullPath;
                        pBox.Size = new Size(size, size + 27);
                        pBox.Image = FindLibraryIcon(familyDirectory.FullName + familyFile.FullPath);
                        pBox.SizeMode = pBox.Image.Height > size || pBox.Image.Width > size ? PictureBoxSizeMode.Zoom : PictureBoxSizeMode.CenterImage;
                        pBox.BorderStyle = BorderStyle.None;
                        pBox.Click += pBoxClick;
                        pBox.DoubleClick += familySelected;
                        familyView.Controls.Add(pBox);
                        pictureToolTip.SetToolTip(pBox, familyFile.Text.Split('_').Last().SplitCamelCase());
                        Label pLabel = new Label();
                        pLabel.Text = familyFile.Text.Split('_').Last().SplitCamelCase();
                        pLabel.Parent = pBox;
                        pLabel.BringToFront();
                        pLabel.Enabled = false;
                        pLabel.Dock = DockStyle.Bottom;
                        pLabel.MaximumSize = new Size(pLabel.Width, 27);
                        pLabel.Height = 27;
                        pLabel.MaximumSize = new Size(0, 0);
                        pLabel.AutoEllipsis = true;
                        pLabel.TextAlign = ContentAlignment.MiddleCenter;
                        pLabel.BackColor = System.Drawing.Color.LightGray;
                        // Increase to next position
                        xPos = xPos + size + gap;
                        if (xPos + size + gap > familyView.Size.Width)
                        {
                            xPos = 0;
                            yPos = yPos + size + 27 + gap;
                        }
                    }
                    cachedNode = dirNode; // Cache Node
                }
                // Selects PictureBox based on selected TreeView
                foreach (System.Windows.Forms.Control control in familyView.Controls)
                {
                    PictureBox pBox = control as PictureBox;
                    try {
                        if (senderNode.FullPath == pBox.Name)
                        {
                            pBox.BorderStyle = BorderStyle.Fixed3D;
                            familyView.ScrollControlIntoView(pBox);
                        }
                        else
                        {
                            pBox.BorderStyle = BorderStyle.None;
                        }
                    }
                    catch { return; }
                }
            }
        }

        private Image FindLibraryIcon(string path)
        {
            return File.Exists(path + ".gif") ? Image.FromFile(path + ".gif") :
            File.Exists(path + ".png") ? Image.FromFile(path + ".png") :
            File.Exists(path + ".jpg") ? Image.FromFile(path + ".jpg") : Properties.Resources.Preview;
        }

        private void pBoxClick(object sender, EventArgs e)
        {
            // Click counter
            ClickTimer.Stop();
            ClickCounter++;
            ClickTimer.Start();

            if (ClickCounter == 2) {
                familySelected(null, null);
                return;
            }

            PictureBox selectedBox = ((PictureBox)sender);
            foreach (System.Windows.Forms.Control control in familyView.Controls)
            {
                PictureBox pBox = control as PictureBox;
                try { pBox.BorderStyle = selectedBox == pBox ? BorderStyle.Fixed3D : BorderStyle.None; } catch { return; }
            }
            FamilyTree.SelectedNode = FamilyTree.Nodes.Find("familyFile", true).First(tn => tn.FullPath == selectedBox.Name);
            FamilyTree.Select();
        }

        // Add family tree service directories
        private void buildFamilyTree()
        {
            foreach (DirectoryInfo serviceDir in familyDirectory.GetDirectories())
            {
                TreeNode serviceNode = FamilyTree.Nodes.Add("serviceDir", serviceDir.Name);

                // Add family tree sub service directories
                foreach (DirectoryInfo subServiceDir in serviceDir.GetDirectories())
                {
                    TreeNode subServiceNode = serviceNode.Nodes.Add("subServiceDir", subServiceDir.Name);

                    // Add family sub service files
                    foreach (FileInfo file in subServiceDir.GetFiles())
                    {
                        if (string.IsNullOrWhiteSpace(search.Text))
                        {
                            if (file.Extension == ".rfa") subServiceNode.Nodes.Add("familyFile", Path.GetFileNameWithoutExtension(file.Name));
                        }
                        else
                        {
                            if (file.Extension == ".rfa" && Path.GetFileNameWithoutExtension(file.Name).ToLower().Contains(search.Text.ToLower()))
                            {
                                TreeNode familyNode = subServiceNode.Nodes.Add("familyFile", Path.GetFileNameWithoutExtension(file.Name));
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(search.Text))
            {
                foreach (TreeNode tn in FamilyTree.Nodes.Find("subServiceDir", true))
                {
                    if (tn.Nodes.Count == 0) tn.Remove();
                }
                FamilyTree.ExpandAll();
                TreeNode firstFound = FamilyTree.GetAllNodes().FirstOrDefault(n => n.Level == 2);
                if (firstFound != null) FamilyTree.SelectedNode = firstFound;
                insertButton.Enabled = false;
            }
            FamilyTree_AfterSelect(null, null);
        }

        public class FamilyPictureBox : PictureBox
        {
            protected override void OnPaint(PaintEventArgs pe)
            {
                if (this.Image != null)
                {
                    //calculate how much the image needs to be shifted in order to be in the center
                    int xOffset = (this.Width - this.Image.Width) / 2;
                    int yOffset = (this.Height - this.Image.Height) / 2;

                    //shift all drawings to the amount calculated
                    pe.Graphics.TranslateTransform(0, -13.5F);
                }
                base.OnPaint(pe);
            }
        }

        private void search_TextChanged(object sender, EventArgs e)
        {
            FamilyTree.Nodes.Clear();
            buildFamilyTree();
        }
    }
}