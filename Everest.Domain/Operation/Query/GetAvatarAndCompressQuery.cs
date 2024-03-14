using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Everest.Domain;
using Incoding.Core.CQRS.Core;

public class GetAvatarAndCompressQuery : QueryBase<byte[]>
{
    public int Id { get; set; }
    protected override byte[] ExecuteResult()
    {
        // Получаем изображение сотрудника по Id
        byte[] employeeImage = Repository.GetById<Employee>(Id).Avatar;

        // Сжимаем изображение до 235x210
        using (var stream = new MemoryStream(employeeImage))
        {
            var imgPhoto = Image.FromStream(stream);
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            int destWidth = 235;
            int destHeight = 210;

            Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            using (Graphics grPhoto = Graphics.FromImage(bmPhoto))
            {
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grPhoto.DrawImage(imgPhoto,
                    new Rectangle(destX, destY, destWidth, destHeight),
                    new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                    GraphicsUnit.Pixel);
                // Конвертируем сжатое изображение в массив байтов и возвращаем его
                using (MemoryStream ms = new MemoryStream())
                {
                    bmPhoto.Save(ms, ImageFormat.Jpeg);
                    return ms.ToArray();
                }
            }
        }
    }
}