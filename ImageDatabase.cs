using System.Drawing;
using System.IO;

namespace DiscordDetective;

public class ImageDatabase
{
    private const string _path = "images";

    public ImageDatabase()
    {
        Directory.CreateDirectory(_path);
    }

    public void Store(Image image, string fileName)
    {
        var path = Path.Combine(_path, fileName);
        image.Save(path);
    }

    public Image? Load(string fileName)
    {
        var path = Path.Combine(_path, fileName);
        if (!File.Exists(path))
        {
            return null;
        }    

        var image = Image.FromFile(path);
        return image;
    }
}
