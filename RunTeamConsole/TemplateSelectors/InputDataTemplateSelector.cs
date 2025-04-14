using RunTeamConsole.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace RunTeamConsole
{
    public class InputDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate
               SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null && item is ExtraInputsSet)
            {
                ExtraInputsSet eisitem = item as ExtraInputsSet;

                if (eisitem.Step == "003OSX-FILESYSTEMEXTENSION")
                    return element.FindResource("OSXFILESYSTEMEXTENSIONtemplate") as DataTemplate;
                else if (eisitem.Step == "002AL11-CREATEDIRECTORY-CIPL")
                    return element.FindResource("CREATEDIRECTORYtemplate") as DataTemplate;
                else if (eisitem.Step.Contains("HEALTHCHECK-SSAPP"))
                    return element.FindResource("TRANSACTIONStemplate") as DataTemplate;
            }

            return null;
        }
    }
}
