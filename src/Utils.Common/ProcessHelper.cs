using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Utils.Common
{
    class ProcessHelper
    {
        private bool _inWindows = true;
        private string _fileName = "cmd.exe";
        private string _arguments = "";

        public ProcessHelper INUnix(string fileName, string arguments)
        {
            _inWindows = false;
            _fileName = fileName;
            _arguments = arguments;

            return this;
        }

        public ProcessHelper SetArguments(string arguments)
        {
            _arguments = arguments;
            return this;
        }

        public string Run(bool isReply = true, bool isWait = false)
        {
            if (_inWindows)
            {
                _arguments = "/C \"" + _arguments + "\"";
            }

            return ExecuteCmd(isReply, isWait);
        }

        private string ExecuteCmd(bool isReply = true, bool isWait = false)
        {
            string output = ""; //输出字符串  

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = _fileName;
            startInfo.Arguments = _arguments; ///C表示执行完命令后马上退出    
            startInfo.UseShellExecute = false;              //不使用系统外壳程序启动    
            startInfo.RedirectStandardInput = false;        //不重定向输入    
            startInfo.RedirectStandardOutput = isReply;     //重定向输出    
            startInfo.RedirectStandardError = false;        //错误流重定向输出    
            startInfo.CreateNoWindow = true;                //不创建窗口   
            
            if (isReply)
            {
                startInfo.StandardOutputEncoding = Encoding.UTF8;
            }
            process.StartInfo = startInfo;
            try
            {
                if (process.Start())
                {
                    if (isReply)
                    {
                        // ReadToEnd方法会阻塞，等到结束才返回，因此不需要WaitForExit
                        output = process.StandardOutput.ReadToEnd();//读取进程的输出
                    }
                    else if (isWait)
                    {
                        process.WaitForExit();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (process != null)
                    process.Close();
            }

            return output;
        }
    }
}
