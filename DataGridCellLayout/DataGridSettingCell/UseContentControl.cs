using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Tongyu.Smart.Client.Controls
{
    public class UseContentControl : ContentControl
    {
        #region Ctor
        public UseContentControl()
        {
            this.Loaded += UseContentControl_Loaded;
            this.Unloaded += UseContentControl_Unloaded;
        }

        private void UseContentControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (LayoutHost == null)
            {
                DataGrid = this.FindParent<DataGrid>();
                // Find our host and attach ourself.
                LayoutHost = DataGrid.GetLayoutHost();
            }

            LayoutHost.AddUseContentControl(this);
        }

        private void UseContentControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (LayoutHost != null)
            {
                LayoutHost.DelUseContentControl(this);
            }
            else
            {
                DataGrid = this.FindParent<DataGrid>();
                // Find our host and attach ourself.
                LayoutHost = DataGrid.GetLayoutHost();
                LayoutHost.DelUseContentControl(this);
            }            
        }
        #endregion

        #region Property DP


        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayNumer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(UseContentControl), new PropertyMetadata(string.Empty));


        #endregion

        #region Property private
        protected DataGridCellLayoutHost LayoutHost { get; private set; }
        protected DataGrid DataGrid { get; private set; }
        #endregion
    }
}
