using SkiaSharp;

namespace MaquinaSonora.Utilities
{
    public static class PaletteExtractor
    {
        public static List<string> GetDominantPaletteHex(string path, int paletteSize = 9, int sampleStrid = 2, bool ignoreTransparent = true)
        {
            using var image = SKBitmap.Decode(path);
            if (image == null) throw new InvalidOperationException("Could not decode image.");

            var counts = new Dictionary<int, int>();

            for(int y = 0; y < image.Height; y += sampleStrid)
            {
                for(int x = 0; x < image.Width; x+= sampleStrid)
                {
                    SKColor c = image.GetPixel(x, y);
                    if (ignoreTransparent && c.Alpha < 16) continue;

                    int r4 = c.Red >> 4;
                    int g4 = c.Green >> 4;
                    int b4 = c.Blue >> 4;

                    int key = (r4 << 8) | (g4 << 4) | b4;
                    counts.TryGetValue(key, out int n);
                    counts[key] = n + 1;
                }
            }

            return counts.OrderByDescending(kv => kv.Value)
                .Take(paletteSize)
                .Select(kv =>
                {
                    int key = kv.Key;
                    int r4 = (key >> 8) & 0xF;
                    int g4 = (key >> 4) & 0xF;
                    int b4 = (key >> 0) & 0xF;

                    int r = (r4 << 4) | r4;
                    int g = (g4 << 4) | g4;
                    int b = (b4 << 4) | b4;
                    return $"#{r:X2}{g:X2}{b:X2}";
                })
                .ToList();

        }

    }
}
