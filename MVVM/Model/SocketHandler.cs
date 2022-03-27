using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace EasySave.MVVM.Model
{
    public sealed class SocketHandler
    {

        public Socket server { get; set; }

        private static SocketHandler instance = null;
        private static readonly object padlock = new object();
        private bool close;
        

        private SocketHandler() {

            close = false;
            try
            {

                //Création du point de communication avec adresse IP locale et un numéro de port
                IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1050);

                // Création du socket pour se connecter au serveur
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                //Demande de connexion au serveur
                server.Connect(ipep);

                Thread ServerThread = new Thread(new ThreadStart(CheckServer));
                ServerThread.Start();

            }
            catch
            {

                MessageBox.Show("Cannot reach Server");
                Thread.Sleep(1500);
                Environment.Exit(0);

            }




        }


        public void Close()
        {
            close = true;

            server.Shutdown(SocketShutdown.Both);
            server.Close();

        }


        private void CheckServer()
        {
            while (true)
            {
                Thread.Sleep(50);
                    
                if (ReceiveData().Contains(@"/!\SERVEROFFLINE/!\") == true)
                {

                   
                    string msg = "Server is offline !";
                    if (Application.Current.Dispatcher.CheckAccess())
                    {
                        MessageBox.Show(Application.Current.MainWindow, msg);
                    }
                    else
                    {
                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => {
                            MessageBox.Show(Application.Current.MainWindow, msg);
                        }));
                    }


                    Environment.Exit(0);
                    break;  
                }

            
            }

        }



        public string ReceiveData()
        {

            //Déclaration d'un buffer de type byte pour enregistrer les données reçues
            byte[] data = new byte[10240];

            //appel de la méthode receive qui reçoit les données envoyées par le serveur et les stocker 
            //dans le tableau data, elle renvoie le nombre d'octet reçus

            if (close == false)
            {
                try
                {
                    int recv = server.Receive(data);
                }
                catch (SocketException exception)
                {

                }

            }


            //transcodage de data en string
            return Encoding.UTF8.GetString(data);
        }

        public static SocketHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new SocketHandler();
                        }
                    }
                }
                return instance;
            }
        }
    }

}
