using DevletXml.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace DevletXml.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment _hostingEnv;
        public HomeController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnv = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var rootPath = Path.Combine(_hostingEnv.WebRootPath, "e-FaturaPaketi");
            XmlSchemaSet schema = new XmlSchemaSet();

            //Çalışan Örnek
            //var schematron = Path.Combine(rootPath, "input.xsd");
            //var xmlPath = Path.Combine(rootPath, "input.xml");
            //schema.Add(null, schematron);

            var schematron = Path.Combine(rootPath, @"schematron\UBL-TR_Main_Schematron.xml");
            var xmlPath = Path.Combine(rootPath, "invalid-fatura.xml");
            schema.Add("", schematron);

            XmlReader rd = XmlReader.Create(xmlPath);
            XDocument doc = XDocument.Load(rd);
            doc.Validate(schema, ValidationEventHandler);

            return View();
        }

        void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            XmlSeverityType type = XmlSeverityType.Warning;
            if (Enum.TryParse<XmlSeverityType>("Error", out type))
            {
                if (type == XmlSeverityType.Error)
                    Console.WriteLine("=====> " + e.Message);
            }
            Console.ResetColor();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
