
using Egorozh.ColorPicker.Dialog;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace RiDe
{
    // Passing Thought
    // I Got so pissed off at Windows not letting me start my sticky note app that I decided
    // that I could make my own and give it to my friends at work - Going for minimalism here.
    // Robin Universe
    // 05 . 19 . 22

    public partial class MainWindow : Window
    {
        public static string settingsFile = AppDomain.CurrentDomain.BaseDirectory + @"\settings.txt";
        public static string defaultFile = AppDomain.CurrentDomain.BaseDirectory + @"\default.note";
        string hexValue = "";
        int COpacity = 50;

        public MainWindow()
        {
            PreviewMouseWheel += Window_PreviewMouseWheel;
            string settings = defaultFile + "=+=+=.=+=+=.=+=+=.=+=+=.=+=+=.=+=+=.";

            // Find out what is the last saved note from settings
            //if (File.Exists(settingsFile)) { settings = System.IO.File.ReadAllText(settingsFile); } else { saveFile(settingsFile, settings); }
            string lastSaveFile = @"";
            string defaultSaveFile = @"C:\Users\Public\Documents\default.note";
            if (File.Exists(GetRegistry("LastFile"))) { lastSaveFile = GetRegistry("LastFile"); }
            else { saveFile(defaultSaveFile, ".=+=+=.=+=+=.=+=+=.=+=+=.=+=+=.=+=+=.=+=+=.=+=+=.=+=+=.=+=+=.=+=+="); lastSaveFile = defaultSaveFile; SaveRegistry("LastFile", lastSaveFile); };

            // Load that note as saveData
            string[] saveData = LoadSaveData(lastSaveFile);

            // Start the foreground process
            InitializeComponent();
            
            // Apply the loaded sava data
            ApplySaveData(saveData);
            
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) { this.DragMove(); }
        }

        private void Grid_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Topmost = Pin.IsChecked;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            SaveMethod(false);
            this.Close();
        }
        public void saveFile(string path, string data) // Write out data to a file
        {
            FileStream fParameter = new FileStream(path, FileMode.Create, FileAccess.Write);
            StreamWriter mWriterParameter = new StreamWriter(fParameter);
            mWriterParameter.BaseStream.Seek(0, SeekOrigin.End);
            mWriterParameter.Write(data);
            mWriterParameter.Flush();
            mWriterParameter.Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveMethod(true);
        }

        private void Executed_Open(object sender, RoutedEventArgs e)
        {

        }

        private void Open_Click(object sender, RoutedEventArgs e) // Browse to open a specific note file
        {
            OpenFileDialog OpenFileDia = new OpenFileDialog();
            OpenFileDia.Filter = "Note file|*.note";
            OpenFileDia.Title = "Open a Note";
            OpenFileDia.DefaultExt = ".note";
            OpenFileDia.ShowDialog();
            if (OpenFileDia.FileName != ""){
                ApplySaveData(LoadSaveData(OpenFileDia.FileName));
                SaveRegistry("LastFile", OpenFileDia.FileName);
            }
        }

        private void SaveLive_Click(object sender, RoutedEventArgs e)
        {

        }

        public void Executed_Exit(object sender, ExecutedRoutedEventArgs e) // Save on quit
        {
            SaveMethod(false);
            this.Close();
        }

        private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e) // Does stuff when the mouse is scrolled and a button is held
        {

            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (e.Delta > 0)
                    this.RenamePromptTextBox.FontSize = this.RenamePromptTextBox.FontSize + 1;

                else if (e.Delta < 0)
                    if (this.RenamePromptTextBox.FontSize - 1 != 0)
                    {
                        this.RenamePromptTextBox.FontSize = this.RenamePromptTextBox.FontSize - 1;
                    }
            }

            else if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                if (e.Delta > 0)
                {
                    if (COpacity != 254)
                    {
                        COpacity++;
                        hexValue = "";
                        hexValue = "#" + Convert.ToString(COpacity, 16) + this.Background.ToString().Remove(0, 3);
                        var bc = new BrushConverter();
                        this.Background = (Brush)bc.ConvertFrom(hexValue);
                    }
                } else if (e.Delta < 0)
                {
                    if (COpacity > 20)
                    {
                        COpacity--;
                        hexValue = "";
                        hexValue = "#" + Convert.ToString(COpacity, 16) + this.Background.ToString().Remove(0, 3);
                        var bc = new BrushConverter();
                        this.Background = (Brush)bc.ConvertFrom(hexValue);
                    }
                }
            }
        }

        public string[] LoadSaveData(string saveFile) // Loads save data or defaults from a specified .note file
        {
            string data = ".=+=+=.=+=+=.=+=+=.=+=+=.=+=+=.=+=+=.=+=+=.=+=+=.=+=+=.=+=+=.=+=+=";

            if (File.Exists(saveFile)) { data = System.IO.File.ReadAllText(saveFile); }
            else
            {
                data = ".=+=+=.=+=+=.=+=+=.=+=+=.=+=+=.=+=+=.=+=+=.=+=+=.=+=+=.=+=+=.=+=+=";
            }

            string[] vs = data.Split("=+=+=");
            return vs;
        }

        public void ApplySaveData(string[] SaveData) // Applies save data to the current window
        {
            string recentText = "";
            string c = "#BC343340";
            int w = 300;
            int h = 300;
            int fs = 14;
            int l = 100;
            int t = 100;

            if (SaveData[0] != ".") { 

                if (SaveData[1] != ".") { recentText = SaveData[1]; }
                if (SaveData[3] != ".") { w = int.Parse(SaveData[3]); }
                if (SaveData[4] != ".") { h = int.Parse(SaveData[4]); }
                if (SaveData[0] != ".") { c = SaveData[0]; }
                if (SaveData[5] != ".") { fs = int.Parse(SaveData[5]); }
                if (SaveData[6] != ".") { l = int.Parse(SaveData[6]); }
                if (SaveData[7] != ".") { t = int.Parse(SaveData[7]); }

            }

            var bc = new BrushConverter();
            this.Left = l;
            this.Top = t;
            this.RenamePromptTextBox.Text = recentText;
            this.RenamePromptTextBox.FontSize = fs;
            this.Height = h;
            this.Width = w;
            this.Background = (Brush)bc.ConvertFrom(c);
        }

        public void SaveRegistry(string keyName, string key)
        {
            RegistryKey regkey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PassingThought");
            regkey.SetValue(keyName, key);
            regkey.Close();

        }

        public string GetRegistry(string keyName)
        {
            RegistryKey regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PassingThought");
            string value = "NoValue";
            if (regkey != null) { if (regkey.GetValue(keyName) != null) { value = regkey.GetValue(keyName).ToString(); } else { value = "NoValue"; } } else { regkey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PassingThought"); }
            regkey.Close();
            return value;
        }
        public void SaveMethod(bool SaveAs) // Method for saving data to a note file
       {
            string data =
                this.Background.ToString() +
                "=+=+=" +
                RenamePromptTextBox.Text.ToString() +
                "=+=+=" +
                Pin.IsChecked.ToString() +
                "=+=+=" +
                this.Width.ToString() +
                "=+=+=" +
                this.Height.ToString() +
                "=+=+=" + 
                this.RenamePromptTextBox.FontSize +
                "=+=+=" +
                this.Left.ToString() +
                "=+=+=" +
                this.Top.ToString();

            string savefile = "";

            if (File.Exists(GetRegistry("LastFile")))
            {
                savefile = GetRegistry("LastFile");
                if (SaveAs == false)
                {
                    if (File.Exists(savefile))
                    {
                        saveFile(savefile, data);
                        SaveRegistry("LastFile", savefile);
                    } else SaveMethod(true);
                } else if ( SaveAs == true ) {
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.Filter = "Note File|*.note";
                    saveFileDialog1.Title = "Save this note";
                    saveFileDialog1.DefaultExt = ".note";
                    saveFileDialog1.ShowDialog();
                    if (saveFileDialog1.FileName != "")
                    {
                        saveFile(saveFileDialog1.FileName, data);
                        SaveRegistry("LastFile", saveFileDialog1.FileName);
                    } else
                    {
                        MessageBox.Show("Failed to save file!");
                    }
                }
            }
        }

        public void Executed_SaveAs(object sender, ExecutedRoutedEventArgs e) 
        {
            SaveMethod(true);
        }

        public void Executed_Save(object sender, ExecutedRoutedEventArgs e)
        {
            SaveMethod(false);
        }

        public void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveMethod(false);
        }

        private void Color_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ColorPickerDialog();

            string r = "";

            dialog.Color = (Color)System.Windows.Media.ColorConverter.ConvertFromString(this.Background.ToString());
            var res = dialog.ShowDialog();
            var col = dialog.Color;
            var bc = new BrushConverter();
            if (res.HasValue)
            {
                this.Background = (Brush)bc.ConvertFrom(col.ToString());
            }
            
        }
    }
}
