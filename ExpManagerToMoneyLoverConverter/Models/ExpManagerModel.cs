using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ExpManagerToMoneyLoverConverter.Models
{
    [Serializable]
    [XmlRoot("expmanager")]
    public class ExpManagerModel
    {
        [XmlArray("operations")]
        [XmlArrayItem("operation")]
        public List<Operation> Operations { get; set; } = new List<Operation>();

        [XmlElement("dictionaries")]
        public ExpManagerDictionaries Dictionaries { get; set; } = new ExpManagerDictionaries();

        [Serializable]
        public class Operation
        {
            [XmlElement("id")]
            public int Id { get; set; }

            [XmlElement("type_id")]
            public OperationTypeEnum Type { get; set; }

            [XmlElement("date")]
            public DateTime Date { get; set; }

            [XmlElement("source_id")]
            public int SourceId { get; set; }

            [XmlElement("destination_id")]
            public int DestinationId { get; set; }

            [XmlElement("quantity")]
            public double Quantity { get; set; }

            [XmlElement("sumperunit")]
            public double SumPerUnit { get; set; }

            [XmlElement("currensy_sum")]
            public double CurrencySum { get; set; }

            [XmlElement("comment")]
            public string Comment { get; set; }
        }

        [Serializable]
        public class ExpManagerDictionaries
        {
            [XmlArray("accounts")]
            [XmlArrayItem("account")]
            public List<Account> Accounts { get; set; } = new List<Account>();

            [XmlArray("categorygroups")]
            [XmlArrayItem("categorygroup")]
            public List<CategoryGroup> CategoryGroups { get; set; } = new List<CategoryGroup>();

            [XmlArray("categories")]
            [XmlArrayItem("category")]
            public List<Category> Categories { get; set; } = new List<Category>();
        }

        [Serializable]
        public class Account
        {
            [XmlElement("id")]
            public int Id { get; set; }

            [XmlElement("name")]
            public string Name { get; set; }

            [XmlElement("balance")]
            public double Balance { get; set; }

            [XmlElement("currensy_id")]
            public int CurrencyId { get; set; }

            [XmlElement("isdefault")]
            public bool IsDefault { get; set; }

            [XmlElement("istotalinclude")]
            public bool IsToTotalInclude { get; set; }
        }

        [Serializable]
        public class CategoryGroup
        {
            [XmlElement("id")]
            public int Id { get; set; }

            [XmlElement("name")]
            public string Name { get; set; }
        }

        [Serializable]
        public class Category
        {
            [XmlElement("id")]
            public int Id { get; set; }

            [XmlElement("name")]
            public string Name { get; set; }

            [XmlElement("group_id")]
            public int GroupId { get; set; }

            [XmlElement("category_type_id")]
            public CategoryTypeEnum CategoryTypeId { get; set; }

            [XmlElement("isdefault")]
            public bool IsDefault { get; set; }
        }

        public enum OperationTypeEnum
        {
            [XmlEnum("0")]
            Расход = 0,
            [XmlEnum("1")]
            Доход = 1,
            [XmlEnum("2")]
            Перевод = 2
        }

        public enum CategoryTypeEnum
        {
            [XmlEnum("0")]
            Расходная = 0,
            [XmlEnum("1")]
            Доходная = 1,
        }
    }
}
