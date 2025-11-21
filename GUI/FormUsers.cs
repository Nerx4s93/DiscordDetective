using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiscordDetective.GUI;

public partial class FormUsers : Form
{
    private ImageList _largeImageList = new ImageList();

    public FormUsers()
    {
        InitializeComponent();
        _largeImageList.ImageSize = new Size(64, 64);
        UpdateListView();
    }

    private async void UpdateListView()
    {
        listView1.Items.Clear();
        _largeImageList.Images.Clear();

        int index = 0;

        foreach (var user in DataManager.Bots)
        {
            var userData = user.user;
            var name = string.IsNullOrEmpty(userData.Username) ? user.token : userData.Username;
            var avatar = await userData.GetAvatar(true);

            if (avatar == null)
            {
                _largeImageList.Images.Add(SystemIcons.Question);
            }
            else
            {
                _largeImageList.Images.Add(avatar);
            }

            var item = new ListViewItem(name)
            {
                ImageIndex = index
            };
            listView1.Items.Add(item);

            index++;
        }

        listView1.LargeImageList = _largeImageList;
        listView1.Invalidate();
    }
}