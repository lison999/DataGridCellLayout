using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Tongyu.Smart.Client.Controls
{
    public static class DataGridCellLayout
    {
        #region Attached
        public static IEnumerable<UseContentControl> GetUseContentControls(this DataGrid obj)
        {
            return (IEnumerable<UseContentControl>)obj.GetValue(UseContentControlsProperty);
        }

        public static void SetUseContentControls(this DataGrid obj, IEnumerable<UseContentControl> value)
        {
            obj.SetValue(UseContentControlsProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UseContentControlsProperty =
            DependencyProperty.RegisterAttached("UseContentControls", typeof(IEnumerable<UseContentControl>), typeof(DataGridCellLayout), new PropertyMetadata(null, OnUseContentControlsChanged));

        private static void OnUseContentControlsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var datagrid = d as DataGrid;
            datagrid?.GetLayoutHost();
        }

        public static DataGridCellLayoutHost GetLayoutHost(this DataGrid dataGrid)
        {
            var value = (DataGridCellLayoutHost)dataGrid.GetValue(LayoutHostProperty);
            if(value==null)
            {
                value = new DataGridCellLayoutHost(dataGrid);
                dataGrid.SetValue(LayoutHostProperty, value);
            }
            return value;
        }

        // Using a DependencyProperty as the backing store for Filter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LayoutHostProperty =
            DependencyProperty.RegisterAttached("LayoutHost", typeof(DataGridCellLayoutHost), typeof(DataGridCellLayoutHost));

        #endregion
    }
}
