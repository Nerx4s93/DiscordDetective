using System.Drawing;
using System.IO;

namespace DiscordDetective;

public class ImageDatabase
{
    private const string _path = "images";
    private readonly string _section;
    public ImageDatabase(string section)
    {
        _section = section;
        Directory.CreateDirectory(_path);
        Directory.CreateDirectory(Path.Combine(_path, section));
    }

    public void Store(Image image, string fileName)
    {
        var path = Path.Combine(_path, _section, fileName + ".png");
        image.Save(path);
    }

    public void Store(Image image, string fileName, Size size)
    {
        var saveImage = new Bitmap(image, size);
        var path = Path.Combine(_path, _section, fileName + ".png");
        saveImage.Save(path);
    }

    public Image? Load(string? fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            return null;
        }

        var path = Path.Combine(_path, _section, fileName + ".png");
        if (!File.Exists(path))
        {
            return null;
        }    

        var image = Image.FromFile(path);
        return image;
    }
}
