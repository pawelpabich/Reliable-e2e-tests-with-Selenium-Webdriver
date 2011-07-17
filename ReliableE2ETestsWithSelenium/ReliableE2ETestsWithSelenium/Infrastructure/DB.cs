using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ReliableE2ETestsWithSelenium.Infrastructure
{
    /// <summary>
    /// NEVER EVER USE IT IN PRODUCTION
    /// </summary>
    public class DB
    {
        public static void Reset()
        {
            foreach(var dbFile in Directory.GetFiles(CalculatePhysicalPathToDB(), "*.txt"))
            {
                File.Delete(dbFile);
            }
        }

        public static IEnumerable<String> GetProducts()
        {
            return Read("Products.txt");
        }

        public static void InsertProducts(IEnumerable<string> products)
        {
            Write("Products.txt", products);
        }

        private static void Write(string fileName, IEnumerable<string> data)
        {
            var path = CalculatePhysicalPathToDB() + fileName;
            File.WriteAllText(path, String.Join(",", data));
        }

        private static IEnumerable<string> Read(string fileName)
        {
            var path = CalculatePhysicalPathToDB() + fileName;
            var content = File.Exists(path) ? File.ReadAllText(path) : null;
            return String.IsNullOrEmpty(content) ? Enumerable.Empty<string>() : content.Split(',');
        }

        private static string CalculatePhysicalPathToDB()
        {
            return IsWebApp() ? CalculatePathToDBWhenUsedFromWebApplication() : CalculatePathToDBWhenUsedFromTests();
        }

        private static bool IsWebApp()
        {
            return HttpContext.Current != null;
        }

        private static string CalculatePathToDBWhenUsedFromTests()
        {
            return Environment.CurrentDirectory + @"\..\..\..\ReliableE2ETestsWithSelenium\App_Data\";
        }

        private static string CalculatePathToDBWhenUsedFromWebApplication()
        {
            return HttpContext.Current.Server.MapPath("~/App_Data/");
        }
    }
}