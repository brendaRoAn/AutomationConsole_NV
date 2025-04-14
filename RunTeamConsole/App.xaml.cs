using RunTeamConsole.Models;
using System;
using System.Reflection;
using System.Windows;
using System.IO;
using File = System.IO.File;
using RunTeamConsole.Views;
using RunTeamConsole.ViewModels.Commands;
using RunTeamConsole.Views.Windows;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Linq;
using WpfTutorialSamples.Commands;
using System.Configuration;

namespace RunTeamConsole
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string _title;
        public string _appVersion;
        private string _itUser;
        private string _userText;

        public string ItUSer
        {
            get { return this._itUser; }
            set
            {
                this._itUser = value;
            }
        }

        public string UserText
        {
            get { return this._userText; }
            set
            {
                this._userText = value;
            }
        }
        //INICIO CODIGO GENERADO POR BRENDA PARA TEST
        public static string InputBox(string Prompt, string Title = "", string DefaultResponse = "", int XPos = -1, int YPos = -1)
        { 
            return DefaultResponse;
        }

        public RelayCommand RunAutomationConsoleCommand { get; private set; }

        UsingCommandsSample requestUserWindow;
        string envuser;
        //FIN CODIGO GENERADO POR BRENDA PARA TEST

        //Program Start

        protected override void OnStartup(StartupEventArgs e)
        {            
            MessageBoxResult result;
            
            //INICIA CÓDIGO QUE FUNCIONA
            /*
            Console.WriteLine();
            string[] arguments = Environment.GetCommandLineArgs();
            Console.WriteLine("GetCommandLineArgs: {0}", string.Join(", ", arguments));
            UserProfile.ItUser = arguments[1];
            */
            //FIN DEL CÓDIGO QUE FUNCIONA
            UserProfile.ItUser = "VDC-ROBEB";

            //INICIA CÓDIGO A PROBAR DEL SPLASHSCREEN
            var splashScreen = new SplashScreen("img/Screen/NewSplash_OLD.png");
            splashScreen.Show(false);
            //FINALIZA CÓDIGO A PROBAR DEL SPLASHSCREEN

            {
                //envuser = ItUSer.ToString().ToUpper();
                //string envuser = Environment.UserName.ToUpper();
                //string envuser = "VDC-ROBEB";

                string filePath = "\\\\10.130.19.40\\ait\\RUNTEAMCONSOLE\\AIT_" + UserProfile.ItUser + "\\";
                string fileName = "\\AC_" + UserProfile.ItUser + ".ini";
                Auxiliar.shareaccess();

                // Check if the file exists to check if the user has the program already opened
                if (!(File.Exists(filePath + fileName)))
                {
                    string watcherPath = "\\\\10.130.19.40\\ait\\TMP\\ORQMASTER\\";
                    string watcherOPFileName = UserProfile.ItUser + ".WATCHERONPROCESS";

                    string acStatus = "\\\\10.130.19.40\\ait\\RUNTEAMCONSOLE\\AC_STATUS\\";
                    var dir = new DirectoryInfo(acStatus);
                    int countProcessingFiles = 0;

                    var statusFileName = UserProfile.ItUser + "_*";
                    foreach (var file in dir.EnumerateFiles(statusFileName))
                        file.Delete();

                    string dateTimeNow = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string toProcessFile = "ProcessList.ToProcess";
                    string processingFile = "ProcessList.Processing";
                    string[] monitorFiles;
                    monitorFiles = Directory.GetFiles(filePath, "*.MONITOR");

                    File.WriteAllText(filePath + fileName, dateTimeNow);

                    //Delete of process files to be able to open with no trouble
                    if (File.Exists(filePath + toProcessFile))
                        File.Delete(filePath + toProcessFile);
                    if (File.Exists(filePath + processingFile))
                        File.Move(filePath + processingFile, filePath + toProcessFile);
                    if (monitorFiles.Length > 0)
                    {
                        foreach (string monitorFile in monitorFiles)
                            File.Delete(monitorFile);
                    }

                    if (File.Exists(watcherPath + watcherOPFileName))
                    {
                        string watcherDupPath = watcherPath + "\\DUPLICATED\\";
                        string watcherDupFileName = UserProfile.ItUser + ".WATCHERONPROCESS" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        File.WriteAllText(watcherDupPath + watcherDupFileName, DateTime.Now.ToString("yyyyMMddHHmmss"));
                    }
                    else
                    {
                        string watcherFileName = UserProfile.ItUser + ".WATCHER";
                        File.WriteAllText(watcherPath + watcherFileName, DateTime.Now.ToString("yyyyMMddHHmmss"));
                    }

                    //Cambio Temporal
                    //UserProfile.ItUser = envuser;
                    //UserProfile.ItUser = "VDC-ROBEB";
                    UserProfile.Host = Environment.MachineName;
                    UserProfile.Domain = Environment.UserDomainName;

                    string tempDepartment = ""; UserProfile.Department = tempDepartment;

                    _title = Application.Current.FindResource("strTitle").ToString();
                    this._appVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

                    if (UserProfile.Domain.ToUpper().Contains("FITADS01") || UserProfile.Domain.ToUpper().Contains("SYNTAX") || UserProfile.Domain.ToUpper().Contains("VDC") || UserProfile.Domain.ToUpper().Contains("LAPTOP-D39AGHSE"))
                    {
                        //This code opens the loading image of Automation Console
                        var lastVersion = Auxiliar.VersionRequest();
                        if (this._appVersion == lastVersion)
                        {

                            base.OnStartup(e);

                            MainWindow window = new MainWindow();
                            window.Title = _title + " - " + lastVersion;

                            Auxiliar.SendLogRequest("Interface Open");
                            UserProfile.CachedCredentials = new Credentials(false, false, true, false, false, false);
                        }
                        else
                        {
                            Auxiliar.SendLogRequest("Interface Version dont match with last version|Current version " + this._appVersion + "|Last version " + lastVersion);
                            do
                            {
                                result = MessageBox.Show("This is not the last version of the tool!" + Environment.NewLine + "Please get the last version provided for Innovation Team", _title, MessageBoxButton.OK, MessageBoxImage.Error);
                            } while (result != MessageBoxResult.OK);

                            Environment.Exit(0);
                        }
                    }
                    else
                    {
                        Auxiliar.SendLogRequest("NOT AUTHORIZED, NO LEGAL COPY!|" + UserProfile.ItUser + "|" + UserProfile.Host + "|" + UserProfile.Domain);
                        do
                        {
                            result = MessageBox.Show("NOT AUTHORIZED, NO LEGAL COPY!", _title, MessageBoxButton.OK, MessageBoxImage.Error);
                        } while (result != MessageBoxResult.OK);

                        Environment.Exit(0);
                    }

                    foreach (var file in dir.EnumerateFiles(statusFileName))
                    {
                        if (file.Extension == ".DONE")
                            countProcessingFiles++;
                    }
                    if (countProcessingFiles > 0)
                    {
                        Auxiliar.SendLogRequest("You have " + countProcessingFiles + " processes running, select the step where you left off and click continue to retake them.");
                        do
                        {
                            result = MessageBox.Show("You have " + countProcessingFiles + " processes running, select the step where you left off and click continue to retake them.", _title, MessageBoxButton.OK, MessageBoxImage.Error);
                        } while (result != MessageBoxResult.OK);
                    }
                }
                else
                {

                    do
                    {
                        string user = UserProfile.ItUser;
                        string text = "Interface cannot be open, it is already open for user " + user;
                        result = MessageBox.Show(text, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                    } while (result != MessageBoxResult.OK);

                    Environment.Exit(0);
                }
            }
            /*else
            {
                Environment.Exit(0);
            }*/
            splashScreen.Close(TimeSpan.FromSeconds(1));
        }


        /*public void RequestUserAndPasswordToOpen()
        {
            requestUserWindow = new ituserWindow_TEST();
            requestUserWindow.DataContext = this;
            requestUserWindow.ShowDialog();
        }*/

        public void OkClickPopup(object obj)
        {
            MessagePopup messagePopup = obj as MessagePopup;
            messagePopup.allowClosing = true;
            messagePopup.Close();
        }
        public bool AlwwaysCanExecute(object obj) { return true; }
    }
}