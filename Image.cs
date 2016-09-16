using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SHUL
{ 
    /// <summary>
    /// 缩略图形式
    /// 添加by lunch 2009-5-7 
    /// </summary>
    public enum ThumbMode
    {
        /// <summary>
        /// 指定高宽缩放（可能变形）
        /// </summary>
        HW,
        /// <summary>
        /// 指定宽，高按比例  
        /// </summary>
        W,
        /// <summary>
        /// 指定高，宽按比例
        /// </summary>
        H,
        /// <summary>
        /// 指定高宽裁减（不变形）   
        /// </summary>
        Cut
    }
    public class Image
    {
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        /// <param name="DeleteOld">是否删除源文件</param>  
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, ThumbMode mode, bool DeleteOld)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode.ToString())
            {
                case "HW"://指定高宽缩放（可能变形）                
                    break;
                case "W"://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                
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
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);

            try
            {

                
                originalImage.Dispose();
                g.Dispose();
                if (DeleteOld)
                {
                    FileInfo fi = new FileInfo(originalImagePath);
                    fi.Delete();
                }
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(bitmap);
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                bitmap.Dispose();
            }
        }
        /// <summary>
        /// 默认HW缩略图
        /// </summary>
        /// <param name="originalImagePath">图片地址</param>
        /// <param name="originalImagePath">宽</param>
        /// <param name="originalImagePath">高</param>
        public static void ThumHW(string originalImagePath, int width, int height)
        {
            MakeThumbnail(originalImagePath, originalImagePath, width, height, ThumbMode.HW, true);
        }
        /// <summary>
        /// 默认H缩略图
        /// </summary>
        /// <param name="originalImagePath">图片地址</param>
        /// <param name="originalImagePath">宽</param>
        /// <param name="originalImagePath">高</param>
        public static void ThumH(string originalImagePath, int width, int height)
        {
            MakeThumbnail(originalImagePath, originalImagePath, width, height, ThumbMode.H, true);
        }
        /// <summary>
        /// 默认W缩略图
        /// </summary>
        /// <param name="originalImagePath">图片地址</param>
        /// <param name="originalImagePath">宽</param>
        /// <param name="originalImagePath">高</param>
        public static void ThumW(string originalImagePath, int width, int height)
        {
            MakeThumbnail(originalImagePath, originalImagePath, width, height, ThumbMode.W, true);
        }
        /// <summary>
        /// 默认Cut缩略图
        /// </summary>
        /// <param name="originalImagePath">图片地址</param>
        /// <param name="originalImagePath">宽</param>
        /// <param name="originalImagePath">高</param>
        public static void ThumCut(string originalImagePath, int width, int height)
        {
            MakeThumbnail(originalImagePath, originalImagePath, width, height, ThumbMode.Cut, true);
        }
       
    }
}
