using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Tongyu.Smart.Models;

namespace Tongyu.Smart.Client.Controls
{

    public class PlaySettingColumns
    {
        public Collection<ColumnPropertyConfig> ColumnPropertyConfigs { get; set; }
        public PlaySettingColumns()
        {
            ColumnPropertyConfigs = new Collection<ColumnPropertyConfig>();
        }

        public Collection<ColumnPropertyConfig> Load()
        {
            try
            {
                if (File.Exists(@".\SettingColumns.config"))
                {
                    var obj = XmlSerializerExtension.Deserialize<Collection<ColumnPropertyConfig>>(@".\SettingColumns.config");
                    if (obj?.Count > 0) ColumnPropertyConfigs.Clear();
                    foreach (var item in obj)
                    {
                        ColumnPropertyConfigs.Add(item);
                    }
                }
                else
                {
                    ColumnPropertyConfigs.Clear();

                    ColumnPropertyConfigs.AddRange(DefaultSettingColumns());

                    var obj = XmlSerializerExtension.Serialize<Collection<ColumnPropertyConfig>>(ColumnPropertyConfigs, @".\SettingColumns.config");
                }

                return ColumnPropertyConfigs;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Collection<ColumnPropertyConfig> DefaultSettingColumns()
        {
            return new DefaultPlaySettingColumns().Load();
        }
    }
}
