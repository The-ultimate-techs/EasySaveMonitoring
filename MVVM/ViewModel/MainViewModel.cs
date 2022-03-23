

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Windows;

namespace EasySave
{
    class MainViewModel : ObservableObject
    {


        public object Language { get; set; }




        //Relay Command for the different views

        public RelayCommand ChangeLanguage { get; set; }
        public RelayCommand CloseCommand { get; set; }








        //Constructor
        public MainViewModel()
        {

            load();




            ChangeLanguage = new RelayCommand(o =>
            {



                switch (Language.ToString())
                {

                    case "en-US":
                        Language = "fr-FR";
                        reload();
                        break;

                    case "fr-FR":
                        Language = "en-US";
                        reload();
                        break;
                }
                reload();
            });


            CloseCommand = new RelayCommand(o =>
            {

                 Environment.Exit(0);
             
            });
        }
        public void reload()
        {

          LoadStringResource(Language.ToString());
        }


        public void load()
        {
            Language = "fr-FR";
            LoadStringResource(Language.ToString());
        }

        private void LoadStringResource(string locale)
        {
            var resources = new ResourceDictionary();

            resources.Source = new Uri("pack://application:,,,/Languages/Language_" + locale + ".xaml", UriKind.Absolute);

            var current = Application.Current.Resources.MergedDictionaries.FirstOrDefault(
                             m => m.Source.OriginalString.EndsWith("Strings.xaml"));


            if (current != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(current);
            }

            Application.Current.Resources.MergedDictionaries.Add(resources);
        }


    }
}
