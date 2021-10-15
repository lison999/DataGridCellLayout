using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Tongyu.Smart.Models;

namespace Tongyu.Smart.Client.Controls
{
    public class PlaySettingColumnsHelper
    {
        #region Private
        private static PlaySettingColumnsHelper _instance;
        private static readonly object padlock = new object();
        private PlaySettingColumns SettingColumns;
        private DefaultPlaySettingColumns DefaultSettingColumns;
        #endregion

        #region Public
        public Collection<ColumnPropertyConfig> ColumnConfigs { get;private set; }
        public Collection<ColumnPropertyConfig> DefaultColumnConfigs { get; private set; }

        static PlaySettingColumnsHelper()
        {
            _instance = new PlaySettingColumnsHelper();
        }

        public static PlaySettingColumnsHelper Instance
        {
            get 
            {
                //lock (padlock)
                //{
                //    if (_instance == null)
                //    {
                //        _instance = new PlaySettingColumnsHelper();
                //    }
                //}
                return _instance; 
            }
        }
        #endregion

        #region Ctor
        private PlaySettingColumnsHelper()
        {
            SettingColumns = new PlaySettingColumns();
            DefaultSettingColumns = new DefaultPlaySettingColumns();
            ColumnConfigs = new Collection<ColumnPropertyConfig>();
            DefaultColumnConfigs = new Collection<ColumnPropertyConfig>();

            DefaultColumnConfigs = DefaultSettingColumns.Load();

            Init();
        }
        #endregion

        #region Methods
       
        public void Init()
        {
            ColumnConfigs = SettingColumns.Load();
        }

        public bool Save(Collection<ColumnPropertyConfig> columnPropertyConfigs)
        {
            try
            {
                var obj = XmlSerializerExtension.Serialize<Collection<ColumnPropertyConfig>>(columnPropertyConfigs, @".\SettingColumns.config");
                Init();
                return obj;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static T DeepCopy<T>(T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                xml.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                retval = xml.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }

        public bool Equals(ColumnPropertyConfig config, object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != config.GetType())
            {
                return false;
            }
            ColumnPropertyConfig columnProperty = obj as ColumnPropertyConfig;

            if (config.Width != columnProperty.Width || config.FontSize != columnProperty.FontSize || config.FontBold != columnProperty.FontBold
                || config.FontColor != columnProperty.FontColor || config.BackColor != columnProperty.BackColor || config.Display != columnProperty.Display
                || config.Horizontal != columnProperty.Horizontal || config.Vertical != columnProperty.Vertical || config.IsUserSetting != columnProperty.IsUserSetting)
            {
                return false;
            }

            return true;
        }
        #endregion

    }

}
