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

        private void Move_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int j = 0;
                
                int startI = Int32.Parse(number.Text);
                for (int i = startI; i < (((filesList.Count)/15) + startI + 1); i++)
                {
                    j++;
                    System.IO.Directory.CreateDirectory(txtPath.Text + @"\" + i);
                }
                j = 0;
                foreach (string file in filesList)
                {

                    File.Move(file, destinationPath + @"\" + $"{(j/15) + startI}" + @"\" + System.IO.Path.GetFileName(file));
                    j++;
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
