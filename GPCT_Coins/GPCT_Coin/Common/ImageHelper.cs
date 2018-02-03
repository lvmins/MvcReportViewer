using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Common
{
   public static class ImageHelper
    {
       public static void MakeThumbnail(string oldImagePath, string newImagePath, int width, int height)
       {
           System.Drawing.Image originalImage = System.Drawing.Image.FromFile(oldImagePath);
           int towidth = width;
           int toheight = height;
           int x = 0;
           int y = 0;
           int ow = originalImage.Width;
           int oh = originalImage.Height;
           if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
           {
               oh = originalImage.Height;
               ow = originalImage.Height * towidth / toheight;
               y = 0;
               x = (originalImage.Width - ow) / 2;
           }
           else
           {
               ow = originalImage.Width;
               oh = originalImage.Width * height / towidth;
               x = 0;
               y = (originalImage.Height - oh) / 2;
           }
           //新建一个bmp图片
           System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
           //新建一个画板
           Graphics g = System.Drawing.Graphics.FromImage(bitmap);
           //设置高质量插值法
           g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
           //设置高质量,低速度呈现平滑程度
           g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
           //清空画布并以透明背景色填充
           g.Clear(Color.Transparent);
           //在指定位置并且按指定大小绘制原图片的指定部分
           g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight), new Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);
           try
           {
               //获取图片类型  
               string fileExtension = System.IO.Path.GetExtension(oldImagePath).ToLower();
               bitmap = KiSharpen(new Bitmap(bitmap), float.Parse("0.5"));
               //按原图片类型保存缩略图片,不按原格式图片会出现模糊,锯齿等问题.  
               switch (fileExtension)
               {
                   case ".gif": bitmap.Save(newImagePath, System.Drawing.Imaging.ImageFormat.Gif); break;
                   case ".jpg": bitmap.Save(newImagePath, System.Drawing.Imaging.ImageFormat.Jpeg); break;
                   case ".bmp": bitmap.Save(newImagePath, System.Drawing.Imaging.ImageFormat.Bmp); break;
                   case ".png": bitmap.Save(newImagePath, System.Drawing.Imaging.ImageFormat.Png); break;
               }

           }
           catch (System.Exception e)
           {

           }
           finally
           {
               originalImage.Dispose();
               bitmap.Dispose();
               g.Dispose();
           }
       }

       /// <summary>
       /// 锐化
       /// </summary>
       /// <param name="b">原始Bitmap</param>
       /// <param name="val">锐化程度。取值[0,1]。值越大锐化程度越高</param>
       /// <returns>锐化后的图像</returns>
       public static Bitmap KiSharpen(Bitmap b, float val)
       {
           if (b == null)
           {
               return null;
           }

           int w = b.Width;
           int h = b.Height;

           try
           {
               Bitmap bmpRtn = new Bitmap(w, h, PixelFormat.Format24bppRgb);
               BitmapData srcData = b.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
               BitmapData dstData = bmpRtn.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
               unsafe
               {
                   byte* pIn = (byte*)srcData.Scan0.ToPointer();
                   byte* pOut = (byte*)dstData.Scan0.ToPointer();
                   int stride = srcData.Stride;
                   byte* p;
                   for (int y = 0; y < h; y++)
                   {
                       for (int x = 0; x < w; x++)
                       {
                           //取周围9点的值。位于边缘上的点不做改变。
                           if (x == 0 || x == w - 1 || y == 0 || y == h - 1)
                           {
                               //不做
                               pOut[0] = pIn[0];
                               pOut[1] = pIn[1];
                               pOut[2] = pIn[2];
                           }
                           else
                           {
                               int r1, r2, r3, r4, r5, r6, r7, r8, r0;
                               int g1, g2, g3, g4, g5, g6, g7, g8, g0;
                               int b1, b2, b3, b4, b5, b6, b7, b8, b0;

                               float vR, vG, vB;

                               //左上
                               p = pIn - stride - 3;
                               r1 = p[2];
                               g1 = p[1];
                               b1 = p[0];

                               //正上
                               p = pIn - stride;
                               r2 = p[2];
                               g2 = p[1];
                               b2 = p[0];

                               //右上
                               p = pIn - stride + 3;
                               r3 = p[2];
                               g3 = p[1];
                               b3 = p[0];

                               //左侧
                               p = pIn - 3;
                               r4 = p[2];
                               g4 = p[1];
                               b4 = p[0];

                               //右侧
                               p = pIn + 3;
                               r5 = p[2];
                               g5 = p[1];
                               b5 = p[0];

                               //右下
                               p = pIn + stride - 3;
                               r6 = p[2];
                               g6 = p[1];
                               b6 = p[0];

                               //正下
                               p = pIn + stride;
                               r7 = p[2];
                               g7 = p[1];
                               b7 = p[0];

                               //右下
                               p = pIn + stride + 3;
                               r8 = p[2];
                               g8 = p[1];
                               b8 = p[0];

                               //自己
                               p = pIn;
                               r0 = p[2];
                               g0 = p[1];
                               b0 = p[0];

                               vR = (float)r0 - (float)(r1 + r2 + r3 + r4 + r5 + r6 + r7 + r8) / 8;
                               vG = (float)g0 - (float)(g1 + g2 + g3 + g4 + g5 + g6 + g7 + g8) / 8;
                               vB = (float)b0 - (float)(b1 + b2 + b3 + b4 + b5 + b6 + b7 + b8) / 8;

                               vR = r0 + vR * val;
                               vG = g0 + vG * val;
                               vB = b0 + vB * val;

                               if (vR > 0)
                               {
                                   vR = Math.Min(255, vR);
                               }
                               else
                               {
                                   vR = Math.Max(0, vR);
                               }

                               if (vG > 0)
                               {
                                   vG = Math.Min(255, vG);
                               }
                               else
                               {
                                   vG = Math.Max(0, vG);
                               }

                               if (vB > 0)
                               {
                                   vB = Math.Min(255, vB);
                               }
                               else
                               {
                                   vB = Math.Max(0, vB);
                               }

                               pOut[0] = (byte)vB;
                               pOut[1] = (byte)vG;
                               pOut[2] = (byte)vR;

                           }

                           pIn += 3;
                           pOut += 3;
                       }// end of x

                       pIn += srcData.Stride - w * 3;
                       pOut += srcData.Stride - w * 3;
                   } // end of y
               }

               b.UnlockBits(srcData);
               bmpRtn.UnlockBits(dstData);

               return bmpRtn;
           }
           catch
           {
               return null;
           }
           finally
           {

           }

       }
    }
}
