using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.AI.MachineLearning;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace WinMLPlayground.WinMLExtensions
{
    public static class WinMLExtensions
    {
        public static async Task<TensorFloat> GetAsTensorFloat(this IRandomAccessStream stream,
            int imageSize, float[] mean = null, float[] std = null)
        {
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
            var softwareBitmap = await decoder.GetSoftwareBitmapAsync();
            softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Rgba8, BitmapAlphaMode.Premultiplied);
            WriteableBitmap innerBitmap = new WriteableBitmap(softwareBitmap.PixelWidth, softwareBitmap.PixelHeight);
            softwareBitmap.CopyToBuffer(innerBitmap.PixelBuffer);

            int channelSize = imageSize * imageSize;

            using (var context = innerBitmap.GetBitmapContext())
            {
                int[] src = context.Pixels;

                var normalized = new float[imageSize * imageSize * 3];

                for (var x = 0; x < imageSize; x++)
                {
                    for (var y = 0; y < imageSize; y++)
                    {
                        var color = innerBitmap.GetPixel(y, x);
                        float r, g, b;

                        r = color.R;
                        g = color.G;
                        b = color.B;

                        if (mean != null && std != null)
                        {
                            r /= 255f;
                            g /= 255f;
                            b /= 255f;

                            r = (r - mean[0]) / std[0];
                            g = (g - mean[1]) / std[1];
                            b = (b - mean[2]) / std[2];
                        }

                        var indexChannelR = (x * imageSize) + y;
                        var indexChannelG = indexChannelR + channelSize;
                        var indexChannelB = indexChannelG + channelSize;

                        normalized[indexChannelR] = r;
                        normalized[indexChannelG] = g;
                        normalized[indexChannelB] = b;
                    }
                }
                return TensorFloat.CreateFromArray(new List<long>() { 1, 3, imageSize, imageSize }, normalized);
            }
        }

        public static Task<TensorFloat> GetAsDefaultImageNetNormalizedTensorFloat(this IRandomAccessStream stream, int imageSize = 224)
            => stream.GetAsTensorFloat(imageSize, new float[] { 0.485f, 0.456f, 0.406f }, new float[] { 0.229f, 0.224f, 0.225f });

        public static async Task<ImageFeatureValue> GetAsImageFeatureValue(this IRandomAccessStream stream)
        {
            SoftwareBitmap softwareBitmap;
            // Create the decoder from the stream 
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);

            // Get the SoftwareBitmap representation of the file in BGRA8 format
            softwareBitmap = await decoder.GetSoftwareBitmapAsync();
            softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);

            VideoFrame videoFrame = VideoFrame.CreateWithSoftwareBitmap(softwareBitmap);

            return ImageFeatureValue.CreateFromVideoFrame(videoFrame);
        }
    }
}
