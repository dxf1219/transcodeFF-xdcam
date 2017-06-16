using MediaInfoLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace transcodeFF
{
    public partial class Form_Main : Form
    {
        public Form_Main()
        {
            InitializeComponent();
            logpath = Application.StartupPath + "\\log";
            if (!System.IO.Directory.Exists(logpath))
            {
                Directory.CreateDirectory(logpath);
            }

            //Application.StartupPath + "\\mediaxml"
            if (!System.IO.Directory.Exists(Application.StartupPath + "\\mediaxml"))
            {
                Directory.CreateDirectory(Application.StartupPath + "\\mediaxml");
            }

            WriteLogNew.writeLog("软件启动!", logpath, "info");
           
            SetText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "软件启动!\n");

            textBox_src.Text = Properties.Settings.Default.srcPath;
            textBox_src.Enabled = false;

            textBox_dest.Text = Properties.Settings.Default.destPath;
            textBox_dest.Enabled = false;

            progressBar_value.Value = 0;
            progressBar_value.Maximum = 100;

            taskstarttime = DateTime.Now;

            timer1.Enabled = false;
            toolStripStatusLabel_timer.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            timer1.Enabled = true;

            scanThread = new Thread(new ThreadStart(transcodeThread));
            scanThread.IsBackground = true;
            scanThread.Start();
        }
        private string logpath;
        private Thread scanThread = null;
        private bool ifsuccessed = false ;
        private int cliptype = 0; // 0 普通 1 广播级
        private string outputfile;
        private int taskdur = 0;
        private DateTime taskstarttime;
        //消息框代理
        private delegate void SetTextBoxCallback(string text);

        private delegate void SetTextCallback(string text);

        private delegate void SetSelectCallback(object Msge);

        private  delegate void Callback_ProgressBar(ProgressBar PBar, int value);

        private delegate void Callback_Label(Label label, string value);
        public static void delegate_ProgressBar(ProgressBar PBar, int value)
        {
            try
            {
                //设置光标的位置到文本尾
                if (PBar.InvokeRequired)
                {
                    Callback_ProgressBar d = new Callback_ProgressBar(delegate_ProgressBar);
                    PBar.Invoke(d, new object[] { PBar, value });
                }
                else
                {
                    PBar.Value = value;
                }
            }
            catch (Exception)
            { }
        }

        public static void delegate_Label(Label label, string value)
        {
            try
            {
                //设置光标的位置到文本尾
                if (label.InvokeRequired)
                {
                    Callback_Label d = new Callback_Label(delegate_Label);
                    label.Invoke(d, new object[] { label, value });
                }
                else
                {
                    label.Text = value;
                }
            }
            catch (Exception)
            { }
        }

        private void setTextBox(string text)
        {
            try
            {
                if (textBox_nowtask.InvokeRequired)
                {
                    SetTextBoxCallback scb = new SetTextBoxCallback(setTextBox);
                    this.Invoke(scb, new object[] { text });
                }
                else
                {
                    textBox_nowtask.Text = text;
                }
            }
            catch
            {

            }
          
        }
        private void SetText(string tt)
        {
            string text = tt;
            try
            {
                if (this.richTextBox1.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(SetText);
                    this.Invoke(d, new object[] { text });
                }
                else
                {
                    if (this.richTextBox1.Lines.Length < Properties.Settings.Default.textclearLength)
                    {
                        this.richTextBox1.AppendText(text);
                        of_SetRichCursor(richTextBox1);
                    }
                    else
                    {
                        this.richTextBox1.Clear();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private void of_SetRichCursor(object msge)
        {
            try
            {
                RichTextBox richbox = (RichTextBox)msge;
                //设置光标的位置到文本尾
                if (richbox.InvokeRequired)
                {
                    SetSelectCallback d = new SetSelectCallback(of_SetRichCursor);
                    this.Invoke(d, new object[] { msge });
                }
                else
                {
                    richbox.Select(richbox.TextLength, 0);
                    //滚动到控件光标处
                    richbox.ScrollToCaret();
                }
            }
            catch (Exception)
            {
            }
        }

        public bool IsFileInUse(string fileName)
        {
            bool inUse = true;
            FileStream fs = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                inUse = false;
            }
            catch
            {
            }
            finally
            {
                if (fs != null)

                    fs.Close();
            }
            return inUse;//true表示正在使用,false没有使用  
        }

        private string replaceSpecialXMLSyntax(string str)
        {
            string sr = str;
            Regex reg = new Regex("[“”《》·&\u0001]");
            Match m = reg.Match(str);
            if (m.Success)
            {
                sr = reg.Replace(str, "");
            }
            return sr;
        }



        /// <summary>
        /// 执行Cmd
        /// </summary>
        /// <param name="lsFileName">ffmpeg.exe</param>
        /// <param name="lsArg"></param>
        public void of_Cmd(string exename , string lsArg)
        {
            System.Diagnostics.Process ptmauto = new System.Diagnostics.Process();
            ptmauto.StartInfo = new System.Diagnostics.ProcessStartInfo();
            
            ptmauto.StartInfo.FileName = exename;

            ptmauto.StartInfo.Arguments = lsArg;
            ptmauto.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            ptmauto.StartInfo.RedirectStandardInput = true;
            ptmauto.StartInfo.RedirectStandardOutput = true;
            ptmauto.StartInfo.RedirectStandardError =true;
            ptmauto.StartInfo.CreateNoWindow = true;
            ptmauto.StartInfo.UseShellExecute = false;
            // 为异步获取订阅事件  
            ptmauto.ErrorDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);

            ptmauto.Start();

            // 异步获取命令行内容  
            ptmauto.BeginErrorReadLine();

        

            //StringBuilder sb = new StringBuilder();
            //using (System.IO.StreamReader sr = ptmauto.StandardError)
            //{
            //    string stdOutput = sr.ReadToEnd();
            //    WriteLogNew.writeLog(stdOutput, logpath, "info");

            //}
            ptmauto.WaitForExit();


            ptmauto.Close();

        }

        private void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            // 这里仅做输出的示例，实际上您可以根据情况取消获取命令行的内容  
            // 参考：process.CancelOutputRead()  
            if (String.IsNullOrEmpty(e.Data) == false)
            {
                SetText(e.Data + "\r\n");
                WriteLogNew.writeLog(e.Data,logpath,"info");
                if (cliptype == 1)
                {
                    if (e.Data.Contains(Properties.Settings.Default.xdcamsuccessString))
                    {
                        ifsuccessed = true;
                    }
                }
                else
                {
                    if (e.Data.Contains(Properties.Settings.Default.h264successString))
                    {
                        ifsuccessed = true;
                    }
                }


            }
                
        }

        private string getOriginFileName(string newfilename)
        {
            string orifilename = newfilename.Replace("_transcode","");
            return orifilename;
        }

        private void transcodeThread()
        {
            while (true)
            {
                try
                {
                    string[] needtranscodeFiles = Directory.GetFiles(Properties.Settings.Default.srcPath, "*.*", SearchOption.TopDirectoryOnly);
                    foreach (string nfile in needtranscodeFiles)
                    {
                        //获取文件
                        if (nfile.Contains("_transcode"))
                        {
                            continue;
                        }
                        //mediainfo过一下
                        bool bool_qfile = IsFileInUse(nfile);
                        if (bool_qfile)
                        {
                            WriteLogNew.writeLog("文件正在被使用:" + nfile, logpath, "info");

                        } //bool_qfile 文件正在被使用
                        else
                        {
                            //将文件改名 
                            string newfilename = Properties.Settings.Default.srcPath +"\\"+ Path.GetFileNameWithoutExtension(nfile) + "_transcode" + Path.GetExtension(nfile);

                            setTextBox(Path.GetFileName(newfilename));

                            try
                            {
                                File.Move(nfile, newfilename);
                                WriteLogNew.writeLog("文件改名成功:" + newfilename, logpath, "info");
                            }
                            catch (Exception ee)
                            {
                                WriteLogNew.writeLog("文件改名失败:" + nfile + ee.ToString(), logpath, "error");
                                continue;
                            }

                            try
                            {
                                WriteLogNew.writeLog("开始检测素材:" + Path.GetFileName(newfilename), logpath, "info");
                                SetText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "开始检测素材:" + Path.GetFileName(newfilename) + "\n");

                                //调用mediainfo 读取音频数量 
                                string xmlmedia = "";
                                MediaInfoXmlClass mediaxml = new MediaInfoXmlClass();
                                try
                                {
                                    xmlmedia = mediaxml.of_GetXmlStr(newfilename);

                                    if (xmlmedia.Contains("error"))
                                    {
                                        WriteLogNew.writeLog("该文件mediainfo获取视频信息出错!" + newfilename + xmlmedia, logpath, "error");
                                        SetText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "该文件mediainfo获取视频信息出错!" + "\n");

                                        //出错处理
                                        string errorPath = Properties.Settings.Default.errorPath + "\\" + getOriginFileName( Path.GetFileName(newfilename));
                                        try
                                        {

                                            File.Move(newfilename, errorPath);
                                            WriteLogNew.writeLog("文件移动到出错目录成功!" + errorPath, logpath, "info");
                                            SetText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "文件移动到出错目录成功!" + "\n");
                                        }
                                        catch (Exception eet)
                                        {
                                            WriteLogNew.writeLog("文件移动到出错目录失败!" + errorPath + eet.ToString(), logpath, "error");
                                        }

                                        continue;
                                    }

                                    if (xmlmedia.Equals("Not Media File"))
                                    {
                                        WriteLogNew.writeLog("该文件非媒体文件!" + newfilename + xmlmedia, logpath, "error");
                                        SetText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "该文件非媒体文件!" + "\n");
                                        //出错处理
                                        string errorPath = Properties.Settings.Default.errorPath + "\\" + getOriginFileName(Path.GetFileName(newfilename));
                                        try
                                        {

                                            File.Move(newfilename, errorPath);
                                            WriteLogNew.writeLog("文件移动到出错目录成功!" + errorPath, logpath, "info");
                                            SetText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "文件移动到出错目录成功!" + "\n");
                                        }
                                        catch (Exception eet)
                                        {
                                            WriteLogNew.writeLog("文件移动到出错目录失败!" + errorPath + eet.ToString(), logpath, "error");

                                        }
                                        continue;
                                    }

                                    string newxmlmediainfo = replaceSpecialXMLSyntax(xmlmedia);

                                    if (!newxmlmediainfo.Equals(xmlmedia))
                                    {
                                        WriteLogNew.writeLog("获取的mediainfo中含有特殊字符:" + xmlmedia, logpath, "info");
                                        xmlmedia = newxmlmediainfo;
                                    }

                                    System.Xml.XmlDocument docmediainfo = new System.Xml.XmlDocument();
                                    try
                                    {
                                        docmediainfo.LoadXml(xmlmedia);

                                    }
                                    catch (Exception ee)
                                    {
                                        WriteLogNew.writeLog("加载mediainfo xml　异常:" + xmlmedia + " " + ee.ToString(), logpath, "error");
                                        //出错处理
                                        string errorPath = Properties.Settings.Default.errorPath + "\\" + getOriginFileName(Path.GetFileName(newfilename));
                                        try
                                        {
                                            File.Move(newfilename, errorPath);
                                            WriteLogNew.writeLog("文件移动到出错目录成功!" + errorPath, logpath, "info");
                                            SetText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "文件移动到出错目录成功!" + "\n");
                                        }
                                        catch (Exception eet)
                                        {
                                            WriteLogNew.writeLog("文件移动到出错目录失败!" + errorPath + eet.ToString(), logpath, "error");

                                        }
                                        continue;
                                    }

                                    try
                                    {

                                    }
                                    catch (Exception ee)
                                    {

                                    }
                                    //判断为HD SD 文件
                                    int filetype = 1; // 0 SD 1 HD 
                                    int clipbiteRate = 0;
                                    bool bool_ifcontainsAudio = true;
                                    try
                                    {
                                        XmlNode xmlNodeDisplayAspectRatio = null;
                                        xmlNodeDisplayAspectRatio = docmediainfo.SelectSingleNode("//item[@Name='DisplayAspectRatio']");
                                        if (xmlNodeDisplayAspectRatio != null)
                                        {
                                            if (xmlNodeDisplayAspectRatio.InnerText.Equals("1.333"))
                                            {//SD 
                                                filetype = 0;
                                                WriteLogNew.writeLog("获取视频文件为标清!" + xmlNodeDisplayAspectRatio.InnerText, logpath, "info");
                                                SetText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "获取视频文件为标清!" + xmlNodeDisplayAspectRatio.InnerText + "\n");
                                            }
                                        }

                                        XmlNode OverallBitRateNode = null;
                                        OverallBitRateNode = docmediainfo.SelectSingleNode("//item[@Name='OverallBitRate']");
                                        if (OverallBitRateNode != null)
                                        {
                                            clipbiteRate = Convert.ToInt32(OverallBitRateNode.InnerText );
                                            WriteLogNew.writeLog("获取视频文件码率!" + OverallBitRateNode.InnerText, logpath, "info");
                                        }

                                        XmlNode xmlnodedur = null;
                                        xmlnodedur = docmediainfo.SelectSingleNode("//item[@Name='Duration']");
                                        if (xmlnodedur != null)
                                        {
                                            taskdur = Convert.ToInt32(xmlnodedur.InnerText);
                                            WriteLogNew.writeLog("获取视频文件时长!" + taskdur, logpath, "info");
                                        }

                                        XmlNode AudioCountNode = null;
                                        AudioCountNode = docmediainfo.SelectSingleNode("//item[@Name='AudioCount']");
                                        if (AudioCountNode == null)
                                        {
                                            bool_ifcontainsAudio = false; 
                                        }
                                    }
                                    catch (Exception ee)
                                    {
                                        WriteLogNew.writeLog("读取mediainfo xml 中节点异常:" +  ee.ToString(), logpath, "error");
                                    }
                            
                                    string mediainfoxmlPath = Application.StartupPath + "\\mediaxml";
                                    string mediaxmlfile = mediainfoxmlPath + "\\" + Path.GetFileName(newfilename) + ".xml";
                                    if (File.Exists(mediaxmlfile))
                                    {
                                        File.Delete(mediaxmlfile);
                                    }

                                    docmediainfo.Save(mediaxmlfile);

                                    //调用ffmpeg 
                                    //对于普通的素材来转码h.264 8M 
                                    //专业素材 转码xdcam 50M 
                                 
                
                                    if (filetype == 1)
                                    {
                                        //判断你是否为专业素材
                                        if (clipbiteRate > Properties.Settings.Default.bitRate)  //码率超过定义
                                        {
                                            WriteLogNew.writeLog("该素材为专业广播级素材!", logpath, "info");
                                            SetText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "该素材为专业广播级素材!" + "\n");

                                            cliptype = 1;
                                        }
                                        else
                                        {
                                            WriteLogNew.writeLog("该素材为普通素材!", logpath, "info");
                                            SetText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "该素材为普通素材!" + "\n");
                                        }
                                    }
                                    string args = "";
                                    if (cliptype == 1)   
                                    {
                                        outputfile = Properties.Settings.Default.destPath + "\\" + getOriginFileName(Path.GetFileNameWithoutExtension(newfilename)) + ".TMP" + Properties.Settings.Default.XDCAMextension;
                                        if (bool_ifcontainsAudio)
                                        {
                                            args = "-i " + "\"" + newfilename + "\" " + " " + Properties.Settings.Default.XDCAMarguments1 + " \"" + outputfile + "\"";

                                        }
                                        else  //不包含音频
                                        {
                                            args = "-i " + "\"" + newfilename + "\" " + " " + Properties.Settings.Default.XDCAMarguments2 + " \"" + outputfile + "\"";
                                        }

                                        if (File.Exists(outputfile))
                                        {
                                            outputfile = Properties.Settings.Default.destPath + "\\" + getOriginFileName(Path.GetFileNameWithoutExtension(newfilename)) + DateTime.Now.ToString("HHmmssfff") + ".TMP" + Properties.Settings.Default.XDCAMextension;
                                        }

                                    }
                                    else  //普通级别 h.264 
                                    {
                                        outputfile = Properties.Settings.Default.destPath + "\\" + getOriginFileName(Path.GetFileNameWithoutExtension(newfilename)) + ".TMP" + Properties.Settings.Default.H264extension;
                                        if (filetype == 0)
                                        {
                                            args = "-i " + "\"" + newfilename + "\" " + " " + Properties.Settings.Default.SDarguments + " \"" + outputfile + "\"";
                                        }
                                        else
                                        {
                                            args = "-i " + "\"" + newfilename + "\" " + " " + Properties.Settings.Default.H264HDarguments + " \"" + outputfile + "\"";
                                        }

                                        if (File.Exists(outputfile))
                                        {
                                            outputfile = Properties.Settings.Default.destPath + "\\" + getOriginFileName(Path.GetFileNameWithoutExtension(newfilename)) + DateTime.Now.ToString("HHmmssfff") + ".TMP" + Properties.Settings.Default.H264extension;
                                        }
                                    }

                                    WriteLogNew.writeLog("开始转码:" + newfilename , logpath, "info");
                                    SetText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "开始转码!" + "\n");
                                    ifsuccessed = false;
                                    string ffmpeg = Application.StartupPath + "\\" + Properties.Settings.Default.transcodeExe+".exe";

                                    delegate_ProgressBar(progressBar_value,0);

                                    taskstarttime = DateTime.Now;

                           

                                    of_Cmd(ffmpeg,args);

                                    if (ifsuccessed)
                                    {
                                        delegate_ProgressBar(progressBar_value, 100);
                                        delegate_Label(label_process, "进度:"+ "100%");
                                        WriteLogNew.writeLog("转码完成:" + newfilename, logpath, "info");
                                        SetText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "转码完成!" + "\n");
                                        //成功处理
                                        try
                                        {
                                            string newoutputfile = outputfile.Replace(".TMP","");
                                            File.Move(outputfile, newoutputfile);
                                            WriteLogNew.writeLog("去掉.tmp:" + newoutputfile, logpath, "info");
                                            SetText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "去掉.tmp！"  + "\n");

                                            File.Delete(newfilename);
                                            WriteLogNew.writeLog("删除原文件:" + newfilename, logpath, "info");
                                            SetText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "删除原文件!" + "\n");
                                        }
                                        catch (Exception ee)
                                        {
                                            WriteLogNew.writeLog("删除原文件失败:" + newfilename+ee.ToString(), logpath, "error");
                                            SetText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "删除原文件失败!" + "\n");
                                        }
                                    }
                                    else
                                    {
                                        WriteLogNew.writeLog("转码失败!" + newfilename, logpath, "info");
                                        SetText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "转码失败!" + "\n");

                                        //出错处理
                                        string errorPath = Properties.Settings.Default.errorPath + "\\" + getOriginFileName(Path.GetFileName(newfilename));
                                        try
                                        {
                                            File.Move(newfilename, errorPath);
                                            WriteLogNew.writeLog("文件移动到出错目录成功!" + errorPath, logpath, "info");
                                            SetText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "文件移动到出错目录成功!" + "\n");
                                        }
                                        catch (Exception eet)
                                        {
                                            WriteLogNew.writeLog("文件移动到出错目录失败!" + errorPath + eet.ToString(), logpath, "error");
                                        }
                                    }
                                 

                                }
                                catch (Exception ee)
                                {
                                    WriteLogNew.writeLog("该文件无法获取视频文件信息!" + newfilename + ee.ToString(), logpath, "error");
                                    SetText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "该文件无法获取视频文件信息!" + "\n");
                                    //出错处理
                                    string errorPath = Properties.Settings.Default.errorPath + "\\" + getOriginFileName(Path.GetFileName(newfilename));
                                    try
                                    {

                                        File.Move(newfilename, errorPath);
                                        WriteLogNew.writeLog("文件移动到出错目录成功!" + errorPath, logpath, "info");
                                        SetText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "文件移动到出错目录成功!" + "\n");
                                    }
                                    catch (Exception eet)
                                    {
                                        WriteLogNew.writeLog("文件移动到出错目录失败!" + errorPath + eet.ToString(), logpath, "error");
                                    }
                                    continue;
                                }

                                cliptype = 0;
                            }
                            catch (Exception ee)
                            {
                                WriteLogNew.writeLog("异常!"  + ee.ToString(), logpath, "error");
                            }

                        }////else  bool_qfile 文件正在被使用
                        
                    }
                }
                catch (Exception ee)
                {
                    WriteLogNew.writeLog("转码线程异常!" + ee.ToString(), logpath, "info");
                }
                System.Threading.Thread.Sleep(Properties.Settings.Default.scanInterval);
                //扫描目录
                //获取文件
                //判断文件是否为视频 
                //非视频文件直接移动到error目录
                //视频文件 判断高标清 
                //高清调用高清参数
                //标清调用标清参数表
                //做完删除原文件
            }
        }
        public static string timediff(DateTime dateBegin, DateTime dateEnd)
        {
            TimeSpan ts1 = new TimeSpan(dateBegin.Ticks);
            TimeSpan ts2 = new TimeSpan(dateEnd.Ticks);
            TimeSpan ts3 = ts1 - ts2;
            //你想转的格式
            return ts3.TotalMilliseconds.ToString();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel_timer.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string nowtaskfiletemp = Properties.Settings.Default.srcPath + "\\" + textBox_nowtask.Text.Trim();
            try
            {
                if (File.Exists(nowtaskfiletemp))
                {
                    string timed = timediff(DateTime.Now, taskstarttime);
                    double ss = Convert.ToDouble(timed)/1000;
                    int iss = (int)Math.Round(ss, 0);

                   
                    delegate_Label(label_time,"总耗时:" + iss.ToString() + "s");


                    FileInfo finowtaskfile = new FileInfo(nowtaskfiletemp);

                    if (File.Exists(outputfile))
                    {
                        // 进行进度比对
                        FileInfo fioutputfile = new FileInfo(outputfile);

                        double nowfilesize = (double)finowtaskfile.Length;
                        if (taskdur > 0)
                        {
                            nowfilesize = taskdur * 1024; //字节数 
                        }

                        if (fioutputfile.Length >= finowtaskfile.Length)
                        {
                            delegate_ProgressBar(progressBar_value, 100);
                        }
                        else
                        {
                            double pp = ((double)fioutputfile.Length / nowfilesize) * 100;
                            WriteLogNew.writeLog("获取进度:" + pp.ToString(), logpath, "info");
                            int va = (int)Math.Round(pp, 0);
                            WriteLogNew.writeLog("获取进度:" + va.ToString(), logpath, "info");
                            delegate_ProgressBar(progressBar_value, va);
                            //label_process
                            delegate_Label(label_process, "进度:"+va.ToString()+"%");
                        }
                    }
                }//if (File.Exists(nowtaskfiletemp))
            }
            catch (Exception ee)
            {
                WriteLogNew.writeLog("进度或耗时获取异常:" + ee.ToString(), logpath, "error");
            }

        }

        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                string proName = Properties.Settings.Default.transcodeExe;
                Process[] p = Process.GetProcessesByName(proName);
                if (p.Length >0)
                {
                    p[0].Kill();
                    WriteLogNew.writeLog("转码进程关闭成功!" , logpath, "info");
                }

           }
            catch (Exception ee)
            {
                WriteLogNew.writeLog("转码进程关闭失败!"+ee.ToString(), logpath, "error");
            }
        }
    }
}
