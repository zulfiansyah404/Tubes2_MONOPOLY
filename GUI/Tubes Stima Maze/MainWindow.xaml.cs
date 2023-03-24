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
using System.Windows.Shapes;
using System.Diagnostics.Eventing.Reader;
using Tubes_Stima_Maze;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace Tubes_Stima_Maze
{
    public partial class MainWindow : Window
    {
        // Atribut matriks char
        private char[,] matriks;
        // private BFSnDFS
        private BFSnDFS bfsdfs;
        public bool IsFileValid(string filePath)
        {
            // Mengecek apakah file memiliki ekstensi txt dengan 3 karakter terakhir 
            // adalah .txt
            if (filePath.Length < 4 || filePath.Substring(filePath.Length - 4) != ".txt")
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

                if (IsFileValid(filepath))
                {
                    // Tulis path file pada textbox pathFile
                    Path.Text = filepath;
                    // Tampilkan matriks pada canvas
                    ShowMatrixOnCanvas(this.matriks);

                }
                else
                {
                    MessageBox.Show("File tidak valid");
                }   
            }
        }

        private void ShowMatrixOnCanvas(char[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            // cellSize = 350/banyaknya kolom
            double cellSize = 350.0 / cols;
            double cellSize2 = 350.0 / rows;

            // Membersihkan canvas
            canvas.Children.Clear();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    // Membuat rectangle
                    Rectangle rect = new Rectangle();
                    rect.Width = cellSize;
                    rect.Height = cellSize2;
                    rect.Stroke = Brushes.Black;
                    rect.Fill = GetColorForChar(matrix[i, j]);
                    

                    // Menempatkan rectangle pada canvas
                    canvas.Children.Add(rect);
                    Canvas.SetLeft(rect, j * cellSize);
                    Canvas.SetTop(rect, i * cellSize2);
                }
            }
        }


        private void SearchGraphDrawing(char[,] matrix)
        {
            int countVisited = 0;
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            // cellSize = 350/banyaknya kolom
            double cellSize = 350.0 / cols;
            double cellSize2 = 350.0 / rows;

            // Membersihkan canvas
            canvas.Children.Clear();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    // Membuat rectangle
                    Rectangle rect = new Rectangle();
                    rect.Width = cellSize;
                    rect.Height = cellSize2;
                    rect.Stroke = Brushes.Black;
                    rect.Tag = matrix[i, j];
                    rect.Fill = GetColorForChar(matrix[i, j]);
                    if (matriks[i,j] == '*')
                    {
                        countVisited++;
                    }

                    // Menempatkan rectangle pada canvas
                    canvas.Children.Add(rect);
                    Canvas.SetLeft(rect, j * cellSize);
                    Canvas.SetTop(rect, i * cellSize2);
                } 
            }
            Nodes_Output.Text = countVisited.ToString();
            canvas.InvalidateVisual();
            Application.Current.Dispatcher.Invoke(() => { });
            //Task.Delay(500).Wait();
        }
        private Brush GetColorForChar(char c)
        {
            switch (c)
            {
                case 'K':
                    return Brushes.Brown;
                case 'T':
                    return Brushes.Gold;
                case 'R':
                    return Brushes.White;
                case 'X':
                    return Brushes.Black;
                default:
                    return Brushes.Purple;
            }
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
            char[,] temp = this.matriks;
            if (Path.Text != "None")
            {
                if (Choose_Algorithm.Text == "DFS")
                {
                    DateTime start = DateTime.Now;
                    Stack<koor> dfs = BFSnDFS.DFS(this.matriks);
                    DateTime end = DateTime.Now;
                    TimeSpan duration = end - start;
                    Execution_Time_Output.Text = duration.TotalMilliseconds.ToString() + " ms";
                    Route_Output.Text = stackKoorToString(dfs);
                }
                else if (Choose_Algorithm.Text == "BFS")
                {
                    DateTime start = DateTime.Now;
                    Queue<koor> bfs = BFSnDFS.BFS(this.matriks);
                    DateTime end = DateTime.Now;
                    TimeSpan duration = end - start;
                    Execution_Time_Output.Text = duration.TotalMilliseconds.ToString() + " ms";
                    Route_Output.Text = queueKoorToString(bfs);
                }

                //SearchGraphDrawing(this.matriks);
                
            } else
            {
                MessageBox.Show("Masukkan dahulu path atau algoritma");
            }
            this.matriks = temp;
            
        }

        private void nguliAnjay()
        {
            int sum = 0;
            for (int i = 0; i < 100000000; i++)
            {
                sum++;
            }
        }

        private string stackKoorToString(Stack<koor> s)
        {
            int count = 0;
            string ans = "";
            bool first = true;
            int N = s.Count;
            int i = 0;
            foreach(var pair in s)
            {
                if (i + 1 == N)
                {
                    break;
                }
                // Ubah warna kolom menjadi ungu
                this.matriks[pair.X,pair.Y] = '*';
                SearchGraphDrawing(this.matriks);
               
                if (first)
                {
                    ans = "(" + pair.X.ToString() + "," + pair.Y.ToString() + ")" + ans;
                    first = false;
                } else
                {
                    ans = "(" + pair.X.ToString() + "," + pair.Y.ToString() + ") --> " + ans;
                    count++;
                }
                i++;
            }
            Steps_Output.Text = count.ToString();
            return ans;
        }

        private string queueKoorToString(Queue<koor> s)
        {
            int count = 0;
            string ans = "";
            bool first = true;
            
            koor node = s.Dequeue();

            foreach(var pair in s)
            {
                //if ((node.X != pair.X) && (node.Y != pair.Y))
                //{
                this.matriks[pair.X, pair.Y] = '*';
                SearchGraphDrawing(this.matriks);
                if (first)
                    {
                        ans += "(" + pair.X.ToString() + "," + pair.Y.ToString() + ")";
                        first = false;
                    }
                    else
                    {
                        ans += " --> (" + pair.X.ToString() + "," + pair.Y.ToString() + ")";
                        count++;
                    }
                    node = pair;
                //}
                
            }
            Steps_Output.Text = count.ToString();
            return ans;
        }

        private void TextPath_Input(object sender, TextCompositionEventArgs e)
        {
        }

        private void Selection_Algorithm_Change(object sender, SelectionChangedEventArgs e)
        {
            if (Algorthm_Box.Text == "BFS")
            {
                Choose_Algorithm.Text = "DFS";
            } else
            {
                Choose_Algorithm.Text = "BFS";
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Choose_Algorithm_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Steps_Output_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
