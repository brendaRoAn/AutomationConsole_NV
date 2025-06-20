﻿using System.Windows;
using System.Windows.Controls;

namespace RunTeamConsole
{
    public class GridViewBehaviours
    {
        public static readonly DependencyProperty CollapseableColumnProperty =
           DependencyProperty.RegisterAttached("CollapseableColumn", typeof(bool), typeof(GridViewBehaviours),
              new UIPropertyMetadata(false, OnCollapseableColumnChanged));

        public static bool GetCollapseableColumn(DependencyObject d)
        {
            return (bool)d.GetValue(CollapseableColumnProperty);
        }

        public static void SetCollapseableColumn(DependencyObject d, bool value)
        {
            d.SetValue(CollapseableColumnProperty, value);
        }

        private static void OnCollapseableColumnChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var header = sender as GridViewColumnHeader;
            if (header == null)
                return;

            header.IsVisibleChanged += AdjustWidth;
        }

        private static void AdjustWidth(object sender, DependencyPropertyChangedEventArgs e)
        {
            var header = sender as GridViewColumnHeader;
            if (header == null)
                return;

            header.Column.Width = header.Visibility == Visibility.Collapsed ? 0 : double.NaN;
        }
    }
}
