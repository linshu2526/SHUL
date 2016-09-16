using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SHUL
{ 
    /// <summary>
    /// ����ͼ��ʽ
    /// ���by lunch 2009-5-7 
    /// </summary>
    public enum ThumbMode
    {
        /// <summary>
        /// ָ���߿����ţ����ܱ��Σ�
        /// </summary>
        HW,
        /// <summary>
        /// ָ�����߰�����  
        /// </summary>
        W,
        /// <summary>
        /// ָ���ߣ�������
        /// </summary>
        H,
        /// <summary>
        /// ָ���߿�ü��������Σ�   
        /// </summary>
        Cut
    }
    public class Image
    {
        /// <summary>
        /// ��������ͼ
        /// </summary>
        /// <param name="originalImagePath">Դͼ·��������·����</param>
        /// <param name="thumbnailPath">����ͼ·��������·����</param>
        /// <param name="width">����ͼ���</param>
        /// <param name="height">����ͼ�߶�</param>
        /// <param name="mode">��������ͼ�ķ�ʽ</param>    
        /// <param name="DeleteOld">�Ƿ�ɾ��Դ�ļ�</param>  
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
                case "HW"://ָ���߿����ţ����ܱ��Σ�                
                    break;
                case "W"://ָ�����߰�����                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://ָ���ߣ�������
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://ָ���߿�ü��������Σ�                
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

            //�½�һ��bmpͼƬ
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //�½�һ������
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //���ø�������ֵ��
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //���ø�����,���ٶȳ���ƽ���̶�
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //��ջ�������͸������ɫ���
            g.Clear(System.Drawing.Color.Transparent);

            //��ָ��λ�ò��Ұ�ָ����С����ԭͼƬ��ָ������
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
                //��jpg��ʽ��������ͼ
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
        /// Ĭ��HW����ͼ
        /// </summary>
        /// <param name="originalImagePath">ͼƬ��ַ</param>
        /// <param name="originalImagePath">��</param>
        /// <param name="originalImagePath">��</param>
        public static void ThumHW(string originalImagePath, int width, int height)
        {
            MakeThumbnail(originalImagePath, originalImagePath, width, height, ThumbMode.HW, true);
        }
        /// <summary>
        /// Ĭ��H����ͼ
        /// </summary>
        /// <param name="originalImagePath">ͼƬ��ַ</param>
        /// <param name="originalImagePath">��</param>
        /// <param name="originalImagePath">��</param>
        public static void ThumH(string originalImagePath, int width, int height)
        {
            MakeThumbnail(originalImagePath, originalImagePath, width, height, ThumbMode.H, true);
        }
        /// <summary>
        /// Ĭ��W����ͼ
        /// </summary>
        /// <param name="originalImagePath">ͼƬ��ַ</param>
        /// <param name="originalImagePath">��</param>
        /// <param name="originalImagePath">��</param>
        public static void ThumW(string originalImagePath, int width, int height)
        {
            MakeThumbnail(originalImagePath, originalImagePath, width, height, ThumbMode.W, true);
        }
        /// <summary>
        /// Ĭ��Cut����ͼ
        /// </summary>
        /// <param name="originalImagePath">ͼƬ��ַ</param>
        /// <param name="originalImagePath">��</param>
        /// <param name="originalImagePath">��</param>
        public static void ThumCut(string originalImagePath, int width, int height)
        {
            MakeThumbnail(originalImagePath, originalImagePath, width, height, ThumbMode.Cut, true);
        }
       
    }
}
