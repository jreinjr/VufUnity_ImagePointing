using UnityEngine;

public static class ImageManipulator
{
    // Helper method to get pixel offset
    private static int GetPixelOffset(int x, int y, int width, int bytesPerPixel)
    {
        return (y * width + x) * bytesPerPixel;
    }

    public static byte[] RotateFlipAndCrop(ref byte[] source, ref byte[] target, int sourceWidth, int sourceHeight, int targetWidth, int targetHeight, int bytesPerPixel)
    {
        // Define the cropping offset to keep the image centered
        int cropX = (sourceWidth - targetHeight) / 2;
        int cropY = (sourceHeight - targetWidth) / 2;

        // Loop through the target width and height
        for (int y = 0; y < targetHeight; y++)
        {
            for (int x = 0; x < targetWidth; x++)
            {
                // Calculate the source coordinates after 90-degree CW rotation
                int sourceX = cropX + y; // Transpose along y-axis
                int sourceY = cropY + (sourceWidth - x - 1); // Flip horizontally before transposing

                int sourceOffset = GetPixelOffset(sourceX, sourceY, sourceWidth, bytesPerPixel);
                int targetOffset = GetPixelOffset(x, y, targetWidth, bytesPerPixel);

                // Copy pixel data from source to target
                for (int b = 0; b < bytesPerPixel; b++)
                {
                    target[targetOffset + b] = source[sourceOffset + b];
                }
            }
        }

        return target;
    }


}
