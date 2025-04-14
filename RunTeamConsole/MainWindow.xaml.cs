using RunTeamConsole.Models;
using RunTeamConsole.ViewModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows;
//using System.Timers;
using System.Net.NetworkInformation;
using System.Threading;
using RunTeamConsole.Views;

namespace RunTeamConsole
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static PrincipalViewModel _principalVMInstance;
        public static PrincipalViewModel PVMInstance { get { return _principalVMInstance; } }
        private static AddProcessViewModel _addPVMInstance;
        ituserWindow confirmationWindow;

        public static AddProcessViewModel AddPVMInstance {
            get { return _addPVMInstance; } 
            set { _addPVMInstance = value; } 
        }
        private static FavoritesViewModel _favoritesInstance;
        public static FavoritesViewModel FVMInstance
        {
            get { return _favoritesInstance; }
        }
        FileSystemWatcher watcher;

        //New Code
        private static ActionBlock<string> _processingBlock;
        private static ConcurrentDictionary<string, int> _processedFiles;
        private static ConcurrentDictionary<string, Timer> _fileTimers = new ConcurrentDictionary<string, Timer>();

        public static int filesCount = 0;
        public static int timer = 0;

        #region Wait for DataAccess Async setup
        /*public async void AsyncDataAccessVariables()
        {
            #region original code
            /*
            var splashScreen = new SplashScreen("img/Screen/NewSplash.png");
            splashScreen.Show(false);

            await Task.Run(async () =>
            {

                while (!DataAccess.AsyncDataReady || !_principalVMInstance.AyncSetupCompleted)
                {
                    await Task.Delay(500);
                }
            });

            _favoritesInstance = new FavoritesViewModel();

            Debug.WriteLine("\n\n-- MainWindow Loaded Favorites --\n\n");

            DataContext = PVMInstance;

            if (!Directory.Exists(Auxiliar.catalogPath + "AIT_" + UserProfile.ItUser))
                Directory.CreateDirectory(Auxiliar.catalogPath + "AIT_" + UserProfile.ItUser);

            // Create a new FileSystemWatcher and set its properties.
            watcher = new FileSystemWatcher(Auxiliar.catalogPath + "AIT_" + UserProfile.ItUser, "*_" + UserProfile.ItUser + ".json");
            
            // Watch for changes in LastAccess and LastWrite times, and
            // the renaming of files or directories.
            watcher.NotifyFilter = NotifyFilters.Attributes
                                     | NotifyFilters.CreationTime
                                     | NotifyFilters.DirectoryName
                                     | NotifyFilters.FileName
                                     | NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.Security
                                     | NotifyFilters.Size;

            watcher.IncludeSubdirectories = true;

            // Add event handlers.
            watcher.Changed += OnChanged;

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            splashScreen.Close(TimeSpan.FromSeconds(1));

            this.Show();
            
        #endregion

            #region new code
            //Start code To ask user on opening the AC
            //confirmationWindow = new ituserWindow();
            //confirmationWindow.DataContext = this;
            //confirmationWindow.ShowDialog();
            //Start code To ask user on opening the AC
            /*var splashScreen = new SplashScreen("img/Screen/NewSplash_OLD.png");
            splashScreen.Show(false);

            await Task.Run(async () =>
            {

                while (!DataAccess.AsyncDataReady || !_principalVMInstance.AyncSetupCompleted)
                {
                    await Task.Delay(500);
                }
            });

            _favoritesInstance = new FavoritesViewModel();

            Debug.WriteLine("\n\n-- MainWindow Loaded Favorites --\n\n");

            DataContext = PVMInstance;

            if (!Directory.Exists(Auxiliar.catalogPath + "AIT_" + UserProfile.ItUser))
                Directory.CreateDirectory(Auxiliar.catalogPath + "AIT_" + UserProfile.ItUser);
            //if (!Directory.Exists(Auxiliar.catalogPath + "AIT_" + UserProfile.ItUser + "_INTER"))
                //Directory.CreateDirectory(Auxiliar.catalogPath + "AIT_" + UserProfile.ItUser + "_INTER");
            //set processed files
            _processedFiles = new ConcurrentDictionary<string, int>();
            _fileTimers = new ConcurrentDictionary<string, Timer>();

            _processingBlock = new ActionBlock<string>(
                async filePath => await MoveFileAsync(filePath),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount,
                    BoundedCapacity = 100
                });

            // Create a new FileSystemWatcher and set its properties.
            //watcher = new FileSystemWatcher(Auxiliar.catalogPath + "AIT_" + UserProfile.ItUser + "_INTER", "*_" + UserProfile.ItUser + ".json")
            watcher = new FileSystemWatcher(Auxiliar.catalogPath + "AIT_" + UserProfile.ItUser, "*_" + UserProfile.ItUser + ".json")
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size,
                IncludeSubdirectories = true,
                EnableRaisingEvents = true
            };

            // Add event handlers.
            watcher.Created += OnChanged;
            watcher.Changed += OnChanged;
            watcher.Renamed += OnChanged;

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            //splashScreen.Close(TimeSpan.FromSeconds(1));

            this.Show();
            #endregion
        }*/
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            _principalVMInstance = new PrincipalViewModel();
            //AsyncDataAccessVariables();
            
            //TEST CODE STARTS HERE
            //var splashScreen = new SplashScreen("img/Screen/NewSplash_OLD.png");
            //splashScreen.Show(false);

            _favoritesInstance = new FavoritesViewModel();
            DataContext = PVMInstance;

            if (!Directory.Exists(Auxiliar.catalogPath + "AIT_" + UserProfile.ItUser))
                Directory.CreateDirectory(Auxiliar.catalogPath + "AIT_" + UserProfile.ItUser);
            //if (!Directory.Exists(Auxiliar.catalogPath + "AIT_" + UserProfile.ItUser + "_INTER"))
            //Directory.CreateDirectory(Auxiliar.catalogPath + "AIT_" + UserProfile.ItUser + "_INTER");
            //set processed files
            _processedFiles = new ConcurrentDictionary<string, int>();
            _fileTimers = new ConcurrentDictionary<string, Timer>();

            _processingBlock = new ActionBlock<string>(
                async filePath => await MoveFileAsync(filePath),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount,
                    BoundedCapacity = 100
                });

            // Create a new FileSystemWatcher and set its properties.
            //watcher = new FileSystemWatcher(Auxiliar.catalogPath + "AIT_" + UserProfile.ItUser + "_INTER", "*_" + UserProfile.ItUser + ".json")
            watcher = new FileSystemWatcher(Auxiliar.catalogPath + "AIT_" + UserProfile.ItUser, "*_" + UserProfile.ItUser + ".json")
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size,
                IncludeSubdirectories = true,
                EnableRaisingEvents = true
            };

            // Add event handlers.
            watcher.Created += OnChanged;
            watcher.Changed += OnChanged;
            watcher.Renamed += OnChanged;

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            //splashScreen.Close(TimeSpan.FromSeconds(1));

            this.Show();
            //TEST CODE ENDS HERE

        }

        private static void OnCreated(object source, FileSystemEventArgs e)
        {
            MessageBox.Show($"File: {e.FullPath} {e.ChangeType}");
        }

        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            #region original code
            /*
            string acStatus = "\\\\10.130.19.40\\ait\\RUNTEAMCONSOLE\\AC_STATUS\\";
            var statusFileName = Environment.UserName.ToUpper() + "_*";
            var dir = new DirectoryInfo(acStatus);
            var ext = new List<string> { ".PROCESSING", ".DONE", ".FAILED", ".WARNING" };

            string filechanged;
            filechanged = e.FullPath;
            try
            {
                _principalVMInstance.ReplaceProcessInfo(filechanged);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error trying replace process info");
            }
            */
            #endregion

            #region old new code
            /*
            //Variable to store the path to read
            string fileRead;
            //variable to store the path to write
            string fileWrite;
            //assign the path to read to variable for execution
            fileRead = e.FullPath;
            //assign the path to write to variable for execution
            fileWrite = e.FullPath.Replace(Environment.UserName.ToUpper() + "\\", Environment.UserName.ToUpper() + "_READ\\");
            var dir = new DirectoryInfo(fileRead);
            var extJson = "*.JSON";

            foreach (var file in dir.EnumerateFiles(extJson))
            {
                if (!(_processedFiles.ContainsKey(fileRead)))
                {
                    _processingBlock = new ActionBlock<string>(
                        fileRead => _processedFiles.GetOrAdd(fileRead, filesCount),
                            new ExecutionDataflowBlockOptions
                            {
                                MaxDegreeOfParallelism = Environment.ProcessorCount,
                                BoundedCapacity = 100
                            });
                }
            }
            //try to execute the process to update AC process (the old version) with JSON process (the new version path stored in fileRead)
            try
            {
                _principalVMInstance.ReplaceProcessInfo(fileRead);

                if (File.Exists(fileWrite))
                    File.Delete(fileWrite);
                File.Move(fileRead, fileWrite);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error trying replace process info");
            }
            */
            #endregion

            #region new code
            if (File.Exists(e.FullPath.ToString()))
            //if (File.Exists("\\\\10.130.19.40\\ait\\RUNTEAMCONSOLE\\AIT_TEST_VDC-ROBEB\\TESTCLOUD\\PROCESSSUMARY" + e.FullPath.Substring(71)))
            {
                // Avoid processing the same file multiple times
                if (_processedFiles.ContainsKey(e.FullPath))
                {
                    return;
                }

                // Debounce logic: reset or set a timer for the file
                
                var timer = _fileTimers.GetOrAdd(e.FullPath, path =>
                {
                    return new Timer(async state =>
                    {
                        // Enqueue the file for processing
                        if (!_processingBlock.Post(e.FullPath))
                        {
                            Console.WriteLine($"Failed to enqueue file for processing: {e.FullPath}");
                        }
                        else
                        {
                            Console.WriteLine($"File enqueued for processing: {e.FullPath}");
                        }
                        // Remove the timer after processing
                        
                        _fileTimers.TryRemove(e.FullPath, out _);
                        if((Timer)state != null)
                            ((Timer)state).Dispose();
                    }, null, Timeout.Infinite, Timeout.Infinite);
                });

                // Reset the timer to fire after 500 milliseconds of inactivity
                timer.Change(500, Timeout.Infinite);
            }
            #endregion
        }

        private async Task MoveFileAsync(string sourcePath)
        {
            //string fileNameTest = sourcePath.Substring(71);
            //sourcePath = "\\\\10.130.19.40\\ait\\RUNTEAMCONSOLE\\AIT_TEST_VDC-ROBEB\\TESTCLOUD\\PROCESSSUMARY" + fileNameTest;
            
            try
            {
                // Attempt to add the file to the processed files dictionary
                if (!_processedFiles.TryAdd(sourcePath, 0))
                { 
                    // File is already being processed
                    return;
                }

                string fileName = Path.GetFileName(sourcePath);
                string destinationPath = sourcePath.Replace(UserProfile.ItUser.ToUpper() + "\\", UserProfile.ItUser.ToUpper() + "_READ\\");
                //string destinationPath = "\\\\10.130.19.40\\ait\\RUNTEAMCONSOLE\\AIT_VDC-ROBEB_READ\\TESTCLOUD\\PROCESSSUMARY" + fileNameTest;

                // Ensure the file is accessible
                await WaitForFileAccessAsync(sourcePath);

                // Move the file, replacing if necessary
                try
                {
                    _principalVMInstance.ReplaceProcessInfo(sourcePath);
                    //archivo procesado porque ya terminó
                    if (File.Exists(destinationPath))
                        File.Delete(destinationPath);
                    File.Move(sourcePath, destinationPath);
                    _processedFiles.Remove(sourcePath, out _);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error trying replace process info");
                }

                Console.WriteLine($"File moved from {sourcePath} to {destinationPath}");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"File not found (might have been moved already): {sourcePath}");
                _processedFiles.TryRemove(sourcePath, out _);
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"IO Exception while moving file {sourcePath}: {ioEx.Message}");
                _processedFiles.TryRemove(sourcePath, out _);
            }
            catch (UnauthorizedAccessException uaEx)
            {
                Console.WriteLine($"Access Exception while moving file {sourcePath}: {uaEx.Message}");
                _processedFiles.TryRemove(sourcePath, out _);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while moving file {sourcePath}: {ex.Message}");
                _processedFiles.TryRemove(sourcePath, out _);
            }
        }

        private async Task WaitForFileAccessAsync(string filePath)
        {
            const int maxAttempts = 10;
            const int delayMilliseconds = 200;
            int attempts = 0;
            bool fileReady = false;

            while (attempts < maxAttempts && !fileReady)
            {
                try
                {
                    using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        if (stream.Length > 0)
                        {
                            fileReady = true;
                        }
                    }
                }
                catch (IOException)
                {
                    attempts++;
                    await Task.Delay(delayMilliseconds);
                }
            }

            if (!fileReady)
            {
                Console.WriteLine($"File is not accessible after multiple attempts: {filePath}");
                throw new IOException($"Cannot access the file: {filePath}");
            }
        }

        // Define the event handlers.
        private static void OnDeleted(object source, FileSystemEventArgs e) =>
            MessageBox.Show($"File: {e.FullPath} {e.ChangeType}");

        private static void OnRenamed(object source, RenamedEventArgs e) =>
            // Specify what is done when a file is renamed.
            MessageBox.Show($"File: {e.OldFullPath} renamed to {e.FullPath}");

        public void SetAddProcessDataContext()
        {
            AddPVMInstance = new AddProcessViewModel();
            DataContext = AddPVMInstance;
        }
        public void SetPrincipalDataContext()
        {
            AddPVMInstance = null;
            DataContext = PVMInstance;
        }
        public void SetFavoritesDataContext()
        {
            DataContext = FVMInstance;
        }
        private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = Auxiliar.serverURL + "Learn",
                UseShellExecute = true
            };
            
            System.Diagnostics.Process.Start(psi);
        }
        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            //string envuser = Environment.UserName.ToUpper();
            //string envuser = "VDC-ROBEB";

            string filePath = "\\\\10.130.19.40\\ait\\RUNTEAMCONSOLE\\AIT_" + UserProfile.ItUser;
            string fileName = "\\AC_" + UserProfile.ItUser + ".ini";
            string toProcessFile = "\\ProcessList.ToProcess";
            string processingFile = "\\ProcessList.Processing";
            string acStatus = "\\\\10.130.19.40\\ait\\RUNTEAMCONSOLE\\AC_STATUS\\";
            var dir = new DirectoryInfo(acStatus);
            var statusFileName = UserProfile.ItUser + "_*";

            //Delete of process files to be able to open with no trouble
            if (File.Exists(filePath + toProcessFile))
                File.Delete(filePath + toProcessFile);
            if (File.Exists(filePath + processingFile))
                File.Delete(filePath + processingFile);

            foreach (var file in dir.EnumerateFiles(statusFileName))
                file.Delete();

            //Delete of file that stops the console from opening on two or more different devices
            File.Delete(filePath + fileName);
            Auxiliar.SendLogRequest("Interface Closed");
            Environment.Exit(0);
        }
    }
}
