using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Tongyu.Smart.Client.Controls
{
    public sealed class DataGridCellLayoutHost
    {
        #region Ctor
        public DataGridCellLayoutHost(DataGrid dataGrid)
        {
            this._dataGrid = dataGrid;
        }
        #endregion

        #region Property Private
        private readonly DataGrid _dataGrid;
        private readonly List<UseContentControl> useContentControls = new List<UseContentControl>();
        #endregion

        #region CallBackHandle
        #endregion

        #region Methods Private        
        internal void AddUseContentControl(UseContentControl useContent)
        {
            useContentControls.Add(useContent);
            _dataGrid.SetUseContentControls(useContentControls.AsEnumerable());
        }
        internal void DelUseContentControl(UseContentControl useContent)
        {
            useContentControls.Remove(useContent);
            _dataGrid.SetUseContentControls(useContentControls.AsEnumerable());
        }

        protected bool GetCellContent(object item, DataGridColumn currentColumn)
        {
            var propertyPath = currentColumn.SortMemberPath;
            //BindingOperations.SetBinding(this, _cellValueProperty, new Binding(propertyPath) { Source = item });
            //var propertyValue = GetValue(_cellValueProperty);
            //BindingOperations.ClearBinding(this, _cellValueProperty);

            return true;
        }

        private static readonly DependencyProperty _cellValueProperty =
            DependencyProperty.Register("_cellValue", typeof(object), typeof(DataGridCellLayoutHost));

        #endregion
    }

    public static class Ext
    {
        internal static T FindParent<T>(this DependencyObject dependencyObject) where T : class
        {
            while (dependencyObject != null)
            {
                var target = dependencyObject as T;
                if (target != null)
                    return target;

                dependencyObject = LogicalTreeHelper.GetParent(dependencyObject) ?? VisualTreeHelper.GetParent(dependencyObject);
            }
            return null;
        }
    }

    [XmlRoot]
    public class ColumnPropertyConfig
    {
        private bool _isUseSetting = false;
        private double _width = 0;

        [XmlElement("ColumnId")]
        public string ColumnId { get; set; } = string.Empty;

        [XmlElement("ColumnName")]
        public string ColumnName { get; set; } = string.Empty;

        [XmlElement("ColumnValue")]
        public object ColumnValue { get; set; } = null;

        [XmlElement("Width")]
        public double Width { get => _width; set => _width = value; }

        [XmlElement("FontSize")]
        public double FontSize { get; set; } = 0;
        [XmlElement("FontBold")]
        public bool FontBold { get; set; } = false;
        [XmlElement("FontColor")]
        public string FontColor { get; set; } = Colors.Transparent.ToString();
        [XmlElement("Display")]
        public bool Display { get; set; } = true;
        [XmlElement("BackColor")]
        public string BackColor { get; set; } = Colors.Transparent.ToString();
        [XmlElement("Horizontal")]
        public HorizontalAlignment Horizontal { get; set; }
        [XmlElement("Vertical")]
        public VerticalAlignment Vertical { get; set; }

        [XmlElement("IsUserSetting")]
        public bool IsUserSetting { get => _isUseSetting; set => _isUseSetting = value; }

        [XmlElement("propertyConfigs")]
        public List<ColumnPropertyConfig> PropertyConfigs { get; set; }= new List<ColumnPropertyConfig>();

        public ColumnPropertyConfig(string columnName)
        {
            this.ColumnName = columnName;
            this.ColumnId = Guid.NewGuid().ToString();
        }

        public ColumnPropertyConfig()
        {
            this.ColumnId = Guid.NewGuid().ToString();
        }        
    }
}
