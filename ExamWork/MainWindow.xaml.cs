using System;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace ExamWork
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            textBox.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
        }

        void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void StartThreads(object sender, RoutedEventArgs e)
        {
            ArrayArgs args = new ArrayArgs();
            args.Size = int.Parse(textBox.Text);
            for (int i = 0; i < args.Size; i++)
            {
                ThreadPool.QueueUserWorkItem(WriteToArray, args);
            }
        }

        private void WriteToArray(object obj)
        {
            var size = obj as ArrayArgs;
            int[] array = new int[size.Size];
            object lockObject = new object();
            lock (lockObject)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] == 0)
                    {
                        array[i] = i;
                    }
                    else
                        continue;
                }
            }
        }

        private void DownloadLink(object sender, RoutedEventArgs e)
        {
            Uri address = new Uri(link.Text);
            WebClient webClient = new WebClient();
            try
            {
                webClient.DownloadFileAsync(address, "");
            }
            catch
            {
                MessageBox.Show("Error! Bad link!");
            }

            using (var context = new ItemContext())
            {
                context.Items.Add(new Item
                {
                    Link = link.Text
                });
                try
                {
                    context.SaveChangesAsync();
                }
                catch
                {
                    MessageBox.Show("Error! Cant save to Db!");
                }
            }
        }
    }
}
