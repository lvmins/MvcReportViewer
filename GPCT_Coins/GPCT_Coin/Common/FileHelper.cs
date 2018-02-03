using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace Common
{
    public class FileHelper
    {
        #region UploadFile
        /// <summary> 
        /// 将本地文件上传到指定的服务器(HttpWebRequest方法) 
        /// </summary> 
        /// <param name="address">文件上传到的服务器</param> 
        /// <param name="fileNamePath">要上传的本地文件（全路径）</param> 
        /// <param name="progressBar">上传进度条</param> 
        /// <returns>成功返回1，失败返回0</returns> 
        public static int Upload_Request(string address, string fileNamePath, ProgressBar progressBar)
        {
            int returnValue = 0;
            // 要上传的文件 
            FileStream fs = new FileStream(fileNamePath, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            // 根据uri创建HttpWebRequest对象 
            address = string.Concat(address, "?filename=", Path.GetFileName(fileNamePath));
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(address));
            httpReq.Method = "POST";
            //对发送的数据不使用缓存 
            httpReq.AllowWriteStreamBuffering = false;
            //设置获得响应的超时时间（30秒） 
            httpReq.Timeout = 30000;
            long fileLength = fs.Length;
            httpReq.ContentLength = fileLength;
            try
            {
                progressBar.Maximum = (int)fs.Length;
                progressBar.Minimum = 0;
                progressBar.Value = 0;
                //每次上传4k 
                int bufferLength = 4096;
                byte[] buffer = new byte[bufferLength];
                //已上传的字节数 
                long offset = 0;
                //开始上传时间 
                DateTime startTime = DateTime.Now;
                int size = r.Read(buffer, 0, bufferLength);
                Stream postStream = httpReq.GetRequestStream();
                while (size > 0)
                {
                    postStream.Write(buffer, 0, size);
                    offset += size;
                    progressBar.Value = (int)offset;
                    TimeSpan span = DateTime.Now - startTime;
                    double second = span.TotalSeconds;
                    Console.WriteLine("已用时：" + second.ToString("F2") + "秒");
                    if (second > 0.1)
                    {
                        Console.WriteLine(" 平均速度：" + (offset / 1024 / second).ToString("0.00") + "KB/秒");
                    }
                    else
                    {
                        Console.WriteLine(" 正在连接…");
                    }
                    Console.WriteLine("已上传：" + (offset * 100.0 / fileLength).ToString("F2") + "%");
                    Console.WriteLine((offset / 1048576.0).ToString("F2") + "M/" + (fileLength / 1048576.0).ToString("F2") + "M");
                    Application.DoEvents();
                    size = r.Read(buffer, 0, bufferLength);
                }
                postStream.Close();
                //获取服务器端的响应 
                WebResponse webRespon = httpReq.GetResponse();
                Stream s = webRespon.GetResponseStream();
                StreamReader sr = new StreamReader(s);
                //读取服务器端返回的消息 
                String sReturnString = sr.ReadLine();
                s.Close();
                sr.Close();
                if (sReturnString == "Success")
                {
                    returnValue = 1;
                }
                else if (sReturnString == "Error")
                {
                    returnValue = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                returnValue = 0;
            }
            finally
            {
                fs.Close();
                r.Close();
            }
            return returnValue;
        }

        /// <summary> 
        /// 将本地文件上传到指定的服务器(WebClient方法) 
        /// </summary> 
        /// <param name="address">文件上传到的服务器</param> 
        /// <param name="fileNamePath">要上传的本地文件（全路径）</param> 
        /// <param name="progressBar">上传进度条</param> 
        /// <returns>成功返回1，失败返回0</returns> 
        public static int Upload_Client(string address, string fileNamePath, ProgressBar progressBar)
        {
            WebClient wc = new WebClient();
            FileStream fs = new FileStream(fileNamePath, FileMode.Open, FileAccess.Read);
            address = string.Concat(address, "?filename=", Path.GetFileName(fileNamePath));
            Stream poststream = wc.OpenWrite(address, "POST");
            byte[] buffer = new byte[1024];
            int mum = (int)fs.Length;
            progressBar.Maximum = mum;
            progressBar.Minimum = 0;
            progressBar.Value = 0;
            //已上传的字节数 
            long offset = 0;
            int length = fs.Read(buffer, 0, buffer.Length);//读取长度
            //开始上传时间 
            DateTime startTime = DateTime.Now;
            while (length > 0)
            {
                poststream.Write(buffer, 0, length);
                offset += length;
                progressBar.Value = (int)offset;
                TimeSpan span = DateTime.Now - startTime;
                double second = span.TotalSeconds;
                Console.WriteLine("已用时：" + second.ToString("F2") + "秒");
                if (second > 0.1)
                {
                    Console.WriteLine(" 平均速度：" + (offset / 1024 / second).ToString("0.00") + "KB/秒");
                }
                else
                {
                    Console.WriteLine(" 正在连接…");
                }
                Console.WriteLine("已上传：" + (offset * 100.0 / mum).ToString("F2") + "%");
                Console.WriteLine((offset / 1048576.0).ToString("F2") + "M/" + (mum / 1048576.0).ToString("F2") + "M");
                Application.DoEvents();
                length = fs.Read(buffer, 0, buffer.Length);
            }
            poststream.Close();
            if (wc.QueryString.ToString() == "Success")
                return 1;
            else
                return 0;
        } 
        #endregion

        #region DownloadFile
        /// <summary>
        /// 下载服务器文件至客户端（不带进度条）
        /// </summary>
        /// <param name="strUrlFilePath">要下载的Web服务器上的文件地址（全路径　如：http://www.dzbsoft.com/test.rar）</param>
        /// <param name="Dir">下载到的目录（存放位置，机地机器文件夹）</param>
        /// <returns>True/False是否上传成功</returns>
        public bool DownLoadFile(string strUrlFilePath, string strLocalDirPath)
        {
            // 创建WebClient实例
            WebClient client = new WebClient();
            //被下载的文件名
            string fileName = strUrlFilePath.Substring(strUrlFilePath.LastIndexOf("/"));
            //另存为的绝对路径＋文件名
            string Path = strLocalDirPath + fileName;
            try
            {
                WebRequest myWebRequest = WebRequest.Create(strUrlFilePath);
            }
            catch (Exception exp)
            {
                MessageBox.Show("文件下载失败：" + exp.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            try
            {
                client.DownloadFile(strUrlFilePath, Path);
                return true;
            }
            catch (Exception exp)
            {
                MessageBox.Show("文件下载失败：" + exp.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        /// <summary>
        /// 下载带进度条代码（普通进度条）
        /// </summary>
        /// <param name="URL">网址</param>
        /// <param name="Filename">文件名</param>
        /// <param name="Prog">普通进度条ProgressBar</param>
        /// <returns>True/False是否下载成功</returns>
        public bool DownLoadFile(string URL, string Filename, ProgressBar Prog)
        {
            try
            {
                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(URL); //从URL地址得到一个WEB请求   
                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse(); //从WEB请求得到WEB响应   
                long totalBytes = myrp.ContentLength; //从WEB响应得到总字节数   
                Prog.Maximum = (int)totalBytes; //从总字节数得到进度条的最大值   
                System.IO.Stream st = myrp.GetResponseStream(); //从WEB请求创建流（读）   
                System.IO.Stream so = new System.IO.FileStream(Filename, System.IO.FileMode.Create); //创建文件流（写）   
                long totalDownloadedByte = 0; //下载文件大小   
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, (int)by.Length); //读流   
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte; //更新文件大小   
                    Application.DoEvents();
                    so.Write(by, 0, osize); //写流   
                    Prog.Value = (int)totalDownloadedByte; //更新进度条   
                    osize = st.Read(by, 0, (int)by.Length); //读流   
                }
                so.Close(); //关闭流
                st.Close(); //关闭流
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 下载带进度条代码(状态栏式进度条）
        /// </summary>
        /// <param name="URL">网址</param>
        /// <param name="Filename">文件名</param>
        /// <param name="Prog">状态栏式进度条ToolStripProgressBar</param>
        /// <returns>True/False是否下载成功</returns>
        public bool DownLoadFile(string URL, string Filename, ToolStripProgressBar Prog)
        {
            try
            {
                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(URL); //从URL地址得到一个WEB请求   
                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse(); //从WEB请求得到WEB响应   
                long totalBytes = myrp.ContentLength; //从WEB响应得到总字节数   
                Prog.Maximum = (int)totalBytes; //从总字节数得到进度条的最大值   
                System.IO.Stream st = myrp.GetResponseStream(); //从WEB请求创建流（读）   
                System.IO.Stream so = new System.IO.FileStream(Filename, System.IO.FileMode.Create); //创建文件流（写）   
                long totalDownloadedByte = 0; //下载文件大小   
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, (int)by.Length); //读流   
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte; //更新文件大小   
                    Application.DoEvents();
                    so.Write(by, 0, osize); //写流   
                    Prog.Value = (int)totalDownloadedByte; //更新进度条   
                    osize = st.Read(by, 0, (int)by.Length); //读流   
                }
                so.Close(); //关闭流   
                st.Close(); //关闭流   
                return true;
            }
            catch
            {
                return false;
            }

        }

        public void startProcess(string fileName)
        {
            //定义一个ProcessStartInfo实例

            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();

            //设置启动进程的初始目录

            info.WorkingDirectory = Application.StartupPath;

            //设置启动进程的应用程序或文档名

            info.FileName = @fileName;

            //设置启动进程的参数

            info.Arguments = "";

            //启动由包含进程启动信息的进程资源

            try
            {

                System.Diagnostics.Process.Start(info);

            }

            catch (System.ComponentModel.Win32Exception we)
            {

                MessageBox.Show(we.Message);

                return;

            }
        }

        /// <summary>        
        /// c#,.net 下载文件        
        /// </summary>        
        /// <param name="URL">下载文件地址</param>       
        /// 
        /// <param name="Filename">下载后的存放地址</param>        
        /// <param name="Prog">用于显示的进度条</param>        
        /// 
        public bool DownLoadFile(string URL, string filename, ProgressBar prog, Label label1)
        {
            float percent = 0;
            try
            {
                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(URL);
                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
                long totalBytes = myrp.ContentLength;
                if (prog != null)
                {
                    prog.Maximum = (int)totalBytes;
                }
                System.IO.Stream st = myrp.GetResponseStream();
                System.IO.Stream so = new System.IO.FileStream(filename, System.IO.FileMode.Create);
                long totalDownloadedByte = 0;
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, (int)by.Length);
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte;
                    System.Windows.Forms.Application.DoEvents();
                    so.Write(by, 0, osize);
                    if (prog != null)
                    {
                        prog.Value = (int)totalDownloadedByte;
                    }
                    osize = st.Read(by, 0, (int)by.Length);

                    percent = (float)totalDownloadedByte / (float)totalBytes * 100;
                    label1.Text = "当前补丁下载进度" + percent.ToString() + "%";
                    System.Windows.Forms.Application.DoEvents(); //必须加注这句代码，否则label1将因为循环执行太快而来不及显示信息
                }
                so.Close();
                st.Close();
                return true;
            }
            catch (System.Exception)
            {
                label1.Text = "下载失败！";
                return false;
            }
        }
        #endregion
    }
}
