using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Runtime.InteropServices;
using System.IO;

namespace Tubes_Stima_Maze
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Atribut matriks char
        private char[,] matriks;

        public bool IsFileValid(string filePath)
        {
            // Mengecek apakah file memiliki ekstensi txt
            if (Path.GetExtension(filePath) != ".txt")
            {
                return false;
            }

            string[] lines = File.ReadAllLines(filePath);
            int rows = lines.Length;
            int cols = lines[0].Split(' ').Length;

            // Mengecek apakah ukuran setiap baris sama
            for (int i = 1; i < rows; i++)
            {
                if (lines[i].Split(' ').Length != cols)
                {
                    return false;
                }
            }

            // Mengecek apakah ukuran setiap kolom sama
            for (int i = 0; i < cols; i++)
            {
                int colLength = lines[0].Split(' ')[i].Length;
                for (int j = 1; j < rows; j++)
                {
                    if (lines[j].Split(' ')[i].Length != colLength)
                    {
                        return false;
                    }
                }
            }

            // Mengisi matriks dengan karakter dari file txt
            char[,] matrix = new char[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                string[] tokens = lines[i].Split(' ');
                for (int j = 0; j < cols; j++)
                {
                    char c = tokens[j][0];
                    if (c == 'K' || c == 'T' || c == 'R' || c == 'X')
                    {
                        matrix[i, j] = c;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            // Menyimpan matriks pada atribut matriks
            this.matriks = matrix;

            return true;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UploadFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            bool? response = openFileDialog.ShowDialog();
            if (response == true)
            {
                string filepath = openFileDialog.FileName;

                MessageBox.Show(filepath);
                if (IsFileValid(filepath))
                {
                    MessageBox.Show("File valid");
                    // Tulis path file pada textbox pathFile
                    pathFile.Text = filepath;
                }
                else
                {
                    MessageBox.Show("File tidak valid");
                }   
            }
        }

        private void buttonBFS_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("BFS");
        }

        private void buttonDFS_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("DFS");
        }

        private void S_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            // Minimize
            this.WindowState = WindowState.Minimized;
        }

        private void Visualize_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BFS_Cheked(object sender, RoutedEventArgs e)
        {

        }
    }
}
