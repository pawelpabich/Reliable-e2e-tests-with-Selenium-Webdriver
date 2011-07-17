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
            var path = CaculatePhysicalPath(fileName);
            File.WriteAllText(path, String.Join(",", data));
        }

        private static IEnumerable<string> Read(string fileName)
        {
            var path = CaculatePhysicalPath(fileName);
            var content = File.ReadAllText(path);
            return String.IsNullOrEmpty(content) ? Enumerable.Empty<string>() : content.Split(',');
        }

        private static string CaculatePhysicalPath(string fileName)
        {
            String path;
            if (HttpContext.Current != null)
            {
                path = CalculatePathWhenUsedFromWebApplication(fileName);
            }
            else
            {
                path = CalculatePathWhenUsedFromTests(fileName);
            }

            return path;
        }

        private static string CalculatePathWhenUsedFromTests(string fileName)
        {
            return Environment.CurrentDirectory + @"\..\..\..\ReliableE2ETestsWithSelenium\App_Data\" + fileName;
        }

        private static string CalculatePathWhenUsedFromWebApplication(string fileName)
        {
            return HttpContext.Current.Server.MapPath("~/App_Data/" + fileName);
        }
    }
}