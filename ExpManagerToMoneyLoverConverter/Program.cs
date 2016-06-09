using System.IO;
using System.Linq;
using ExpManagerToMoneyLoverConverter.Helpers;
using ExpManagerToMoneyLoverConverter.Models;

namespace ExpManagerToMoneyLoverConverter
{
    class Program
    {
        private static readonly string MoneyLoverXml = File.ReadAllText("./Content/MoneyLoverExample.xml");
        private static readonly string ExpManagerXml = File.ReadAllText("./Content/ExpManagerExample.xml");

        private static readonly MoneyLoverModel MoneyLoverModel = SimpleSerializer.DeserializeXmlToObject<MoneyLoverModel>(MoneyLoverXml);
        private static readonly ExpManagerModel ExpManagerModel = SimpleSerializer.DeserializeXmlToObject<ExpManagerModel>(ExpManagerXml);
        
        static void Main(string[] args)
        {
            StartConverting();
        }

        private static void StartConverting()
        {
            ConvertCategories();
        }

        static void ConvertCategories()
        {
            MoneyLoverModel.Tables.FirstOrDefault(t => t.Name == "categories")?.Rows.Clear();
        }
    }
}
