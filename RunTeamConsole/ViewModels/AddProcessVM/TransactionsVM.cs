using RunTeamConsole.Models;
using RunTeamConsole.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using static RunTeamConsole.Models.TransactionsPackage;

namespace RunTeamConsole.ViewModels
{
    public partial class AddProcessViewModel : ObservableObject
    {
        private bool _selectAllTransactionsOnSelectedListCheckboxIsChecked, _selectAllTransactionsCheckboxIsChecked;
        private ObservableCollection<TransactionsPackage> _availableTransactionsPackages;
        private TransactionsPackage _selectedTransactionsPackage;
        private ObservableCollection<Transaction> _selectedTransactionsList;

        public RelayCommand MoveToSelectedTransactions { get; private set; }
        public RelayCommand RemoveFromSelectedTransactions { get; private set; }
        public RelayCommand SelectAllTransactionsCommand { get; private set; }
        public RelayCommand SelectAllTransactionsOnSelectedListCommand { get; private set; }

        public TransactionsPackage SelectedTransactionsPackage
        {
            get { return this._selectedTransactionsPackage; }
            set
            {
                this._selectedTransactionsPackage = value;
                this.OnPropertyChanged("SelectedTransactionsPackage");
            }
        }

        public ObservableCollection<TransactionsPackage> AvailableTransactionsPackages
        {
            get { return this._availableTransactionsPackages; }
            set
            {
                if (_availableTransactionsPackages != value)
                {
                    _availableTransactionsPackages = value;
                    this.OnPropertyChanged("AvailableTransactionsPackages");
                }
            }
        }

        public IEnumerable<Transaction> SelectedTransactions
        {
            get
            {
                if (SelectedTransactionsPackage != null)
                    return SelectedTransactionsPackage.Transactions.Where(o => o.IsSelected);
                else
                    return null;
            }
        }

        public IEnumerable<Transaction> SelectedTransactionsOnSelectedList
        {
            get { return SelectedTransactionsList.Where(o => o.IsSelectedOnSelectedList); }
        }

        public ObservableCollection<Transaction> SelectedTransactionsList
        {
            get { return this._selectedTransactionsList; }
            set
            {
                _selectedTransactionsList = value;
                this.OnPropertyChanged("SelectedTransactionsList");
            }
        }

        public void AddSelectedTransactions(object obj)
        {
            foreach (Transaction t in SelectedTransactions.ToList())
            {
                SelectedTransactionsList.Add(t);
                t.IsSelected = false;
                t.IsEnabled = false;
            }
            List<string> tcodes = new List<string>();
            foreach (Transaction t in SelectedTransactionsList)
            {
                tcodes.Add(t.TCode);
            }
            foreach (ExtraInputsSet set in ExtraInputs)
            {
                set.InputsSet[0].Value = String.Join(",", tcodes.ToArray());
            }
            SelectAllTransactionsCheckboxIsChecked = false;
            if (SelectedTransactionsList.Count > SelectedTransactionsOnSelectedList.ToList().Count)
                SelectAllTransactionsOnSelectedListCheckboxIsChecked = false;
        }
        public bool CanAddSelectedTransactions(object obj)
        {
            if (SelectedTransactionsPackage != null)
            {
                if (SelectedTransactions.ToList().Count > 0)
                    return true;
                else
                    return false;
            }
            else return false;
        }
        public void RemoveSelectedTransactions(object obj)
        {
            foreach (Transaction t in SelectedTransactionsOnSelectedList.ToList())
            {
                SelectedTransactionsList.Remove(t);
                t.IsSelectedOnSelectedList = false;
                t.IsEnabled = true;
            }
            List<string> tcodes = new List<string>();
            foreach (Transaction t in SelectedTransactionsList)
            {
                tcodes.Add(t.TCode);
            }
            foreach (ExtraInputsSet set in ExtraInputs)
            {
                set.InputsSet[0].Value = String.Join(",", tcodes.ToArray());
            }
            if (SelectedTransactionsList.Count > SelectedTransactionsOnSelectedList.ToList().Count)
                SelectAllTransactionsOnSelectedListCheckboxIsChecked = false;
        }
        public bool CanRemoveSelectedTransactions(object obj)
        {
            if (SelectedTransactionsOnSelectedList.ToList().Count > 0)
                return true;
            else
                return false;
        }

        public bool SelectAllTransactionsCheckboxIsChecked
        {
            get { return this._selectAllTransactionsCheckboxIsChecked; }
            set
            {
                this._selectAllTransactionsCheckboxIsChecked = value;
                this.OnPropertyChanged("SelectAllTransactionsCheckboxIsChecked");
            }
        }
        public void ChangeSelectAllTransactions(object obj)
        {
            CheckBox checkbok = (obj as CheckBox);
            foreach (Transaction trns in SelectedTransactionsPackage.Transactions)
            {
                if (trns.IsEnabled)
                {
                    if (checkbok.IsChecked.HasValue && checkbok.IsChecked.Value)
                    {
                        trns.IsSelected = true;
                    }
                    else
                    {
                        trns.IsSelected = false;
                    }
                }
            }
        }

        public bool CanChangeSelectAllTransactions(object obj)
        {
            return true;
        }

        public bool SelectAllTransactionsOnSelectedListCheckboxIsChecked
        {
            get { return this._selectAllTransactionsOnSelectedListCheckboxIsChecked; }
            set
            {
                this._selectAllTransactionsOnSelectedListCheckboxIsChecked = value;
                this.OnPropertyChanged("SelectAllTransactionsOnSelectedListCheckboxIsChecked");
            }
        }
        public void ChangeSelectAllTransactionsOnSelectedList(object obj)
        {
            CheckBox checkbok = (obj as CheckBox);
            foreach (Transaction trns in SelectedTransactionsList)
            {
                if (checkbok.IsChecked.HasValue && checkbok.IsChecked.Value)
                {
                    trns.IsSelectedOnSelectedList = true;
                }
                else
                {
                    trns.IsSelectedOnSelectedList = false;
                }
            }
        }

        public bool CanChangeSelectAllTransactionsOnSelectedList(object obj)
        {
            if (SelectedTransactionsList.Count > 0)
                return true;
            else
                return false;
        }
        
    }
}