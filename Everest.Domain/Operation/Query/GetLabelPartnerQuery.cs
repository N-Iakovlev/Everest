using Everest.Domain;
using Incoding.Core.CQRS.Core;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using Incoding.Core.CQRS.Core;

namespace Everest.Domain;

public class GetLabelPartnerQuery : QueryBase<byte[]>
{
    public int Id { get; set; }
    protected override byte[] ExecuteResult()
    {
       
        byte[] label = Repository.GetById<Partner>(Id).Label;

        // Сжимаем изображение до 235x210
        using (var stream = new MemoryStream(label))
        {
            using (Image<Rgb24> image = Image.Load<Rgb24>(stream))
    
            {
                image.Mutate(x => x.Resize(235, 210));

                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, new JpegEncoder());
                    return ms.ToArray();
                }
            }
        }
    }
}