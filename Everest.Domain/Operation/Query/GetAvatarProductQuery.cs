﻿using Incoding.Core.CQRS.Core;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;

namespace Everest.Domain;

public  class GetAvatarProductQuery : QueryBase<byte[]>
{
    public int Id { get; set; }
    protected override byte[] ExecuteResult()
    {
        byte[] productImage = Repository.GetById<Product>(Id).ProductPhoto;
    
        // Сжимаем изображение до 290x300
        using (var stream = new MemoryStream(productImage))
        {
            using (Image<Rgb24> image = Image.Load<Rgb24>(stream))
    
            {
                image.Mutate(x => x.Resize(290, 300));
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, new JpegEncoder());
                    return ms.ToArray();
                }
            }
        }
    }
}