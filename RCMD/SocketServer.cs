using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RCMD
{
    class SocketServer
    {
        private TcpListener myListener;
        private TcpClient newClient;
        public BinaryReader br;
        public BinaryWriter bw;
        SocketServer(string ipStr, int port)
        {
            IPAddress ip = IPAddress.Parse(ipStr);//服务器端ip
            myListener = new TcpListener(ip, port);//创建TcpListener实例
            myListener.Start();//start
            newClient = myListener.AcceptTcpClient();//等待客户端连接
            while (true)
            {
                try
                {
                    NetworkStream clientStream = newClient.GetStream();//利用TcpClient对象GetStream方法得到网络流
                    br = new BinaryReader(clientStream);
                    string receive = null;
                    receive = br.ReadString();//读取
                    
                }
                catch
                {
                    
                }
            }
        }
    }
}
