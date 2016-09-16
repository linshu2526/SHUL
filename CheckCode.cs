using System;
using System.Collections.Generic;
using System.Text;

using System.Web;
using System.Web.SessionState;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace SHUL
{
    public class CheckCodeHandle : IHttpHandler, IRequiresSessionState
    {
        HttpContext CURRETN_CONTEXT;
        public bool IsReusable
        {
            get { return true; }
        }
        public void ProcessRequest(HttpContext context)
        {
            this.CURRETN_CONTEXT = context;
            string checkCode = this.RandomText();
            CURRETN_CONTEXT.Session["_CheckCode"] = checkCode;
            HttpContext.Current.Session["_CheckCode2"] = checkCode;
            this.RenderImage(checkCode);
        }
        private string RandomText()
        {
            string result = string.Empty;
            Random random = new Random();
            for (int num2 = 0; num2 < 5; num2++)
            {
                char ch1;
                int num1 = random.Next();
                if ((num1 % 2) == 0)
                {
                    ch1 = (char)((ushort)(0x30 + ((ushort)(num1 % 10))));
                }
                else
                {
                    ch1 = (char)((ushort)(0x41 + ((ushort)(num1 % 0x1a))));
                }
                result = result + ch1.ToString();
            }
            
            return result;
        }
        private void RenderImage(string checkCode)
        {
            if ((checkCode != null) && (checkCode.Trim() != string.Empty))
            {
                Bitmap bitmap1 = new Bitmap((int)Math.Ceiling(checkCode.Length * 12.5), 0x16);
                Graphics graphics1 = Graphics.FromImage(bitmap1);
                try
                {
                    Random random1 = new Random();
                    graphics1.Clear(Color.White);
                    for (int num1 = 0; num1 < 0x19; num1++)
                    {
                        int num2 = random1.Next(bitmap1.Width);
                        int num3 = random1.Next(bitmap1.Width);
                        int num4 = random1.Next(bitmap1.Height);
                        int num5 = random1.Next(bitmap1.Height);
                        graphics1.DrawLine(new Pen(Color.Silver), num2, num4, num3, num5);
                    }
                    Font font1 = new Font("Arial", 12f, FontStyle.Italic | FontStyle.Bold);
                    LinearGradientBrush brush1 = new LinearGradientBrush(new Rectangle(0, 0, bitmap1.Width, bitmap1.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                    graphics1.DrawString(checkCode, font1, brush1, 2f, 2f);
                    for (int num6 = 0; num6 < 100; num6++)
                    {
                        int num7 = random1.Next(bitmap1.Width);
                        int num8 = random1.Next(bitmap1.Height);
                        bitmap1.SetPixel(num7, num8, Color.FromArgb(random1.Next()));
                    }
                    graphics1.DrawRectangle(new Pen(Color.Silver), 0, 0, bitmap1.Width - 1, bitmap1.Height - 1);
                    MemoryStream stream1 = new MemoryStream();
                    bitmap1.Save(stream1, ImageFormat.Gif);
                    CURRETN_CONTEXT.Response.ClearContent();
                    CURRETN_CONTEXT.Response.ContentType = "image/Gif";
                    CURRETN_CONTEXT.Response.BinaryWrite(stream1.ToArray());
                }
                finally
                {
                    graphics1.Dispose();
                    bitmap1.Dispose();
                }
            }
        }
        


        public static bool Equals(string input)
        {
            bool result = false;
            if (HttpContext.Current.Session["_CheckCode"] != null)
            {
                HttpContext.Current.Session["_CheckCode3"] = HttpContext.Current.Session["_CheckCode"];
                if (!string.IsNullOrEmpty(input))
                {
                    string trimInput = input.Trim();
                    if (trimInput.Length == 5)
                    {
                        if (string.Equals(HttpContext.Current.Session["_CheckCode"].ToString(), trimInput, StringComparison.OrdinalIgnoreCase))
                        {
                            result = true;
                        }
                    }
                }
                HttpContext.Current.Session.Remove("_CheckCode");
            }
            return result;
        }

        /// <summary>
        /// ºÏ≤È—È÷§¬Î
        /// </summary>
        /// <returns></returns>
        public static bool CheckCode(string checkcode)
        {
            if (!string.IsNullOrEmpty(checkcode))
            {
                if (Equals(checkcode))
                {
                    return true;
                }
            }
            return false;
        }
    }
}