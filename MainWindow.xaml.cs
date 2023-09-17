using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DancingProgressBars
{
    public partial class MainWindow : Window
    {
        private List<ProgressBarThread> progressBarThreads = new List<ProgressBarThread>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartDancing_Click(object sender, RoutedEventArgs e)
        {
            ProgressBarContainer.Children.Clear();
            progressBarThreads.Clear();

            int numberOfProgressBars = 200;

            for (int i = 0; i < numberOfProgressBars; i++)
            {
                ProgressBar progressBar = new ProgressBar();
                progressBar.Width = 200;
                progressBar.Height = 20;
                progressBar.Margin = new Thickness(0, 10, 0, 10);

                ProgressBarContainer.Children.Add(progressBar);

                ProgressBarThread thread = new ProgressBarThread(progressBar);
                progressBarThreads.Add(thread);
                thread.Start();
            }
        }


        private class ProgressBarThread
        {
            private ProgressBar progressBar;
            private Thread thread;
            private bool running = true;

            public ProgressBarThread(ProgressBar progressBar)
            {
                this.progressBar = progressBar;
                this.thread = new Thread(ProgressBarDance);
            }

            public void Start()
            {
                thread.Start();
            }

            public void ProgressBarDance()
            {
                Random random = new Random();

                while (running)
                {
                    Thread.Sleep(random.Next(100, 500));
                    progressBar.Dispatcher.Invoke(() =>
                    {
                        double progress = progressBar.Value + random.NextDouble() * 10;
                        if (progress > 100)
                        {
                            progress = 0;
                            progressBar.Foreground = new SolidColorBrush(Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)));
                        }
                        progressBar.Value = progress;
                    });
                }
            }
        }
    }
}
