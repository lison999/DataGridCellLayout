using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tongyu.Smart.Client.Controls;
using Tongyu.Smart.Client.Views.Setting;
using Tongyu.Smart.Models;

namespace DataGridCellLayout
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public ViewModel viewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            ObservableCollection<Order> items = new ObservableCollection<Order>();
            items.Add(new Order() { OrderId = Guid.NewGuid().ToString(), ClientId = Guid.NewGuid().ToString() }) ;
            items.Add(new Order() { OrderId = Guid.NewGuid().ToString(), ClientId = Guid.NewGuid().ToString() });
            items.Add(new Order() { OrderId = Guid.NewGuid().ToString(), ClientId = Guid.NewGuid().ToString() });
            items.Add(new Order() { OrderId = Guid.NewGuid().ToString(), ClientId = Guid.NewGuid().ToString() });
            items.Add(new Order() { OrderId = Guid.NewGuid().ToString(), ClientId = Guid.NewGuid().ToString() });
            items.Add(new Order() { OrderId = Guid.NewGuid().ToString(), ClientId = Guid.NewGuid().ToString() });
            items.Add(new Order() { OrderId = Guid.NewGuid().ToString(), ClientId = Guid.NewGuid().ToString() });

            viewModel = new ViewModel();
            viewModel.Orders = items;
            DataContext = viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new SettingDataGridColumn().Show();
        }
    }

    public class ViewModel : BindableBase
    {
        public MainWindow _CurrentWindow { get; set; }


        private ObservableCollection<Order> orders;

        public ObservableCollection<Order> Orders
        {
            get { return orders; }
            set { orders = value; RaisePropertyChanged(nameof(Orders)); }
        }

        private List<ColumnPropertyConfig> columnPropertyConfigs = new List<ColumnPropertyConfig>();

        private IEnumerable<UseContentControl> _UseCellControls;

        public IEnumerable<UseContentControl> UseCellControls
        {
            get { return _UseCellControls; }
            set { _UseCellControls = value; RaisePropertyChanged(nameof(UseCellControls)); }
        }

        private ICommand _ParentDataGridLoadedCommand;

        public ICommand ParentDataGridLoadedCommand
        {
            get
            {
                if (_ParentDataGridLoadedCommand == null)
                {
                    _ParentDataGridLoadedCommand = new DelegateCommand<MainWindow>(ParentDataGridLoadedCommandHandle);
                }
                return _ParentDataGridLoadedCommand;
            }
        }
        #region SettingColumnsHandle

        public ViewModel()
        {
            SubscribeMessage();
        }

        #region Messenger
        private void SubscribeMessage()
        {
            Messenger.Instance.GetEvent<SettingColumnsMessageEvent>().Subscribe((x) =>
            {
                PlaySettingColumnsHelper.Instance.Init();
                columnPropertyConfigs = PlaySettingColumnsHelper.Instance.ColumnConfigs.ToList();
                SettingCellLayout();

            });
        }
        #endregion

        private void ParentDataGridLoadedCommandHandle(MainWindow window)
        {
            _CurrentWindow = window;
            //通用设置：宽要设置一整列
            SettingCellLayout();
        }

        private void SettingCellLayout()
        {
            //通用设置：宽要设置一整列
            foreach (var item in _CurrentWindow._parentOrders.Columns)
            {
                var cellContent = item as DataGridTemplateColumn;
                var dataTempalte = cellContent?.CellTemplate as DataTemplate;
                var useControl = (dataTempalte?.LoadContent() as UseContentControl);
                var pathName = useControl?.DisplayMemberPath;
                var model = columnPropertyConfigs.FirstOrDefault(f => f.ColumnName.Equals(pathName));
                if (model == null || !model.IsUserSetting) continue;
                item.Width = model.Width;
                item.Visibility = model.Display ? Visibility.Visible : Visibility.Collapsed;
            }

            if (UseCellControls == null) return;
            foreach (var item in UseCellControls)
            {
                //设置每一个单元格
                SettingOneCellLayout(item);
            }
        }

        private void SettingOneCellLayout(UseContentControl item)
        {
            string bindName = item.DisplayMemberPath;
            var model = columnPropertyConfigs.FirstOrDefault(f => f.ColumnName.Equals(bindName));
            var dataGridCell = item.FindParent<DataGridCell>();
            if (model == null || dataGridCell == null || !model.IsUserSetting || !model.Display) return;
            //--------------------------通用设置-Start----------------------------------
            //Width单独拿出来设置
            //item.Width = model.Width;

            //字体大小、粗体设置
            if (model.FontSize > 0)
            {
                dataGridCell.FontSize = model.FontSize;
            }
            if (model.FontBold)
            {
                dataGridCell.FontWeight = FontWeights.Bold;
            }

            //字体颜色、背景颜色设置
            if (!string.IsNullOrEmpty(model.FontColor) && model.FontColor != Colors.Transparent.ToString())
            {
                dataGridCell.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(model.FontColor));
            }
            if (!string.IsNullOrEmpty(model.BackColor) && model.BackColor != Colors.Transparent.ToString())
            {
                dataGridCell.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(model.BackColor));
            }

            //字体上下、左右对齐方式设置
            item.HorizontalAlignment = model.Horizontal;
            item.VerticalAlignment = model.Vertical;
            //---------------------------通用设置-End----------------------------------------- 
        }
        #endregion
    }

    public class Order
    {
        public string OrderId { get; set; }
        public string ClientId { get; set; }
    }
}
