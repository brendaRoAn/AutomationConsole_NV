
using RunTeamConsole.Models;
using RunTeamConsole.Models.Packages;
using RunTeamConsole.ViewModels.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace RunTeamConsole.ViewModels
{
    public partial class AddProcessViewModel : ObservableObject
    {
        private bool _selectAllJavaComponentsCheckboxIsChecked;
        private bool _selectAllJavaComponentsOnSelectedListCheckboxIsChecked;
        private ObservableCollection<JavaComponent> _javaComponentsCatalog;
        private ObservableCollection<JavaComponent> _selectedJavaComponentsList;
        public RelayCommand MoveToSelectedJavaComponents { get; private set; }
        public RelayCommand RemoveFromSelectedJavaComponents { get; private set; }
        public RelayCommand SelectAllJavaComponentsCommand { get; private set; }
        public RelayCommand SelectAllJavaComponentsOnSelectedListCommand { get; private set; }

        public ObservableCollection<JavaComponent> JavaComponentsCatalog
        {
            get { return this._javaComponentsCatalog; }
            set
            {
                if (_javaComponentsCatalog != value)
                {
                    _javaComponentsCatalog = value;
                    OnPropertyChanged("JavaComponentsCatalog");
                }
            }
        }
        public IEnumerable<JavaComponent> SelectedJavaComponents
        {
            get
            {
                return JavaComponentsCatalog.Where(o => o.IsSelected);
            }
        }

        public ObservableCollection<JavaComponent> SelectedJavaComponentsList
        {
            get { return this._selectedJavaComponentsList; }
            set
            {
                _selectedJavaComponentsList = value;
                this.OnPropertyChanged("SelectedJavaComponentsList");
            }
        }

        public IEnumerable<JavaComponent> SelectedJavaComponentsOnSelectedList
        {
            get
            {
                return SelectedJavaComponentsList.Where(o => o.IsSelectedOnSelectedList);
            }
        }

        public bool SelectAllJavaComponentsCheckboxIsChecked
        {
            get { return this._selectAllJavaComponentsCheckboxIsChecked; }
            set
            {
                this._selectAllJavaComponentsCheckboxIsChecked = value;
                this.OnPropertyChanged("SelectAllJavaComponentsCheckboxIsChecked");
            }
        }

        public bool SelectAllJavaComponentsOnSelectedListCheckboxIsChecked
        {
            get { return this._selectAllJavaComponentsOnSelectedListCheckboxIsChecked; }
            set
            {
                this._selectAllJavaComponentsOnSelectedListCheckboxIsChecked = value;
                this.OnPropertyChanged("SelectAllJavaComponentsOnSelectedListCheckboxIsChecked");
            }
        }

        public void AddSelectedJavaComponents(object obj)
        {
            foreach (JavaComponent component in SelectedJavaComponents.ToList())
            {
                SelectedJavaComponentsList.Add(component);
                component.IsSelected = false;
                component.IsEnabled = false;
            }
            SelectAllJavaComponentsCheckboxIsChecked = false;
            if (SelectedJavaComponentsList.Count > SelectedJavaComponentsOnSelectedList.ToList().Count)
                SelectAllJavaComponentsOnSelectedListCheckboxIsChecked = false;
        }
        public bool CanAddSelectedJavaComponents(object obj)
        {
            if (SelectedJavaComponents.ToList().Count > 0)
                return true;
            else
                return false;
        }
        public void RemoveSelectedJavaComponents(object obj)
        {
            foreach (JavaComponent component in SelectedJavaComponentsOnSelectedList.ToList())
            {
                SelectedJavaComponentsList.Remove(component);
                component.IsSelectedOnSelectedList = false;
                component.IsEnabled = true;
            }
            if (SelectedJavaComponentsList.Count > SelectedJavaComponentsOnSelectedList.ToList().Count)
                SelectAllJavaComponentsOnSelectedListCheckboxIsChecked = false;
        }
        public bool CanRemoveSelectedJavaComponents(object obj)
        {
            if (SelectedJavaComponentsOnSelectedList.ToList().Count > 0)
                return true;
            else
                return false;
        }

        public void ChangeSelectAllJavaComponents(object obj)
        {
            CheckBox checkbok = (obj as CheckBox);
            foreach (JavaComponent component in JavaComponentsCatalog)
            {
                if (component.IsEnabled)
                {
                    if (checkbok.IsChecked.HasValue && checkbok.IsChecked.Value)
                    {
                        component.IsSelected = true;
                    }
                    else
                    {
                        component.IsSelected = false;
                    }
                }
            }
        }

        public bool CanChangeSelectAllJavaComponents(object obj)
        {
            return true;
        }

        public void ChangeSelectAllJavaComponentsOnSelectedList(object obj)
        {
            CheckBox checkbok = (obj as CheckBox);
            foreach (JavaComponent component in SelectedJavaComponentsList)
            {
                if (checkbok.IsChecked.HasValue && checkbok.IsChecked.Value)
                {
                    component.IsSelectedOnSelectedList = true;
                }
                else
                {
                    component.IsSelectedOnSelectedList = false;
                }
            }
        }

        public bool CanChangeSelectAllJavaComponentsOnSelectedList(object obj)
        {
            if (SelectedJavaComponentsList.Count > 0)
                return true;
            else
                return false;
        }
        private bool FilterServersByJavaStack(object item)
        {
            ServerSystem server = item as ServerSystem;
            if (server != null)
            {
                if (string.IsNullOrEmpty(server.Stack))
                    return false;
                else
                    return _selectedProcess.Subtype.Trim().ToUpper().Contains(server.Stack.Trim().ToUpper()) && !server.ProductType.Trim().ToUpper().Contains("WEBD") && !server.Region.Trim().ToUpper().Contains("EU");
            }
            else
                return false;
        }
    }
}
