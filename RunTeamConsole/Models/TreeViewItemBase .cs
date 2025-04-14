using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RunTeamConsole.Models
{
    public class TreeViewItemBase : ObservableObject
	{
		private bool isSelected;
		public bool IsSelected
		{
			get { return this.isSelected; }
			set
			{
				if (value != this.isSelected)
				{
					this.isSelected = value;
					OnPropertyChanged("IsSelected");
				}
			}
		}

		private bool isExpanded;
		public bool IsExpanded
		{
			get { return this.isExpanded; }
			set
			{
				if (value != this.isExpanded)
				{
					this.isExpanded = value;
					OnPropertyChanged("IsExpanded");
				}
			}
		}

    }
}

