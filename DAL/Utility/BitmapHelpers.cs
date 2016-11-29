using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;



namespace DAL.Utility
{
 public static class BitmapHelpers
    {
        //public static Bitmap LoadAndResizeBitmap(this string fileName, int width, int height)
        //{
        //    // First we get the the dimensions of the file on disk
        //    BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
        //    BitmapFactory.DecodeFile(fileName, options);

        //    // Next we calculate the ratio that we need to resize the image by
        //    // in order to fit the requested dimensions.
        //    int outHeight = options.OutHeight;
        //    int outWidth = options.OutWidth;
        //    int inSampleSize = 1;

        //    if (outHeight > height || outWidth > width)
        //    {
        //        inSampleSize = outWidth > outHeight
        //                           ? outHeight / height
        //                           : outWidth / width;
        //    }

        //    // Now we will load the image and have BitmapFactory resize it for us.
        //    options.InSampleSize = inSampleSize;
        //    options.InJustDecodeBounds = false;
        //    Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

        //    return resizedBitmap;
        //}

        //public static Bitmap GetBitmap(byte[] bitmap)
        //{
        //    Bitmap map = BitmapFactory.DecodeByteArray(bitmap, 0, bitmap.Length);
        //    return map;
        //}
        //public static byte[] GetBitMapAsBytes(Bitmap bmp)
        //{
        //    MemoryStream stream = new MemoryStream();
        //    bmp.Compress(Bitmap.CompressFormat.Png, 100, stream);
        //    byte[] byteArray = stream.ToArray();
        //    return byteArray;
        //}

        //internal static object GetBitmap()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
