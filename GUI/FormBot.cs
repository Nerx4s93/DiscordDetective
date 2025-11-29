using System.Windows.Forms;

namespace DiscordDetective.GUI;

public partial class FormBot : Form
{
    private readonly string _token;

    public FormBot(string token)
    {
        InitializeComponent();
        _token = token;
    }
}
