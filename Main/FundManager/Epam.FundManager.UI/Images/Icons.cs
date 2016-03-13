using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Epam.FundManager.UI.Images
{
    public static class Icons
    {
        #region Utility

        public static Image GetImage(string name)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string resourceName = executingAssembly.GetManifestResourceNames().SingleOrDefault(resourseName => resourseName.EndsWith(name));
            if (!string.IsNullOrWhiteSpace(resourceName))
            {
                Stream file = executingAssembly.GetManifestResourceStream(resourceName);
                return (file != null) ? Image.FromStream(file) : null;
            }

            Assembly entryAssembly = Assembly.GetEntryAssembly();
            resourceName = entryAssembly.GetManifestResourceNames().SingleOrDefault(resourseName => resourseName.EndsWith(name));
            if (!string.IsNullOrWhiteSpace(resourceName))
            {
                Stream file = entryAssembly.GetManifestResourceStream(resourceName);
                return (file != null) ? Image.FromStream(file) : null;
            }

            return null;
        }

        public static BitmapImage GetBitmapImage(Bitmap image)
        {
            var bi = new BitmapImage();
            bi.BeginInit();
            var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            ms.Seek(0, SeekOrigin.Begin);
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }

        public static ImageBrush GetBrush(Bitmap image)
        {
            return new ImageBrush { ImageSource = GetBitmapImage(image) };
        }

        public static Icon GetIcon(Bitmap image)
        {
            return Icon.FromHandle(image.GetHicon());
        }

        public static ImageBrush GetBrush(string name)
        {
            using (var image = (Bitmap)GetImage(name))
            {
                return GetBrush(image);
            }
        }

        public static BitmapImage GetBitmapImage(string name)
        {
            using (var image = (Bitmap)GetImage(name))
            {
                return GetBitmapImage(image);
            }
        }

        public static Icon GetIcon(string name)
        {
            using (var image = (Bitmap)GetImage(name))
            {
                return GetIcon(image);
            }
        }

        public static ImageBrush GetBrush(Icon icon)
        {
            return GetBrush(icon.ToBitmap());
        }


        #endregion

        #region Application Image

        public static readonly Icon ApplicationIcon = GetIcon("Epam48.png");

        public static readonly ImageBrush Application = GetBrush("Epam48.png");

        public static readonly Image ApplicationImage = GetImage("Epam48.png");

        public static readonly BitmapImage ApplicationBitmapImage = GetBitmapImage("Epam48.png");

        #endregion
    }
}