using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RunTeamConsole.Views.SapInstallPostSteps
{
    /// <summary>
    /// Interaction logic for SapInstallSCC4.xaml
    /// </summary>
    public partial class SapInstallSCC4 : UserControl
    {
        public SapInstallSCC4()
        {
            InitializeComponent();

            string[] currencyOptions = new[]
            {
                "EUR",
                "USD"
            };
            Scc4CurrencyCombobox.ItemsSource = currencyOptions;

            string[] clientRoleOptions = new[]
            {
                "S:SAP REFERENCE",
                "P:SAP PRODUCTION",
                "T:SAP TEST",
                "C:SAP CUSTOMIZING",
                "D:SAP DEMO",
                "E:SAP TRAINING EDUCATION"
            };
            Scc4ClientRoleCombobox.ItemsSource = clientRoleOptions;

            string[] changesAndTOptions = new[] 
            {
                "1:CHANGES WITHOUT AUTOMATIC RECORDING",
                "2:AUTOMATIC RECORDING OF CHANGES",
                "3:NO CHANGES ALLOWED",
                "4:CHANGES W/O AUTOMATIC RECORDING NO TRANSPORTS ALLOWED"
            };
            Scc4ChangesAndTransportCombobox.ItemsSource = changesAndTOptions;

            string[] crossClientOptions = new[]
            {
                "C001:CHANGES FO REPOSITORY AND CROSS CLIENTE CUSTOMIZING ALLOWED OK",
                "C002:No Changes to cross-client customizing objects",
                "C003:No changes to repository Objects",
                "C004:No Changes to repository and corss-client customizing objes"
            };
            Scc4CrossClientCombobox.ItemsSource = crossClientOptions;

            string[] clientCopyCompOptions = new[]
            {
                "CCTP1:no overwriting level 1",
                "CCTP2:Protection level 0: No Restriction",
                "CCTP3:protection level 2: No Overwriting, no external availability"
            };
            Scc4CopyComparisonToolCombobox.ItemsSource = clientCopyCompOptions;

            string[] cattAndEcattOptions = new[]
            {
                "CER1:Ecatt and catt not allowed",
                "CER2:-eCatt and catt allowed",
                "CER3:eCatt and Catt only allowed for 'Trusted RFC'",
                "CER4:ecatt allowed, bu FUN/ABAP and Catt not allowed",
                "CER5:ecatt allowed, bu FUN/ABAP and Catt not allowed"
            };
            Scc4CattAndEcattRestCombobox.ItemsSource= cattAndEcattOptions;
        }
    }
}
