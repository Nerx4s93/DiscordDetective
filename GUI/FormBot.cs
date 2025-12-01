using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.EntityFrameworkCore;

namespace DiscordDetective.GUI;

public partial class FormBot : Form
{
    private readonly DatabaseContext _databaseContext;
    private readonly string _token;

    public FormBot(DatabaseContext databaseContext, string token)
    {
        InitializeComponent();
        _databaseContext = databaseContext;
        _token = token;
        _ = LoadBotsAsync();
    }

    #region Загрузка

    private async Task LoadBotsAsync()
    {
        var botDbDTO = _databaseContext.Bots.AsNoTracking().First(b => b.Token == _token);
        var userDbDTO = _databaseContext.Users.AsNoTracking().Where(u => u.Id == botDbDTO.UserId);
    }

    #endregion
}
