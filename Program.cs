using System.Drawing.Imaging;

namespace ico2png;

internal static class Program
{
    static void Main()
    {
        string filePath = Environment.GetCommandLineArgs()[1];
        foreach (var size in GetIconSizes(filePath))
        {
            using var icon = Icon.ExtractIcon(filePath, 0, size);
            if (icon is null) continue;

            var pngFileName = $"{Path.GetFileNameWithoutExtension(filePath)}_{size}x{size}.png";
            icon.ToBitmap().Save(pngFileName, ImageFormat.Png);
        }
    }

    static int[] GetIconSizes(string filePath)
    {
        const int ICON_HEADER_SIZE = 6;
        const int ICON_DIRECTORY_SIZE = 16;

        var bytes = File.ReadAllBytes(filePath);

        var imageCount = BitConverter.ToUInt16(bytes, 4);

        var sizes = new int[imageCount];
        for (int i = 0; i < imageCount; ++i)
        {
            sizes[i] = bytes[ICON_HEADER_SIZE + i * ICON_DIRECTORY_SIZE];
            if (sizes[i] == 0) sizes[i] = 256;
        }
        return sizes;
    }
}