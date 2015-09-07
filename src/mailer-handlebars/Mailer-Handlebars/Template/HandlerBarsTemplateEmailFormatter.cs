using System;
using System.IO;

namespace Mailer.Handlebars.Template
{
    public class HandlerBarsTemplateEmailFormatter : IEmailFormatter
    {        
        private readonly string _rootTemplatePath = "Content/Emails/";

        public HandlerBarsTemplateEmailFormatter(string fullPathTemplate)
        {
            _rootTemplatePath= fullPathTemplate;
        }

        public string GetContent<T>(string templateName, T data)
        {
            //var rootPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            var file = String.Format("{0}\\{1}.html", _rootTemplatePath, templateName);
            //var path = rootPath + file;
            var contentTemplate = File.ReadAllText(Path.GetFullPath(file));
            var template = HandlebarsDotNet.Handlebars.Compile(contentTemplate);
            var content = template(data);
            return content;
        }
    }
}
