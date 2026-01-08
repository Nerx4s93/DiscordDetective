using System.Threading.Tasks;

namespace Logger.Pages;

public interface IPageElement
{ 
    Task Print(ILoggerService logger);
}