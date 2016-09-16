using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;

namespace SHUL
{
    public class EmailModel
    {
        public string server { get; set; }
        public int port { get; set; }
        public string loginname { get; set; }
        public string loginpassword { get; set; }
    }
    public class MailTest
    {
        string encode = "GB2312";
        NetworkStream ns;
        StreamReader sr;
        SslStream sslStream;
        //static int count;
        //static int count2;

        /// <summary>
        /// 获取接收邮件服务器登陆信息，登陆
        /// </summary>
        public string server = string.Empty;//服务器
        public int port = 0;//端口
        public string loginname = string.Empty;//登陆名
        public string loginpassword = string.Empty;//登陆密码
        public MailTest(EmailModel _EmailModel)
        {
            server = _EmailModel.server;
            port = _EmailModel.port;
            loginname = _EmailModel.loginname;
            loginpassword = _EmailModel.loginpassword;
        }


        public ArrayList GetNewMail()
        {

            int mailnum = GetNewMailNum();//获取有邮件封数
            ArrayList getnewmail = new ArrayList();//实例化主方法
            for (int i = 0; i < mailnum; i++)
            {

                //Connect();//登录邮箱
                string UIDL = MailID(i+1);//获取邮件唯一标识符
                ArrayList getmaillist = GetReadMail(i+1); //获取邮件信息
                MailMessage msg = new MailMessage();
                msg.To.Add(GetMailTo(getmaillist));//收件人
                msg.From = GetMailFrom(getmaillist);//发件人
                List<string> listSubject = new List<string>();// getmaillist;// "";// GetMailSubject(getmaillist);//标题列表处理
                string title = ProcessSubject(listSubject, msg);//标题处理之后
                string mailbody = "";// GetMailBody(getmaillist);//正文
                mailbody = ProcessBody(mailbody);//正文处理
                msg.Body = DecodeBase64(mailbody).Trim();//正文编码处理
                Disconnect();//断开服务器
            }



            return getnewmail;

        }



        /// <summary>
        /// 登陆服务器
        /// </summary>
        private void Connect()
        {
            TcpClient sender = new TcpClient(server, port);
            Byte[] outbytes;
            string input;
            string readuser = string.Empty;
            string readpwd = string.Empty;
            try
            {
                ns = sender.GetStream();
                sslStream = new SslStream(ns, false);
                sslStream.AuthenticateAsClient(server);
                sr = new StreamReader(sslStream);

                string str = sr.ReadLine();
                //检查密码
                input = "USER " + loginname + "\r\n";
                outbytes = Encoding.GetEncoding(encode).GetBytes(input.ToCharArray());
                sslStream.Write(outbytes, 0, outbytes.Length);
                readuser = sr.ReadLine();

                input = "PASS " + loginpassword + "\r\n";
                //outbytes = Encoding.GetEncoding(encode).GetBytes(input.ToCharArray());
                outbytes = Encoding.GetEncoding(encode).GetBytes(input.ToCharArray());
                sslStream.Write(outbytes, 0, outbytes.Length);

                readpwd = sr.ReadLine();

            }
            catch 
            {
                throw;
            }

        }




        /// <summary>
        /// 断开服务器
        /// </summary>
        public void Disconnect()
        {
            string input = "QUIT" + "\r\n";//"QUIT" 即表示断开连接
            Byte[] outbytes = System.Text.Encoding.GetEncoding(encode).GetBytes(input.ToCharArray());
            try
            {
                sslStream.Write(outbytes, 0, outbytes.Length);
                sslStream.Close();//关闭数据流
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// 获取邮件封数
        /// </summary>
        /// <returns></returns>
        public int GetNewMailNum()
        {
            Byte[] outbytes;
            string input;
            int countmail;
            try
            {
                Connect();
                //DisconnectTorF = false;
                input = "STAT" + "\r\n";
                outbytes = Encoding.GetEncoding(encode).GetBytes(input.ToCharArray());
                sslStream.Write(outbytes, 0, outbytes.Length);
                string thisResponse = sr.ReadLine();
                if (thisResponse.Substring(0, 4) == "-ERR")
                {
                    return -1;
                }
                string[] tmpArray;
                tmpArray = thisResponse.Split(' ');
                countmail = Convert.ToInt32(tmpArray[1]);
                Disconnect();
                //DisconnectTorF = true;
                return countmail;
            }
            catch 
            {
                throw;
                //return -1;
            }
        }



        /// <summary>
        /// 从服务器读邮件信息
        /// </summary>
        /// <param name="tcpc"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private ArrayList GetReadMail(int getmailnumber)
        {

            Byte[] outbytes;
            string input;
            string line = "";
            string line2 = "读取错误";
            input = "RETR " + getmailnumber.ToString() + "\r\n";//处理返回由参数标识的邮件的全部文本，<RETR>标记
            outbytes = System.Text.Encoding.GetEncoding(encode).GetBytes(input.ToCharArray());
            sslStream.Write(outbytes, 0, outbytes.Length);
            ArrayList getmaillist = new ArrayList();
            ArrayList getmaillist2 = new ArrayList();
            getmaillist2.Add(line2);
            try
            {
                StreamReader srtext = new StreamReader(sslStream, Encoding.GetEncoding(encode));
                //每份邮件以英文“.”结束
                do
                {
                    line = srtext.ReadLine();
                    line = line.ToLower();
                    if (line.IndexOf("-err") > -1)
                    //if (line == "-ERR Message can not load")
                    {
                        return getmaillist2;
                    }
                    getmaillist.Add(line);
                } while (line != ".");
                getmaillist.RemoveAt(getmaillist.Count - 1);
                return getmaillist;
            }
            catch
            {
                throw;
                //listView1.Items.Add("×邮件读取失败，返回重读");
                //return getmaillist2;
            }
        }


        /// <summary>
        /// 获取邮件唯一标识符
        /// </summary>
        /// <param name="getmaillist"></param>
        /// <returns></returns>
        public string MailID(int getmailnumber)
        {
            Byte[] outbytes;
            string uidl;
            string str = "UIDL " + getmailnumber.ToString() + "\r\n";
            outbytes = System.Text.Encoding.GetEncoding(encode).GetBytes(str.ToCharArray());
            try
            {
                sslStream.Write(outbytes, 0, outbytes.Length);
                StreamReader srtext = new StreamReader(sslStream, Encoding.GetEncoding(encode));

                uidl = srtext.ReadLine();
                uidl = uidl.Remove(0, 4).Remove(0, getmailnumber.ToString().Length + 1);
                return uidl;
            }
            catch
            {
                return "";
            }
        }



        /// <summary>
        /// 标题处理
        /// </summary>
        /// <param name="getmaillist"></param>
        /// <param name="msg"></param>
        public string ProcessSubject(List<string> listSubject, MailMessage msg)
        {
            string title = "";
            if (listSubject.Count > 0)//有标题
            {
                string mailsubject = listSubject[0].Trim();
                if (mailsubject.Length > 11 && mailsubject.StartsWith("=?") && mailsubject.EndsWith("?="))
                {
                    foreach (string strPartSubject in listSubject)
                        msg.Subject += Decode(strPartSubject.Split('?'));

                }
                else
                {
                    msg.Subject = mailsubject;
                }
            }
            title = msg.Subject;
            return title;
        }



        /// <summary>
        /// 获取邮件的收件人地址
        /// </summary>
        /// <param name="getmaillist"></param>
        /// <returns></returns>
        public string GetMailTo(ArrayList getmaillist)
        {
            IEnumerator mailnum = getmaillist.GetEnumerator();
            List<string> listDisplayName = new List<string>();
            List<string> listAddress = new List<string>();
            bool findDisplayName = false;
            bool findAddress = false;
            bool isOnlyAddress = false;
            while (mailnum.MoveNext())
            {
                string line = (mailnum.Current as string).Trim();
                if (line.StartsWith("To:") || line.StartsWith("to:"))
                {
                    int endIndex = line.IndexOf(">");
                    if (endIndex == -1)
                        endIndex = line.Length;
                    int addressBegin = line.IndexOf("<");
                    if (addressBegin == -1)//没有常规地址部分
                    {
                        if (line.Contains("@"))//该来自信息没有标题,只有地址
                        {
                            findAddress = true;
                            isOnlyAddress = true;
                            listAddress.Add(line.Substring(5).Trim());
                        }
                        else
                        {
                            findDisplayName = true;
                            listDisplayName.Add(line);
                        }
                    }
                    else if (addressBegin > -1)//有地址部分,也就有标题部分
                    {
                        findDisplayName = true;
                        findAddress = true;
                        listDisplayName.Add(line.Substring(5, addressBegin - 5).Trim());
                        listAddress.Add(line.Substring(addressBegin + 1, endIndex - addressBegin - 1).Trim());
                    }
                }
                else if (isOnlyAddress)//只有地址
                {
                    if (line.Length == 0)
                        break;
                    else if (line.Contains(":"))
                        break;
                    else
                        listAddress.Add(line);
                }
                else if (findDisplayName)
                {
                    if (findAddress)//接收地址
                    {
                        if (line.Contains(":"))
                            break;
                        else
                        {
                            int endIndex = line.IndexOf(">");
                            if (endIndex == -1)
                                endIndex = line.Length;
                            listAddress.Add(line.Substring(0, endIndex).Trim());
                        }
                    }
                    else//没有找到过地址
                    {
                        int endIndex = line.IndexOf(">");
                        if (endIndex == -1)
                            endIndex = line.Length;
                        int addressBegin = line.IndexOf("<");
                        if (addressBegin == -1)//没有地址部分
                            listDisplayName.Add(line);
                        else if (addressBegin > -1)//有地址部分
                        {
                            findAddress = true;
                            listDisplayName.Add(line.Substring(0, addressBegin).Trim());
                            listAddress.Add(line.Substring(addressBegin + 1, endIndex - addressBegin - 1).Trim());
                        }
                    }

                }
            }
            string strAddress = "";
            string strDisplayName = "";

            if (listDisplayName.Count > 0)//有标题
            {
                string strPartDisplayName1 = listDisplayName[0].Replace("\"", "").Trim();
                if (strPartDisplayName1.Length > 11 && strPartDisplayName1.StartsWith("=?") && strPartDisplayName1.EndsWith("?="))
                {
                    foreach (string strPartDisplayName in listDisplayName)
                        strDisplayName += Decode(strPartDisplayName.Split('?'));
                }
                else
                    strDisplayName = strPartDisplayName1;
            }
            foreach (string strPartAddress in listAddress)
            {
                strAddress += strPartAddress;
            }
            return strAddress;

        }





        /// <summary>
        /// 获取邮件的发送人地址
        /// </summary>
        /// <param name="getmaillist"></param>
        /// <returns></returns>
        private MailAddress GetMailFrom(ArrayList getmaillist)
        {
            IEnumerator mailnum = getmaillist.GetEnumerator();
            List<string> listDisplayName = new List<string>();
            List<string> listAddress = new List<string>();
            bool findDisplayName = false;
            bool findAddress = false;
            bool isOnlyAddress = false;
            while (mailnum.MoveNext())
            {
                string line = (mailnum.Current as string).Trim();
                if (line.StartsWith("From:") || line.StartsWith("from:"))
                {
                    int endIndex = line.IndexOf(">");
                    if (endIndex == -1)
                        endIndex = line.Length;
                    int addressBegin = line.IndexOf("<");
                    if (addressBegin == -1)//没有常规地址部分
                    {
                        if (line.Contains("@"))//该来自信息没有标题,只有地址
                        {
                            findAddress = true;
                            isOnlyAddress = true;
                            listAddress.Add(line.Substring(5).Trim());
                        }
                        else
                        {
                            findDisplayName = true;
                            listDisplayName.Add(line);
                        }
                    }
                    else if (addressBegin > -1)//有地址部分,也就有标题部分
                    {
                        findDisplayName = true;
                        findAddress = true;
                        listDisplayName.Add(line.Substring(5, addressBegin - 5).Trim());
                        listAddress.Add(line.Substring(addressBegin + 1, endIndex - addressBegin - 1).Trim());
                    }
                }
                else if (isOnlyAddress)//只有地址
                {
                    if (line.Length == 0)
                        break;
                    else if (line.Contains(":"))
                        break;
                    else
                        listAddress.Add(line);
                }
                else if (findDisplayName)
                {
                    if (findAddress)//接收地址
                    {
                        if (line.Contains(":"))
                            break;
                        else
                        {
                            int endIndex = line.IndexOf(">");
                            if (endIndex == -1)
                                endIndex = line.Length;
                            listAddress.Add(line.Substring(0, endIndex).Trim());
                        }
                    }
                    else//没有找到过地址
                    {
                        int endIndex = line.IndexOf(">");
                        if (endIndex == -1)
                            endIndex = line.Length;
                        int addressBegin = line.IndexOf("<");
                        if (addressBegin == -1)//没有地址部分
                            listDisplayName.Add(line);
                        else if (addressBegin > -1)//有地址部分
                        {
                            findAddress = true;
                            listDisplayName.Add(line.Substring(0, addressBegin).Trim());
                            listAddress.Add(line.Substring(addressBegin + 1, endIndex - addressBegin - 1).Trim());
                        }
                    }
                }
            }
            string strAddress = "";
            string strDisplayName = "";

            if (listDisplayName.Count > 0)//有标题
            {
                string strPartDisplayName1 = listDisplayName[0].Replace("\"", "").Trim(); //=?utf-8?B?5Lqy54ix55qEIDExNzg3NzIxMjNxcWNv?=
                if (strPartDisplayName1.Length > 11 && strPartDisplayName1.StartsWith("=?") && strPartDisplayName1.EndsWith("?="))
                {
                    foreach (string strPartDisplayName in listDisplayName)
                        strDisplayName += Decode(strPartDisplayName.Split('?'));
                }
                else
                    strDisplayName = strPartDisplayName1;
            }
            foreach (string strPartAddress in listAddress)
            {
                strAddress += strPartAddress;
            }
            return new MailAddress(strAddress);

        }



        /// <summary>
        /// 处理正文
        /// </summary>
        /// <returns></returns>
        public string ProcessBody(string mailbody)
        {
            string body = "";
            string pbody = mailbody;// "";// strjq.strqz(mailbody, "Content-Type: text/plain;", "--");
            if (pbody != "")
            {
                body = "";// strjq.strqz(pbody, "Content-Transfer-Encoding: base64", "").Trim();
            }
            else
            {
                body = mailbody;
            }
            return body;

        }


        /// <summary>
        /// 邮件编码
        /// </summary>
        /// <param name="strss"></param>
        /// <returns></returns>
        public string Decode(string[] strss)
        {
            //获取标题的编码方式
            Encoding b = Encoding.GetEncoding(strss[1]);
            string code = strss[3];
            string decode = "";
            byte[] byteCode = null;
            if (strss[2].ToUpper() == "B")
            {
                byteCode = DecodeBase64(ref strss[3]);
            }
            else if (strss[2].ToUpper() == "Q")
            {
                byteCode = DecodeQP(ref strss[3]);
            }
            try
            {
                decode = b.GetString(byteCode);
            }
            catch
            {
                decode = code;
            }
            if (decode.Contains("\0"))
                decode = decode.Replace("\0", "");
            return decode;
        }



        /// <summary>
        /// 邮件标题解码①
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public byte[] DecodeBase64(ref string code)
        {
            string st = code + "000";//
            string strcode = st.Substring(0, (st.Length / 4) * 4);
            return Convert.FromBase64String(strcode);
        }

        /// <summary>
        /// 邮件标题解码②
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public byte[] DecodeQP(ref string code)
        {
            string[] textArray1 = code.Split(new char[] { '=' });
            byte[] buf = new byte[textArray1.Length];
            try
            {
                for (int i = 0; i < textArray1.Length; i++)
                {
                    if (textArray1[i].Trim() != string.Empty)
                    {
                        byte[] buftest = new byte[2];
                        buf[i] = (byte)int.Parse(textArray1[i].Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    }
                }
            }
            catch
            {
                return null;
            }
            return buf;
        }

        /// <summary>
        /// 邮件正文编码③
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string DecodeBase64(string code)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(code);
            try
            {
                string strtext = System.Text.Encoding.GetEncoding(encode).GetString(bytes);
                int i = strtext.IndexOf("【文章编号】");
                if (i == -1)
                {
                    strtext = System.Text.Encoding.GetEncoding("utf-8").GetString(bytes);
                }

                int n = strtext.IndexOf("【文章编号】");
                if (n == -1)
                {
                    strtext = System.Text.Encoding.GetEncoding(encode).GetString(bytes);
                }
                decode = strtext;
            }
            catch
            {
                decode = code;
            }
            return decode;
        }

    }
}