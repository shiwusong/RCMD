using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RCMD
{
    public class CMD
    {
        public CMD(){
            Console.WriteLine("请输入要执行的命令:");
            Process p = new Process();
            //设置要启动的应用程序
            p.StartInfo.FileName = "cmd.exe";
            //是否使用操作系统shell启动
            p.StartInfo.UseShellExecute = false;
            // 接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardInput = true;
            //输出信息
            p.StartInfo.RedirectStandardOutput = true;
            // 输出错误
            p.StartInfo.RedirectStandardError = true;
            //不显示程序窗口
            p.StartInfo.CreateNoWindow = true;
            //启动程序
            p.Start();
            Thread thread = new Thread(new ParameterizedThreadStart(output));
            thread.Start(p);
            input(p);
        }

        public void output(object obj)
        {
            Process p = (Process)obj;
            while (true)
            {
                string msg = p.StandardOutput.ReadLine();
                if (msg != null)
                {
                    Console.WriteLine(msg);
                }
            }
        }

        public void input(Process p)
        {
            while (true)
            {
                string strInput = Console.ReadLine();
                p.StandardInput.WriteLine(strInput);
            }
        }
    }
}
