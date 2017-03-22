using Android.Graphics;
using Android.Media;

namespace RatingApp.Helpers
{
    static class BitmapHelpers
    {
        public const string emulator = "Emulator";
        public static Bitmap GetAndRotateBitmap(string fileName)
        {
            Bitmap bitmap = BitmapFactory.DecodeFile(fileName);

            //// Images are being saved in landscape, 
            ///  so rotate them back to portrait 
            //// See https://forums.xamarin.com/discussion/5409/photo-being-saved-in-landscape-not-portrait
            //// See http://developer.android.com/reference/android/media/ExifInterface.html
            using (Matrix mtx = new Matrix())
            {
                if (Android.OS.Build.Product.Contains(emulator))
                {
                    mtx.PreRotate(90);
                }
                else
                {
                    ExifInterface exif = new ExifInterface(fileName);
                    var orientation = (Orientation)exif.GetAttributeInt(ExifInterface.TagOrientation, (int)Orientation.Normal);

                    switch (orientation)
                    {
                        case Orientation.Rotate90:
                            mtx.PreRotate(90);
                            break;
                        case Orientation.Rotate180:
                            mtx.PreRotate(180);
                            break;
                        case Orientation.Rotate270:
                            mtx.PreRotate(270);
                            break;
                        default:
                            break;
                    }
                }

                if (mtx != null)
                    bitmap = Bitmap.CreateBitmap(bitmap, 0, 0, bitmap.Width, bitmap.Height, mtx, false);
            }

            return bitmap;
        }
    }
}