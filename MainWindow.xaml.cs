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
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Configuration;
using YoutubeDLSharp;
using System.Threading;


namespace BDownloader
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            this.WindowStyle = WindowStyle.None;
            string firstOpenValue = System.Configuration.ConfigurationManager.AppSettings["firstopen"];
            if (firstOpenValue == "1")
            {
                string workDirectory = System.IO.Directory.GetCurrentDirectory();
                string selectPicture = $"{workDirectory}\\90379898_p0.png";
                Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["files"].Value = selectPicture;
                config.AppSettings.Settings["firstopen"].Value = "2";
                config.Save(ConfigurationSaveMode.Modified);
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");
            }
            else if (firstOpenValue == "2")
            {
                string file;
                file = System.Configuration.ConfigurationManager.AppSettings["files"];
                MainGrid.Background = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri(file)),
                    Stretch = Stretch.UniformToFill,
                    Opacity = 0.5
                };
            }
            //string file;
            //file = System.Configuration.ConfigurationManager.AppSettings["files"];
            //MainGrid.Background = new ImageBrush
            //{
            //    ImageSource = new BitmapImage(new Uri(file)),
            //    Stretch = Stretch.UniformToFill,
            //    Opacity = 0.5
            //};
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }
        //Modify
        private void ImageChange_Click(object sender, RoutedEventArgs e)
        {
            string file;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Title = "请选择图片文件";
            dialog.Filter = "所有文件(*.*)|*.*";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                file = dialog.FileName;
                MainGrid.Background = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri(file)),
                    Stretch = Stretch.UniformToFill,
                    Opacity = 0.5
                };
                Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["files"].Value = file;
                config.Save(ConfigurationSaveMode.Modified);
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");

            }
        }

        private void MinSize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseMainWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ShowWindow1_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.ShowDialog();
        }

        private void UrlEntry_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UrlTip.Text = "";
        }

        private void UrlEntry_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UrlTip.Text = "";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void DownloadPath_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PathTip.Text = null;
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择下载目录";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string folderPath = dialog.SelectedPath;
                DownloadPath.Text = folderPath;
            }
        }

        private void DownloadPath_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PathTip.Text = null;
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择下载目录";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string folderPath = dialog.SelectedPath;
                DownloadPath.Text = folderPath;
            }
        }

        private void IPEntry_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ipblock.Text = null;
        }

        private void IPEntry_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ipblock.Text = null;
        }

        private void LocalProxy_Checked(object sender, RoutedEventArgs e)
        {
            ipblock.Text = null;
            IPEntry.Text = "http://127.0.0.1/1080";
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ND.IsChecked == true)
            {
                YoutubeDL youtubeDL = new YoutubeDL();
                string fileDirectory = System.IO.Directory.GetCurrentDirectory();
                string youtubedlFilePath = fileDirectory + "\\youtube-dl.exe";
                string ffmpegFilePath = fileDirectory + "\\ffmpeg.exe";
                Console.WriteLine(ffmpegFilePath);
                Console.WriteLine(youtubedlFilePath);
                youtubeDL.YoutubeDLPath = youtubedlFilePath;
                youtubeDL.FFmpegPath = ffmpegFilePath;
                youtubeDL.OutputFolder = DownloadPath.Text;
                var res = await youtubeDL.RunVideoDownload(UrlEntry.Text, "bestvideo+bestaudio/best", YoutubeDLSharp.Options.DownloadMergeFormat.Mp4);
            }
            else if (YD.IsChecked == true)
            {
                YoutubeDL youtubeDL = new YoutubeDL();
                string fileDirectory = System.IO.Directory.GetCurrentDirectory();
                string youtubedlFilePath = fileDirectory + "\\youtube-dl.exe";
                string ffmpegFilePath = fileDirectory + "\\ffmpeg.exe";
                youtubeDL.YoutubeDLPath = youtubedlFilePath;
                youtubeDL.FFmpegPath = ffmpegFilePath;
                youtubeDL.OutputFolder = DownloadPath.Text;
                //var res = await youtubeDL.RunVideoDownload(UrlEntry.Text, "bestvideo+bestaudio/best");
                var info = await youtubeDL.RunVideoDataFetch(UrlEntry.Text);
                YoutubeDLSharp.Metadata.VideoData video = info.Data;
                Type type = video.Subtitles.GetType();
                Console.WriteLine(type);
                //YoutubeDLSharp.Metadata.FormatData[] formats = video.Formats;
                Dictionary<string, YoutubeDLSharp.Metadata.SubtitleData[]> mySubs = new Dictionary<string, YoutubeDLSharp.Metadata.SubtitleData[]>();
                mySubs = video.Subtitles;
                Console.WriteLine(mySubs);
            }
            else if (BD.IsChecked == true)
            {
                //MainWindow mainWindow = new MainWindow();
                //Thread thread1 = new Thread(new ThreadStart(mainWindow.Thread1));
                //thread1.Start();
                //Task task = new Task(() =>
                //  {
                //      MainWindow mainWindow = new MainWindow();
                //      Thread thread1 = new Thread(new ThreadStart(mainWindow.Thread1));
                //      thread1.Start();
                //      string workDirectory = System.IO.Directory.GetCurrentDirectory();
                //      string cdDirectory = $"cd {workDirectory}";
                //      string downloadCommand = $"get-video {UrlEntry.Text} -o {DownloadPath.Text}";
                //      System.Diagnostics.Process process = new System.Diagnostics.Process();
                //      process.StartInfo.FileName = "cmd.exe";
                //      process.StartInfo.UseShellExecute = false;
                //      process.StartInfo.RedirectStandardInput = true;
                //      process.StartInfo.RedirectStandardOutput = true;
                //      process.StartInfo.RedirectStandardError = true;
                //      process.StartInfo.CreateNoWindow = true;
                //      process.Start();

                //      process.StandardInput.WriteLine($"{cdDirectory}&{downloadCommand}&exit");
                //      process.StandardInput.AutoFlush = true;
                //      process.WaitForExit();
                //      process.Close();
                //      Console.WriteLine(downloadCommand);
                //  });
                //task.Start();
                string workDirectory = System.IO.Directory.GetCurrentDirectory();
                string cdDirectory = $"cd {workDirectory}";
                string downloadCommand = $"get-video -o {DownloadPath.Text} {UrlEntry.Text}";
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                Console.WriteLine(downloadCommand);
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                process.StandardInput.WriteLine($"{cdDirectory}&{downloadCommand}&exit");
                process.StandardInput.AutoFlush = true;
                process.WaitForExit();
                process.Close();
                Console.WriteLine(downloadCommand);
                

            }
        }

        //public void Thread1()
        //{

        //    string workDirectory = System.IO.Directory.GetCurrentDirectory();
        //    string cdDirectory = $"cd {workDirectory}";
        //    string downloadCommand = $"get-video {UrlEntry.Text} -o {DownloadPath.Text}";
        //    System.Diagnostics.Process process = new System.Diagnostics.Process();
        //    process.StartInfo.FileName = "cmd.exe";
        //    process.StartInfo.UseShellExecute = false;
        //    process.StartInfo.RedirectStandardInput = true;
        //    process.StartInfo.RedirectStandardOutput = true;
        //    process.StartInfo.RedirectStandardError = true;
        //    process.StartInfo.CreateNoWindow = true;
        //    process.Start();

        //    process.StandardInput.WriteLine($"{cdDirectory}&{downloadCommand}&exit");
        //    process.StandardInput.AutoFlush = true;
        //    process.WaitForExit();
        //    process.Close();
        //    Console.WriteLine(downloadCommand);

        //}
    }
}
