using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace RCMD
{
    class Program
    {
        static public TcpListener myListener;
        static public TcpClient newClient;
        static public BinaryReader br;
        static public BinaryWriter bw;
        static public Process p;
        static void Main(string[] args)
        {
            #region socket初始化
            string settingStr = File.ReadAllText(@"Config.yaml");
            var deserializer = new Deserializer();
            Config config = deserializer.Deserialize<Config>(settingStr);
            myListener = new TcpListener(IPAddress.Any, config.PORT);//创建TcpListener实例
            myListener.Start();//start
            newClient = myListener.AcceptTcpClient();//等待客户端连接
            #endregion

            #region cmd初始化
            Console.WriteLine("请输入要执行的命令:");
            p = new Process();
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
            #endregion

            Thread t = new Thread(output);
            t.Start();
            while (true)
            {
                try
                {
                    NetworkStream clientStream = newClient.GetStream();//利用TcpClient对象GetStream方法得到网络流
                    br = new BinaryReader(clientStream);
                    string receive = null;
                    receive = br.ReadString();//读取
                    p.StandardInput.WriteLine(receive);
                }
                catch
                {

                }
            }


        }
        static public void output()
        {
            while (true)
            {
                string msg = p.StandardOutput.ReadLine();
                if (msg != null&& newClient!=null)
                {
                    Console.WriteLine(msg);
                    NetworkStream clientStream = newClient.GetStream();
                    bw = new BinaryWriter(clientStream);
                    //写入
                    bw.Write(msg);
                }
            }
        }
    }
}
