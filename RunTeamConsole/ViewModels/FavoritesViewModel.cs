using MassiveTestHelper.Models;
using Newtonsoft.Json;
using RunTeamConsole.Models;
using RunTeamConsole.ViewModels.Commands;
using RunTeamConsole.Views;
using RunTeamConsole.Views.Favorites;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace RunTeamConsole.ViewModels
{
    public class FavoritesViewModel : ObservableObject
    {
        private UserControl _currentControl;
        //private ObservableCollection<ExtraInputsSet> _summaryinptutsextra;
        public RelayCommand ChangePrincipalViewCommand { get; private set; }
        public RelayCommand ChangeAddProcessViewCommand { get; private set; }

        public RelayCommand PreScheduleFavoriteProfileCommand { get; private set; }
        public RelayCommand PreScheduleFavoriteSummaryProfileCommand { get; private set; }
        public RelayCommand BackToListViewCommand { get; private set; }
        public RelayCommand ScheduleFavoriteProfileCommand { get; private set; }
        public RelayCommand EditFavoriteProfileCommand { get; private set; }
        public RelayCommand ShowShareFavoriteWindowCommand { get; private set; }
        public RelayCommand ShareFavoriteProfileCommand { get; private set; }
        public RelayCommand DeleteFavoriteProfileCommand { get; private set; }

        private ObservableCollection<Favorites> _favoriteProfileList;
        private Favorites _favoriteSelected;

        public string CurrentUser { get; private set; }
        public string ShareItUser { get; set; }

        public FavoritesViewModel()
        {
            _favoriteProfileList = new ObservableCollection<Favorites>(DataAccess.GetFavorites());

            ChangePrincipalViewCommand = new RelayCommand(ChangeToPrincipalVM, AlwwaysCanExecute);
            ChangeAddProcessViewCommand = new RelayCommand(ChangeToAddProcessVM, AlwwaysCanExecute);

            PreScheduleFavoriteProfileCommand = new RelayCommand(ShowFavoritesSummary, CanShowFavoritesSummary);
            //PreScheduleFavoriteSummaryProfileCommand = new RelayCommand(ShowFavoritesSummaryRefresh, CanShowFavoritesSummaryRefresh);
            BackToListViewCommand = new RelayCommand(ShowFavoritesList, CanShowFavoritesList);
            ScheduleFavoriteProfileCommand = new RelayCommand(ScheduleProcesses, CanScheduleProcesses);
            EditFavoriteProfileCommand = new RelayCommand(EditFavoriteProfile, CanEditProfile);
            ShowShareFavoriteWindowCommand = new RelayCommand(ShowShareFavoriteWindow, FavoritesIsSelected);
            ShareFavoriteProfileCommand = new RelayCommand(ShareFavoriteProfile, CanShareFavorite);
            DeleteFavoriteProfileCommand = new RelayCommand(DeleteFavoriteProfile, FavoritesIsSelected);

            _currentControl = new FavoritesList();
            CurrentUser = UserProfile.ItUser;
        }

        public ObservableCollection<Favorites> FavoriteProfileList
        {
            get { return this._favoriteProfileList; }
            set
            {
                this._favoriteProfileList = value;
                this.OnPropertyChanged("FavoriteProfileList");
            }
        }

        public Favorites FavoriteSelected
        {
            get { return this._favoriteSelected; }
            set
            {
                this._favoriteSelected = value;
                this.OnPropertyChanged("FavoriteSelected");
                if (this._favoriteSelected != null)
                {
                    if (this._favoriteSelected.SummarySelectedProcesses == null)
                    {
                        this._favoriteSelected.SummarySelectedProcesses = this._favoriteSelected.Processes.First();
                    }
                }
            }
        }

        public UserControl CurrentControl
        {
            get { return this._currentControl; }
            set
            {
                this._currentControl = value;
                this.OnPropertyChanged("CurrentControl");
            }
        }
        /*
        public ObservableCollection<ExtraInputsSet> SummaryExtraIputSet
        {
            get { return this._summaryinptutsextra; }
            set
            {
                this._summaryinptutsextra = value;
                this.OnPropertyChanged("SummaryExtraIputSet");
            }
        }
        */
        public void ChangeToPrincipalVM(object obj)
        {
            CurrentControl = new FavoritesList();
            FavoriteSelected = null;
            MainWindow myWindow = Application.Current.MainWindow as MainWindow;
            myWindow.SetPrincipalDataContext();
        }
        public void ChangeToAddProcessVM(object obj)
        {
            CurrentControl = new FavoritesList();
            FavoriteSelected = null;
            MainWindow myWindow = Application.Current.MainWindow as MainWindow;
            myWindow.SetAddProcessDataContext();
        }

        public void ShowFavoritesSummary(object obj)
        {
            if (this._favoriteSelected.Processes[0].ProjectName.ToUpper().Contains("SYSTEMCOPY"))
            {
                CurrentControl = new FavoritesSummaryRefresh();
            }
            else
            {
                CurrentControl = new FavoritesSummary();
            }
        }

        public bool CanShowFavoritesSummary(object obj)
        {
            if (FavoriteSelected == null || (CurrentControl is FavoritesSummary) || (CurrentControl is FavoritesSummaryRefresh))
                return false;
            else
                return true;
        }

        public void ShowFavoritesList(object obj)
        {
            CurrentControl = new FavoritesList();
        }

        public bool CanShowFavoritesList(object obj)
        {
            if (CurrentControl is FavoritesSummary || CurrentControl is FavoritesSummaryRefresh)
                return true;
            else
                return false;
        }

        public void ScheduleProcesses(object obj)
        {
            string creationTime, sid, customer, pName, ituser, idx, pas;

            ituser = UserProfile.ItUser;

            FavoriteSelected.LastExecution = DateTime.Now;
            string jsonString = JsonConvert.SerializeObject(FavoriteSelected);
            string userProyectPath = Auxiliar.catalogPath + "AIT_" + UserProfile.ItUser + "\\" + FavoriteSelected.Processes.First().ProjectName + "\\FAVORITES\\";
            string processFileName = FavoriteSelected.Name + ".JSON";
            Auxiliar.CreateFile(userProyectPath, processFileName, jsonString);

            foreach (Process p in FavoriteSelected.Processes)
            {
                sid = p.SID;
                pas = p.PAS;
                customer = p.Customer;
                pName = p.ProjectName;
                creationTime = Auxiliar.GenerateTimeStamp(Auxiliar.ConvertToEST());
                p.TimeStamp = creationTime;

                if (!p.ApplReq)
                    idx = creationTime + "_" + sid + pas.Substring(pas.Length - 3).ToUpper() + '@' + customer + "_" + pName + "_" + ituser;
                else
                {
                    if (sid.Length > 3)
                    {
                        idx = creationTime + "_" + sid + '@' + customer + "_" + pName + "_" + ituser;
                    }
                    else
                    {
                        idx = creationTime + "_" + sid + customer + "_" + pName + "_" + ituser;
                    }
                }
                p.Idx = idx;
                p.User = ituser;
                Auxiliar.CreateNewProcessFiles(p, FavoriteSelected.SaltConnectivity);
            }

            CurrentControl = new FavoritesList(); 
            /*
             * Show confirmation message
             */
        }
        public bool CanScheduleProcesses(object obj)
        {
            if (FavoriteSelected != null && (CurrentControl is FavoritesSummary || CurrentControl is FavoritesSummaryRefresh))
                return true;
            else
                return false;
        }

        public void EditFavoriteProfile(object obj)
        {

        }
        public bool CanEditProfile(object obj)
        {
            /*if (FavoriteSelected != null)
                return true;
            else*/
                return false;
        }

        public void ShowShareFavoriteWindow(object obj)
        {
            ShareProcess shareWindow = new ShareProcess();
            shareWindow.Owner = Application.Current.MainWindow;
            shareWindow.DataContext = this;
            shareWindow.ShowDialog();
        }

        public void ShareFavoriteProfile(object obj)
        {
            ShareProcess shareWindow = (obj as ShareProcess);

            string jsonString = JsonConvert.SerializeObject(FavoriteSelected);
            string userProyectPath = Auxiliar.catalogPath + "AIT_" + ShareItUser + "\\" + FavoriteSelected.Processes.First().ProjectName + "\\FAVORITES\\";
            string processFileName = FavoriteSelected.Name + ".JSON";
            Auxiliar.CreateFile(userProyectPath, processFileName, jsonString);

            shareWindow.Close();
            ShareItUser = "";

            CurrentControl = new FavoritesList();
            /*
             * Show confirmation message
             */
        }

        public bool CanShareFavorite(object obj)
        {
            if(ShareItUser != null) {
                if(ShareItUser.Length > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
                }
        }

        public void DeleteFavoriteProfile(object obj)
        {
            MessageBoxResult result;
            result = MessageBox.Show("Are you shure you want to remove the configuration called " + FavoriteSelected.Name + " ?", Auxiliar.appTitle, MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
            if(result == MessageBoxResult.OK)
            {
                DataAccess.DiscardFav(FavoriteSelected);
                FavoriteProfileList.Remove(FavoriteSelected);
                CurrentControl = new FavoritesList();
            }
        }
        public bool FavoritesIsSelected(object obj)
        {
            if (FavoriteSelected != null)
                return true;
            else
                return false;
        }

        public bool AlwwaysCanExecute(object obj) { return true; }
    }
}
