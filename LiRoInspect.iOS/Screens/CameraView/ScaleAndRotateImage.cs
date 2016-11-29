using System;
using UIKit;
using CoreGraphics;
using System.Drawing;
using System.Diagnostics;

namespace LiRoInspect.iOS
{
	public static class ScaleAndRotateImage
	{
		static ScaleAndRotateImage()
		{
		}
		/// <summary>
		/// Scales the and rotate image view.
		/// </summary>
		/// <returns>The and rotate image view.</returns>
		/// <param name="imageIn">Image in.</param>
		/// <param name="orIn">Or in.</param>
		public static UIImage ScaleAndRotateImageView(UIImage imageIn, UIImageOrientation orIn)
		{
			float kMaxResolution = 1024;
			UIImage imageCopy = imageIn;
			try
			{
				CGImage imgRef = imageIn.CGImage;
				imageIn.Dispose();
				imageIn = null;
				float width = imgRef.Width;
				float height = imgRef.Height;
				Debug.WriteLine(string.Format("ScaleAndRotateImageView - line# {0}", 29));
				CGAffineTransform transform = CGAffineTransform.MakeIdentity();
				RectangleF bounds = new RectangleF(0, 0, width, height);

				if (width > kMaxResolution || height > kMaxResolution)
				{
					float ratio = width / height;

					if (ratio > 1)
					{
						bounds.Width = kMaxResolution;
						bounds.Height = bounds.Width / ratio;
					}
					else
					{
						bounds.Height = kMaxResolution;
						bounds.Width = bounds.Height * ratio;
					}
				}

				float scaleRatio = bounds.Width / width;
				SizeF imageSize = new SizeF(width, height);
				UIImageOrientation orient = orIn;
				float boundHeight;
				Debug.WriteLine(string.Format("ScaleAndRotateImageView - line# {0}", 53));
				switch (orient)
				{
					case UIImageOrientation.Up:                                        //EXIF = 1
						transform = CGAffineTransform.MakeIdentity();
						break;

					case UIImageOrientation.UpMirrored:                                //EXIF = 2
						transform = CGAffineTransform.MakeTranslation(imageSize.Width, 0f);
						transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
						break;

					case UIImageOrientation.Down:                                      //EXIF = 3
						transform = CGAffineTransform.MakeTranslation(imageSize.Width, imageSize.Height);
						transform = CGAffineTransform.Rotate(transform, (float)Math.PI);
						break;

					case UIImageOrientation.DownMirrored:                              //EXIF = 4
						transform = CGAffineTransform.MakeTranslation(0f, imageSize.Height);
						transform = CGAffineTransform.MakeScale(1.0f, -1.0f);
						break;

					case UIImageOrientation.LeftMirrored:                              //EXIF = 5
						boundHeight = bounds.Height;
						bounds.Height = bounds.Width;
						bounds.Width = boundHeight;
						transform = CGAffineTransform.MakeTranslation(imageSize.Height, imageSize.Width);
						transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
						transform = CGAffineTransform.Rotate(transform, 3.0f * (float)Math.PI / 2.0f);
						break;

					case UIImageOrientation.Left:                                      //EXIF = 6
						boundHeight = bounds.Height;
						bounds.Height = bounds.Width;
						bounds.Width = boundHeight;
						transform = CGAffineTransform.MakeTranslation(0.0f, imageSize.Width);
						transform = CGAffineTransform.Rotate(transform, 3.0f * (float)Math.PI / 2.0f);
						break;

					case UIImageOrientation.RightMirrored:                             //EXIF = 7
						boundHeight = bounds.Height;
						bounds.Height = bounds.Width;
						bounds.Width = boundHeight;
						transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
						transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
						break;

					case UIImageOrientation.Right:                                     //EXIF = 8
						boundHeight = bounds.Height;
						bounds.Height = bounds.Width;
						bounds.Width = boundHeight;
						transform = CGAffineTransform.MakeTranslation(imageSize.Height, 0.0f);
						transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
						break;

					default:
						//throw new Exception("Invalid image orientation");
						Debug.WriteLine(string.Format("ScaleAndRotateImageView Invalid image orientation - line# {0}", 110));
						break;
				}

				try
				{
					Debug.WriteLine(string.Format("ScaleAndRotateImageView - line# {0}", 115));
					UIGraphics.BeginImageContext(bounds.Size);

					CGContext context = UIGraphics.GetCurrentContext();

					if (orient == UIImageOrientation.Right || orient == UIImageOrientation.Left)
					{
						context.ScaleCTM(-scaleRatio, scaleRatio);
						context.TranslateCTM(-height, 0);
					}
					else
					{
						context.ScaleCTM(scaleRatio, -scaleRatio);
						context.TranslateCTM(0, -height);
					}
					Debug.WriteLine(string.Format("ScaleAndRotateImageView - line# {0}", 130));
					context.ConcatCTM(transform);
					context.DrawImage(new RectangleF(0, 0, width, height), imgRef);


					// added context dispose - to free memory used by picture image
					imgRef.Dispose();
					imgRef = null;

					imageCopy.Dispose();
					imageCopy = null;

					imageCopy = UIGraphics.GetImageFromCurrentImageContext();

					UIGraphics.EndImageContext();
					// added context dispose - to free memory used by the graphics context
					context.Dispose();
					context = null;

					imageIn = null;
				}
				catch (Exception ex)
				{
					Debug.WriteLine("Exception Occured in ScaleAndRotateImageView  - line # 164 method due to " + ex.Message);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception Occured in ScaleAndRotateImageView - line # 169 method due to " + ex.Message);
			}

			return imageCopy;
		}
	}
}

