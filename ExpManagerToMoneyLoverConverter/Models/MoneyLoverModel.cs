using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ExpManagerToMoneyLoverConverter.Models
{
    [Serializable]
    [XmlRoot("export-database")]
    public class MoneyLoverModel
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = "/data/data/com.bookmark.money/databases/MoneyLoverS2 backup_date=\"2016-06-08\" app_version=\"android-3.2.108\"";

        [XmlElement("table")]
        public List<Table> Tables { get; set; } = new List<Table>();

        public class Table
        {
            [XmlAttribute("name")]
            public string Name { get; set; }

            [XmlElement("row")]
            public List<Row> Rows { get; set; } = new List<Row>();

            public class Row
            {
                [XmlElement("col")]
                public List<Col> Cols { get; set; } = new List<Col>();

                public class Col
                {
                    [XmlAttribute("name")]
                    public string Name { get; set; }

                    [XmlText]
                    public string Value { get; set; }
                }
            }
        }
    }
}