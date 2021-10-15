using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Tongyu.Smart.Client.Controls;
using Tongyu.Smart.Models;

namespace Tongyu.Smart.Client.ViewModels.Setting
{
    public class SettingDataGridColumnViewModel : BindableBase, IDisposable
    {
        #region Ctor

        public SettingDataGridColumnViewModel()
        {
            _dialogCoordinator = DialogCoordinator.Instance;

            Horizontals = EnumExtension.GetEnumList<HorizontalAlignment>();       
            verticals = EnumExtension.GetEnumList<VerticalAlignment>();          

            SettingColumns = new ObservableCollection<ColumnPropertyConfig>();

            defaultSettingColumns = new List<ColumnPropertyConfig>(PlaySettingColumnsHelper.Instance.DefaultColumnConfigs);
            SettingColumnsLoad();
            SettingColumnModel = SettingColumns?.FirstOrDefault();            
        }
      
        #endregion

        #region Property private
        private readonly IDialogCoordinator _dialogCoordinator;
        private ObservableCollection<ColumnPropertyConfig> _settingColumns;
        private ColumnPropertyConfig _settingColumnModel;
        private Color _SelecteFontColor;
        private Color _SelecteBackColor;

        private bool isWindowClose;
        private List<ColumnPropertyConfig> defaultSettingColumns;
        private EnumDescription<HorizontalAlignment> _HorizontalModel;
        private EnumDescription<VerticalAlignment> _VerticalModel;
        private ICommand _saveCommand;
        private ICommand _saveDefalutCommand;

        #endregion

        #region Property public
        public ObservableCollection<ColumnPropertyConfig> SettingColumns
        {
            get { return _settingColumns; }
            set
            {
                _settingColumns = value;
                RaisePropertyChanged(nameof(SettingColumns));
            }
        }

        public ColumnPropertyConfig SettingColumnModel
        {
            get { return _settingColumnModel; }
            set
            {
                _settingColumnModel = value;
                if (null != _settingColumnModel)
                {
                    if (!string.IsNullOrEmpty(_settingColumnModel.FontColor))
                    {
                        SelecteFontColor = (Color)ColorConverter.ConvertFromString(_settingColumnModel.FontColor);
                    }
                    else
                    {
                        SelecteFontColor = Colors.Transparent;
                    }

                    if (!string.IsNullOrEmpty(_settingColumnModel.BackColor))
                    {
                        SelectedBackColor = (Color)ColorConverter.ConvertFromString(_settingColumnModel.BackColor);
                    }
                    else
                    {
                        SelectedBackColor = Colors.Transparent;
                    }


                    HorizontalModel = Horizontals?.FirstOrDefault(F=>F.Item ==_settingColumnModel.Horizontal)??Horizontals?.FirstOrDefault();
                    VerticalModel = verticals?.FirstOrDefault(F=>F.Item ==_settingColumnModel.Vertical)?? verticals?.FirstOrDefault();
                   
                }
                RaisePropertyChanged(nameof(SettingColumnModel));
            }
        }

        public Color SelecteFontColor
        {
            get { return _SelecteFontColor; }
            set
            {
                if (_SelecteFontColor != value)
                {
                    _SelecteFontColor = value;
                    SettingColumnModel.FontColor = _SelecteFontColor.ToString();
                    RaisePropertyChanged(nameof(SelecteFontColor));
                }
            }
        }

        public Color SelectedBackColor
        {
            get { return _SelecteBackColor; }
            set
            {
                if (_SelecteBackColor != value)
                {
                    _SelecteBackColor = value;
                    SettingColumnModel.BackColor = _SelecteBackColor.ToString();
                    RaisePropertyChanged(nameof(SelectedBackColor));
                }
            }
        }

        public bool IsWindowClose
        {
            get { return isWindowClose; }
            set
            {
                SetProperty<bool>(ref isWindowClose, value);
                if (isWindowClose)
                {
                    //释放内存
                    Dispose();
                }
            }
        }

        public List<EnumDescription<HorizontalAlignment>> Horizontals { get; set; }

        public List<EnumDescription<VerticalAlignment>> verticals { get; set; }      

        public EnumDescription<HorizontalAlignment> HorizontalModel
        {
            get { return _HorizontalModel; }
            set { _HorizontalModel = value;
                SettingColumnModel.Horizontal = _HorizontalModel.Item;
                RaisePropertyChanged(nameof(HorizontalModel)); }
        }
      
        public EnumDescription<VerticalAlignment> VerticalModel
        {
            get { return _VerticalModel; }
            set { _VerticalModel = value;
                SettingColumnModel.Vertical = _VerticalModel.Item;
                RaisePropertyChanged(nameof(VerticalModel)); }
        }
        #endregion

        #region Commands       
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new DelegateCommand(SaveCommandHandle);
                }
                return _saveCommand;
            }
        }
      
        public ICommand SaveDefalutCommand
        {
            get
            {
                if (_saveDefalutCommand == null)
                {
                    _saveDefalutCommand = new DelegateCommand(SaveDefalutCommandAction);
                }
                return _saveDefalutCommand;
            }
        }

        #endregion Commands

        #region Methods Private

        private void SaveCommandHandle()
        {
            var defaultModel = defaultSettingColumns?.FirstOrDefault(f => f.ColumnId == SettingColumnModel.ColumnId);
            if (SelecteFontColor != Colors.Transparent)
            {
                SettingColumnModel.FontColor = SelecteFontColor.ToString();
            }
            else
            {
                SettingColumnModel.FontColor = defaultModel.FontColor;
            }
            if (SelectedBackColor != Colors.Transparent)
            {
                SettingColumnModel.FontColor = SelecteFontColor.ToString();
            }
            else
            {
                SettingColumnModel.BackColor = defaultModel.BackColor;
            }

            //修改IsUserSetting
            foreach (var item in PlaySettingColumnsHelper.Instance.ColumnConfigs)
            {
                var defaultColumn = defaultSettingColumns?.FirstOrDefault(f => f.ColumnId == item.ColumnId);
                if (Equals(item, defaultColumn))
                {
                    SettingColumnModel.IsUserSetting = false;
                }
                else
                {
                    SettingColumnModel.IsUserSetting = true;
                }
            }

            ColumnPropertyConfig copyModel = new ColumnPropertyConfig();
            copyModel = SettingColumnModel;

            if (SaveColumnConfig())
            {
                SettingColumnModel = SettingColumns.FirstOrDefault(f => f.ColumnId == copyModel.ColumnId);
            }
        }

        private bool SaveColumnConfig()
        {
            try
            {
                bool b = PlaySettingColumnsHelper.Instance.Save(SettingColumns);
                if (b)
                {
                    SettingColumnsLoad();
                    Messenger.Instance.GetEvent<SettingColumnsMessageEvent>().Publish(new SettingColumnsMessage());

                }
                return b;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void SaveDefalutCommandAction()
        {
            SettingColumns.Clear();
            SettingColumns = new ObservableCollection<ColumnPropertyConfig>(defaultSettingColumns);
            if (SaveColumnConfig())
                SettingColumnModel = SettingColumns.FirstOrDefault();
            RaiseProperty();
        }

        private void SettingColumnsLoad()
        {
            SettingColumns.Clear();
            PlaySettingColumnsHelper.Instance.Init();
            foreach (var item in PlaySettingColumnsHelper.Instance.ColumnConfigs)
            {
                var defaultColumn = defaultSettingColumns?.FirstOrDefault(f => f.ColumnId == item.ColumnId);
                if (PlaySettingColumnsHelper.Instance.Equals(item, defaultColumn))
                {
                    item.IsUserSetting = false;
                }
                else
                {
                    item.IsUserSetting = true;
                }
                SettingColumns.Add(item);              
            }
        }

        private void RaiseProperty()
        {
            RaisePropertyChanged(nameof(SettingColumns));
            RaisePropertyChanged(nameof(SettingColumnModel));
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            SettingColumns?.Clear();
        }
        #endregion
    }

    public class EnumDescription<T>
    {
        public T Item { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
