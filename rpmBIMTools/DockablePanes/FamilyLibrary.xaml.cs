using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

using Autodesk.Revit;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Events;

namespace rpmBIMTools.DockablePanes
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class FamilyLibrary : Page, IDockablePaneProvider
    {
        private DirectoryInfo familyDirectory = new DirectoryInfo(@"C:\rpmBIM\TempFamilies\");
        private DirectoryInfo customDirectory = new DirectoryInfo(@"C:\rpmBIM\TempFamilies\Custom\");
        private DirectoryInfo serviceDir = null;
        private DirectoryInfo groupDir = null;
        private BitmapSource defaultImage = null;
        private BitmapSource addCustomImage = null;
        private BitmapSource errorCustomImage = null;
        private BitmapSource upgradeCustomImage = null;
        public FileInfo familyFile;
        public List<FileInfo> familyIcon = new List<FileInfo>();
        public Family family;
        public FamilySymbol familySymbol;

        public FamilyLibrary()
        {
            InitializeComponent();
        }

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            data.FrameworkElement = this as FrameworkElement;
            data.InitialState = new DockablePaneState();
            data.InitialState.DockPosition = DockPosition.Left;
        }

        private void familyLibrary_Initialized(object sender, EventArgs e)
        {
            defaultImage = EmbededBitmap(Properties.Resources.Preview);
            addCustomImage = EmbededBitmap(Properties.Resources.FamilyAdd32);
            errorCustomImage = EmbededBitmap(Properties.Resources.Warning16);
            upgradeCustomImage = EmbededBitmap(Properties.Resources.Lightning16);
            foreach (DirectoryInfo serviceDir in familyDirectory.GetDirectories())
            {
                if (serviceDir.Name == "Custom" || serviceDir.GetDirectories().Count() != 0) {
                    int index = serviceBox.Items.Add(serviceDir.Name);
                    if (serviceDir.Name == Properties.Settings.Default.familyLibrary_serviceDir) serviceBox.SelectedIndex = index;
                }
            }
            this.serviceBox.IsDropDownOpen = true; // UI Fix to correct revit freeze issue
            this.serviceBox.IsDropDownOpen = false; // UI Fix to correct revit freeze issue
        }

        private void goToHelp(object sender, RequestNavigateEventArgs e)
        {
            Hyperlink source = sender as Hyperlink;
            if (source != null)
            {
                System.Diagnostics.Process.Start(source.NavigateUri.ToString());
                e.Handled = true;
            }
        }

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(search.Text))
            {
                serviceBox.IsEnabled = true;
                groupBox.IsEnabled = true;
                serviceBox_SelectionChanged(null, null);         
                if (groupBox.SelectedIndex != -1) groupBox_SelectionChanged(null, null);
            }
            else
            {
                serviceBox.IsEnabled = false;
                groupBox.IsEnabled = false;
                FamilyItems.Items.Clear();
                FileInfo[] foundFamilies = familyDirectory.GetFiles("*" + search.Text + "*.rfa", SearchOption.AllDirectories).ToArray();
                PopulateFamilyGrid(foundFamilies);
            }
        }

        private void serviceBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Properties.Settings.Default.familyLibrary_serviceDir = serviceBox.SelectedValue.ToString();
            Properties.Settings.Default.Save();
            groupBox.Items.Clear();
            serviceDir = familyDirectory.GetDirectories().FirstOrDefault(f => f.Name == serviceBox.SelectedValue.ToString());
            if (serviceDir != null)
            {
                if (serviceDir.Name == "Custom")
                {
                    groupBox.IsEnabled = false;
                    FileInfo[] foundFamilies = serviceDir.GetFiles().Where(f => f.Extension == ".rfa").ToArray();
                    PopulateFamilyGrid(foundFamilies);
                    FamilyItems.Items.Add(new FamilyItem()
                    {
                        file = "AddCustomFamily",
                        Name = "Add New\nFamily",
                        Path = addCustomImage,
                        Stretch = Stretch.None,
                        Tooltip = "Click to add new family",
                        Menu = "Hidden",
                        IconStatus = "Update Icon"
                    });
                }
                else
                {
                    groupBox.IsEnabled = true;
                }
                this.UpdateLayout();

                foreach (DirectoryInfo groupDir in serviceDir.GetDirectories())
                {
                    if (groupDir.GetFiles().Where(f => f.Extension == ".rfa").Count() != 0)
                    {
                        int index = groupBox.Items.Add(groupDir.Name);
                        if (groupDir.Name == Properties.Settings.Default.familyLibrary_groupDir) groupBox.SelectedIndex = index;
                    }
                }
                if (groupBox.SelectedIndex == -1) groupBox.SelectedIndex = 0;
            }
        }

        private void groupBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (groupBox.SelectedValue == null) return;
            Properties.Settings.Default.familyLibrary_groupDir = groupBox.SelectedValue.ToString();
            Properties.Settings.Default.Save();
            groupDir = serviceDir.GetDirectories().FirstOrDefault(f => f.Name == groupBox.SelectedValue.ToString());
            if (groupDir != null)
            {
                FileInfo[] foundFamilies = groupDir.GetFiles().Where(f => f.Extension == ".rfa").ToArray();
                PopulateFamilyGrid(foundFamilies);
            }
        }

        private void PopulateFamilyGrid(FileInfo[] families)
        {
            FamilyItems.Items.Clear();
            string MenuStatus = serviceDir.Name == "Custom" && string.IsNullOrWhiteSpace(search.Text) ? "Visable" : "Hidden";
            foreach (FileInfo file in families)
            {
                string filePath = System.IO.Path.Combine(file.DirectoryName, System.IO.Path.GetFileNameWithoutExtension(file.FullName));
                ImageSource imgSrc = FindLibraryIcon(filePath);
                BasicFileInfo bfi = BasicFileInfo.Extract(filePath + ".rfa");
                int validCheck = string.Compare(new string(bfi.SavedInVersion.Where(char.IsDigit).ToArray()), Load.revitVer);
                string validVer = validCheck != 0 ? "Visable" : "Hidden";
                FamilyItems.Items.Add(new FamilyItem()
                {
                    file = filePath + ".rfa",
                    Name = System.IO.Path.GetFileNameWithoutExtension(file.FullName).Split('_').Last().SplitCamelCase(),
                    Path = imgSrc,
                    Warning = validCheck > 0 ? errorCustomImage : upgradeCustomImage,
                    Stretch = imgSrc.Height > 98 || imgSrc.Width > 98 ? Stretch.Fill : Stretch.None,
                    Tooltip = System.IO.Path.GetFileNameWithoutExtension(file.FullName),
                    Menu = MenuStatus,
                    IconStatus = imgSrc == defaultImage ? "Generate Icon" : "Update Icon",
                    ValidVersion = validVer
                });
            }
        }

        private ImageSource FindLibraryIcon(string path)
        {
            return File.Exists(path + ".gif") ? GetImageCache(path + ".gif") :
            File.Exists(path + ".png") ? GetImageCache(path + ".png") :
            File.Exists(path + ".jpg") ? GetImageCache(path + ".jpg") : defaultImage;
        }

        private void RenameLibraryIcons(string previousPath, string newPath)
        {
            if (File.Exists(previousPath + ".gif") && !File.Exists(newPath + ".gif")) File.Move(previousPath + ".gif", newPath + ".gif");
            if (File.Exists(previousPath + ".png") && !File.Exists(newPath + ".png")) File.Move(previousPath + ".png", newPath + ".png");
            if (File.Exists(previousPath + ".jpg") && !File.Exists(newPath + ".jpg")) File.Move(previousPath + ".jpg", newPath + ".jpg");
            DeleteLibraryIcons(previousPath);
        }

        private void DeleteLibraryIcons(string path)
        {
            FileInfo gif = new FileInfo(path + ".gif");
            FileInfo png = new FileInfo(path + ".png");
            FileInfo jpg = new FileInfo(path + ".jpg");
            if (gif.Exists && !gif.IsFileLocked()) gif.Delete();
            if (png.Exists && !png.IsFileLocked()) png.Delete();
            if (jpg.Exists && !jpg.IsFileLocked()) jpg.Delete();
        }

        private static BitmapImage GetImageCache(string path)
        {
            BitmapImage bmi = new BitmapImage();
            bmi.BeginInit();
            bmi.CacheOption = BitmapCacheOption.OnLoad;
            bmi.UriSource = new Uri(path);
            bmi.EndInit();
            return bmi;
        }

        private class FamilyItem
        {
            public string file { get; set; }
            public ImageSource Path { get; set; }
            public ImageSource Warning { get; set; }
            public string Name { get; set; }
            public Stretch Stretch { get; set; }
            public string Tooltip { get; set; }
            public string Menu { get; set; }
            public string IconStatus { get; set; }
            public string ValidVersion { get; set; }
        }

        private BitmapSource EmbededBitmap(System.Drawing.Bitmap bitmap)
        {
            BitmapSource destination;
            destination = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(),
                IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            destination.Freeze();
            return destination;
        }

        private void loadAllTypes_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Main Family Insertion Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void queFamily(object sender, RoutedEventArgs e)
        {
            string fileName = ((Button)sender).Tag.ToString();

            if (fileName == "AddCustomFamily")
            {
                addCustomFamily();
                return;
            }

            FileInfo file = new FileInfo(fileName);
            if (file.Exists)
            {
                familyFile = file;
            }
            else
            {
                TaskDialog.Show("NGB Family Library", "Family Not Found");
            }
        }

        public void addCustomFamily()
        {
            try
            {
                Microsoft.Win32.OpenFileDialog newFamily = new Microsoft.Win32.OpenFileDialog();
                newFamily.Title = "Add Custom Family File";
                newFamily.DefaultExt = ".rva";
                newFamily.Filter = "Revit Family Files (*.rfa)|*.rfa";
                newFamily.Multiselect = true;
                Nullable<bool> result = newFamily.ShowDialog();
                if (result == true)
                {
                    foreach (string filePath in newFamily.FileNames)
                    {
                        FileInfo newFamilyPath = new FileInfo(filePath);
                        if (newFamilyPath.Exists)
                        {
                            FileInfo newCustomFamilyPath = new FileInfo(customDirectory.FullName + newFamilyPath.Name);
                            if (newCustomFamilyPath.Exists)
                            {
                                TaskDialog overwrite = new TaskDialog("Family Already Exists");
                                overwrite.MainInstruction = "Family already exists";
                                overwrite.MainContent = "Family: " + newCustomFamilyPath.Name + "\nDo you want to overwrite?";
                                overwrite.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No;
                                overwrite.DefaultButton = TaskDialogResult.Yes;
                                if (overwrite.Show() == TaskDialogResult.No) continue;
                            }
                            newFamilyPath.CopyTo(newCustomFamilyPath.FullName, true);
                            DeleteLibraryIcons(System.IO.Path.Combine(newCustomFamilyPath.DirectoryName, System.IO.Path.GetFileNameWithoutExtension(newCustomFamilyPath.FullName)));
                            familyIcon.Add(newCustomFamilyPath);
                            serviceBox_SelectionChanged(null, null);
                        }
                        else
                        {
                            TaskDialog.Show("NGB Family Library", "Revit Family " + System.IO.Path.GetFileName(filePath) + " Not Found");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("NGB Family Library", ex.Message);
            }
        }

        public void InsertCustomFamily(object sender, RoutedEventArgs e)
        {
            Button button = ((ContextMenu)((MenuItem)sender).Parent).PlacementTarget as Button;
            familyFile = new FileInfo(button.Tag.ToString());
        }

        public void EditCustomFamily(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = ((ContextMenu)((MenuItem)sender).Parent).PlacementTarget as Button;
                FileInfo editFamily = new FileInfo(button.Tag.ToString());
                if (editFamily.Exists) Load.uiApp.OpenAndActivateDocument(editFamily.FullName);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("NGB Family Library", ex.Message);
            }
        }

        public void RenameCustomFamily(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = ((ContextMenu)((MenuItem)sender).Parent).PlacementTarget as Button;
                FileInfo currentPath = new FileInfo(button.Tag.ToString());
                Rename rDialog = new Rename(System.IO.Path.GetFileNameWithoutExtension(currentPath.Name));
                if (rDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FileInfo newPath = new FileInfo(System.IO.Path.Combine(currentPath.DirectoryName, rDialog.newText.Text + ".rfa"));
                    if (newPath.Exists)
                    {
                        TaskDialog.Show("NGB Family Library", "Custom family with that name already exists");
                    }
                    else
                    {
                        File.Move(currentPath.FullName, newPath.FullName);
                        RenameLibraryIcons(
                            System.IO.Path.Combine(currentPath.DirectoryName, System.IO.Path.GetFileNameWithoutExtension(currentPath.Name)),
                            System.IO.Path.Combine(newPath.DirectoryName, rDialog.newText.Text)
                            );
                        serviceBox_SelectionChanged(null, null);
                    }
                }
            }
            catch (Exception ex) { TaskDialog.Show("NGB Family Library", ex.Message); }
        }

        public void DeleteCustomFamily(object sender, RoutedEventArgs e)
        {
            Button button = ((ContextMenu)((MenuItem)sender).Parent).PlacementTarget as Button;
            FileInfo deleteFamily = new FileInfo(button.Tag.ToString());
            if (!deleteFamily.IsFileLocked())
            {
                deleteFamily.Delete();
                DeleteLibraryIcons(System.IO.Path.Combine(deleteFamily.DirectoryName, System.IO.Path.GetFileNameWithoutExtension(deleteFamily.FullName)));
                serviceBox_SelectionChanged(null, null);
            }
            else
            {
                TaskDialog.Show("NGB Family Library", "Family file is being used by another application, unable to delete.");
            }
        }

        public void GenerateIcon(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = ((ContextMenu)((MenuItem)sender).Parent).PlacementTarget as Button;
                FileInfo file = new FileInfo(button.Tag.ToString());
                if (file.Exists)
                {
                    familyIcon.Add(file);
                }
                else
                {
                    TaskDialog.Show("NGB Family Library", "Family Not Found");
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("NGB Family Library", ex.Message);
            }
        }

        public void insertFamily(object sender, IdlingEventArgs e)
        {
            UIApplication uiApp = sender as UIApplication;
            Document doc = uiApp.ActiveUIDocument.Document;
            IList<string> familySymbolNames;

            if (familyFile != null)
            {
                // Load single family type Mode
                using (Transaction t = new Transaction(doc, "Temp Load Family"))
                {
                    // Load family or find in project
                    t.Start();
                    family = new FilteredElementCollector(doc).OfClass(typeof(Family)).FirstOrDefault(f => f.Name == System.IO.Path.GetFileNameWithoutExtension(familyFile.Name)) as Family;
                    if (family != null)
                    {
                        doc.Delete(family.Id); // delete family
                    }

                    bool loaded = doc.LoadFamily(familyFile.FullName, new familyLoadOptions(), out family);

                    if (!loaded)
                    {
                        family = new FilteredElementCollector(doc).OfClass(typeof(Family)).FirstOrDefault(f => f.Name == System.IO.Path.GetFileNameWithoutExtension(familyFile.Name)) as Family;
                        if (family == null)
                        {
                            TaskDialog.Show("NGB Family Library", "Unable to load family");
                            familyFile = null;
                            return;
                        }
                    }
                    familySymbolNames = family.GetFamilySymbolIds().Select(eId => (doc.GetElement(eId) as FamilySymbol).Name).ToList();
                    t.RollBack();
                }

                // Load all family type Mode
                if ((bool)loadAllTypes.IsChecked)
                {

                    using (Transaction t = new Transaction(doc, "Load Family"))
                    {
                        // Load family or find in project
                        t.Start();
                        bool loaded = doc.LoadFamily(familyFile.FullName, new familyLoadOptions(), out family);
                        if (!loaded)
                        {
                            family = new FilteredElementCollector(doc).OfClass(typeof(Family)).FirstOrDefault(f => f.Name == System.IO.Path.GetFileNameWithoutExtension(familyFile.Name)) as Family;
                            if (family == null)
                            {
                                TaskDialog.Show("NGB Family Library", "Unable to load family");
                                t.RollBack();
                                return;
                            }
                        }

                        // Add additional types if count doesn't match
                        if (familySymbolNames.Count != family.GetFamilySymbolIds().Count)
                        {
                            foreach (string symbol in familySymbolNames)
                            {
                                Load.liveDoc.LoadFamilySymbol(familyFile.FullName, symbol, new familyLoadOptions(), out familySymbol);
                            }
                        }
                        t.Commit();
                    }

                    // Add type selection here
                    IList<ElementId> familySymbolIds = family.GetFamilySymbolIds().ToList();
                    if (familySymbolIds.Count == 1)
                    {
                        familySymbol = doc.GetElement(familySymbolIds.First()) as FamilySymbol;
                    }
                    else
                    {
                        symbolLibrarySelection symbolLibrarySelection = new symbolLibrarySelection();
                        int top = 0;
                        foreach (ElementId fs in familySymbolIds)
                        {
                            FamilySymbol symbol = doc.GetElement(fs) as FamilySymbol;
                            if (symbol != null)
                            {
                                System.Windows.Forms.RadioButton radioButton = new System.Windows.Forms.RadioButton();
                                radioButton.Text = symbol.Name;
                                radioButton.Left = 5;
                                radioButton.Top = top;
                                radioButton.Width = 225;
                                radioButton.Height = 30;
                                radioButton.Click += symbolSelectionAllTypes;
                                symbolLibrarySelection.symbolPanel.Controls.Add(radioButton);
                                top = top + 30;
                            }
                        }
                        symbolLibrarySelection.ShowDialog();
                    }
                }
                else
                {
                    // Add type selection here

                    if (familySymbolNames.Count() == 1)
                    {
                        using (Transaction t = new Transaction(Load.liveDoc, "Load Family Symbol"))
                        {
                            // Load family or find in project
                            t.Start();
                            bool loaded = Load.liveDoc.LoadFamilySymbol(familyFile.FullName, familySymbolNames.First(), new familyLoadOptions(), out familySymbol);
                            t.Commit();
                            if (!loaded)
                            {
                                familySymbol = new FilteredElementCollector(Load.liveDoc)
                                    .OfClass(typeof(FamilySymbol))
                                    .Cast<FamilySymbol>()
                                    .FirstOrDefault(fs => fs.Family.Name == System.IO.Path.GetFileNameWithoutExtension(familyFile.Name) && fs.Name == familySymbolNames.First());
                            }
                        }
                    }
                    else
                    {
                        symbolLibrarySelection symbolLibrarySelection = new symbolLibrarySelection();
                        int top = 0;
                        foreach (string symbolName in familySymbolNames)
                        {
                            System.Windows.Forms.RadioButton radioButton = new System.Windows.Forms.RadioButton();
                            radioButton.Text = symbolName;
                            radioButton.Left = 5;
                            radioButton.Top = top;
                            radioButton.Width = 225;
                            radioButton.Height = 30;
                            radioButton.Click += symbolSelectionSingleType;
                            symbolLibrarySelection.symbolPanel.Controls.Add(radioButton);
                            top = top + 30;
                        }
                        symbolLibrarySelection.ShowDialog();
                    }
                }
                familyFile = null; // Finish by removing the family from the load que
                Load.familyLibraryPane.serviceBox.IsDropDownOpen = true; // UI Fix to correct revit freeze issue
                Load.familyLibraryPane.serviceBox.IsDropDownOpen = false; // UI Fix to correct revit freeze issue
            }

            // Insert Family Symbol into Revit project
            if (familySymbol != null)
            {
                ElementType familyType = doc.GetElement(familySymbol.Id) as ElementType;
                uiApp.ActiveUIDocument.PostRequestForElementTypePlacement(familyType);
                familySymbol = null;
            }

            // Generate or Update Family Icon
            if (familyIcon.Count != 0)
            {
                try
                {
                    using (Transaction t = new Transaction(doc, "Obtain Family Icon"))
                    {
                        t.Start();
                        Family tempFamily = new FilteredElementCollector(doc).OfClass(typeof(Family)).FirstOrDefault(f => f.Name == System.IO.Path.GetFileNameWithoutExtension(familyIcon.First().Name)) as Family;
                        if (family != null)
                        {
                            doc.Delete(family.Id); // delete family
                        }
                        bool loaded = doc.LoadFamily(familyIcon.First().FullName, new familyLoadOptions(), out tempFamily);
                        if (loaded)
                        {
                            ElementId symbolId = tempFamily.GetFamilySymbolIds().FirstOrDefault();
                            if (symbolId != null)
                            {
                                ElementType type = doc.GetElement(symbolId) as ElementType;
                                System.Drawing.Image image = type.GetPreviewImage(new System.Drawing.Size(100, 100));
                                if (image != null)
                                {
                                    image.Save(System.IO.Path.Combine(familyIcon.First().DirectoryName, System.IO.Path.GetFileNameWithoutExtension(familyIcon.First().FullName)) + ".gif", System.Drawing.Imaging.ImageFormat.Gif);
                                    serviceBox_SelectionChanged(null, null);
                                }
                            }
                        }
                        t.RollBack();
                    }
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", ex.Message);
                }
                familyIcon.Remove(familyIcon.First());
            }

        }

        private void symbolSelectionAllTypes(object sender, EventArgs e)
        {
            System.Windows.Forms.RadioButton rb = sender as System.Windows.Forms.RadioButton;

            rb.FindForm().DialogResult = System.Windows.Forms.DialogResult.OK;
            familySymbol = family.GetFamilySymbolIds().Select(eId => Load.liveDoc.GetElement(eId) as FamilySymbol).FirstOrDefault(fs => fs.Name == rb.Text);
        }

        private void symbolSelectionSingleType(object sender, EventArgs e)
        {
            System.Windows.Forms.RadioButton rb = sender as System.Windows.Forms.RadioButton;
            rb.FindForm().DialogResult = System.Windows.Forms.DialogResult.OK;
            using (Transaction t = new Transaction(Load.liveDoc, "Load Family Symbol"))
            {
                // Load family or find in project
                t.Start();
                bool loaded = Load.liveDoc.LoadFamilySymbol(familyFile.FullName, rb.Text, out familySymbol);
                t.Commit();
                if (!loaded)
                {
                    familySymbol = new FilteredElementCollector(Load.liveDoc)
                        .OfClass(typeof(FamilySymbol))
                        .Cast<FamilySymbol>()
                        .FirstOrDefault(fs => fs.Family.Name == System.IO.Path.GetFileNameWithoutExtension(familyFile.Name) && fs.Name == rb.Text);
                }
            }
        }
    }
}