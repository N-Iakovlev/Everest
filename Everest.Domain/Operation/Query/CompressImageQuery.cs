using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Processing;
using Incoding.Core.CQRS.Core;

namespace Everest.Domain;
public class CompressImageQuery : QueryBase<byte[]>
{
    public byte[] ImageData { get; set; }
    public int TargetWidth { get; set; }
    public int TargetHeight { get; set; }

    protected override byte[] ExecuteResult()
    {
        using (var stream = new MemoryStream(ImageData))
        {
            using (Image<Rgb24> image = Image.Load<Rgb24>(stream))
            {
                image.Mutate(x => x.Resize(TargetWidth, TargetHeight));

                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, new JpegEncoder());
                    return ms.ToArray();
                }
            }
        }
    }
}
