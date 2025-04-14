using RunTeamConsole.Models;
using RunTeamConsole.Models.Packages;
using RunTeamConsole.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace RunTeamConsole.ViewModels
{
    public partial class AddProcessViewModel : ObservableObject
    {
        private bool _selectAllScriptOnSelectedListCheckboxIsChecked, _selectAllScriptCheckboxIsChecked;
        private ObservableCollection<ScriptPackage> _availableScriptPackages;
        private ScriptPackage _selectedScriptPackage;
        private ObservableCollection<Script> _selectedScriptList;

        public RelayCommand MoveToSelectedScript { get; private set; }
        public RelayCommand RemoveFromSelectedScript { get; private set; }
        public RelayCommand SelectAllScriptCommand { get; private set; }
        public RelayCommand SelectAllScriptOnSelectedListCommand { get; private set; }

        public ScriptPackage SelectedScriptPackage
        {
            get { return this._selectedScriptPackage; }
            set
            {
                this._selectedScriptPackage = value;
                this.OnPropertyChanged("SelectedScriptPackage");
            }
        }

        public ObservableCollection<ScriptPackage> AvailableScriptPackages
        {
            get { return this._availableScriptPackages; }
            set
            {
                if (_availableScriptPackages != value)
                {
                    _availableScriptPackages = value;
                    OnPropertyChanged("AvailableScriptPackages");
                }
            }
        }
        
        public IEnumerable<Script> SelectedScript
        {
            get
            {
                if (SelectedScriptPackage != null)
                    return SelectedScriptPackage.ScriptList.Where(o => o.IsSelected);
                else
                    return null;
            }
        }
        
        public IEnumerable<Script> SelectedScriptOnSelectedList
        {
            get 
            {
                return SelectedScriptList.Where(o => o.IsSelectedOnSelectedList);
            }
        }

        public ObservableCollection<Script> SelectedScriptList
        {
            get { return this._selectedScriptList; }
            set
            {
                _selectedScriptList = value;
                OnPropertyChanged("SelectedScriptList");
            }
        }
        
        public void AddSelectedScript(object obj)
        {
            foreach (Script s in SelectedScript.ToList())
            {
                SelectedScriptList.Add(s);
                s.IsSelected = false;
                s.IsEnabled = false;
            }
            
            /*SelectAllScriptCheckboxIsChecked = false;
            if (SelectedScriptList.Count > SelectedScriptOnSelectedList.ToList().Count)
                SelectAllScriptOnSelectedListCheckboxIsChecked = false;*/
        }
        public bool CanAddSelectedScript(object obj)
        {
            if (SelectedScriptPackage != null)
            {
                if (SelectedScript.ToList().Count > 0)
                    return true;
                else
                    return false;
            }
            else return false;
        }
        
        public void RemoveSelectedScript(object obj)
        {
            foreach (Script t in SelectedScriptOnSelectedList.ToList())
            {
                SelectedScriptList.Remove(t);
                t.IsSelectedOnSelectedList = false;
                t.IsEnabled = true;
            }
            
            /*if (SelectedScriptList.Count > SelectedScriptOnSelectedList.ToList().Count)
                SelectAllScriptOnSelectedListCheckboxIsChecked = false;*/
        }
        public bool CanRemoveSelectedScript(object obj)
        {
            if (SelectedScriptOnSelectedList != null)
            {
                if (SelectedScriptOnSelectedList.ToList().Count > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public bool SelectAllScriptCheckboxIsChecked
        {
            get { return this._selectAllScriptCheckboxIsChecked; }
            set
            {
                this._selectAllScriptCheckboxIsChecked = value;
                this.OnPropertyChanged("SelectAllScriptCheckboxIsChecked");
            }
        }
        
        public void ChangeSelectAllScript(object obj)
        {
            CheckBox checkbok = (obj as CheckBox);
            foreach (Script script in SelectedScriptPackage.ScriptList)
            {
                if (script.IsEnabled)
                {
                    if (checkbok.IsChecked.HasValue && checkbok.IsChecked.Value)
                    {
                        script.IsSelected = true;
                    }
                    else
                    {
                        script.IsSelected = false;
                    }
                }
            }
        }
        
        public bool CanChangeSelectAllScript(object obj)
        {
            return true;
        }

        public bool SelectAllScriptOnSelectedListCheckboxIsChecked
        {
            get { return this._selectAllScriptOnSelectedListCheckboxIsChecked; }
            set
            {
                this._selectAllScriptOnSelectedListCheckboxIsChecked = value;
                this.OnPropertyChanged("SelectAllScriptOnSelectedListCheckboxIsChecked");
            }
        }
        public void ChangeSelectAllScriptOnSelectedList(object obj)
        {
            CheckBox checkbok = (obj as CheckBox);
            foreach (Script script in SelectedScriptList)
            {
                if (checkbok.IsChecked.HasValue && checkbok.IsChecked.Value)
                {
                    script.IsSelectedOnSelectedList = true;
                }
                else
                {
                    script.IsSelectedOnSelectedList = false;
                }
            }
        }

        public bool CanChangeSelectAllScriptOnSelectedList(object obj)
        {
            if (SelectedScriptList.Count > 0)
                return true;
            else
                return false;
        }

    }
}