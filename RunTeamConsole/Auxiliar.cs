using Newtonsoft.Json;
using RunTeamConsole.Models;
using RunTeamConsole.Models.DB2Install;
using RunTeamConsole.Models.Packages;
using RunTeamConsole.Views.Principal.Windows;
using RunTeamConsole.Views.SapInstallPostSteps;
using RunTeamConsole.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Windows;

namespace RunTeamConsole
{
    static class Auxiliar
    {
        public static string appName = Application.Current.FindResource("strApplicationName").ToString();
        public static string appTitle = Application.Current.FindResource("strTitle").ToString();

       public static string productiveRelease = "PROD";

        // Configuration valid for Cloned environment

        //----------------------------------------------------------
        // Configuration valid for Productive environment
                
        public const string rootPath = "\\\\10.130.19.40\\AIT\\";
        public const string catalogPath = rootPath + "RUNTEAMCONSOLE\\";
        public static string serverURL = "http://10.130.103.69/";
        //public static string serverBackupURL = "http://10.130.103.14/";
        public static string serverBackupURL = "http://10.130.102.31/";
        public const string catalogscriptPath = rootPath + "RUNTEAMCONSOLE";
                
        // Configuration valid for local environment
        
        //----------------------------------------------------------

        // DO NOT EDIT------------------------------------------------
        public static string monitorUrl = serverURL + "api/";
        public static string monitorBkpUrl = serverBackupURL + "api/";
        public static List<Process> ProcessInitConfig { get; set; }
        public static List<Process> ExportCatalog;
        public static List<Process> ImportCatalog;
        public static List<Certificate> Certificates;
        public static List<Models.Authorization> Authorizations;
        //------------------------------------------------------------
           
        //----------------------------------------------------------
        //  Configuration valid for Productive environment
        
        public static string vdcUser = "VDC\\automation.it";
        public static string vdcPass = "4UT0M@710N_T3#M!";

        public static string syntaxUser = "SYNTAX\\automation.it";
        public static string syntaxPass = "4UT0M@710N_T3#M!";

        public static List<ServerSystem> SalServerList { get; internal set; }
        public static List<SaltMaster> SaltMastersList { get; internal set; }

        public static readonly HttpClient Client = new HttpClient(new HttpClientHandler()
        {
            UseDefaultCredentials = true
        });

        public static string shareaccess()
        {
            string path = Directory.GetCurrentDirectory();
            string nombrearchivo;
            nombrearchivo = path + "\\temporary.vbs";
            string someText = "Set Arg=Wscript.arguments" + "\n" +
                "ServerShare =replace(Arg(0),\"+\",\"\")" + "\n" +
                "UserName =replace(Arg(1),\"+\",\"\")" + "\n" +
                "Password =replace(Arg(2),\"+\",\"\")" + "\n" +
                "'msgbox ServerShare + \" - \" + UserName + \" - \" + Password" +  "\n" +
                "on error resume Next" + "\n" +
                "Set NetworkObject = CreateObject(\"WScript.Network\")" + "\n" +
                "Set FSO = CreateObject(\"Scripting.FileSystemObject\")" + "\n" +
                "NetworkObject.MapNetworkDrive \"\", ServerShare, true, UserName, Password" + "\n" +
                "Set Directory = FSO.GetFolder(ServerShare)" + "\n" +
                "'For Each FileName In Directory.Files" + "\n" +
                "'    WScript.Echo FileName.Name" + "\n" +
                "'Next" + "\n" +
                "if Err.Number>0 then" + "\n" +
                "     msgbox \"Please try again, Connectivity issue.\"" + "\n" +
                "end if" + "\n" +
                "Set FileName = Nothing" + "\n" +
                "Set Directory = Nothing" + "\n" +
                "Set FSO = Nothing" + "\n" +
                "'NetworkObject.RemoveNetworkDrive ServerShare, True, False" + "\n" +
                "Set ShellObject = Nothing" + "\n" +
                "Set NetworkObject = Nothing";


            File.WriteAllText(nombrearchivo, someText);
            if (File.Exists(nombrearchivo))
            {
                System.Diagnostics.Process.Start("wscript.exe", nombrearchivo + " +" + catalogscriptPath + " +" + syntaxUser + " +" + syntaxPass);
                Thread.Sleep(1000);
                File.Delete(nombrearchivo);
                Thread.Sleep(1000);
            }
            return "false";
        }

        public static string VersionRequest()
        {
            MessageBoxResult result;
            string resp;
            string sharedresp;
            if (Auxiliar.productiveRelease != "TEST")
            {
                sharedresp = shareaccess();
            }

            WebRequest request = WebRequest.Create(serverURL + "Marketplace/LastAppVersion?application=" + appName.ToUpper());
                        
            // If required by the server, set the credentials.

            if (Auxiliar.productiveRelease == "TEST")   
            {
                // Configuration valid for local environment
                request.Credentials = CredentialCache.DefaultCredentials;
            }
            else
            {
                // Configuration valid for Cloned environment
                request.Credentials = new NetworkCredential(Auxiliar.vdcUser, Auxiliar.vdcPass);
            }

            try
            {
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                // The using block ensures the stream is automatically closed.
                using (Stream dataStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    string responseFromServer = reader.ReadToEnd();
                    // Display the content.
                    resp = responseFromServer;
                }

                // Close the response.
                response.Close();

                return resp;
            }
            catch
            {
                Console.WriteLine("No response from server");
                request = WebRequest.Create(serverBackupURL + "Marketplace/LastAppVersion?application=" + appName.ToUpper());
                
                try
                {
                    // Get the response.
                    WebResponse response = request.GetResponse();
                    // Display the status.
                    Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                    // Get the stream containing content returned by the server.
                    // The using block ensures the stream is automatically closed.
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.
                        resp = responseFromServer;
                    }

                    // Close the response.
                    response.Close();

                    SendLogRequest("No response from server " + serverURL);
                    return resp;
                }
                catch
                {
                    do
                    {
                        result = MessageBox.Show("Sorry for the inconvenience.\nInnovation Suite is under maintenance", appTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                    } while (result != MessageBoxResult.OK);

                    Environment.Exit(0);

                    return null;
                }
            }
        }

        public static List<Certificate> GetAllUserCertificates()
        {
            List<Certificate> output = new List<Certificate>();

            string resp;

            WebRequest request = WebRequest.Create(
              serverURL + "Certificates/GetCertificates?ituser=" + UserProfile.ItUser);

            if (Auxiliar.productiveRelease == "TEST")
            {
                // Configuration valid for local environment
                request.Credentials = CredentialCache.DefaultCredentials;
            }
            else
            {
                // Configuration valid for Cloned environment
                request.Credentials = new NetworkCredential(Auxiliar.vdcUser, Auxiliar.vdcPass);
            }

            try
            {
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                // Get the stream containing content returned by the server.
                // The using block ensures the stream is automatically closed.
                using (Stream dataStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    string responseFromServer = reader.ReadToEnd();
                    // Display the content.
                    resp = responseFromServer;
                }

                // Close the response.
                response.Close();

                //return resp;
                output = JsonConvert.DeserializeObject<List<Certificate>>(resp);
            }
            catch
            {

            }
            return output;
        }

        public static List<Models.Authorization> GetAllUserAuthorizations()
        {
            List<Models.Authorization> output = new List<Models.Authorization>();

            string resp;

            WebRequest request = WebRequest.Create(
              serverURL + "Authorizations/GetAuthorizations?ituser=" + UserProfile.ItUser);

            if (Auxiliar.productiveRelease == "TEST")
            {
                // Configuration valid for local environment
                request.Credentials = CredentialCache.DefaultCredentials;
            }
            else
            {
                // Configuration valid for Cloned environment
                request.Credentials = new NetworkCredential(Auxiliar.vdcUser, Auxiliar.vdcPass);
            }

            try
            {
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                // Get the stream containing content returned by the server.
                // The using block ensures the stream is automatically closed.
                using (Stream dataStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    string responseFromServer = reader.ReadToEnd();
                    // Display the content.
                    resp = responseFromServer;
                }

                // Close the response.
                response.Close();

                output = JsonConvert.DeserializeObject<List<Models.Authorization>>(resp);
            }
            catch
            {

            }
            return output;
        }

        public static void SendLogRequest(string systemData)
        {
            MessageBoxResult result;
            string dataToSend = "application=" + appName.ToUpper() + "&clientTime=" + GenerateTimeStamp(ConvertToEST()) + "&ituser=" + UserProfile.ItUser + "&hostname=" + 
                UserProfile.Host + "&systemData=" + systemData;
            WebRequest request = WebRequest.Create(
              serverURL + "Monitoring/WriteApplicationLog?" + dataToSend);

            request.UseDefaultCredentials = true;
            request.PreAuthenticate = true;

            if (Auxiliar.productiveRelease == "TEST")
            {
                // Configuration valid for local environment
                request.Credentials = CredentialCache.DefaultCredentials;
            }
            else
            {
                // Configuration valid for Cloned environment
                request.Credentials = new NetworkCredential(Auxiliar.vdcUser, Auxiliar.vdcPass);
            }

            try
            {
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            }
            catch
            {
                dataToSend = dataToSend + "|No response from server " + serverURL;
                Console.WriteLine("No response from server");
                request = WebRequest.Create(serverBackupURL + "Monitoring/WriteApplicationLog?" + dataToSend);

                try
                {
                    // Get the response.
                    WebResponse response = request.GetResponse();
                    // Display the status.
                    Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                }
                catch
                {
                    /*
                    do
                    {
                        result = MessageBox.Show("Sorry for the inconvenience.\nInnovation Suite is under maintenance", appTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                    } while (result != MessageBoxResult.OK);

                    Environment.Exit(0);
                    */
                }
            }
        }

        public static DateTime DateTimeFromTimeStamp(string timeStamp)
        {
            int year, month, day, hour, minute, second;
            DateTime dateTime;

            year = Int32.Parse(timeStamp.Substring(0, 4));
            month = Int32.Parse(timeStamp.Substring(4, 2));
            day = Int32.Parse(timeStamp.Substring(6, 2));
            hour = Int32.Parse(timeStamp.Substring(8, 2));
            minute = Int32.Parse(timeStamp.Substring(10, 2));
            second = Int32.Parse(timeStamp.Substring(12, 2));

            dateTime = new DateTime(year, month, day, hour, minute, second);

            return dateTime;
        }

        public static DateTime ConvertToEST()
        {
            var timeUtc = DateTime.UtcNow;
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
            return today;
        }

        public static string GenerateTimeStamp(DateTime dateTime)
        {
            string timeStamp;
            timeStamp = dateTime.ToString("yyyyMMddHHmmss");
            return timeStamp;
        }
        
        public static string GetStatusImage(string status)
        {
            string statusImage = "";
            switch (status)
            {
                case "Scheduled":
                    statusImage = "\\img\\status\\scheduledmini.png";
                    break;
                case "Processing":
                case "Finishing":
                    statusImage = "\\img\\status\\processingmini.png";
                    break;
                case "DONE":
                case "COMPLETED":
                    statusImage = "\\img\\status\\completedmini.png";
                    break;
                case "WARNING":
                    statusImage = "\\img\\status\\warning-mini.png";
                    break;
                case "empty":
                case "":
                    statusImage = "\\img\\status\\emptymini.png";
                    break;
                default:
                    statusImage = "\\img\\status\\errormini.png";
                    break;
            }
            return statusImage;
        }

        public static string GetCategoryImage(string category)
        {
            string categoryImage = "";
            if (!String.IsNullOrEmpty(category))
            {
                switch (category.ToUpper())
                {
                    case "SAP":
                        categoryImage = "\\img\\processIcons\\sap-logo.png";
                        break;
                    case "HEALTHCHECK":
                        categoryImage = "/img/processIcons/hc-ico.png";
                        break;
                    case "ORACLE":
                        categoryImage = "/img/processIcons/db-ico.png";
                        break;
                    case "OS":
                    case "OSX":
                        categoryImage = "/img/processIcons/osx-ico.png";
                        break;
                    case "OSW":
                        categoryImage = "/img/processIcons/win-logo.png";
                        break;
                    case "CLOUD":
                        categoryImage = "/img/processIcons/cloud-ico.png";
                        break;
                    default:
                        categoryImage = "\\img\\processIcons\\other-ico.png";
                        break;
                }
            }
            return categoryImage;
        }

        public static string GetApplicationPath(string processName)
        {
            string applicationPath = rootPath + processName;
            return applicationPath;
        }

        public static string GetAppCatalogPath(string processName)
        {
            string applicationPath = GetApplicationPath(processName);
            string catPath = applicationPath + "\\CATALOG\\";
            return catPath;
        }

        public static string GetPrePath(string processName)
        {
            string applicationPath = GetApplicationPath(processName);
            string prePath = applicationPath + "\\AIT_PRE\\";
            return prePath;
        }

        public static string GetAitFilesPath(string processName)
        {
            string applicationPath = GetApplicationPath(processName);
            string path = applicationPath + "\\AIT_FILES\\";
            return path;
        }

        public static string GetAitDonePath(string processName)
        {
            string aitFilesPath = GetAitFilesPath(processName);
            string path = aitFilesPath + "AIT_DONE\\";
            return path;
        }
        
        public static string GetCompletedPath(string processName)
        {
            string aitFilesPath = GetAitFilesPath(processName);
            string path = aitFilesPath + "AIT_COMPLETED\\";
            return path;
        }

        public static string GetAitInterPath(string processName)
        {
            string aitFilesPath = GetAitFilesPath(processName);
            string path = aitFilesPath + "AIT_INTER\\";
            return path;
        }

        public static string GetAitDiscardPath(string processName)
        {
            string aitFilesPath = GetAitFilesPath(processName);
            string path = aitFilesPath + "AIT_DISCARD\\";
            return path;
        }

        public static string GetProcessPath(string idx)
        {
            string applicationPath, dateString, sidCustomer, SID, customer, processPath, appName;
            string[] idxArray;
            idxArray = idx.Split("_");
            dateString = idxArray[0].Substring(0, 8);
            sidCustomer = idxArray[1];
            if (sidCustomer.Contains('@'))
            {
                var tempArray = sidCustomer.Split('@');
                SID = tempArray[0];
                customer = tempArray[1];
            }
            else
            {
                SID = sidCustomer.Substring(0, 3);
                customer = sidCustomer.Substring(3);
            }
            appName = idxArray[2];
            applicationPath = GetApplicationPath(appName);
            processPath = applicationPath + "\\AIT_" + UserProfile.ItUser.ToUpper() + "\\AIT_" + customer + "\\AIT_" + SID + "\\";

            return processPath;
        }

        public static string GetConfingPath(string idx)
        {
            return GetProcessPath(idx) + "AIT_CNF\\";
        }

        public static string GetProcessingPath(string idx)
        {
            return GetProcessPath(idx) + "AIT_REQUEST\\PROCESSING\\";
        }

        public static string GetOnModulePath(string idx)
        {
            return GetProcessingPath(idx) + "OnModule\\";
        }

        public static string GetProcessedPath(string idx)
        {
            string processedFolder = GetOnModulePath(idx) + "PROCESSED\\";

            return processedFolder;
        }

        public static string GetProcessedAbortPath(string idx)
        {
            string timestamp = GenerateTimeStamp(ConvertToEST());
            string processedFolder = GetProcessedPath(idx) + timestamp + "\\";
            return processedFolder;
        }

        public static string GetEvidencePath(string idx)
        {
            string dateString;
            string[] idxArray;
            idxArray = idx.Split("_");
            dateString = idxArray[0].Substring(0, 8);
            return GetProcessPath(idx) + "EVIDENCE\\" + dateString + "\\";
        }
        
        public static void ShowMessage(string idx, string message, string status)
        {
            string[] idxArray;
            string sid, customer, application, title;
            MessageBoxImage icon;

            idxArray = idx.Split("_");
            if (idxArray[1].Contains('@'))
            {
                var tempArray = idxArray[1].Split('@');
                sid = tempArray[0];
                customer = tempArray[1];
            }
            else
            {
                sid = idxArray[1].Substring(0, 3);
                customer = idxArray[1].Substring(3);
            }
            application = idxArray[2];

            title = MainWindow.PVMInstance.Processes.Where(x => x.ProjectName == application).FirstOrDefault().Title;

            switch (status)
            {
                case "DONE":
                case "COMPLETED":
                    icon = MessageBoxImage.Information;
                    break;
                case "WARNING":
                    icon = MessageBoxImage.Warning;
                    break;
                default:
                    icon = MessageBoxImage.Error;
                    break;
            }

            Auxiliar.SendLogRequest("Message shown|" + idx + "|status " + status + "|message " + message);

            MessageBox.Show(message.Replace("\\n", "\n"), title + " - " + sid + " " + customer, MessageBoxButton.OK, icon);
        }

        public static void ShowMessage(string message, string status)
        {
            string title;
            MessageBoxImage icon;
                   
            title = "Can't execute more processes";

            switch (status)
            {
                case "DONE":
                case "COMPLETED":
                    icon = MessageBoxImage.Information;
                    break;
                case "WARNING":
                    icon = MessageBoxImage.Warning;
                    break;
                default:
                    icon = MessageBoxImage.Error;
                    break;
            }

            Auxiliar.SendLogRequest("Message shown|" + "|status " + status + "|message " + message);

            MessageBox.Show(message.Replace("\\n", "\n"), title, MessageBoxButton.OK, icon);
        }

        public static void ShowPopupMessage(string idx, string message, string status)
        {
            string[] idxArray;
            string sid, customer, application, title;

            idxArray = idx.Split("_");
            if (idxArray[1].Contains('@'))
            {
                var tempArray = idxArray[1].Split('@');
                sid = tempArray[0];
                customer = tempArray[1];
            }
            else
            {
                sid = idxArray[1].Substring(0, 3);
                customer = idxArray[1].Substring(3);
            }
            application = idxArray[2];

            Process p = MainWindow.PVMInstance.Processes.Where(x => x.Idx == idx).FirstOrDefault();

            title = p.PAS + " - " + sid + " " + customer + " " + p.Title;

            Application.Current.Dispatcher.Invoke((Action)delegate {
                MessagePopup messagePopup = new MessagePopup(message, status, title);
                messagePopup.Owner = Application.Current.MainWindow;
                messagePopup.Topmost = true;
                messagePopup.DataContext = MainWindow.PVMInstance;
                messagePopup.Show();
            });
        }
        
        public static void ShowOptionsMessage(string idx, OptionsMessage optionsMessage)
        {
            string[] idxArray;
            string sid, customer, application, title;

            idxArray = idx.Split("_");
            if (idxArray[1].Contains('@'))
            {
                var tempArray = idxArray[1].Split('@');
                sid = tempArray[0];
                customer = tempArray[1];
            }
            else
            {
                sid = idxArray[1].Substring(0, 3);
                customer = idxArray[1].Substring(3);
            }
            application = idxArray[2];

            Process p = MainWindow.PVMInstance.Processes.Where(x => x.Idx == idx).FirstOrDefault();

            title = p.PAS + " - " + sid + " " + customer + " " + p.Title;

            Application.Current.Dispatcher.Invoke((Action)delegate {
                OptionsMessageWindow messagePopup = new OptionsMessageWindow(idx, optionsMessage.Title, optionsMessage.Body, optionsMessage.Tooltip, optionsMessage.ContinueCommandText, optionsMessage.RepeatCommandText, optionsMessage.AlternCommandText, title);
                messagePopup.Owner = Application.Current.MainWindow;
                messagePopup.Topmost = true;
                messagePopup.DataContext = MainWindow.PVMInstance;
                messagePopup.Show();
            });
        }

        public static List<string[]> GetStepConfig(string idx)
        {
            string stepsConfigFileName;
            string[] contentList;
            List<string[]> listConfig = new List<string[]>();

            stepsConfigFileName = GetConfingPath(idx) + idx + "_STEPS.CONF";
            if (File.Exists(stepsConfigFileName))
            {
                contentList = File.ReadAllLines(stepsConfigFileName);
                for (int i = 1; i < contentList.Count(); i++)
                {
                    listConfig.Add(contentList[i].Split("|"));
                }
            }
            return listConfig;
        }

        //codificar base64
        public static string Base64_Encode(string str)
        {
            byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(encbuff);
        }
        //decodificar base64
        public static string Base64_Decode(string str)
        {
            try
            {
                byte[] encbuff = System.Convert.FromBase64String(str);
                return System.Text.ASCIIEncoding.ASCII.GetString(encbuff);
            }
            catch
            {
                return "";
            }
        }

        public static void CreateNewProcessFiles(Process p, bool useSaltStack)
        {
            string prePath, processPath, cnfPath, processingPath, evidencePath, traceFolderName, preContent, trgFileName, trgHeader, trgContent, trgInstFileName, trgInstHeader,
                trgInstContent, crdtlsFileName, crdtlsHeader, crdtlsContent, sapPostCrdtlsFileName, sapPostCrdtlsHeader, sapPostCrdtlsContent, mailFileName, mailHeader, mailContent,
                stepsFileName, stepsHeader;
            string stopFileName, bundleFileName, oswFileName, oswContent, title, transactions, webFileName, webContent, saltServersListFileName, saltParamsFileName;
            string srcDBFileName, srcDBContent, trgSAPFileName, trgSAPContent, trgDBFileName, trgDBContent, trgBasisClntFileName, trgBasisClntContent, trgBasisClntContent2,
                trgCustClntFileName, trgCustClntContent, trgCustClntContent2, BDLSFileName, exportFileName, importFileName, restoreModeFileName;
            Process realated;

            List<string> stepsContent, bundleContent = new List<string>(), saltServersListContent = new List<string>(), saltParamsContent = new List<string>(),
                sourceDBParamsContent = new List<string>();

            prePath = GetPrePath(p.ProjectName);
            CreateFolder(prePath);
            processPath = GetProcessPath(p.Idx);

            char[] processSumaryPath = GetProcessSumaryPath(p.ProjectName).ToCharArray();

            string folderName = new string(processSumaryPath.Reverse().ToArray());
            folderName = folderName.Remove(0, 1);
            string interPath = new string(folderName.Reverse().ToArray());
            interPath = interPath.Replace("_READ", "");

            string interFolder = interPath + "_INTER\\";
            if (!Directory.Exists(interFolder))
                CreateFolder(interFolder);

            if (processPath.Contains("ONModule") || processPath.Contains("ONModuleOSX"))
            {
                CreateFolder(processPath);
            }

            CreateFolder(processPath);

            if (p.SID.Contains("GLB"))
            {
                realated = MainWindow.PVMInstance.Processes.Where(x => x.ProjectName == p.ProjectName && x.SID == p.SID && x.Customer == p.Customer).FirstOrDefault();
            }
            else
            {
                realated = MainWindow.PVMInstance.Processes.Where(x => x.ProjectName == p.ProjectName && x.SID == p.SID && x.Customer == p.Customer && x.PAS == p.PAS && x.DBS == p.DBS).FirstOrDefault();
            }
            
            title = p.Title;

            if (realated != null)
            {
                switch (realated.CurrentStatus)
                {
                    case "Scheduled":
                    case "COMPLETED":
                    case "ABORTED":
                        var result = MessageBox.Show("There is a process for " + p.SID + " system already\nDo you want to start over?", title + " - " + p.Customer + " " + p.DBS, MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            RenamePreviousProcess(realated);
                        }
                        else
                        {
                            return;
                        }
                        break;
                    default:
                        MessageBox.Show("There is another process running for the " + p.SID + " system.\nTo schedule a new process for that system please " + 
                            "click Abort first.", title + " - " + p.Customer + " " + p.DBS, MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                }
            }
            else if (Auxiliar.ProcessExists(p))
            {
                MessageBox.Show("There is another process scheduled for the " + p.SID + " system.\nIf you are not able to see the process on the main window yet, " +
                    "please wait a moment.\n\n If problem persist pleas contact Innovation Team", title + " - " + p.Customer + " " + p.DBS, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            cnfPath = GetConfingPath(p.Idx);
            CreateFolder(cnfPath);

            processingPath = GetProcessingPath(p.Idx);
            CreateFolder(processingPath);

            evidencePath = GetEvidencePath(p.Idx);
            CreateFolder(evidencePath);

            traceFolderName = "\\AIT_TMP\\LOG\\TRACE\\";

            trgFileName = p.Idx + "_TARGET.CNF";
            trgHeader = "idx|SID|SAP Host|SAP TYPE|SAP OS|DB  HOST|DB TYPE|DB OS|INST TYPE";
            trgContent = p.Idx + "|" + p.SID + "|" + p.PAS + "|" + p.PASType + "|" + p.PASOS + "|" + p.DBS + "|" + p.DBSType + "|" + p.PASOS + "|" + p.IntanceType;
            CreateFile(cnfPath, trgFileName, new string[] { trgHeader, trgContent });

            trgDBFileName = p.Idx + ".TARGETDB";
            trgDBContent = p.DBSOS + "|" + p.DBS + "|" + p.InstanceNum + "|000|" + p.SID + "|" + p.PASType + "|" + p.Credentials.DBUser + "|" + p.Credentials.DBPass + "|" + p.TargetSystemDB;
            CreateFile(cnfPath, trgDBFileName, trgDBContent);
            CreateFile(evidencePath, trgDBFileName, trgDBContent);

            crdtlsFileName = p.Idx + "_CREDENTIALS.CNF";
            crdtlsHeader = "idx|SAPGUIUSER|SAPGUIPASS|OSUSER|OSPASS|SIDADMUSER|SIDADMPASS|DBUSER|DBPASS|DBSCHEMAPASS";
            
            crdtlsContent = p.Idx + "|" + p.Credentials.SAPGuiUser + "|" + p.Credentials.SAPGuiPass + "|" + p.Credentials.OSUser.ToLower() + "||" + p.Credentials.SIDAdmUser + "|" + 
                p.Credentials.SIDAdmPass + "|" + p.Credentials.DBUser + "|" + p.Credentials.DBPass + "|" + p.Credentials.DBSchemaPass;
            CreateFile(cnfPath, crdtlsFileName, new string[] { crdtlsHeader, crdtlsContent });

            if (String.IsNullOrEmpty(UserProfile.CachedCredentials.SAPGuiUser) && !String.IsNullOrEmpty(p.Credentials.SAPGuiUser))
            {
                UserProfile.CachedCredentials.SAPGuiUser = p.Credentials.SAPGuiUser;
            }
            if (String.IsNullOrEmpty(UserProfile.CachedCredentials.SAPGuiPass) && !String.IsNullOrEmpty(p.Credentials.SAPGuiPass))
            {
                UserProfile.CachedCredentials.SAPGuiPass = p.Credentials.SAPGuiPass;
            }
            if (String.IsNullOrEmpty(UserProfile.CachedCredentials.SIDAdmUser) && !String.IsNullOrEmpty(p.Credentials.SIDAdmUser))
            {
                UserProfile.CachedCredentials.SIDAdmUser = p.Credentials.SIDAdmUser;
            }
            if (String.IsNullOrEmpty(UserProfile.CachedCredentials.SIDAdmPass) && !String.IsNullOrEmpty(p.Credentials.SIDAdmPass))
            {
                UserProfile.CachedCredentials.SIDAdmPass = p.Credentials.SIDAdmPass;
            }
            if (String.IsNullOrEmpty(UserProfile.CachedCredentials.DBUser) && !String.IsNullOrEmpty(p.Credentials.DBUser))
            {
                UserProfile.CachedCredentials.DBUser = p.Credentials.DBUser;
            }
            if (String.IsNullOrEmpty(UserProfile.CachedCredentials.DBPass) && !String.IsNullOrEmpty(p.Credentials.DBPass))
            {
                UserProfile.CachedCredentials.DBPass = p.Credentials.DBPass;
            }
            if (String.IsNullOrEmpty(UserProfile.CachedCredentials.DBSchemaPass) && !String.IsNullOrEmpty(p.Credentials.DBSchemaPass))
            {
                UserProfile.CachedCredentials.DBSchemaPass = p.Credentials.DBSchemaPass;
            }

            if (p.ProjectName.ToUpper().Contains("SYSTEMCOPY") || p.ProjectName.ToUpper().Contains("REFRESH"))
            {
                srcDBFileName = p.Idx + ".SOURCEDB";
                srcDBContent = p.SourceDBSOS + "|" + p.SourceDBS + "|" + p.SourceInstanceNum  + "|000|" + p.SourceSID + "|" + p.SourceType + "||";
                CreateFile(cnfPath, srcDBFileName, srcDBContent);
                CreateFile(evidencePath, srcDBFileName, srcDBContent);

                trgSAPFileName = p.Idx + ".TARGETSAP";
                trgSAPContent = p.PASOS + "|" + p.PAS + "|" + p.InstanceNum  + "|000|" + p.SID + "|" + p.PASType + "|" + p.Credentials.SAPGuiUser + "|" + p.Credentials.SAPGuiPass;
                CreateFile(cnfPath, trgSAPFileName, trgSAPContent);

                trgBasisClntFileName = p.Idx + ".TARGETBASISCLIENT";
                trgBasisClntContent = p.PAS + "|" + p.InstanceNum + "|000|" + p.SID + "|" + p.PASType + "|DDIC|" + p.Credentials.TargetDDICPass;
                trgBasisClntContent2 = p.PAS + "|" + p.InstanceNum + "|000|" + p.SID + "|" + p.PASType + "|ITOPER|" + p.Credentials.TargetITOPERPass;
                CreateFile(cnfPath, trgBasisClntFileName, new string[] { trgBasisClntContent, trgBasisClntContent2 });
                
                trgCustClntFileName = p.Idx + ".TARGETCUSTCLIENT";
                trgCustClntContent = p.PAS + "|" + p.InstanceNum + "|" + p.CustomerSAPClient + "|" + p.SID + "|" + p.PASType + "|DDIC|" + p.Credentials.TargetDDICCustomerPass;
                trgCustClntContent2 = p.PAS + "|" + p.InstanceNum + "|" + p.CustomerSAPClient + "|" + p.SID + "|" + p.PASType + "|ITOPER|" + p.Credentials.TargetITOPERPass;
                CreateFile(cnfPath, trgCustClntFileName, new string[] { trgCustClntContent, trgCustClntContent2 });
                
                exportFileName = "COMPONENTS_LIST.EXPORT";
                List<string> exportListFileContent = new List<string>() { };
                foreach(Component component in p.ExportTablesComponents)
                {
                    exportListFileContent.Add(component.Name);
                }
                CreateFile(evidencePath, exportFileName, exportListFileContent.ToArray());
                
                importFileName = "COMPONENTS_LIST.IMPORT";
                List<string> importListFileContent = new List<string>() { };
                foreach(ImportComponent component in p.ImportTablesComponents)
                {
                    importListFileContent.Add(component.Name);
                }
                CreateFile(evidencePath, importFileName, importListFileContent.ToArray());

                if (File.Exists(GetAppCatalogPath(p.ProjectName) + "COMPONENTTABLES.CATALOG"))
                {
                    File.Copy(GetAppCatalogPath(p.ProjectName) + "COMPONENTTABLES.CATALOG", evidencePath + "COMPONENTTABLES.CATALOG");
                }
                
                BDLSFileName = p.Idx + "_LOGICALSYSTEM.CNF";
                List<string> BDLSListFileContent = new List<string>() { };
                for (int i = 0; i < p.BDLSList.Count; i++)
                {
                    BDLSListFileContent.Add(p.BDLSList[i].SourceSID + "CLNT" + p.BDLSList[i].SourceClient + ":" + p.BDLSList[i].TargetSID + "CLNT" + p.BDLSList[i].TargetClient);
                }
                CreateFile(cnfPath, BDLSFileName, String.Join(",",BDLSListFileContent.ToArray()) + "@A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z@A*");
                
                restoreModeFileName = p.Idx + ".RESTOREMODE";
                List<string> restoreModeFileContent = new List<string>() { };
                restoreModeFileContent.Add("MANUAL/AUTOM|RESTORETIME (YYYY-MM-DD_HH24:MI:SS)");

                if ((p.SelectedFlowMode.Contains("AUTO") || p.SystemCopyModules.Contains("DBRESTORE")) && p.RestoreDateTime.HasValue)
                {
                    restoreModeFileContent.Add("AUTO|" + p.RestoreDateTime.Value.ToString("yyyy-MM-dd_HH:mm:ss"));
                }
                else
                {
                    restoreModeFileContent.Add(p.SelectedFlowMode + "|");
                }

                restoreModeFileContent.Add("CV-Streams|" + p.CVStreams);
                CreateFile(cnfPath, restoreModeFileName, restoreModeFileContent.ToArray());
                CreateFile(evidencePath, restoreModeFileName, restoreModeFileContent.ToArray());
                if (p.ProjectName.ToUpper().Contains("SYSTEMCOPY") && p.ProjectName.ToUpper().Contains("ORACLE"))
                    CreateSaltParamsFile(p);
            }
            else
            {
                if (p.Credentials.ShowWebCredentials)
                {
                    webFileName = p.Idx + ".WEB";
                    webContent = p.PAS + "|" + p.Credentials.SIDAdmUser + "|" + p.Credentials.SIDAdmPass + "|" + p.Credentials.WebUser + "|" + Base64_Encode(p.Credentials.WebPass) + "|" + UserProfile.Domain;
                    CreateFile(cnfPath, webFileName, webContent);
                }

                /*if (p.OSType.ToUpper() == "WINDOWS")
                {
                    oswFileName = p.Idx + ".OSW";
                    oswContent = p.PAS + "|" + p.Credentials.SIDAdmUser + "|" + p.Credentials.SIDAdmPass + "|" + p.Credentials.OSUser + "|" + p.Credentials.OSPass + "|" + UserProfile.Domain;
                    CreateFile(cnfPath, oswFileName, oswContent);
                }*/

                if (p.BrideServer)
                {
                    string severListFileName = p.Idx + "_SERVERS.LIST";
                    
                    List<string> serverListFileContent = new List<string>() { };

                    for (int i = 0; i < p.ServerList.Count; i++)
                    {
                        if (p.ServerList[i].DBType.Trim().ToUpper() == "DB")
                        {
                            serverListFileContent.Add(p.ServerList[i].Customer.Replace(" ", "").ToUpper() + "|" + p.ServerList[i].SID.Trim() + "|" + p.ServerList[i].Hostname + "|" + 
                                p.ServerList[i].Environment + "|HANA|" + p.ServerList[i].CIDI.Trim() + "|" + p.ServerList[i].OS.Trim() + "|" + p.ServerList[i].Stack + "|" + p.ServerList[i].ProductType);
                        }
                        else
                        {
                            serverListFileContent.Add(p.ServerList[i].Customer.Replace(" ", "").ToUpper() + "|" + p.ServerList[i].SID.Trim() + "|" + p.ServerList[i].Hostname + "|" + p.ServerList[i].Environment + "|" + 
                                p.ServerList[i].DBType + "|" + p.ServerList[i].CIDI.Trim() + "|" + p.ServerList[i].OS.Trim() + "|" + p.ServerList[i].Stack + "|" + p.ServerList[i].ProductType);
                        }
                    }

                    CreateFile(evidencePath, severListFileName, serverListFileContent.ToArray());

                    if (p.Subtype.ToUpper().Contains("JAVA"))
                    {
                        string javaCompFileName = p.Idx + ".J2EECOMPONENTS";

                        CreateFile(evidencePath, javaCompFileName, String.Join("|", p.JavaComponents.Select(x => x.Name).ToArray()));
                    }
                }

                trgInstFileName = p.Idx + "_HOSTSAPMDTTARGET.CNF";
                trgInstHeader = "idx|SID|DB HOST|MANDT|INSTANCE";
                trgInstContent = p.Idx + "|" + p.SID + "|" + p.DBS + "|000|" + p.InstanceNum;
                CreateFile(cnfPath, trgInstFileName, new string[] { trgInstHeader, trgInstContent });

                sapPostCrdtlsFileName = p.Idx + "_SAPPOSTCRED.CNF";
                sapPostCrdtlsHeader = "idx|SAPALTERNUSER|SAPALTERNPASS";
                sapPostCrdtlsContent = p.Idx + "|SAP*|PASS";
                CreateFile(cnfPath, sapPostCrdtlsFileName, new string[] { sapPostCrdtlsHeader, sapPostCrdtlsContent });

                if (p.OraclePackages.Count > 0)
                {
                    bundleFileName = p.Idx + ".CATALOG";
                    
                    foreach (OraclePackage.PackageFile file in p.OraclePackages.First().PackageFiles)
                    {
                        bundleContent.Add(p.PASOS + "|" + p.OraclePackages.First().DBVersion + "|" + p.OraclePackages.First().Name + "|" + 
                            file.ShortName + "|" + file.Name + "|" + file.Description + "|" + file.Path + "|" + file.PackageFileMode);
                    }
                 
                    CreateFile(evidencePath, bundleFileName, bundleContent.ToArray());
                }
                
                if (p.SAPHostAgentPackages.Count > 0)
                {
                    bundleFileName = p.Idx + ".CATALOG";
                    bundleContent.Add(p.SAPHostAgentPackages.First().OSType + "|" + p.SAPHostAgentPackages.First().Version + "|" + p.SAPHostAgentPackages.First().Patch + "|UNIVERSAL|" + 
                        p.SAPHostAgentPackages.First().FileName + "|" + p.SAPHostAgentPackages.First().FilePath + "|" + p.SAPHostAgentPackages.First().OS + "|" + p.SAPHostAgentPackages.First().PackageFileMod );
                    CreateFile(evidencePath, bundleFileName, bundleContent.ToArray());
                }
                
                if (p.SAPKernelPackages.Count > 0)
                {
                    bundleFileName = p.Idx + ".CATALOG";
                    
                    foreach (SAPKernelPackage.SAPKernelFile file in p.SAPKernelPackages.First().PackageFiles)
                    {
                        bundleContent.Add(p.SAPKernelPackages.First().Unicode + "|" + p.SAPKernelPackages.First().Version + "|" + p.SAPKernelPackages.First().Patch + "|" 
                            + p.SAPKernelPackages.First().DB + "|" + file.Name + "|" + file.Path + "|" + file.OS + "|" + file.PackageFileMode);
                    }
                    
                    CreateFile(evidencePath, bundleFileName, bundleContent.ToArray());
                }
            }

            if (useSaltStack)
            {
                string saltMasterFileName = p.Idx + ".CLOUD";
                
                string saltMasterContent = "CUSTOMER|ENVIRONMENT|HOSTNAME|USER|KEY|TYPE|PORT|AUTH"; 
                CreateFile(evidencePath, saltMasterFileName, saltMasterContent);

                saltServersListFileName = p.Idx + ".SALT.SERVERS.LIST";
                if (p.ProjectName.Trim().ToUpper().Equals("STARTSAPCRMSYBASEHACLOUD") || p.ProjectName.Trim().ToUpper().Equals("STOPSAPCRMSYBASEHACLOUD"))
                {
                    string serverData;
                    List<string> serverInfo = new List<string>();
                    serverData = p.Customer + "|" + p.SID + "|" + p.HadrPrimaryDbServer.Hostname + "|" + p.HadrPrimaryDbServer.Environment + "|" + p.HadrPrimaryDbServer.DBType + "|DB_PRIM|" + p.HadrPrimaryDbServer.OS + "|" + p.HadrPrimaryDbServer.Stack + "|" + p.HadrPrimaryDbServer.ProductType;
                    serverInfo.Add(serverData);
                    
                    serverData = p.Customer + "|" + p.SID + "|" + p.HadrStandbyDbServer.Hostname + "|" + p.HadrStandbyDbServer.Environment + "|" + p.HadrStandbyDbServer.DBType + "|DB_STBY|" + p.HadrStandbyDbServer.OS + "|" + p.HadrStandbyDbServer.Stack + "|" + p.HadrStandbyDbServer.ProductType;
                    serverInfo.Add(serverData);

                    serverData = p.Customer + "|" + p.SID + "|" + p.HadrSapAcscScsServer.Hostname + "|" + p.HadrSapAcscScsServer.Environment + "|" + p.HadrSapAcscScsServer.DBType + "|SAP_CS|" + p.HadrSapAcscScsServer.OS + "|" + p.HadrSapAcscScsServer.Stack + "|" + p.HadrSapAcscScsServer.ProductType;
                    serverInfo.Add(serverData);

                    serverData = p.Customer + "|" + p.SID + "|" + p.HadrSapErsServer.Hostname + "|" + p.HadrSapErsServer.Environment + "|" + p.HadrSapErsServer.DBType + "|SAP_ERS|" + p.HadrSapErsServer.OS + "|" + p.HadrSapErsServer.Stack + "|" + p.HadrSapErsServer.ProductType;
                    serverInfo.Add(serverData);

                    foreach(ServerSystem server in p.HadrSapAasServer)
                    {
                        serverData = p.Customer + "|" + p.SID + "|" + server.Hostname + "|" + server.Environment + "|" + server.DBType + "|AAS|" + server.OS + "|" + server.Stack + "|" + server.ProductType;
                        serverInfo.Add(serverData);
                    }
                    CreateFile(evidencePath, saltServersListFileName, serverInfo.ToArray());
                }
                else
                {
                    for (int i = 0; i < p.ServerList.Count; i++)
                    {
                        if (p.ServerList[i].CIDI == "" && p.ServerList[i].DBType == "DB" && (p.ServerList[i].ProductType.ToLower() == "tenant" || p.ServerList[i].ProductType.ToLower() == "system"))
                        {
                            p.ServerList[i].CIDI = "DO";
                            p.ServerList[i].DBType = "HANA";
                        }

                        saltServersListContent.Add(p.ServerList[i].Customer.Replace(" ", "").ToUpper() + "|" + p.ServerList[i].SID.Trim() + "|" + p.ServerList[i].Hostname + "|" +
                            p.ServerList[i].Environment + "|" + p.ServerList[i].DBType + "|" + p.ServerList[i].CIDI.Trim() + "|" +
                            p.ServerList[i].OS.Trim() + "|" + p.ServerList[i].Stack + "|" + p.ServerList[i].ProductType + "|");
                    }
                    CreateFile(evidencePath, saltServersListFileName, saltServersListContent.ToArray());
                }
               
                //CreateFile(evidencePath, saltServersListFileName, saltServersListContent.ToArray());
                CreateSaltParamsFile(p);
                CreateCatalogFile(p, useSaltStack);
            }

            mailFileName = p.Idx + "_MAIL.CNF";
            mailHeader = "idx|EMAIL";
            mailContent = p.Idx + "|" + String.Join(";", p.EmailDest);
            CreateFile(cnfPath, mailFileName, new string[] { mailHeader, mailContent });//7debajo

            stepsFileName = p.Idx + "_STEPS.CONF";
            stepsHeader = "STEPNAME|PROCESSAUTO|EMAIL|REPEATAUTO|REPEATDATE|REPEATTIME|TRY";
            stepsContent = new List<string>() { stepsHeader };

            //generate all steps stop files, stepsConfig, extrainputs & transactions
            for (int i = 0; i < p.StepList.Count(); i++)
            {
                stopFileName = p.Idx + "_" + p.StepList[i].Name + ".STOP";
                CreateFile(processingPath, stopFileName, p.TimeStamp);

                if (p.StepList[i].RepeatDate != null && p.StepList[i].RepeatTime != null)
                {
                    stepsContent.Add(p.StepList[i].Name + "|" + p.StepList[i].AutoDefault.ToString() + "|" + p.StepList[i].EmailDefault.ToString() + "|" +
                        p.StepList[i].RepeatAuto.ToString() + "|" + p.StepList[i].RepeatDate.ToString() + "|" + p.StepList[i].RepeatTime.ToString() + "|0");
                }
                else
                {
                    stepsContent.Add(p.StepList[i].Name + "|" + p.StepList[i].AutoDefault.ToString() + "|" + p.StepList[i].EmailDefault.ToString() + "|" +p.StepList[i].RepeatAuto.ToString() + "|||0");
                }

                if (p.StepList[i].ExtraInputs.InputsSet != null)
                {
                    string extraInputFileName = p.Idx + "." + p.StepList[i].Name;
                    string inputsstring = "";
                    foreach (ExtraInput ei in p.StepList[i].ExtraInputs.InputsSet)
                    {
                        if (p.StepList[i].ExtraInputs.InputsSet.IndexOf(ei) == 0)
                        {
                            inputsstring += ei.Value;
                        }
                        else
                        {
                            inputsstring += "|" + ei.Value;
                        }
                    }

                    CreateFile(evidencePath, extraInputFileName, inputsstring);
                }

                if (p.StepList[i].Transactions.Length > 0)
                {
                    if (p.StepList[i].ExtraInputs.InputsSet != null)
                    {
                        transactions = p.StepList[i].ExtraInputs.InputsSet[0].Value;
                    }
                    else
                    {
                        transactions = p.StepList[i].Transactions;
                    }

                    string saptransFileName = p.Idx + "_" + p.StepList[i].Name + ".SAPTRANS";
                    string saptransHeader = "idx|HOSTNAME|SID|SAPUSER|SAPPASS|MANDT|INSTANCE|TRANSACTIONS";
                    string saptransContent = p.Idx + "|" + p.PAS + ".us.fit|" + p.SID + "|" + p.Credentials.SAPGuiUser + "|" + p.Credentials.SAPGuiPass + "|000|" + p.InstanceNum + "|" + transactions;
                    CreateFile(cnfPath, saptransFileName, new string[] { saptransHeader, saptransContent });
                }

                if (p.StepList[i].TransactionsList.Count > 0)
                {
                    string saptransFileName = p.Idx + "_" + p.StepList[i].Name + ".SAPTRANS";
                    string saptransHeader = "idx|HOSTNAME|SID|SAPUSER|SAPPASS|MANDT|INSTANCE|TRANSACTIONS";
                    transactions = String.Join(",",p.StepList[i].TransactionsList.Select(x => x.TCode).ToArray());
                    string saptransContent = p.Idx + "|" + p.PAS + ".us.fit|" + p.SID + "|" + p.StepList[i].TransactionsList.First().ClientSet.User + "|" + 
                        p.StepList[i].TransactionsList.First().ClientSet.Password + "|" + p.StepList[i].TransactionsList.First().ClientSet.ClientNum + "|" + p.InstanceNum + "|" + transactions;

                    CreateFile(cnfPath, saptransFileName, new string[] { saptransHeader, saptransContent });
                }
            }
            
            CreateFile(cnfPath, stepsFileName, stepsContent.ToArray());

            if (p.IntanceType == "ALL")
            {
                string applFileName = p.Idx + "_APPL.CNF";

                string applHeader = "idx|INSTANCE|SID|DIHOST|SAPTYPE|SAPOS|CI";

                List<string> applContent = new List<string>() { applHeader };

                for (int i = 0; i < p.ApplList.Count; i++)
                {
                    applContent.Add(p.Idx + "|DI" + p.InstanceNum + "|" + p.SID + "|" + p.ApplList[i].Hostname + "|" + p.ApplList[i].Type + "|" + p.ApplList[i].OS + "|" + p.PAS);
                }

                CreateFile(evidencePath, applFileName, applContent.ToArray());
            }

            preContent = p.Customer + "|" + p.SID + "|" + p.PASType + "|" + p.PAS + "|" + p.InstanceNum + "|" + p.Credentials.SIDAdmUser + "|" + 
                p.Credentials.SIDAdmPass + "|" + p.Credentials.OSUser + "|" + Base64_Encode(p.Credentials.OSPass) + "|000|" + p.DBSType + "|" + cnfPath + "|" +traceFolderName + "|" + 
                p.StepList[0].Name + "|" + p.ProjectName;
            
            CreateFile(prePath, p.Idx + ".PRE", preContent);
            
            ProcessExecution TempProcessExecution = new ProcessExecution { Idx = p.Idx, ProcessName = p.ProjectName, CurrentStep = p.CurrentStepIndex, GroupName = p.Team, User = p.User, 
                PAS = p.PAS, DBS = p.DBS, SID = p.SID, Title = p.Description, Customer = p.Customer, CreateDate = Auxiliar.DateTimeFromTimeStamp( p.TimeStamp )};
            PostProcessTraking(TempProcessExecution, "ProcessExecutions/");
            Auxiliar.SendLogRequest("Schedule process| " + p.Idx);

            p.RealTimeLog = "Waiting Start.";
            //string userProyectPath = catalogPath + "AIT_" + UserProfile.ItUser + "\\" + p.ProjectName + "\\PROCESSSUMARY\\";
            string readPath = catalogPath + "AIT_" + UserProfile.ItUser + "_READ\\" + p.ProjectName + "\\PROCESSSUMARY\\";
            try
            {
                string jsonString = JsonConvert.SerializeObject(p);
                
                if (!File.Exists(readPath + p.Idx + ".JSON"))
                {
                    CreateFile(readPath, p.Idx + ".JSON", jsonString);
                    string generalPath = Auxiliar.catalogPath + "AIT_" + UserProfile.ItUser;
                    string filePath = generalPath + "\\" + p.Idx + ".MONITOR";
                    string projectName = p.Idx.Split("_")[2];
                    //string fileToWrite = readPath + "\\" + projectName + "\\PROCESSSUMARY\\" + p.Idx + ".JSON";
                    string fileToWrite = readPath + p.Idx + ".JSON";
                    Thread.Sleep(1000);

                    File.WriteAllText(filePath, fileToWrite);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading json file");
            }
        }

        //This function creates the SAP Install Params File for all Process
        public static void CreateCatalogFile(Process p, bool useSaltStack)
        {
            //Initialize the variables declaration
            string evidencePath, saltCatalogFileName, bundleFileName;
            List<string> bundleContent = new List<string>();
            //End of variables declaration

            //Initialize the variables asignation
            evidencePath = GetEvidencePath(p.Idx);
            if (useSaltStack)
                saltCatalogFileName = p.Idx + ".SALT.CATALOG";
            else
                saltCatalogFileName = p.Idx + ".CATALOG";
            bundleFileName = p.Idx + ".CATALOG";
            //End of variables declaration

            //Initialize the folder creation
            if (!(Directory.Exists(evidencePath)))
                CreateFolder(evidencePath);
            //End the forlder creation

            //Initialize to add data to content
            if (p.OraclePackages.Count > 0)
            {
                foreach (OraclePackage.PackageFile file in p.OraclePackages.First().PackageFiles)
                {
                    bundleContent.Add(p.PASOS + "|" + p.OraclePackages.First().DBVersion + "|" + p.OraclePackages.First().Name + "|" + file.ShortName + "|" + file.Name + "|" + file.Description + "|" + file.Path + "|" + file.PackageFileMode);
                }
            }

            else if (p.SAPHostAgentPackages.Count > 0)
            {
                bundleContent.Add(p.SAPHostAgentPackages.First().OSType + "|" + p.SAPHostAgentPackages.First().Version + "|" + p.SAPHostAgentPackages.First().Patch + "|UNIVERSAL|" + p.SAPHostAgentPackages.First().FileName + "|" + p.SAPHostAgentPackages.First().FilePath + "|" + p.SAPHostAgentPackages.First().OS + "|" + p.SAPHostAgentPackages.First().PackageFileMod);
            }

            else if (p.SAPKernelPackages.Count > 0)
            {
                foreach (SAPKernelPackage.SAPKernelFile file in p.SAPKernelPackages.First().PackageFiles)
                {
                    bundleContent.Add(p.SAPKernelPackages.First().Unicode + "|" + p.SAPKernelPackages.First().Version + "|" + p.SAPKernelPackages.First().Patch + "|" + p.SAPKernelPackages.First().DB + "|" + file.Name + "|" + file.Path + "|" + file.OS + "|" + file.PackageFileMode);
                }
            }

            else if (p.SapInstallCatalogs.Count > 0)
            {
                foreach (SapInstallCatalog.SapCatalogFile file in p.SapInstallCatalogs.First().CatalogFiles)
                {
                    bundleContent.Add(p.SapInstallCatalogs.First().OpSysType + "|" + p.SapInstallCatalogs.First().OsDist + "|" + p.SapInstallCatalogs.First().OsArch + "|" + p.SapInstallCatalogs.First().SapProd + "|" + p.SapInstallCatalogs.First().SapStack + "|" + p.SapInstallCatalogs.First().SapKernel + "|" + p.SapInstallCatalogs.First().DbName + "|" + p.SapInstallCatalogs.First().DbVersion + "|" + p.SapInstallCatalogs.First().DbPatch + "|" + file.OsPatch + "|" + file.Name + "|" + file.ControlData + "|" + file.ControlValue + "|" + file.Description + "|" + file.Control1 + "|" + file.Control2 + "|" + file.Control3 + "|" + file.Control4 + "|" + file.Control5);
                }
            }

            else if (p.Db2InstallCatalogs.Count > 0)
            {
                foreach (Db2Install.Db2InstallFile file in p.Db2InstallCatalogs.First().File)
                {
                    bundleContent.Add(p.Db2InstallCatalogs.First().OsType + "|" + p.Db2InstallCatalogs.First().OsDistribution + "|" + p.Db2InstallCatalogs.First().OsArchitecture + "|" + p.Db2InstallCatalogs.First().SapProduct + "|" + p.Db2InstallCatalogs.First().SapStack + "|" + p.Db2InstallCatalogs.First().SapKernel + "|" + p.Db2InstallCatalogs.First().Db + "|" + p.Db2InstallCatalogs.First().DbVersion + "|" + p.Db2InstallCatalogs.First().DbPatch + "|" + file.OsPatch + "|" + file.FileName + "|" + file.ControlData + "|" + file.ControlValue + "|" + file.FileDescription + "|" + file.Control1 + "|" + file.Control2 + "|" + file.Control3 + "|" + file.Control4 + "|" + file.Control5);
                }
            }
            CreateFile(evidencePath, saltCatalogFileName, bundleContent.ToArray());
            CreateFile(evidencePath, bundleFileName, bundleContent.ToArray());
            
            //End to add data to content
        }

        public static void CreateServerListFile(Process p)
        {
            string evidencePath, saltSourceServerListFileName, saltTargetServerListFileName;
            List<string> saltSourceServersListContent = new List<string>(), saltTargetServersListContent = new List<string>();

            evidencePath = GetEvidencePath(p.Idx);
            saltSourceServerListFileName = p.Idx + ".SALT.SERVER.SOURCE.LIST";
            saltTargetServerListFileName = p.Idx + ".SALT.SERVERS.LIST";

            for (int i = 0; i < p.ServerList.Count; i++)
            {
                if (p.ServerList[i].CIDI == "" && p.ServerList[i].DBType == "DB" && (p.ServerList[i].ProductType.ToLower() == "tenant" || p.ServerList[i].ProductType.ToLower() == "system"))
                {

                    p.ServerList[i].CIDI = "DO";
                    p.ServerList[i].DBType = "HANA";
                }

                saltTargetServersListContent.Add(p.ServerList[i].Customer.Replace(" ", "").ToUpper() + "|" + p.ServerList[i].SID.Trim() + "|" + p.ServerList[i].Hostname + "|" + p.ServerList[i].Environment + "|" + p.ServerList[i].DBType + "|" + p.ServerList[i].CIDI.Trim() + "|" + p.ServerList[i].OS.Trim() + "|" + p.ServerList[i].Stack + "|" + p.ServerList[i].ProductType + "|");
            }

            if (p.SourceServers[0].CIDI == "" && p.SourceServers[0].DBType == "DB" && (p.SourceServers[0].ProductType.ToLower() == "tenant" || p.SourceServers[0].ProductType.ToLower() == "system"))
            {
                p.SourceServers[0].CIDI = "DO";
                p.SourceServers[0].DBType = "HANA";
            }

            saltSourceServersListContent.Add(p.Customer.Replace(" ", "").ToUpper() + "|" + p.SourceSID + "|" + p.SourceDBS + "|" + p.SourceServers[0].Environment + "|" + p.SourceServers[0].DBType + "|" + p.SourceServers[0].CIDI.Trim() + "|" + p.SourceServers[0].OS.Trim() + "|" + p.SourceServers[0].Stack + "|" + p.SourceServers[0].ProductType + "|");

            //Create file
            CreateFile(evidencePath, saltTargetServerListFileName, saltTargetServersListContent.ToArray());
            CreateFile(evidencePath, saltSourceServerListFileName, saltSourceServersListContent.ToArray());
        }

        //This function is to upload the SAP INSTALL POST STEPS files
        public static void UploadSapPAFile(Process p)
        {

            if (p.ProjectName.ToUpper() == "SAPINSTALLPOSTACTIVITIES" || p.ProjectName.ToUpper() == "SAPINSTALLPOST")
            {
                //Assign variables to insert data into file
                List<string> postActivities = new List<string>();
                string path = "\\\\10.130.19.40\\ait\\SAPLIGHTORQ\\TMPUC\\TMPCONF\\" + p.Idx + "\\";
                string targetLicensePath = path + SapInstallLicenseFile.fileName;
                string targetStrustPath = path + SapInstallSTRUST02.fileCertificateName;


                Directory.CreateDirectory(path);

                //Copy of file LICENSEFILE
                File.Copy(SapInstallLicenseFile.fullName, targetLicensePath);

                //Copy of file STRUST02
                for (int i = 0; i < p.Strust02List.Count(); i++)
                {
                    File.Copy(p.Strust02List[i].CertificatePath, targetStrustPath);
                }

                //TRANS files generation
                #region TRANS file generation
                string al11Trans = p.Idx + "_AL11P.TRANS";
                postActivities.Add("NameOfDirectoryParameter, DirectoryPath,ValidForServerName!!");
                for (int i = 0; i < p.Al11List.Count(); i++)
                    postActivities.Add("NameOfDirectoryParameter=" + p.Al11List[i].DirectoryName + ", DirectoryPath=" + p.Al11List[i].DirectoryPath + ", ValidFor=" + p.Al11List[i].ValidForServer + "!! ");
                CreateFile(path, al11Trans, postActivities.ToArray());
                postActivities.Clear();

                string db13Trans = p.Idx + "_DB13.TRANS";
                postActivities.Add("JOBNAME,DATEJOB,TIMEJOB,PERIOD,PARAMETERS,RECRange");
                for (int i = 0; i < p.Db13List.Count(); i++)
                    postActivities.Add(p.Db13List[i].Job + "," + p.Db13List[i].StartDate + "," + p.Db13List[i].Recurrence + "," + p.Db13List[i].Range);
                CreateFile(path, db13Trans, postActivities.ToArray());
                postActivities.Clear();

                string rz04Trans = p.Idx + "_RZ04P.TRANS";
                postActivities.Add("OperationModeName, Description, Starting time, End time!!");
                for (int i = 0; i < p.Rz04List.Count(); i++)
                    postActivities.Add(p.Rz04List[i].OperationName + "," + p.Rz04List[i].Description + "," + p.Rz04List[i].InTime + "," + p.Rz04List[i].EndTime + "!! ");
                CreateFile(path, rz04Trans, postActivities.ToArray());
                postActivities.Clear();

                string rz10addpTrans = p.Idx + "_RZ10ADDP.TRANS";
                postActivities.Add("NAME=VALUE!!");
                for (int i = 0; i < p.AddpList.Count(); i++)
                    postActivities.Add(p.AddpList[i].AddpName + "=" + p.AddpList[i].AddpValue + "!! ");
                CreateFile(path, rz10addpTrans, postActivities.ToArray());
                postActivities.Clear();

                string rz10fqicpTrans = p.Idx + "_RZ10FQICP.TRANS";
                postActivities.Add("NAME=VALUE!!");
                for (int i = 0; i < p.FqicpList.Count(); i++)
                    postActivities.Add(p.FqicpList[i].FqicpName + "=" + p.FqicpList[i].FqicpValue + "!! ");
                CreateFile(path, rz10fqicpTrans, postActivities.ToArray());
                postActivities.Clear();

                string rz12Trans = p.Idx + "_RZ12.TRANS";
                postActivities.Add("ServerGroup,Instance,usequotas,maxqueue,maxlogin,Maxseparatelogons,maxwp,minfreewp,maxcomm,maxwaittime!!");
                for (int i = 0; i < p.Rz12List.Count(); i++)
                    postActivities.Add(p.Rz12List[i].GroupName + "," + p.Rz12List[i].InstanceGroup + "," + p.Rz12List[i].ActivatedNumber + "," + p.Rz12List[i].MaxQueue + "," + p.Rz12List[i].MaxLogin + "," + p.Rz12List[i].MaxSeparateLogons + "," + p.Rz12List[i].Maxwp + "," + p.Rz12List[i].Minfreewp + "," + p.Rz12List[i].Maxcomm + "," + p.Rz12List[i].MaxWaitTime + "!! ");
                CreateFile(path, rz12Trans, postActivities.ToArray());
                postActivities.Clear();

                string rz70Trans = p.Idx + "_RZ70P.TRANS";
                postActivities.Add("GATEWAY HOST,GATEWAY SERVICE!!");
                for (int i = 0; i < p.Rz70List.Count(); i++)
                    postActivities.Add(p.Rz70List[i].GatewayHost + "," + p.Rz70List[i].GatewayService + "!! ");
                CreateFile(path, rz70Trans, postActivities.ToArray());
                postActivities.Clear();

                string scc4Trans = p.Idx + "_SCC4P.TRANS";
                postActivities.Add("Client,ClientName,CityName,LogicalName,StdCurrencyName,ClientRoleType,ChangesAndTTransportForClientSpectObj,CROSS CLIENT OBJECT CHANGES,ClientCopyComparisonToolProt,CattAndEcattRestrcitrions!!");
                for (int i = 0; i < p.Scc4List.Count(); i++)
                    postActivities.Add(p.Scc4List[i].Client + "," + p.Scc4List[i].ClientName + "," + p.Scc4List[i].ClientCity + "," + p.Scc4List[i].LogicalName + "," + p.Scc4List[i].Currency + "," + p.Scc4List[i].ClientRole.Remove(1) + "," + p.Scc4List[i].ChangesAndTransport.Remove(1) + "," + p.Scc4List[i].CrossClient.Remove(4) + "," + p.Scc4List[i].CopyComparisonTool.Remove(5) + "," + p.Scc4List[i].CattAndEcattRest.Remove(4) + "!! ");
                CreateFile(path, scc4Trans, postActivities.ToArray());
                postActivities.Clear();

                string licenseFileTrans = p.Idx + "_SLICENSE.TRANS";
                postActivities.Add(SapInstallLicenseFile.fileName);
                CreateFile(path, licenseFileTrans, postActivities.ToArray());
                postActivities.Clear();

                string sm21Trans = p.Idx + "_SM21P.TRANS";
                postActivities.Add(p.Sm21From.ToString("dd") + "." + p.Sm21From.ToString("MM") + "." + p.Sm21From.ToString("yyyy") + "," + p.Sm21From.ToString("HH") + ":" + p.Sm21From.ToString("mm") + ":" + p.Sm21From.ToString("ss"));
                CreateFile(path, sm21Trans, postActivities.ToArray());
                postActivities.Clear();

                string sm36Trans = p.Idx + "_SM36P.TRANS";
                postActivities.Add("MANDT,INSTANCE,SAPUSER!!SAPPASS");
                postActivities.Add("MANDT=000,INSTANCE=00,SAPUSER=" + p.Sm36SapUser + "!!SAPPASS=" + p.Sm36SapPassword);
                CreateFile(path, sm36Trans, postActivities.ToArray());
                postActivities.Clear();

                string sm61Trans = p.Idx + "_SM61P.TRANS";
                postActivities.Add("GROUP NAME,INSTANCE!!");
                for (int i = 0; i < p.Sm61List.Count(); i++)
                    postActivities.Add(p.Sm61List[i].GroupName + "," + p.Sm61List[i].Instance + "!! ");
                CreateFile(path, sm61Trans, postActivities.ToArray());
                postActivities.Clear();

                string smlgTrans = p.Idx + "_SMLG.TRANS";
                postActivities.Add("CUSTOMER NAME,INSTANCE GROUP,IP GROUP,RFC ENABLED,RFC TYPE!!");
                for (int i = 0; i < p.SmlgList.Count(); i++)
                    postActivities.Add(p.SmlgList[i].CustomerName + "," + p.SmlgList[i].InstanceGroup + "," + p.SmlgList[i].IpGroup + "," + p.SmlgList[i].RfcEnabled.ToString() + "," + p.SmlgList[i].RfcType.Remove(1) + "!! ");
                CreateFile(path, smlgTrans, postActivities.ToArray());
                postActivities.Clear();

                string st22Trans = p.Idx + "_ST22P.TRANS";
                postActivities.Add("STARTDATE,ENDDATE,START TIME,ENDTIME,USERNAME");
                postActivities.Add(p.St22From.ToString("dd") + "." + p.St22From.ToString("MM") + "." + p.St22From.ToString("yyyy") + "," + p.St22To.ToString("dd") + "." + p.St22To.ToString("MM") + "." + p.St22To.ToString("yyyy") + "," + p.St22From.ToString("HH") + ":" + p.St22From.ToString("mm") + ":" + p.St22From.ToString("ss") + "," + p.St22To.ToString("HH") + ":" + p.St22To.ToString("mm") + ":" + p.St22To.ToString("ss") + "," + p.St22User);
                CreateFile(path, st22Trans, postActivities.ToArray());
                postActivities.Clear();

                string strust02Trans = p.Idx + "_STRUSTSSO2P.TRANS";
                postActivities.Add("CERTIFICATE NAME,CERTIFICATE TYPE,CERTIFICATEPATH!!");
                for (int i = 0; i < p.Strust02List.Count(); i++)
                    postActivities.Add(p.Strust02List[i].CertificateName + "," + p.Strust02List[i].CertificateType + "," + p.Strust02List[i].CertificatePath + "!! ");
                CreateFile(path, strust02Trans, postActivities.ToArray());
                postActivities.Clear();
                #endregion

                #region KY file generation
                string kyFileName, kyHeader, kyContent = "", kyContentHeader = "TRANSACTIONS:";
                for (int i = 0; i < p.StepList.Count(); i++)
                {
                    if (p.StepList[i].TransactionsList.Count > 0)
                    {
                        kyFileName = p.Idx + "_" + p.StepList[i].Name + ".KY";
                        kyHeader = "idx:" + p.Idx;
                        
                        for (int j = 0; j < p.StepList[i].TransactionsList.Count(); j++)
                        {
                            kyContent = kyContent + p.StepList[i].TransactionsList[j].TCode + "!" + p.Idx + "_" + p.StepList[i].TransactionsList[j].TCode + ".TRANS@";
                        }
                        kyContent = kyContentHeader + kyContent;
                        
                        CreateFile(path, kyFileName, new string[] { kyHeader, kyContent });

                        #region CATALOG file configuration
                        string catalogFileName = p.Idx + "_" + p.StepList[i].Name + "-DESTINATION.CATALOG";
                        List<string> catalogContent = new List<string>();

                        catalogContent.Add("idx_file:" + p.Idx);
                        catalogContent.Add("VARIANT:CUST");
                        catalogContent.Add("PREFIX:POST");
                        catalogContent.Add("Host:" + p.PAS);
                        catalogContent.Add("SID:" + p.SID);
                        catalogContent.Add("Mndt:" + p.Credentials.ClientsList[0].ClientNum);
                        catalogContent.Add("Inst:" + p.InstanceNum);
                        catalogContent.Add("User:" + p.Credentials.ClientsList[0].User);
                        catalogContent.Add("Pass:" + p.Credentials.ClientsList[0].Password);
                        CreateFile(path, catalogFileName, catalogContent.ToArray());
                        #endregion
                    }
                }
                #endregion
            }
        }

        public static void UploadQualityCheckFile(Process p)
        {
            List<string> qcData = new List<string>();
            string path = "\\\\10.130.19.40\\ait\\SAPLIGHTORQ\\TMPUC\\TMPCONF\\" + p.Idx + "\\";

            string sm21Trans = p.Idx + "_QCSM21P.TRANS";
            qcData.Add("TO,FROM");
            qcData.Add(p.Sm21From.ToString("dd") + "." + p.Sm21From.ToString("MM") + "." + p.Sm21From.ToString("yyyy") + "," + p.Sm21From.ToString("HH") + ":" + p.Sm21From.ToString("mm") + ":" + p.Sm21From.ToString("ss"));
            CreateFile(path, sm21Trans, qcData.ToArray());
            qcData.Clear();

            string st22Trans = p.Idx + "_QCST22P.TRANS";
            qcData.Add("STARTDATE,ENDDATE,STARTTIME,ENDTIME,USERNAME");
            qcData.Add(p.St22From.ToString("dd") + "." + p.St22From.ToString("MM") + "." + p.St22From.ToString("yyyy") + "," + p.St22From.ToString("HH") + ":" + p.St22From.ToString("mm") + ":" + p.St22From.ToString("ss") + "," + p.St22To.ToString("dd") + "." + p.St22To.ToString("MM") + "." + p.St22To.ToString("yyyy") + "," + p.St22To.ToString("HH") + ":" + p.St22To.ToString("mm") + ":" + p.St22To.ToString("ss") + "," + p.St22User + "!! ");
            CreateFile(path, st22Trans, qcData.ToArray());
            qcData.Clear();

            #region KY file generation
            string kyFileName, kyHeader, kyContent = "", kyContentHeader = "TRANSACTIONS:";
            for (int i = 0; i < p.StepList.Count(); i++)
            {
                if (!(p.StepList[i].Name.Contains("MSG")))
                {
                    if (p.StepList[i].TransactionsList.Count > 0)
                    {
                        kyFileName = p.Idx + "_" + p.StepList[i].Name + ".KY";
                        kyHeader = "idx:" + p.Idx;
                        kyContent = "";
                        
                        for (int j = 0; j < p.StepList[i].TransactionsList.Count(); j++)
                        {
                            kyContent = kyContent + p.StepList[i].TransactionsList[j].TCode + "!" + p.Idx + "_" + p.StepList[i].TransactionsList[j].TCode + ".TRANS@";
                        }
                        kyContent = kyContentHeader + kyContent;
                        
                        CreateFile(path, kyFileName, new string[] { kyHeader, kyContent });

                        #region CATALOG file configuration
                        string catalogFileName = p.Idx + "_" + p.StepList[i].Name + "-DESTINATION.CATALOG";
                        List<string> catalogContent = new List<string>();

                        catalogContent.Add("idx_file:" + p.Idx);
                        catalogContent.Add("VARIANT:CUST");
                        catalogContent.Add("PREFIX:POST");
                        catalogContent.Add("Host:" + p.PAS);
                        catalogContent.Add("SID:" + p.SID);
                        catalogContent.Add("Mndt:" + p.Credentials.ClientsList[0].ClientNum);
                        catalogContent.Add("Inst:" + p.InstanceNum);
                        catalogContent.Add("User:" + p.Credentials.ClientsList[0].User);
                        catalogContent.Add("Pass:" + p.Credentials.ClientsList[0].Password);
                        CreateFile(path, catalogFileName, catalogContent.ToArray());
                        #endregion
                    }
                }
            }
            #endregion
        }
        public static void GenerateNGZTFolders(Process p)
        {
            string path = "\\\\10.130.19.40\\ait\\SAPLIGHTORQ\\TMPUC\\USER\\" + p.User + "\\" + p.Idx + "\\IDX_";

            string idx_EVD = path + "EVD";
            string idx_Status = path + "STATUS";
            string idx_LOG = path + "LOG";

            var paths = new[] { idx_EVD, idx_Status, idx_LOG };

            foreach (var idx_file in paths)
            {
                try
                {
                    // Determine whether the directory exists.
                    if (Directory.Exists(idx_file))
                    {
                        Console.WriteLine($"Skipping path '{idx_file}' because it exists already.");
                        continue;
                    }

                    // Try to create the directory.
                    var di = Directory.CreateDirectory(idx_file);
                    Console.WriteLine($"Created path '{idx_file}' successfully at {Directory.GetCreationTime(idx_file)}.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"The process failed: {e}");
                }
            }
        }
        //This function creates the SAP Install Params File for all Process
        public static void CreateSaltParamsFile(Process p)
        {
            //Initialize the variables declaration
            string fileName, fileNameOnPremise, evidencePath;
            List<string> saltParamsContent, sourceDBParamsContent, targetDBParamsContent;
            //End of variables declaration

            //Initialize the variables asignation
            fileName = p.Idx + ".SALT.PARAMS";
            fileNameOnPremise = p.Idx + ".PARAMS";
            evidencePath = GetEvidencePath(p.Idx);
            saltParamsContent = new List<string>();
            sourceDBParamsContent = new List<string>();
            targetDBParamsContent = new List<string>();
            //End of variables declaration

            //Initialize the folder creation
            if (!(Directory.Exists(evidencePath)))
                CreateFolder(evidencePath);
            //End the forlder creation

            //Initialize to add data to content

            //Add data if the subtype of the process has JAVA
            if (p.ProjectName.ToUpper().Contains("JAVA"))
            {
                saltParamsContent.Add("J2EECOMPONENTS:" + String.Join("|", p.JavaComponents.Select(x => x.Name).ToArray()));
            }

            //Add data if the process is an ORACLE UPGRADE
            if (p.Description.ToUpper().Contains("UPGRADE") && p.Description.ToUpper().Contains("ORACLE"))
            {
                saltParamsContent.Add("DB_SCHEMA_USER:" + p.Credentials.DBUser);
                saltParamsContent.Add("DB_SCHEMA_PASSW:" + p.Credentials.DBSchemaPass);
            }

            //Add data if the process is a SYSTEMCOPY with ORACLE database
            if (p.ProjectName.ToUpper().Contains("SYSTEMCOPY") && p.ProjectName.ToUpper().Contains("ORACLE"))
            {
                string SourceDBserverFileName = p.Idx + ".SALT.SERVER.SOURCE.LIST";

                sourceDBParamsContent.Add(p.Customer + "|" + p.SourceSID + "|" + p.SourceDBS + "|PRD|" + p.DBSType + "|CI|" + p.SourceDBSOS + "|ABAP|" + p.SourceType + "|");

                CreateFile(evidencePath, SourceDBserverFileName, sourceDBParamsContent.ToArray());

                saltParamsContent.Add("Restore Mode:AUTO");
                saltParamsContent.Add("Restore Time:" + p.RestoreDateTime.Value.ToString("yyyy-MM-dd_HH:mm:ss"));
                saltParamsContent.Add("CV-Streams:" + p.CVStreams);
                saltParamsContent.Add("Source DB Name:" + p.SourceSID);
                saltParamsContent.Add("Source DB Server:" + p.SourceDBS);
                saltParamsContent.Add("Source DB OS Type:" + p.SourceDBSOS);
                saltParamsContent.Add("Target DB Name:" + p.SID);
                saltParamsContent.Add("Target DB Server:" + p.PAS);
                saltParamsContent.Add("Target DB OS Type:" + p.OSType);
                saltParamsContent.Add("Target SAP DB User:" + p.Credentials.DBUser);
                saltParamsContent.Add("Target SAP DB Pass:" + p.Credentials.DBPass);
            }

            //Add data if the process is a SYSTEMCOPY ORACLE databse with SALT
            if (p.ProjectName.ToUpper().Equals("SAPSYSTEMCOPYORADBCOPYCLOUD"))
            {
                DateTime restore = (DateTime)p.RestoreDateTime;

                //Salt Params File
                saltParamsContent.Add("Restore Mode:AUTO");
                saltParamsContent.Add("Restore Time:" + restore.ToString("yyyy-MM-dd_HH:mm:ss"));
                saltParamsContent.Add("CV-Streams:" + p.CVStreams);
                saltParamsContent.Add("Source DB Name:" + p.SourceSID);
                saltParamsContent.Add("Source DB Server:" + p.SourceDBS);
                saltParamsContent.Add("Source DB OS Type:" + p.SourceServers[0].OS);
                for (int i = 0; i < p.ServerList.Count(); i++)
                {
                    saltParamsContent.Add("Target DB Name:" + p.ServerList[i].SID);
                    saltParamsContent.Add("Target DB Server:" + p.ServerList[i].Hostname);
                    saltParamsContent.Add("Target DB OS Type:" + p.ServerList[i].OS);
                }
                saltParamsContent.Add("Target SAP DB User:" + p.Credentials.DBUser);
                saltParamsContent.Add("Target SAP DB Pass:" + p.Credentials.DBPass);
            }

            //Add data if the process is a SAPINSTALL
            if (p.ProjectName.ToUpper().Equals("SAPINSTALLCLOUD"))
            {
                saltParamsContent.Add("SAP SID: " + p.SapInstall.SapSId);
                saltParamsContent.Add("ASCS Instance Number: " + p.SapInstall.AscsInstNum);
                saltParamsContent.Add("PAS Instance Number: " + p.SapInstall.PasInstNum);
                saltParamsContent.Add("Hana DB Name: " + p.SapInstall.HanaDbName);
                saltParamsContent.Add("HANA Instance Number: " + p.SapInstall.HanaInstNum);
                saltParamsContent.Add("sapsys gID: " + p.SapInstall.SapSysGId);
                saltParamsContent.Add("sapinst gID: " + p.SapInstall.SapInsGId);
                saltParamsContent.Add("DBSIDadm gID: " + p.SapInstall.DbSIdAdmGId);
                saltParamsContent.Add("DBSIDadm uID: " + p.SapInstall.DbSIdAdmUId);
                saltParamsContent.Add("SIDadm uID: " + p.SapInstall.SidAdmUId);
                saltParamsContent.Add("sapadm uID: " + p.SapInstall.SapAdmUId);
                saltParamsContent.Add("Schema Name: " + p.SapInstall.DbScehmaName);
                saltParamsContent.Add("Master Password: " + p.SapInstall.MasterPass);
                saltParamsContent.Add("Virtual Host: " + p.SapInstall.VirtualHost);
                saltParamsContent.Add("Virtual Host Interface: " + p.SapInstall.VirtHostInter);
                saltParamsContent.Add("Set Domain: " + p.SapInstall.SetDomain.ToString().ToLower());
                saltParamsContent.Add("Domain Name: " + p.SapInstall.DomainName);
            }

            if (p.ProjectName.ToUpper().Equals("SAPINSTALLORACLECLOUD"))
            {
                saltParamsContent.Add("SAP SID: " + p.SapInstall.SapSId);
                saltParamsContent.Add("ASCS Instance Number: " + p.SapInstall.AscsInstNum);
                saltParamsContent.Add("PAS Instance Number: " + p.SapInstall.PasInstNum);
                saltParamsContent.Add("sapsys gID: " + p.SapInstall.SapSysGId);
                saltParamsContent.Add("sapinst gID: " + p.SapInstall.SapInsGId);
                saltParamsContent.Add("sapadm uID: " + p.SapInstall.SapAdmUId);
                saltParamsContent.Add("SIDadm uID: " + p.SapInstall.SidAdmUId);
                saltParamsContent.Add("SAP Hostname: " + p.SapInstall.SapHostname);
                saltParamsContent.Add("SAP Virtual Hostname: " + p.SapInstall.SapVirtualHostname);
                saltParamsContent.Add("Database Name: " + p.SapInstall.DatabaseName);
                saltParamsContent.Add("OraSID gID: " + p.SapInstall.OraSidGId);
                saltParamsContent.Add("OraSID uID: " + p.SapInstall.OraSidUId);
                saltParamsContent.Add("Oracle Listener port: " + p.SapInstall.OracleListenerPort);
                saltParamsContent.Add("Database Hostname: " + p.SapInstall.DatabaseHn);
                saltParamsContent.Add("Database Virtual Hostname: " + p.SapInstall.DatabaseVirtualHn);
                saltParamsContent.Add("Set Domain: " + p.SapInstall.SetDomain.ToString().ToLower());
                saltParamsContent.Add("Domain Name: " + p.SapInstall.DomainName);
                saltParamsContent.Add("Master Password: " + p.SapInstall.MasterPass);
            }

            if (p.ProjectName.ToUpper().Equals("SAPINSTALLHANACLOUD"))
            {
                saltParamsContent.Add("SAP SID: " + p.SapInstall.SapSId);
                saltParamsContent.Add("ASCS Instance Number: " + p.SapInstall.AscsInstNum);
                saltParamsContent.Add("PAS Instance Number: " + p.SapInstall.PasInstNum);
                saltParamsContent.Add("Hana DB Name: " + p.SapInstall.HanaDbName);
                saltParamsContent.Add("HANA Instance Number: " + p.SapInstall.HanaInstNum);
                saltParamsContent.Add("sapsys gID: " + p.SapInstall.SapSysGId);
                saltParamsContent.Add("sapinst gID: " + p.SapInstall.SapInsGId);
                saltParamsContent.Add("DBSIDadm uID: " + p.SapInstall.DbSIdAdmUId);
                saltParamsContent.Add("SIDadm uID: " + p.SapInstall.SidAdmUId);
                saltParamsContent.Add("sapadm uID: " + p.SapInstall.SapAdmUId);
                saltParamsContent.Add("Schema Name: " + p.SapInstall.DbScehmaName);
                saltParamsContent.Add("Master Password: " + p.SapInstall.MasterPass);
                saltParamsContent.Add("Database Virtual Hostname: " + p.SapInstall.VirtualHost);
                saltParamsContent.Add("Database Hostname: " + p.SapInstall.DatabaseHn);
                saltParamsContent.Add("SAP Virtual Hostname: " + p.SapInstall.VirtualHostSap);
                saltParamsContent.Add("SAP Hostname: " + p.SapInstall.SapHostname);
                saltParamsContent.Add("Set Domain: " + p.SapInstall.SetDomain.ToString().ToLower());
                saltParamsContent.Add("Domain Name: " + p.SapInstall.DomainName);
            }

            if (p.ProjectName.ToUpper().Equals("SAPINSTALLAASCLOUD"))
            {
                saltParamsContent.Add("SAP SID: " + p.SapInstall.SapSId);
                saltParamsContent.Add("AAS Instance Number: " + p.SapInstall.AscsInstNum);
                saltParamsContent.Add("sapinst gID: " + p.SapInstall.SapInsGId);
                saltParamsContent.Add("sapadm uID: " + p.SapInstall.SapAdmUId);
                saltParamsContent.Add("PAS Hostname: " + p.SapInstall.SapPasHnm);
                saltParamsContent.Add("AAS Hostname: " + p.SapInstall.SapAasHnm);
                saltParamsContent.Add("AAS Virtual Hostname: " + p.SapInstall.SapAasVHnm);
                saltParamsContent.Add("Master Password: " + p.SapInstall.MasterPass);
                saltParamsContent.Add("Set Domain: " + p.SapInstall.SetDomain.ToString().ToLower());
                saltParamsContent.Add("Domain Name: " + p.SapInstall.DomainName);
            }

            //Add data if the process is a SAPINSTALLPOSTACTIVITIES
            if (p.ProjectName.ToUpper().Equals("SAPPOSTHANACLOUD"))
            {
                saltParamsContent.Add("HANA Instance Number: " + p.SapInstall.HanaInstNum);
                saltParamsContent.Add("Hana DB Name: " + p.SapInstall.HanaDbName);
                saltParamsContent.Add("System DB Password: " + p.SapInstall.MasterPass);

                CreateFile(evidencePath, fileNameOnPremise, saltParamsContent.ToArray());
            }

            if (p.ProjectName.ToUpper().Equals("SAPPOSTORACLOUD"))
            {
                saltParamsContent.Add("Task: " + p.TaskOracle);
                
                CreateFile(evidencePath, fileNameOnPremise, saltParamsContent.ToArray());
            }

            //Add data if the process is a DB2INSTALL
            if (p.ProjectName.ToUpper().Equals("DB2INSTALLCLOUD") && p.ServerList[0].OS.ToUpper().Equals("LINUX"))
            {
                saltParamsContent.Add("Pacemaker: " + p.Db2Pacemaker.ToUpper());
            }

            if (p.ProjectName.ToUpper().Equals("STARTSAPCRMSYBASEHACLOUD") || p.ProjectName.ToUpper().Equals("STOPSAPCRMSYBASEHACLOUD"))
            {
                saltParamsContent.Add("SapsaPass: " + p.HadrSapsaPassword);
                saltParamsContent.Add("DrAdminUser: " + p.HadrDisRecUser);
                saltParamsContent.Add("DrAdminPass: " + p.HadrDisRecPass);
            }
            //Create file
            CreateFile(evidencePath, fileName, saltParamsContent.ToArray());
            //End to add data to content
        }

        public static void RenamePreviousProcess(Process p)
        {
            string preFile, discardPath, idxFilename, syspath, prePath, finalIdxPath, renamedSysPath, processSumaryPath, processSumaryDiscardPath, processSumaryFile;
            //New reading variable
            string processSumaryWritePath,processSumaryWriteDiscardPath, processSumaryWriteFile;

            prePath = GetPrePath(p.ProjectName);
            discardPath = GetAitDiscardPath(p.ProjectName);
            //Name of path where fileRead from MainWindow.OnChaged() is located
            processSumaryPath = GetProcessSumaryPath(p.ProjectName);
            //Name of path where fileWrite from MainWindow.OnChaged() is located
            processSumaryWritePath = GetProcessSumaryPath(p.ProjectName);
            //this following line is the right one but for test we will assign the same value of fileRead from MainWindow.OnChaged()
            //processSumaryDiscardPath = processSumaryWritePath.Replace(Environment.UserName.ToUpper() + "\\", Environment.UserName.ToUpper() + "_READ\\");
            //Name of path where fileRead discarted from MainWindow.OnChaged() is located
            processSumaryDiscardPath = processSumaryPath + "DISCARD\\";
            //Name of path where fileWrite discarted from MainWindow.OnChaged() is located
            //this following line is the right one but for test we will assign the same value of fileRead from MainWindow.OnChaged()
            //processSumaryWriteDiscardPath = processSumaryPath.Replace(Environment.UserName.ToUpper() + "\\", Environment.UserName.ToUpper() + "_READ\\") + "DISCARD\\";
            processSumaryWriteDiscardPath = processSumaryPath + "DISCARD\\";

            CreateFolder(discardPath);
            CreateFolder(processSumaryDiscardPath);

            //Name of fileRead from MainWindow.OnChaged()
            processSumaryFile = p.Idx + ".JSON";
            //Name of fileWrite from MainWindow.OnChaged()
            processSumaryWriteFile = p.Idx + "1.JSON";

            finalIdxPath = GetAitDonePath(p.ProjectName);
            preFile = p.Idx + ".PRE";
            idxFilename = p.Idx + ".IDX";
            syspath = GetProcessPath(p.Idx);
            renamedSysPath = syspath.Substring(0, syspath.Length - 1) + p.Idx;
            string acStatus = "\\\\10.130.19.40\\ait\\RUNTEAMCONSOLE\\AC_STATUS\\";
            string postActivities = "\\\\10.130.19.40\\ait\\SAPLIGHTORQ\\TMPUC\\TMPCONF\\" + p.Idx;
            string acStatusFilePath = acStatus + UserProfile.ItUser.ToUpper() + "_" + p.Idx + "." + p.CurrentStatus;
            string acFileContent = p.Idx + "|" + p.CurrentStatus + "|" + DateTime.Now.ToString("yyyyMMddHHmmss");

            if (File.Exists(prePath + preFile))
            {
                if (!File.Exists(discardPath + preFile))
                    File.Move(prePath + preFile, discardPath + preFile);
                else
                    File.Delete(prePath + preFile);
            }

            if (File.Exists(finalIdxPath + idxFilename))
            {
                if (!File.Exists(discardPath + idxFilename))
                    File.Move(finalIdxPath + idxFilename, discardPath + idxFilename);
                else
                    File.Delete(finalIdxPath + idxFilename);
            }

            if (Directory.Exists(syspath))
            {
                if (Directory.Exists(renamedSysPath))
                    Directory.Delete(renamedSysPath);
                Directory.Move(syspath, renamedSysPath);
            }

            if(File.Exists(acStatusFilePath))
            {
                File.Delete(acStatusFilePath);
            }

            if (Directory.Exists(postActivities))
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(postActivities);
                foreach (FileInfo file in di.GetFiles())
                    file.Delete();
                Directory.Delete(postActivities);
            }

            if (MainWindow.PVMInstance != null)
            {
                MainWindow.PVMInstance.Processes.Remove(p);

                int count = MainWindow.PVMInstance.SelectedProcesses.Count();
                MainWindow.PVMInstance.SelectedProcessesCount = count;
                if (count > 0)
                {
                    MainWindow.PVMInstance.SelectedProcessesStatus = MainWindow.PVMInstance.SelectedProcesses.First().CurrentStatus;
                }
                else
                {
                    MainWindow.PVMInstance.SelectedProcessesStatus = "";
                }
            }
            Auxiliar.SendLogRequest("Process removed from interface|" + p.Idx);

            if (File.Exists(processSumaryPath + processSumaryFile))
            {
                #region Original Code
                /*
                if (File.Exists(processSumaryDiscardPath + processSumaryFile))
                    File.Delete(processSumaryDiscardPath + processSumaryFile);
                File.Move(processSumaryPath + processSumaryFile, processSumaryDiscardPath + processSumaryFile);
                */
                #endregion

                //Check if fileRead from MainWindow.OnChaged() exists as a discarded file, if so, delete it
                if (File.Exists(processSumaryDiscardPath + processSumaryFile))
                    File.Delete(processSumaryDiscardPath + processSumaryFile);
                //Move fileRead from MainWindow.OnChaged() to discard path
                File.Move(processSumaryPath + processSumaryFile, processSumaryDiscardPath + processSumaryFile);
            }

            if (File.Exists(processSumaryWritePath + processSumaryWriteFile))
            {
                //Check if fileWrite from MainWindow.OnChaged() exists as a discarded file, if so, delete it
                if (File.Exists(processSumaryWriteDiscardPath + processSumaryWriteFile))
                    File.Delete(processSumaryWriteDiscardPath + processSumaryWriteFile);
                //Move fileWrite from MainWindow.OnChaged() to discard path
                File.Move(processSumaryWritePath + processSumaryWriteFile, processSumaryWriteDiscardPath + processSumaryWriteFile);
            }
        }

        public static bool ProcessExists(Process p)
        {
            string preFile, idxFilename, prePath, finalIdxPath;

            prePath = GetPrePath(p.ProjectName);
            finalIdxPath = GetAitDonePath(p.ProjectName);
            preFile = p.Idx + ".PRE";
            idxFilename = p.Idx + ".IDX";

            if (File.Exists(prePath + preFile) || File.Exists(finalIdxPath + idxFilename))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static void ApplyFilterToProcess(Process process)
        {
            if (MainWindow.PVMInstance.FilterList.Count == 0)
            {
                process.IsHidden = false;
            }
            else
            {
                process.IsHidden = true;
                foreach (Filter filter in MainWindow.PVMInstance.FilterList)
                {
                    if (filter.IsApplied == true)
                    {
                        switch (filter.Type)
                        {
                            case "Process Type":
                                if (process.ProjectName == filter.Variable)
                                {
                                    process.IsHidden = false;
                                }
                                break;
                            case "Customer":
                                if (process.Customer == filter.Variable)
                                {
                                    process.IsHidden = false;
                                }
                                break;
                            case "Environment":
                                if (process.Environment == filter.Variable)
                                {
                                    process.IsHidden = false;
                                }
                                break;
                            case "Status":
                                if (process.CurrentStatus == filter.Variable)
                                {
                                    process.IsHidden = false;
                                }
                                break;
                        }
                    }
                }
            }
        }

        public static void DeleteRealTimeStepLog(string stepName, string idx) {

            string  logStepName, logStepFileName;

            logStepName = stepName.Substring(3).Replace("-", "_");
  	        logStepFileName = GetEvidencePath(idx) + idx + "\\" + logStepName + ".OUT";
            Auxiliar.SendLogRequest("Trying to delete RealTimeLog|" + idx + "|" + stepName);
            RenameFile(logStepFileName, logStepFileName + GenerateTimeStamp(ConvertToEST()));
        }

        private static string GetProcessSumaryPath(string processName)
        {
            //string applicationPath = catalogPath + "AIT_" + UserProfile.ItUser + "\\" + processName + "\\PROCESSSUMARY\\";
            string applicationPath = catalogPath + "AIT_" + UserProfile.ItUser + "_READ\\" + processName + "\\PROCESSSUMARY\\";
            return applicationPath;
        }

        public static void SendStatusByEmail(Process p)
        {
            string timeStampStr, separatorStr, fileName, emailFilesPath, destStr, subjectStr, attachStr, bodyStr, title;
            
            timeStampStr = p.CurrentStatusDateTime.ToString("MM'/'dd'/'yyyy HH:mm:ss");
            title = ProcessInitConfig.Where(x=>x.ProjectName == p.ProjectName).FirstOrDefault().Title;

            separatorStr = "==============================";
            fileName = p.Idx + "_EMAILTOSEND.TXT";
            emailFilesPath = rootPath + "MAILPROCESSING\\TOPROCESS\\";

            destStr = "TOS|" + String.Join(";", p.EmailDest);
            subjectStr = "SBJ|[" + title + "] - " + p.SID + " " + p.Customer + " Step:" + p.CurrentStepName.Substring(0,3) + " Status:" + p.CurrentStatus;
            
            attachStr = "ATTACHMENT1|" ;
            Step currentStep = p.StepList.Where(x => x.Index == p.CurrentStepIndex).FirstOrDefault();
            if(currentStep != null)
            {
                foreach (string evidence in currentStep.Evidence)
                {
                    attachStr += evidence + ";";
                }
            }
            if(!String.IsNullOrEmpty(p.Attachment))
                attachStr += p.Attachment + ";";

            bodyStr = "BODY|Step: " + p.CurrentStepName + "\nStatus: " + p.CurrentStatus + "\nDate Time: " + timeStampStr + "\nMessage: " + p.Message;
            
            List<string> contentStr = new List<string>() { destStr, separatorStr , subjectStr , separatorStr , attachStr , separatorStr , bodyStr };

            Auxiliar.SendLogRequest("Status sent by email|" + p.Idx + "|step " + p.CurrentStepName + "|status " + p.CurrentStatus + "|status dateTime " + p.CurrentStatusDateTime + "|destintations " + String.Join(";", p.EmailDest));
            
            File.WriteAllLines(emailFilesPath + fileName, contentStr);
        }

        public static void WriteOSPassOnCredentialsFile(string idx, string osPass, string osUser)
        {
            string credentialsFile, oswFileName, oswContent;
            string[] credentialsContent, credentialsContentArray, oswContentArray;
            credentialsFile = Auxiliar.GetConfingPath(idx) + idx + "_CREDENTIALS.CNF";
            oswFileName = Auxiliar.GetConfingPath(idx) + idx + ".OSW";
            if (File.Exists(credentialsFile))
            {
                credentialsContent = File.ReadAllLines(credentialsFile);
                credentialsContentArray = credentialsContent[1].Split("|");
                credentialsContentArray[3] = osUser;
                credentialsContentArray[4] = osPass;
                credentialsContent[1] = String.Join("|", credentialsContentArray);
                File.WriteAllLines(credentialsFile, credentialsContent);
            }
            else
                ShowMessage(idx, "File not fond: " + credentialsFile + "\nPlease contact innovation team", "Error");

            if (File.Exists(oswFileName))
            {
                oswContent = File.ReadAllText(oswFileName);
                oswContentArray = oswContent.Split("|");
                oswContentArray[3] = osUser;
                oswContentArray[4] = osPass;
                oswContent = String.Join("|", oswContentArray);
                File.WriteAllText(oswFileName, oswContent);
            }
        }

        public static bool SubOrqIsOn(Process p)
        {
            string suborqFileName, fileContent, tempIdx = "";
            DateTime lastUpdate;
            DateTime now = ConvertToEST();

            CultureInfo provider = CultureInfo.InvariantCulture;

            suborqFileName = rootPath + "CLUSTER\\SAP_" + p.ProjectName + "_" + p.Customer + "_" + p.SID + "_" + p.ProjectName + "_" + p.User + ".LS";

            if (File.Exists(suborqFileName))
            {
                fileContent = File.ReadAllText(suborqFileName);
                if (fileContent.Length > 0)
                {
                    if (fileContent.Contains("|"))
                    {
                        tempIdx = fileContent.Split("|")[1];
                        if (tempIdx != p.Idx)
                            return false;
                        else
                        {
                            lastUpdate = Auxiliar.DateTimeFromTimeStamp(fileContent);

                            if (now <= lastUpdate.AddSeconds(10))
                                return true;
                            else
                                return false;
                        }
                    }
                    else
                    {
                        lastUpdate = Auxiliar.DateTimeFromTimeStamp(fileContent);

                        if (now <= lastUpdate.AddSeconds(10))
                            return true;
                        else
                            return false;
                    }
                }
                else
                    return false;
            } 
            else
                return false;
        }
        
        public static void createDispatcheFile(string projectName)
        {
            string dispatcherPath, dispatcherDuplicatedPath, dispatcherFileName, dispatcherOnProcessFileName, dispatcherDuplicatedFileName, fileContent;

            DateTime now = ConvertToEST();

            dispatcherPath = rootPath + "TMP\\ORQMASTER\\";
            dispatcherFileName = projectName + ".ORQ";
            dispatcherOnProcessFileName = projectName + ".ONPROCESS";
            dispatcherDuplicatedPath = dispatcherPath + "DUPLICATED\\";

            if (File.Exists(dispatcherPath + dispatcherFileName) || File.Exists(dispatcherPath + dispatcherOnProcessFileName))
            {
                fileContent = now.ToString("yyyyMMdd-HHmmssfff");
                dispatcherDuplicatedFileName = projectName + "-" + fileContent + ".DONE";
                CreateFile(dispatcherDuplicatedPath, dispatcherDuplicatedFileName, fileContent);
            }
            else
            {
                fileContent = now.ToString("yyyyMMdd-HHmmss");
                CreateFile(dispatcherPath, dispatcherFileName, fileContent);
            }
        }

        public static bool PostProcessTraking(object json, string url)
        {
            //if (UserProfile.ItUser.ToUpper() == "ITFLORR" || UserProfile.ItUser.ToUpper() == "ITVIVA" || UserProfile.ItUser.ToUpper() == "ITCANUA" || UserProfile.ItUser.ToUpper() == "ITRODRV" || UserProfile.ItUser.ToUpper() == "ITROBLB" || UserProfile.ItUser.ToUpper() == "VDC-FLORR" || UserProfile.ItUser.ToUpper() == "VDC-CANUA" || UserProfile.ItUser.ToUpper() == "VDC-RODRV" || UserProfile.ItUser.ToUpper() == "VDC-ROBEB" || UserProfile.ItUser.ToUpper() == "VDC-VIVAG")
            if (UserProfile.ItUser.ToUpper() == "ITFLORR" || UserProfile.ItUser.ToUpper() == "ITVIVA" || UserProfile.ItUser.ToUpper() == "ITCANUA" || UserProfile.ItUser.ToUpper() == "ITRODRV" || UserProfile.ItUser.ToUpper() == "ITROBLB" || UserProfile.ItUser.ToUpper() == "VDC-FLORR" || UserProfile.ItUser.ToUpper() == "VDC-CANUA" || UserProfile.ItUser.ToUpper() == "VDC-RODRV" || UserProfile.ItUser.ToUpper() == "VDC-VIVAG")
                return POSTData(json, monitorBkpUrl + url);
            else
                return POSTData(json, monitorUrl + url);
        }

        public static bool POSTData(object json, string url)
        {
            WebRequest request = WebRequest.Create(url);

            // If required by the server, set the credentials.
            // Configuration valid for local environment
            request.UseDefaultCredentials = true;
            request.PreAuthenticate = true;
            if (Auxiliar.productiveRelease == "TEST")
            {
                // Configuration valid for local environment
                request.Credentials = CredentialCache.DefaultCredentials;
            }
            else
            {
                // Configuration valid for Cloned environment
                request.Credentials = new NetworkCredential(Auxiliar.vdcUser, Auxiliar.vdcPass);
            }

            // Set the Method property of the request to POST.
            request.Method = "POST";

            // Create POST data and convert it to a byte array.
            string postData = JsonConvert.SerializeObject(json);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/json";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;

            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();

            // Get the response.
            try
            {
                WebResponse response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusCode);
                if (((HttpWebResponse)response).StatusCode == System.Net.HttpStatusCode.Created)
                    return true;
                // Get the stream containing content returned by the server.
                // The using block ensures the stream is automatically closed.
                using (dataStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    string responseFromServer = reader.ReadToEnd();
                    // Display the content.
                    Console.WriteLine(responseFromServer);
                }

                // Close the response.
                response.Close();
                return true;
            }
            catch (Exception e)
            {
                try
                {
                    WebResponse response = request.GetResponse();
                    // Display the status.
                    Console.WriteLine(((HttpWebResponse)response).StatusCode);
                    if (((HttpWebResponse)response).StatusCode == System.Net.HttpStatusCode.Created)
                        return true;
                    // Get the stream containing content returned by the server.
                    // The using block ensures the stream is automatically closed.
                    using (dataStream = response.GetResponseStream())
                    {
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.
                        Console.WriteLine(responseFromServer);
                    }

                    // Close the response.
                    response.Close();
                    return true;
                }
                catch
                {
                    try
                    {
                        WebResponse response = request.GetResponse();
                        // Display the status.
                        Console.WriteLine(((HttpWebResponse)response).StatusCode);
                        if (((HttpWebResponse)response).StatusCode == System.Net.HttpStatusCode.Created)
                            return true;
                        // Get the stream containing content returned by the server.
                        // The using block ensures the stream is automatically closed.
                        using (dataStream = response.GetResponseStream())
                        {
                            // Open the stream using a StreamReader for easy access.
                            StreamReader reader = new StreamReader(dataStream);
                            // Read the content.
                            string responseFromServer = reader.ReadToEnd();
                            // Display the content.
                            Console.WriteLine(responseFromServer);
                        }

                        // Close the response.
                        response.Close();
                        return true;
                    }
                    catch
                    {
                        return true;
                    }
                }
            }
        }

        public static void PutProcessTraking(object json, string url)
        {
            //if (UserProfile.ItUser.ToUpper() == "ITFLORR" || UserProfile.ItUser.ToUpper() == "ITVIVA" || UserProfile.ItUser.ToUpper() == "ITCANUA" || UserProfile.ItUser.ToUpper() == "ITRODRV" || UserProfile.ItUser.ToUpper() == "ITROBLB" || UserProfile.ItUser.ToUpper() == "VDC-FLORR" || UserProfile.ItUser.ToUpper() == "VDC-CANUA" || UserProfile.ItUser.ToUpper() == "VDC-RODRV" || UserProfile.ItUser.ToUpper() == "VDC-ROBEB" || UserProfile.ItUser.ToUpper() == "VDC-VIVAG")
            if (UserProfile.ItUser.ToUpper() == "ITFLORR" || UserProfile.ItUser.ToUpper() == "ITVIVA" || UserProfile.ItUser.ToUpper() == "ITCANUA" || UserProfile.ItUser.ToUpper() == "ITRODRV" || UserProfile.ItUser.ToUpper() == "ITROBLB" || UserProfile.ItUser.ToUpper() == "VDC-FLORR" || UserProfile.ItUser.ToUpper() == "VDC-CANUA" || UserProfile.ItUser.ToUpper() == "VDC-RODRV" || UserProfile.ItUser.ToUpper() == "VDC-VIVAG")
                PUTData(json, monitorBkpUrl + url);
            else
                PUTData(json, monitorUrl + url);
        }

        public static void PUTData(object json, string url)
        {
            WebRequest request = WebRequest.Create(url);

            // If required by the server, set the credentials.
            if (Auxiliar.productiveRelease == "TEST")
            {
                // Configuration valid for local environment
                request.Credentials = CredentialCache.DefaultCredentials;
            }
            else
            {
                // Configuration valid for Cloned environment
                request.Credentials = new NetworkCredential(Auxiliar.vdcUser, Auxiliar.vdcPass);
            }

            // Set the Method property of the request to POST.
            request.Method = "PUT";

            // Create POST data and convert it to a byte array.
            string postData = JsonConvert.SerializeObject(json);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/json";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;

            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();

            try
            {
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusCode);

                // Get the stream containing content returned by the server.
                // The using block ensures the stream is automatically closed.
                using (dataStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    string responseFromServer = reader.ReadToEnd();
                    // Display the content.
                    Console.WriteLine(responseFromServer);
                }

                // Close the response.
                response.Close();
            }
            catch (Exception ex) 
            {
                try
                {
                    // Get the response.
                    WebResponse response = request.GetResponse();
                    // Display the status.
                    Console.WriteLine(((HttpWebResponse)response).StatusCode);

                    // Get the stream containing content returned by the server.
                    // The using block ensures the stream is automatically closed.
                    using (dataStream = response.GetResponseStream())
                    {
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.
                        Console.WriteLine(responseFromServer);
                    }

                    // Close the response.
                    response.Close();
                }
                catch
                {
                    try
                    {
                        // Get the response.
                        WebResponse response = request.GetResponse();
                        // Display the status.
                        Console.WriteLine(((HttpWebResponse)response).StatusCode);

                        // Get the stream containing content returned by the server.
                        // The using block ensures the stream is automatically closed.
                        using (dataStream = response.GetResponseStream())
                        {
                            // Open the stream using a StreamReader for easy access.
                            StreamReader reader = new StreamReader(dataStream);
                            // Read the content.
                            string responseFromServer = reader.ReadToEnd();
                            // Display the content.
                            Console.WriteLine(responseFromServer);
                        }

                        // Close the response.
                        response.Close();
                    }
                    catch
                    {
                    }
                }
            }
        }

        public static ProcessExecution GetProcessExecution(string idx)
        {
            ProcessExecution output;

            string urlReq = "";
            //if (UserProfile.ItUser.ToUpper() == "ITFLORR" || UserProfile.ItUser.ToUpper() == "ITVIVA" || UserProfile.ItUser.ToUpper() == "ITCANUA" || UserProfile.ItUser.ToUpper() == "ITRODRV" || UserProfile.ItUser.ToUpper() == "ITROBLB" || UserProfile.ItUser.ToUpper() == "VDC-FLORR" || UserProfile.ItUser.ToUpper() == "VDC-CANUA" || UserProfile.ItUser.ToUpper() == "VDC-RODRV" || UserProfile.ItUser.ToUpper() == "VDC-ROBEB" || UserProfile.ItUser.ToUpper() == "VDC-VIVAG")
            if (UserProfile.ItUser.ToUpper() == "ITFLORR" || UserProfile.ItUser.ToUpper() == "ITVIVA" || UserProfile.ItUser.ToUpper() == "ITCANUA" || UserProfile.ItUser.ToUpper() == "ITRODRV" || UserProfile.ItUser.ToUpper() == "ITROBLB" || UserProfile.ItUser.ToUpper() == "VDC-FLORR" || UserProfile.ItUser.ToUpper() == "VDC-CANUA" || UserProfile.ItUser.ToUpper() == "VDC-RODRV" || UserProfile.ItUser.ToUpper() == "VDC-VIVAG")
                urlReq = Auxiliar.monitorBkpUrl + "ProcessExecutions/GetProcessExecutionByIDX/" + idx;
            else
                urlReq = Auxiliar.monitorUrl + "ProcessExecutions/GetProcessExecutionByIDX/" + idx;

            WebRequest requestProcesses = WebRequest.Create(urlReq);

            // If required by the server, set the credentials.
            // Configuration valid for local environment
            if (Auxiliar.productiveRelease == "TEST")
            {
                // Configuration valid for local environment
                requestProcesses.Credentials = CredentialCache.DefaultCredentials;
            }
            else
            {
                // Configuration valid for Cloned environment
                requestProcesses.Credentials = new NetworkCredential(Auxiliar.vdcUser, Auxiliar.vdcPass);
            }

            // Get the response.
            WebResponse responseProcesses = requestProcesses.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)responseProcesses).StatusDescription);

            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            using (Stream dataStream = responseProcesses.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                output = JsonConvert.DeserializeObject<ProcessExecution>(responseFromServer);
            }

            // Close the response.
            responseProcesses.Close();
            
            return output;
        }

        public static StepExecution GetStepExecution(string idx, int stepindex)
        {
            StepExecution output = null;

            string urlReq = "";
            //if (UserProfile.ItUser.ToUpper() == "ITFLORR" || UserProfile.ItUser.ToUpper() == "ITVIVA" || UserProfile.ItUser.ToUpper() == "ITCANUA" || UserProfile.ItUser.ToUpper() == "ITRODRV" || UserProfile.ItUser.ToUpper() == "ITROBLB" || UserProfile.ItUser.ToUpper() == "VDC-FLORR" || UserProfile.ItUser.ToUpper() == "VDC-CANUA" || UserProfile.ItUser.ToUpper() == "VDC-RODRV" || UserProfile.ItUser.ToUpper() == "VDC-ROBEB" || UserProfile.ItUser.ToUpper() == "VDC-VIVAG")
            if (UserProfile.ItUser.ToUpper() == "ITFLORR" || UserProfile.ItUser.ToUpper() == "ITVIVA" || UserProfile.ItUser.ToUpper() == "ITCANUA" || UserProfile.ItUser.ToUpper() == "ITRODRV" || UserProfile.ItUser.ToUpper() == "ITROBLB" || UserProfile.ItUser.ToUpper() == "VDC-FLORR" || UserProfile.ItUser.ToUpper() == "VDC-CANUA" || UserProfile.ItUser.ToUpper() == "VDC-RODRV" || UserProfile.ItUser.ToUpper() == "VDC-VIVAG")
                urlReq = Auxiliar.monitorBkpUrl + "StepExecutions/" + idx + "/" + stepindex;
            else
                urlReq = Auxiliar.monitorUrl + "StepExecutions/" + idx + "/" + stepindex;

            WebRequest requestProcesses = WebRequest.Create(urlReq);

            // If required by the server, set the credentials.
            if (Auxiliar.productiveRelease == "TEST")
            {
                // Configuration valid for local environment
                requestProcesses.Credentials = CredentialCache.DefaultCredentials;
            }
            else
            {
                // Configuration valid for Cloned environment
                requestProcesses.Credentials = new NetworkCredential(Auxiliar.vdcUser, Auxiliar.vdcPass);
            }
            try
            {
                // Get the response.
                WebResponse responseProcesses = requestProcesses.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)responseProcesses).StatusDescription);

                // Get the stream containing content returned by the server.
                // The using block ensures the stream is automatically closed.
                using (Stream dataStream = responseProcesses.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    string responseFromServer = reader.ReadToEnd();
                    // Display the content.
                    output = JsonConvert.DeserializeObject<StepExecution>(responseFromServer);
                }

                // Close the response.
                responseProcesses.Close();
            }
            catch (Exception)
            {

            }
            return output;
        }

        internal static StatusExecution GetStatusExecution(string idx, int stepindex, string status, DateTime dateTime)
        {
            StatusExecution output = null;

            string urlReq = "";
            //            if (UserProfile.ItUser.ToUpper() == "ITFLORR" || UserProfile.ItUser.ToUpper() == "ITVIVA" || UserProfile.ItUser.ToUpper() == "ITCANUA" || UserProfile.ItUser.ToUpper() == "ITRODRV" || UserProfile.ItUser.ToUpper() == "ITROBLB" || UserProfile.ItUser.ToUpper() == "VDC-FLORR" || UserProfile.ItUser.ToUpper() == "VDC-CANUA" || UserProfile.ItUser.ToUpper() == "VDC-RODRV" || UserProfile.ItUser.ToUpper() == "VDC-ROBEB" || UserProfile.ItUser.ToUpper() == "VDC-VIVAG")
            if (UserProfile.ItUser.ToUpper() == "ITFLORR" || UserProfile.ItUser.ToUpper() == "ITVIVA" || UserProfile.ItUser.ToUpper() == "ITCANUA" || UserProfile.ItUser.ToUpper() == "ITRODRV" || UserProfile.ItUser.ToUpper() == "ITROBLB" || UserProfile.ItUser.ToUpper() == "VDC-FLORR" || UserProfile.ItUser.ToUpper() == "VDC-CANUA" || UserProfile.ItUser.ToUpper() == "VDC-RODRV" || UserProfile.ItUser.ToUpper() == "VDC-VIVAG")
                urlReq = Auxiliar.monitorBkpUrl + "StatusExecutions/" + idx + "/" + stepindex + "/" + status + "/" + dateTime;
            else
                urlReq = Auxiliar.monitorUrl + "StatusExecutions/" + idx + "/" + stepindex + "/" + status + "/" + dateTime;

            WebRequest requestProcesses = WebRequest.Create(urlReq);

            // If required by the server, set the credentials.
            // Configuration valid for local environment
            if (Auxiliar.productiveRelease == "TEST")
            {
                // Configuration valid for local environment
                requestProcesses.Credentials = CredentialCache.DefaultCredentials;
            }
            else
            {
                // Configuration valid for Cloned environment
                requestProcesses.Credentials = new NetworkCredential(Auxiliar.vdcUser, Auxiliar.vdcPass);
            }
            try
            {
                // Get the response.
                WebResponse responseProcesses = requestProcesses.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)responseProcesses).StatusDescription);

                // Get the stream containing content returned by the server.
                // The using block ensures the stream is automatically closed.
                using (Stream dataStream = responseProcesses.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    string responseFromServer = reader.ReadToEnd();
                    // Display the content.
                    output = JsonConvert.DeserializeObject<StatusExecution>(responseFromServer);
                }

                // Close the response.
                responseProcesses.Close();
            }
            catch (Exception ex)
            {

            }
            return output;
        }

        public static DateTime nextRepeatDateTime(string repeatDate, string repeatTime)
        {
            DateTime output = DateTime.Today.AddMonths(12), tempDate;
            string[] dateArray = repeatDate.Split(",");
            string[] timeArray = repeatTime.Split(",");
            TimeSpan diff;

            if(repeatDate == "ALL")
            {
                for(int i=0; i<timeArray.Length; i++)
                {
                    TimeSpan time = timeSpanFromString(timeArray[i]);
                    tempDate = DateTime.Today.Add(time);
                    diff = tempDate - DateTime.Now;
                    if (diff > TimeSpan.Zero)
                    {
                        if (diff < output - DateTime.Now)
                        {
                            output = tempDate;
                        }
                    }
                    else
                    {
                        tempDate = DateTime.Today.AddDays(1).Add(time);
                        diff = tempDate - DateTime.Now;
                        if (diff < output - DateTime.Now)
                        {
                            output = tempDate;
                        }
                    }
                }
            }
            else
            {
                for (int j = 0; j < dateArray.Length; j++)
                {
                    if(DateTime.Today.DayOfWeek == dayOfWeekFromString(dateArray[j]))
                    {
                        for (int i = 0; i < timeArray.Length; i++)
                        {
                            TimeSpan time = timeSpanFromString(timeArray[i]);
                            tempDate = DateTime.Today.Add(time);
                            diff = tempDate - DateTime.Now;
                            if (diff > TimeSpan.Zero)
                            {
                                if (diff < output - DateTime.Now)
                                {
                                    output = tempDate;
                                }
                            }
                            else
                            {
                                tempDate = DateTime.Today.AddDays(7).Add(time);
                                diff = tempDate - DateTime.Now;
                                if (diff < output - DateTime.Now)
                                {
                                    output = tempDate;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < timeArray.Length; i++)
                        {
                            TimeSpan time = timeSpanFromString(timeArray[i]);
                            tempDate = GetNextWeekday(DateTime.Today, dayOfWeekFromString(dateArray[j])).Add(time);
                            diff = tempDate - DateTime.Now;
                            if (diff < output - DateTime.Now)
                            {
                                output = tempDate;
                            }
                        }
                    }
                }
            }
            return output;
        }

        static TimeSpan timeSpanFromString(string timeString)
        {
            TimeSpan output;
            if (timeString.Contains(":"))
            {
                string[] timeArray = timeString.Split(":");
                switch (timeArray.Length)
                {
                    case 2:
                        output = new TimeSpan(Int32.Parse(timeArray[0]), Int32.Parse(timeArray[1]), 0);
                        break;
                    case 3:
                        output = new TimeSpan(Int32.Parse(timeArray[0]), Int32.Parse(timeArray[1]), Int32.Parse(timeArray[2]));
                        break;
                    default:
                        output = TimeSpan.Zero;
                        break;
                }
            }
            else
            {
                output = new TimeSpan(Int32.Parse(timeString), 0, 0);
            }

            return output;
        }

        static DayOfWeek dayOfWeekFromString(string day)
        {
            DayOfWeek output;
            switch (day.ToUpper())
            {
                case "SUN":
                    output = DayOfWeek.Sunday;
                    break;
                case "MON":
                    output = DayOfWeek.Monday;
                    break;
                case "TUE":
                    output = DayOfWeek.Tuesday;
                    break;
                case "WEN":
                    output = DayOfWeek.Wednesday;
                    break;
                case "THU":
                    output = DayOfWeek.Thursday;
                    break;
                case "FRI":
                    output = DayOfWeek.Friday;
                    break;
                default:
                    output = DayOfWeek.Saturday;
                    break;
            }
            return output;
        }

        static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }
        
        public static void RenameFile(string sourceFileName, string destFileName)
        {
            bool flag = true;
            if (File.Exists(sourceFileName))
            {
                do
                {
                    try
                    {
                        File.Move(sourceFileName, destFileName);
                        flag = false;
                    }
                    catch(Exception e)
                    {
                        Auxiliar.SendLogRequest("Can't rename file|" + sourceFileName + "|Error Message: " + e.Message);
                        Thread.Sleep(3);
                    }
                } while (flag);
            }
        }

        public static void CreateFile(string path, string name, string content)
        {
            CreateFolder(path);
            try
            {
                File.WriteAllText(path + name, content);
            }catch(Exception e)
            {
                Auxiliar.SendLogRequest("File already exist|Error Message: " + e.Message);
            }
        }

        public static void CreateFile(string path, string name, string[] content)
        {
            CreateFolder(path);
            File.WriteAllLines(path + name, content);
        }

        public static string CreateFolder(string path)
        {
            int auxSize;
            string pathAux, reversedStr;
            if (path != "\\\\fitmedia" || path != "\\\\fitmedia\\AIT")
            {
                reversedStr = Reverse(path);
                if (reversedStr.IndexOf("\\") == 0)
                {
                    path = path.Substring(0, path.Length - 1);
                    reversedStr = Reverse(path);
                }

                auxSize = path.Length - reversedStr.IndexOf("\\");
                pathAux = path.Substring(0, auxSize);

                if (!Directory.Exists(path))
                {
                    CreateFolder(pathAux);
                    try
                    {
                        Directory.CreateDirectory(path);
                    }
                    catch (Exception e)
                    {
                        Auxiliar.SendLogRequest("Unable to create Directory. " + e.Message);
                        MessageBox.Show("Unable to create Directory.\n Path to create: \n" + path, "Automation Console", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            return path;
        }

        public static void MoveCompletedIdx(Process process)
        {
            string idxFileName = process.Idx + ".IDX";
            string idxFilePath = GetAitDonePath(process.ProjectName) + "\\" + idxFileName;
            string completedPath = GetCompletedPath(process.ProjectName);
            if (File.Exists(idxFilePath))
            {
                if (!Directory.Exists(completedPath))
                    Directory.CreateDirectory(completedPath);
                if (!File.Exists(completedPath + idxFileName))
                    File.Move(idxFilePath, completedPath + idxFileName);
                else
                    File.Delete(idxFilePath);
            }
        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
