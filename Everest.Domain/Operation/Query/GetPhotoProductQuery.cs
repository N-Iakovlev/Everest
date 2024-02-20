namespace Everest.Domain;
using Incoding.Core.CQRS.Core;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;

public class GetPhotoProductQuery : QueryBase<byte[]>
{
    public int Id { get; set; }
    protected override byte[] ExecuteResult()
    {
        byte[] productImage = Repository.GetById<Product>(Id).ProductPhoto;

        // Сжимаем изображение до 80x80
        using (var stream = new MemoryStream(productImage))
        {
            var imgPhoto = Image.FromStream(stream);
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            int destWidth = 80;
            int destHeight = 80;

            Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            using (Graphics grPhoto = Graphics.FromImage(bmPhoto))
            {
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grPhoto.DrawImage(imgPhoto,
                    new Rectangle(destX, destY, destWidth, destHeight),
                    new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                    GraphicsUnit.Pixel);
                using (MemoryStream ms = new MemoryStream())
                {
                    bmPhoto.Save(ms, ImageFormat.Jpeg);
                    return ms.ToArray();
                }
            }
        }
    }
}