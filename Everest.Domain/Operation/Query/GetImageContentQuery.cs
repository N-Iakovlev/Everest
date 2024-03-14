using Incoding.Core.CQRS.Core;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;

namespace Everest.Domain;


public class GetImageContentQuery : QueryBase<byte[]> 
{
    public int Id { get; set; }
    protected override byte[] ExecuteResult()
    {
        // Получаем изображение сотрудника по Id
        byte[] contentImage = Repository.GetById<Content>(Id).ContentImage;

        // Сжимаем изображение до 1920x1080
        using (var stream = new MemoryStream(contentImage))
        {
            using (Image<Rgb24> image = Image.Load<Rgb24>(stream))

            {
                image.Mutate(x => x.Resize(1920, 1080));

                // Конвертируем сжатое изображение в массив байтов и возвращаем его
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, new JpegEncoder());
                    return ms.ToArray();
                }
            }
        }
    }

}
