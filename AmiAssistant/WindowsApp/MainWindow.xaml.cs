﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
using Microsoft.Win32;
using IWshRuntimeLibrary;
using Speaker_Engine;
using AmiFriendo.CommandHandler;

namespace WindowsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CommandStore commandStore = new CommandStore();
        System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
        public MainWindow()
        {
            InitializeComponent();
            ni.Icon = Resource1.Hidden_Icon;

            //Form move
            MainBorder.MouseLeftButtonDown += new MouseButtonEventHandler(layoutRoot_MouseLeftButtonDown);

            // autorun
            SetAutorunValue(true);
        }

        private void Hidden_BTN_Click(object sender, RoutedEventArgs e)
        {
            ni.Visible = true;
            ni.DoubleClick += (sndr, args) =>
            {
                this.Show();
                WindowState = WindowState.Normal;
            };
            this.Hide();
        }
        private void Close_BTN_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //Form move
        void layoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ContentControl User_Bubble = new ContentControl();
                User_Bubble.Content = User_Text.Text;
                User_Bubble.Style = Resources["BubbleLeftStyle"] as Style;
                Chat_Panel.Children.Add(User_Bubble);
                Ami_Respond(commandStore.Execute(User_Text.Text));
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Listen.Speaker();
            string res = Listen.respond;
            ContentControl User_Bubble = new ContentControl();
            User_Bubble.Content =res;
            User_Bubble.Style = Resources["BubbleLeftStyle"] as Style; ;
            Chat_Panel.Children.Add(User_Bubble);
            Ami_Respond(commandStore.Execute(res));
            //await Listen.Speaker_Talk("Hi");
            //string res = "Hi";
            //ContentControl Ami_bubble = new ContentControl();
            //Ami_bubble.Content = res;
            //Ami_bubble.Style = Resources["BubbleRightStyle"] as Style; ;
            //Chat_Panel.Children.Add(Ami_bubble);
        }

        private async void Ami_Respond(string res)
        {
            ContentControl Ami_bubble = new ContentControl();
            Ami_bubble.Content = res;
            Ami_bubble.Style = Resources["BubbleRightStyle"] as Style; ;
            Chat_Panel.Children.Add(Ami_bubble);
            await Listen.Speaker_Talk(res);
        }

        private static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                System.IO.File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }

        public bool SetAutorunValue(bool autorun)
        {
            const string name = "AmiAssistant";
            const string nameExe = "WindowsApp.exe";
            string ExePathSource = System.Windows.Forms.Application.StartupPath;
            var ExeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) 
                + System.IO.Path.DirectorySeparatorChar + name + System.IO.Path.DirectorySeparatorChar;
            

            if (System.Windows.Forms.Application.ExecutablePath == ExeDirectory + nameExe)
            {
                return true;
            }
                
            if (!Directory.Exists(ExeDirectory + name))
            {
                Directory.CreateDirectory(ExeDirectory);
            }

            //DirectoryCopy(ExePathSource, ExeDirectory, true);
            CopyFilesRecursively(ExePathSource, ExeDirectory);

            RegistryKey reg;
            reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
            try
            {
                if (autorun)
                {
                    reg.SetValue(name, ExeDirectory + nameExe);

                    // create shortcut
                    WshShell shell = new WshShell();
                    string shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\AmiAssistant.lnk";
                    IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                    shortcut.Description = "Ярлык для AmiAssistant";
                    shortcut.TargetPath = ExeDirectory + nameExe;
                    shortcut.IconLocation = ExeDirectory + @"Resources" + System.IO.Path.DirectorySeparatorChar + @"Hidden_Icon.ico";
                    shortcut.Save();
                }
                else
                {
                    reg.DeleteValue(name);
                }
                    
                reg.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }
        private void EN_RADIO_Checked(object sender, RoutedEventArgs e)
        {

        }
        private void EN_RADIO_Unchecked(object sender, RoutedEventArgs e)
        {

        }
        private void RU_RADIO_Checked(object sender, RoutedEventArgs e)
        {

        }
        private void RU_RADIO_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Install_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Install_Checked(object sender, RoutedEventArgs e)
        {

        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            for (int i = Chat_Panel.Children.Count; i > 0; i--)
            {
                Chat_Panel.Children.RemoveAt(i-1);
            }
        }
          private void Microphone_BTN_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Voice_BTN_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}
