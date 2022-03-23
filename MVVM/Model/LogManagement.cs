﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using EasySave.MVVM.ObjectsForSerialization;

namespace EasySave.MVVM.Model
{
    class LogManagement : FileSave
    {
        
        
        private System.Diagnostics.Stopwatch Stopwatch;
        private long TimeDuration;
        private bool ProcessRunning;


        public LogManagement()
        {
            Stopwatch = new System.Diagnostics.Stopwatch();
            SetProcessRunning(false);

            if (!Directory.Exists(GetDirectoryPath()))
            {
                Directory.CreateDirectory(GetDirectoryPath());
            }
            if (!Directory.Exists(GetDirectoryPath()+ "SaveFilesLogs\\"))
            {
                Directory.CreateDirectory(GetDirectoryPath() + "SaveFilesLogs\\");
            }
            if (!Directory.Exists(GetDirectoryPath() + "SaveFilesLogs\\DailyLog\\"))
            {
                Directory.CreateDirectory(GetDirectoryPath() + "SaveFilesLogs\\DailyLog\\");
            }

            string path = GetDirectoryPath() + "SaveFilesLogs/StateLog.Json";

            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {

                    sw.WriteLine("[]");

                }
            }
        }



        public bool DailyLogGénérator(string Title , string SourceDirectory , string DestinationDirectory, string Type )
        {
            SetTitle(Title);
            SetSourceDirectory(SourceDirectory.Replace("\\","\\\\"));
            SetDestinationDirectory(DestinationDirectory.Replace("\\", "\\\\"));
            SetType(Type);




            string path = GetDirectoryPath() + "SaveFilesLogs\\DailyLog\\" + GetTitle() + ".Json";


            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    
                    sw.WriteLine("[\n { \n   \"Name\": \"" + GetTitle() + "\",\n   \"FileSource\": \"" + GetSourceDirectory() + "\",\n   \"FileTarget\": \"" + GetDestinationDirectory() + "\",\n   \"destPath\": \"\",\n   \"FileSize\": " + FileSize(SourceDirectory) + ",\n   \"FileTransferTime\": " + GetTimeDuration() + ",\n   \"time\": \"" + DateTime.Now.ToString("dd/MM/yyyy  HH:mm:ss") + "\"\n }\n]");

                }
            }
            else 
            {
                // This text is always added, making the file longer over time
                // if it is not deleted.

                StreamReader reader = new StreamReader(File.OpenRead(path));
                string fileContent = reader.ReadToEnd();
                reader.Close();

                fileContent = fileContent.Replace("}\n]", "},");
                StreamWriter writer = new StreamWriter(File.OpenWrite(path));
                writer.Write(fileContent);
                writer.Close();


                using (StreamWriter sw = File.AppendText(path))
                {
                   

                    sw.WriteLine(" { \n   \"Name\": \"" + GetTitle() + "\",\n   \"FileSource\": \"" + GetSourceDirectory() + "\",\n   \"FileTarget\": \"" + GetDestinationDirectory() + "\",\n   \"destPath\": \"\",\n   \"FileSize\": "+FileSize(SourceDirectory)+",\n   \"FileTransferTime\": " + GetTimeDuration() + ",\n   \"time\": \"" + DateTime.Now.ToString("dd/MM/yyyy  HH:mm:ss") + "\"\n }\n]");

                    sw.Close();

                }
            }
            return true;
        }



        public bool RunningLogGénérator(string Title, string SourceDirectory, string DestinationDirectory,  int TotalFilesToCopy, long TotalFilesSize, int NbFilesLeftToDo , bool state)
        
        {

            string path = GetDirectoryPath() + "SaveFilesLogs/StateLog.Json";


            string myJsonFile = File.ReadAllText(path);
            var myJsonList = JsonConvert.DeserializeObject<List<RunningLog>>(myJsonFile);
            bool trigger = false;
            foreach (RunningLog obj in myJsonList)
            {
                if (obj.Name == Title && trigger == false)
                {
                    obj.SourceFilePath = SourceDirectory;
                    obj.TargetFilePath = DestinationDirectory;
                    obj.State = (state == true) ? "ACTIVE" : "END"; 
                    obj.TotalFilesToCopy = TotalFilesToCopy;
                    obj.TotalFilesSize = TotalFilesSize;
                    obj.NbFilesLeftToDo = NbFilesLeftToDo;
                    obj.Progression = ((TotalFilesToCopy - NbFilesLeftToDo)*100 / TotalFilesToCopy);
                    trigger = true;
                }
            }
            if (trigger == false)
            {
                RunningLog Newobj = new RunningLog();
                Newobj.Name = Title;
                Newobj.SourceFilePath = SourceDirectory;
                Newobj.TargetFilePath = DestinationDirectory;
                Newobj.State = (state == true) ? "ACTIVE" : "END";
                Newobj.TotalFilesToCopy = TotalFilesToCopy;
                Newobj.TotalFilesSize = TotalFilesSize;
                Newobj.NbFilesLeftToDo = NbFilesLeftToDo;
                Newobj.Progression = ((TotalFilesToCopy - NbFilesLeftToDo) * 100 / TotalFilesToCopy);
                trigger = true;

                myJsonList.Add(Newobj);

            }


            string json = JsonConvert.SerializeObject(myJsonList, Formatting.Indented);

            using (StreamWriter sw = File.CreateText(path))
            {

                sw.WriteLine(json);
                sw.Close();

            }


            return true;
        }



        public bool RunningLogDeleted(string Title)

        {


            string path = GetDirectoryPath() + "SaveFilesLogs/StateLog.Json";
            string myJsonFile = File.ReadAllText(path);
            var myJsonList = JsonConvert.DeserializeObject<List<RunningLog>>(myJsonFile);
           
            foreach (RunningLog obj in myJsonList)
            {
                if (obj.Name == Title )
                {
                    obj.SourceFilePath = "";
                    obj.TargetFilePath = "";
                    obj.State = "DELETED";
                    obj.TotalFilesToCopy = 0;
                    obj.TotalFilesSize = 0;
                    obj.NbFilesLeftToDo = 0;
                    obj.Progression =0;
                   
                }
            }
            
            string json = JsonConvert.SerializeObject(myJsonList, Formatting.Indented);

            using (StreamWriter sw = File.CreateText(path))
            {

                sw.WriteLine(json);
                sw.Close();

            }


            return true;
        }





        public List<RunningLog> LogReader()
        {

            string path = GetDirectoryPath() + "SaveFilesLogs/StateLog.Json";
            var myJsonFile = File.ReadAllText(path);
            List<RunningLog> ListRunningLog = new List<RunningLog>();


            try
            {
                ListRunningLog = JsonConvert.DeserializeObject<List<RunningLog>>(myJsonFile);

            }
            catch (InvalidCastException e)
            {

                RunningLog RunningLog = new RunningLog();
                RunningLog.State = "ACTIVE";

                ListRunningLog.Add(RunningLog);
            }



            return ListRunningLog;

        }


        public void BeginSaveFileExecution()
        {

            Stopwatch.Restart();
            Stopwatch.Start();
        }

        public long EndSaveFileExecution()
        {
                        
            Stopwatch.Stop();
            SetTimeDuration(Stopwatch.ElapsedMilliseconds);
            return GetTimeDuration();
        }

        public void BeginEndProcess()
        {
            this.ProcessRunning = !this.ProcessRunning;
        }


        public long GetTimeDuration()
        {
            return this.TimeDuration;
        }

        public void SetTimeDuration(long TimeDuration)
        {
            this.TimeDuration = TimeDuration;
        }

        public void SetProcessRunning (bool ProcessRunning)
        {
            this.ProcessRunning = ProcessRunning;
        }

        public bool GetProcessRunning()
        {
            return this.ProcessRunning;
        }




    }
}
