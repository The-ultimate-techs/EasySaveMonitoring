

using EasySave.MVVM.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Data;

namespace EasySave
{
    class MainViewModel : ObservableObject
    {


        public object Language { get; set; }
        SocketHandler SocketHandler = SocketHandler.Instance;


        public ObservableCollection<RunningSaveFile> TileList { get; set; }
   


        //Relay Command for the different views

        public RelayCommand ChangeLanguage { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public RelayCommand PlayCommand { get; set; }
        public RelayCommand PauseCommand { get; set; }
        public RelayCommand StopCommand { get; set; }









        //Constructor
        public MainViewModel()
        {
            TileList = new ObservableCollection<RunningSaveFile>();
            load();

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(ReceiveData);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();




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
                SocketHandler.Close();
                Environment.Exit(0);

            });


            PlayCommand = new RelayCommand(o =>

            {
                TileList.Clear();
                RunningSaveFile RunningSaveFile = o as RunningSaveFile;
                RunningSaveFile.CurrentAction = "REMOTEPLAYED";
                byte[] byData = System.Text.Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(RunningSaveFile));

                for (int i = 0; i < 50; i++)
                {
                    Thread.Sleep(10);
                    SocketHandler.server.Send(byData);
                }


            });




            PauseCommand = new RelayCommand(o =>

            {

                TileList.Clear();
                RunningSaveFile RunningSaveFile = o as RunningSaveFile;
                RunningSaveFile.CurrentAction = "REMOTEPAUSED";
                byte[] byData = System.Text.Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(RunningSaveFile));

                for (int i = 0; i < 50; i++)
                {
                    Thread.Sleep(10);
                    SocketHandler.server.Send(byData);
                }


                
            });



            StopCommand = new RelayCommand(o =>

            {

                TileList.Clear();
                RunningSaveFile RunningSaveFile = o as RunningSaveFile;
                RunningSaveFile.CurrentAction = "REMOTESTOPPED";
                byte[] byData = System.Text.Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(RunningSaveFile));

                for (int i = 0; i < 50; i++)
                {
                    Thread.Sleep(10);
                    SocketHandler.server.Send(byData);
                }
                
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







        private void ReceiveData(object sender, EventArgs e)
        {

            string Rawstring = SocketHandler.ReceiveData();
            string Flag = @"/!\STOPSENDING/!\";
            string Offline = @"/!\SERVEROFFLINE/!\";

            if (Rawstring.Contains("]") == true && Rawstring.Contains(Flag) == false && Rawstring.Contains(Offline) == false)
            {

                int index;
                string JsonString;



                if (Rawstring.Count(f => f == ']') > 1 || Rawstring.Count(f => f == '[') > 1)
                {
                    index = Rawstring.IndexOf("]");
                    JsonString = Rawstring.Substring(index + 1);
                    index = JsonString.IndexOf("]");
                    JsonString = JsonString.Substring(0, index + 1);
                }
                else
                {
                    JsonString = Rawstring;
                }


                List<RunningSaveFile> BufferList = JsonConvert.DeserializeObject<List<RunningSaveFile>>(JsonString);

                TileList.Clear();

                foreach(RunningSaveFile RunningSaveFile in BufferList)
                {
                    TileList.Add(RunningSaveFile);

                }



            }

            if (Rawstring.Contains(Flag) == true)
            {
                TileList.Clear();
            }



        }







    }





    class RunningSaveFile
    {
        public string Title { get; set; }
        public int progressionBuffer { get; set; }
        public int progression { get; set; }
        public long TotalFile { get; set; }
        public object PauseState { get; set; }
        public object PlayState { get; set; }
        public bool StopState { get; set; }// if true save is running 
        public bool StopButton { get; set; } // if false button is not clickable
        public string CurrentAction { get; set; }







        public RunningSaveFile()
        {



        }



    }
}






