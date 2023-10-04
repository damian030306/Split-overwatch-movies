using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using WMPLib;
using System.Globalization;

namespace Split_overwatch_movies
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        bool FolderWithMoviesSelected = false, TargetFolderSelected = false;
        private string destinationPath;
        private List<string> filesList;
        public MainWindow()
        {
            InitializeComponent();
            number.Text = "1";
            filesList = new List<string>();
            StartTimeStamps.Text = "00:00:00";






        }
        
        private void FolderWithMovies_Click(object sender, RoutedEventArgs e)
        {
            //System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            //var result = openFileDlg.ShowDialog();
            //if (result.ToString() != string.Empty)
            //{
            //    txtPath.Text = openFileDlg.SelectedPath;
            //}
            //path = txtPath.Text;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = " Movies(*.MP4)|*.MP4| " + 
                "All files (*.*)|*.*"; 
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string FolderWithmovies = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                if ((File.Exists(FolderWithmovies)) || (Directory.Exists(FolderWithmovies)))
                {
                    MoviesList.DataContext = openFileDialog.FileNames;
                    filesList = openFileDialog.FileNames.ToList();
                    //foreach (string file in openFileDialog.FileNames)
                    //{
                    //    //txtPath.Text += file; 
                    //    int i = openFileDialog.FileNames.Length;
                    //}
                    FolderWithMoviesSelected = true;
                    if(FolderWithMoviesSelected && TargetFolderSelected)
                    {
                        Move.IsEnabled = true;
                    }
                    FolderWithMovies.Background = new SolidColorBrush(Color.FromRgb(56, 126, 232));
                }
                else
                {
                    FolderWithMovies.Background = new SolidColorBrush(Color.FromRgb(227, 39, 83));//Red
                }





            }



        }
        
        private void TargetFolder_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog folderBrowser = new OpenFileDialog();
            // Set validate names and check file exists to false otherwise windows will
            // not let you select "Folder Selection."
            folderBrowser.ValidateNames = false;
            folderBrowser.CheckFileExists = false;
            folderBrowser.CheckPathExists = true;
            folderBrowser.Multiselect = false;
            // Always default to Folder Selection.
            folderBrowser.FileName = "Folder Selection.";
            if (folderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //txtPath.Text = folderBrowser.FileName + " ";
                //txtPath.Text += ;

                destinationPath = System.IO.Path.GetDirectoryName(folderBrowser.FileName);
                if ((File.Exists(destinationPath)) || (Directory.Exists(destinationPath)))
                {
                    if (System.IO.Directory.EnumerateDirectories(destinationPath).Count() != 0)
                    {
                        txtPath.Text = "Please empty destination directory";
                        txtPath.Background = new SolidColorBrush(Color.FromRgb(227,39,83));//Red
                        destinationPath = null;
                        TargetFolderSelected = false;
                        txtPath.Foreground = Brushes.White;

                    }
                    else
                    {
                        
                        TargetFolderSelected = true;
                        if (FolderWithMoviesSelected && TargetFolderSelected)
                        {
                            Move.IsEnabled = true;
                        }
                        txtPath.Text = destinationPath;
                        txtPath.Foreground = Brushes.Black;
                        txtPath.Background = new SolidColorBrush(Color.FromRgb(128, 171, 237));

                       
                    }
                    
                }
                else
                {
                    txtPath.Text = "Destination doesn't exist";
                    txtPath.Foreground = Brushes.White;
                    txtPath.Background = new SolidColorBrush(Color.FromRgb(227, 39, 83));//Red
                    destinationPath = null;

                }
            }
            //var dialog = (IFileOpenDialog)new FileOpenDialog();
            //if (!string.IsNullOrEmpty(InputPath))
            //{
            //    if (CheckHr(SHCreateItemFromParsingName(InputPath, null, typeof(IShellItem).GUID, out var item), throwOnError) != 0)
            //        return null;

            //    dialog.SetFolder(item);
            //}
            
        }
        private void Clear()
        {
            TargetFolderSelected = false;
            FolderWithMoviesSelected = false;
            MoviesList.DataContext = null;
            number.Text = "1";
            Move.IsEnabled = false;
            destinationPath = null;
            txtPath.Background = new SolidColorBrush(Color.FromRgb(128, 171, 237));
            FolderWithMovies.Background = new SolidColorBrush(Color.FromRgb(56, 126, 232));
            txtPath.Foreground = Brushes.Black;
            txtPath.Text = null;

        }
        private void number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void StartTimeStamps_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[0-9]{1,2}:[0-5]{1}[0-9]{1}:[0-5]{1}[0-9]{1}$");
            e.Handled = regex.IsMatch(e.Text);

        }
            

        private void Move_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var player = new WindowsMediaPlayer();
                int j = 0;
                
                int startI = Int32.Parse(number.Text);
                if (MoveToSeperateDir.IsChecked ?? true)
                {
                    for (int i = startI; i < (((filesList.Count + 14) / 15) + startI); i++)
                    {
                        j++;
                        System.IO.Directory.CreateDirectory(txtPath.Text + @"\" + i);
                    }
                }
                string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                j = 0;
                string t = StartTimeStamps.Text;
                CultureInfo provider = CultureInfo.InvariantCulture;
                DateTime TimeStamps = DateTime.ParseExact(StartTimeStamps.Text.ToString(), "HH:mm:ss", provider);
                //TimeStamps = DateTime.ParseExact("00:00:00", "HH:mm:SS", new CultureInfo("en-US", true));
                using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(destinationPath , "TimeStamps.txt")))
                {

                    


                    foreach (string file in filesList)
                {

                    
                    if(TimeToFilename.IsChecked ?? true)
                    {
                        DateTime data = File.GetLastWriteTime(file);

                        string dateTimeFromFile = data.ToString("HH.mm dd-MM-YY");
                        string dateTimeFromFileReverse = data.ToString("yy-MM-dd HH.mm");
                        string fileName = System.IO.Path.GetFileName(file).ToString();
                        
                        fileName = fileName.Remove(fileName.Length - 1 - 3, 4);
                        if(MoveToSeperateDir.IsChecked ?? true) 
                            {
                             File.Move(file, destinationPath + @"\" + $"{(j / 15) + startI}" + @"\" + fileName + " " + dateTimeFromFile + ".mp4");
                        }else
                        {
                            File.Move(file, destinationPath + @"\" + dateTimeFromFileReverse + " " + fileName  + ".mp4");
                        }
                        
                        //File.Move(file, destinationPath + @"\" + $"{(j / 15) + startI}" + @"\" + System.IO.Path.GetFileName(file));
                    }
                    if (TimeToFilename.IsChecked ?? false)
                    {
                        //File.Move(file, destinationPath + @"\" + $"{(j / 15) + startI}" + @"\" + System.IO.Path.GetFileName(file));
                    }
                    j++;
                    if(GenerateTimeStamps.IsChecked ?? true)
                    {

                      
                        var clip = player.newMedia(file);
                            DateTime data = File.GetLastWriteTime(file);
                            string fileName = System.IO.Path.GetFileName(file).ToString();
                            Match matchFileName = Regex.Match(fileName, "[0-9]{2}-[0-9]{2}-[0-9]{2}_[0-9]{2}-[0-9]{2}");
                            if(matchFileName.Success == false)  
                                {
                                outputFile.WriteLine(TimeStamps.ToString("HH:mm:ss") + $" - {fileName.Substring(0, fileName.Length-4)}"); 
                                }
                            else
                            {
                                outputFile.WriteLine(TimeStamps.ToString("HH:mm:ss") + $" - {matchFileName.Value}");
                            }
                            
                            TimeStamps += TimeSpan.FromMilliseconds(clip.);
                            string t2 = clip.durationString;

                        }

                }
                }
                
                if (GenerateTimeStamps.IsChecked ?? true)
                {
                    
                }
                
                txtPath.Text = $"Moved {j} files";
                txtPath.Foreground = Brushes.Black;
                txtPath.Background = new SolidColorBrush(Color.FromRgb(128,237,215));
                Move.IsEnabled = false;
            }
            catch (IOException excpetion)
            {

                txtPath.Text = excpetion.Message;
                txtPath.Background = new SolidColorBrush(Color.FromRgb(227, 39, 83));
                txtPath.Foreground = Brushes.White;
            }
            catch ( Exception excpetion)
            {

                txtPath.Text = excpetion.Message;
                txtPath.Background = new SolidColorBrush(Color.FromRgb(227, 39, 83));
                txtPath.Foreground = Brushes.White;
            }
            finally
            {

                
            }
            
            

            
            
        }
    }
}
