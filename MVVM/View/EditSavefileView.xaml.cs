﻿using EasySave.MVVM.ObjectsForSerialization;
using EasySave.MVVM.Model;
using Newtonsoft.Json;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasySave.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour EditSavefileView.xaml
    /// </summary>
    public partial class EditSavefileView : UserControl

    {


        FileSaveManagement FileSaveManagement = new FileSaveManagement();
        public EditSavefileView()
        {
            InitializeComponent();


        }

        private void ButtonnoClick(object sender, RoutedEventArgs e)
        {

            FullFillForm(TitleSelected.SelectedItem);
            Modify1.Visibility = Visibility.Visible;
            Buttonno.Visibility = Visibility.Hidden;
            Buttonyes.Visibility = Visibility.Hidden;
            Areyousure.Visibility = Visibility.Hidden;
        }


        private void ButtonEditClick1(object sender, RoutedEventArgs e)
        {


            Modify1.Visibility = Visibility.Hidden;
            Buttonno.Visibility = Visibility.Visible;
            Buttonyes.Visibility = Visibility.Visible;
            Areyousure.Visibility = Visibility.Visible;


        }

        private void ButtonyesClick(object sender, RoutedEventArgs e)
        {


            Modify1.Visibility = Visibility.Visible;
            Buttonno.Visibility = Visibility.Hidden;
            Buttonyes.Visibility = Visibility.Hidden;
            Areyousure.Visibility = Visibility.Hidden;
        }

        private void SaveFileName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void WriteText1(object sender, RoutedEventArgs e)
        {

        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {

            if (TitleSelected.Tag != null)
            {
                
                FullFillForm(TitleSelected.Tag);
            }


        }

        private void WriteText2(object sender, RoutedEventArgs e)
        {

        }

  

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                       
            FullFillForm(TitleSelected.SelectedItem);

        }



        private void SearchClick(object sender, RoutedEventArgs e)
        {

        }

        private void BrowseButton1Click(object sender, RoutedEventArgs e)
        {

            VistaFolderBrowserDialog dlg = new VistaFolderBrowserDialog();
            dlg.ShowNewFolderButton = true;
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                SourcePath.Text = dlg.SelectedPath;
                SourcePath.Focus();

            }



        }

        private void BrowseButton2Click(object sender, RoutedEventArgs e)
        {

            VistaFolderBrowserDialog dlg = new VistaFolderBrowserDialog();
            dlg.ShowNewFolderButton = true;
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                DestPath.Text = dlg.SelectedPath;
                DestPath.Focus();

            }

        }


        public void FullFillForm(object name)
        {


            
            string myJsonFile = File.ReadAllText(FileSaveManagement.GetSaveFileDirectory() + name.ToString() + ".json");

            SaveFileJson FileDetail = new SaveFileJson { };

            FileDetail = JsonConvert.DeserializeObject<SaveFileJson>(myJsonFile);

            SourcePath.Text = FileDetail.SourcePath;
            SourcePath.Focus();
            DestPath.Text = FileDetail.DestPath;
            DestPath.Focus();

            if (FileDetail.Type == "COMPLETE")
            {
                Diferential.IsChecked = false;
                Complete.IsChecked = true;

            }

            if (FileDetail.Type == "PARTIAL")
            {
                Complete.IsChecked = false;
                Diferential.IsChecked = true;

            }


            TitleSelected.Text = name.ToString();



        }



    }
}
