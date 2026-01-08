using System.Threading.Tasks;

namespace DiscordDetective.Logging.Pages;

public interface IPageElement
{ 
    Task Print(ILoggerService logger);
}