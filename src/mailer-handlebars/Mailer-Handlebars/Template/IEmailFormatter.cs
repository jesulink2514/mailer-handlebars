namespace Mailer.Handlebars.Template
{
    public interface IEmailFormatter
    {        
        string GetContent<T>(string templateName,T data);
    }
}