using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using ExpManagerToMoneyLoverConverter.Helpers;
using ExpManagerToMoneyLoverConverter.Models;

namespace ExpManagerToMoneyLoverConverter
{
    class Program
    {
        private static readonly string MoneyLoverXml = File.ReadAllText("./Content/MoneyLoverExample.xml");
        private static readonly string ExpManagerXml = File.ReadAllText("./Content/ExpManagerExample.xml");
        private static readonly string MoneyLoverXmlOutputPath = "./Content/MoneyLoverOutput.xml";

        private static readonly MoneyLoverModel MoneyLoverModel = SimpleSerializer.DeserializeXmlToObject<MoneyLoverModel>(MoneyLoverXml);
        private static readonly ExpManagerModel ExpManagerModel = SimpleSerializer.DeserializeXmlToObject<ExpManagerModel>(ExpManagerXml);
        
        static void Main(string[] args)
        {
            StartConverting();
        }

        private static void StartConverting()
        {
            ConvertCategories();
            var outputXml = HttpUtility.HtmlDecode(SimpleSerializer.SerializeToXml(MoneyLoverModel));
            // replace double quotas for the 'name' attribute with a single quotas
            outputXml = Regex.Replace(outputXml, "(name=\")(.+)(\")", "name='$2'");

            File.WriteAllText(MoneyLoverXmlOutputPath, outputXml, Encoding.UTF8);
        }

        static void ConvertCategories()
        {
            var moneyLoverCateriesRow = MoneyLoverModel.Tables.FirstOrDefault(t => t.Name == "categories")?.Rows ?? new List<MoneyLoverModel.Table.Row>();
            moneyLoverCateriesRow?.Clear();

            var categoryGroupIdShift = 1000;
            moneyLoverCateriesRow.AddRange(ExpManagerModel.Dictionaries.CategoryGroups.Select(categoryGroup => new MoneyLoverModel.Table.Row()
            {
                Cols = new List<MoneyLoverModel.Table.Row.Col>()
                {
                    new MoneyLoverModel.Table.Row.Col() { Name = "cat_id", Value = (categoryGroupIdShift + categoryGroup.Id).ToString() },
                    new MoneyLoverModel.Table.Row.Col() { Name = "cat_name", Value = categoryGroup.Name },
                    new MoneyLoverModel.Table.Row.Col() { Name = "cat_type", Value = "2" }, // Expense by default
                    new MoneyLoverModel.Table.Row.Col() { Name = "cat_img", Value = "" },
                    new MoneyLoverModel.Table.Row.Col() { Name = "account_id", Value = "4" },
                    new MoneyLoverModel.Table.Row.Col() { Name = "parent_id", Value = "0" },
                    new MoneyLoverModel.Table.Row.Col() { Name = "flag", Value = "0" },
                    new MoneyLoverModel.Table.Row.Col() { Name = "uuid", Value = Guid.NewGuid().ToString("N").ToLower() },
                    new MoneyLoverModel.Table.Row.Col() { Name = "meta_data", Value = "" },
                }
            }));

            moneyLoverCateriesRow.AddRange(ExpManagerModel.Dictionaries.Categories.Select(category => new MoneyLoverModel.Table.Row()
            {
                Cols = new List<MoneyLoverModel.Table.Row.Col>()
                {
                    new MoneyLoverModel.Table.Row.Col() { Name = "cat_id", Value = category.Id.ToString() },
                    new MoneyLoverModel.Table.Row.Col() { Name = "cat_name", Value = category.Name },
                    // ExpManager: 0 - expense, 1 - income. MoneyLover: 2- expense, 1 - income
                    new MoneyLoverModel.Table.Row.Col() { Name = "cat_type", Value = (category.CategoryTypeId == 0 ? 2 : 1).ToString() },
                    new MoneyLoverModel.Table.Row.Col() { Name = "cat_img", Value = "" },
                    new MoneyLoverModel.Table.Row.Col() { Name = "account_id", Value = "4" },
                    new MoneyLoverModel.Table.Row.Col() { Name = "parent_id", Value = (categoryGroupIdShift + category.GroupId).ToString() },
                    new MoneyLoverModel.Table.Row.Col() { Name = "flag", Value = "0" },
                    new MoneyLoverModel.Table.Row.Col() { Name = "uuid", Value = Guid.NewGuid().ToString("N").ToLower() },
                    new MoneyLoverModel.Table.Row.Col() { Name = "meta_data", Value = "" },
                }
            }));
        }
    }
}
