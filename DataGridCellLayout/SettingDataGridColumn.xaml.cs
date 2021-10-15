using System.Windows;
using Tongyu.Smart.Client.Controls;
using Tongyu.Smart.Client.ViewModels.Setting;

namespace Tongyu.Smart.Client.Views.Setting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SettingDataGridColumn : Window
    {
        public SettingDataGridColumn()
        {
            InitializeComponent();
            this.DataContext = new SettingDataGridColumnViewModel();
        }
    }
}
