﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RunTeamConsole.Views
{
    public partial class Log : Window
    {
        public Log()
        {
            InitializeComponent();
            DataContext = MainWindow.PVMInstance;
        }
    }
}
