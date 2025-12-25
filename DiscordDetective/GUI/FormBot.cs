using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.EntityFrameworkCore;

namespace DiscordDetective.GUI;

public partial class FormBot : Form
{
    private readonly ImageDatabase _imageDatabase;
    private readonly string _token;

    public FormBot(ImageDatabase imageDatabase, string token)
    {
        InitializeComponent();
        _imageDatabase = imageDatabase;
        _token = token;
        _ = LoadBotsAsync();
    }

    #region Загрузка

    private async Task LoadBotsAsync()
    {
        var databaseContext = new DatabaseContext();
        var botDbDTO = databaseContext.Bots.AsNoTracking().First(b => b.Token == _token);
        var userDbDTO = databaseContext.Users.AsNoTracking().First(u => u.Id == botDbDTO.UserId);

        var avatar = _imageDatabase.Load(userDbDTO.Avatar) ?? SystemIcons.Error.ToBitmap();
        avatar = new Bitmap(avatar, pictureBoxAvatar.Size);
        pictureBoxAvatar.Image = avatar;

        labelUserId.Text = $"Id: {userDbDTO.Id}";
        labelUserUsername.Text = $"Username: {userDbDTO.Username}";
        labelUserGlobalName.Text = CheckNull("GlobalName", userDbDTO.GlobalName);
        labelUserDiscriminator.Text = $"Discriminator: {userDbDTO.Discriminator}";
        labelAvatar.Text = CheckNull("Avatar", userDbDTO.Avatar);
        labelBanner.Text = CheckNull("Banner", userDbDTO.Banner);
        labelAccentColor.Text = CheckNull("AccentColor", userDbDTO.AccentColor);
        labelEmail.Text = CheckNull("Email", userDbDTO.Email);
        labelVerified.Text = CheckNull("Verified", userDbDTO.Verified);
        richTextBoxBio.Text = userDbDTO.Bio == null ? "null" : userDbDTO.Bio;
    }

    private string CheckNull(string name, object? obj)
    {
        if (obj == null)
        {
            return $"{name}: null";
        }
        return $"{name}: {obj}";
    }

    #endregion
}
