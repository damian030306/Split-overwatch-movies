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




namespace Split_overwatch_movies
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string path = "";
        public MainWindow()
        {
            InitializeComponent();
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
                MoviesList.DataContext = openFileDialog.FileNames;
                foreach (string file in openFileDialog.FileNames)
                {
                    //txtPath.Text += file; 
                }
                
            }



        }
        
        private void TargetFolder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
