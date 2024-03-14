using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Incoding.Core.CQRS.Core;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Everest.Domain;
    public class GetAvatarContentQuery : QueryBase<byte[]>
{
        public int Id { get; set; }
        protected override byte[] ExecuteResult()
        {
            // Получаем изображение сотрудника по Id
            byte[] contentImage = Repository.GetById<Content>(Id).ContentImage;

            // Сжимаем изображение до 80x50
            using (var stream = new MemoryStream(contentImage))
            {
                using (Image<Rgb24> image = Image.Load<Rgb24>(stream))

                {
                    image.Mutate(x => x.Resize(80, 50));

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

