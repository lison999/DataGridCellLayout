using DataGridCellLayout;
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

    public class DefaultPlaySettingColumns
    {
        public Collection<ColumnPropertyConfig> ColumnPropertyConfigs { get; set; }
        public DefaultPlaySettingColumns()
        {
            ColumnPropertyConfigs = new Collection<ColumnPropertyConfig>();
        }

        public Collection<ColumnPropertyConfig> Load()
        {
            try
            {
                if (File.Exists(@".\DefaultSettingColumns.config"))
                {
                    var obj = XmlSerializerExtension.Deserialize<Collection<ColumnPropertyConfig>>(@".\DefaultSettingColumns.config");
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

                    var obj = XmlSerializerExtension.Serialize<Collection<ColumnPropertyConfig>>(ColumnPropertyConfigs, @".\DefaultSettingColumns.config");
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
            Collection<ColumnPropertyConfig> configs = new Collection<ColumnPropertyConfig>();
            var parentOrder = new Order();
            var column1 = new ColumnPropertyConfig(nameof(parentOrder.OrderId));
            var column2 = new ColumnPropertyConfig(nameof(parentOrder.ClientId));


            configs.Add(column1);
            configs.Add(column2);

            return configs;
        }
    }
}
