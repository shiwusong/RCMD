using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace RCMDClient
{
    class Program
    {
        static public TcpClient client;
        static public BinaryReader br;
        static public BinaryWriter bw;
        static void Main(string[] args)
        {
            string settingStr = File.ReadAllText(@"Config.yaml");
            var deserializer = new Deserializer();
            Config config = deserializer.Deserialize<Config>(settingStr);
            client = new TcpClient(config.IP, config.PORT);
            Thread t = new Thread(output);
            t.Start();
            while (true)
            {
                string str = Console.ReadLine();
                NetworkStream clientStream = client.GetStream();
                bw = new BinaryWriter(clientStream);
                bw.Write(str);
            }
        }
        public static void output()
        {
            while (true)
            {
                try
                {
                    NetworkStream clientStream = client.GetStream();
                    br = new BinaryReader(clientStream);
                    string receive = null;

                    receive = br.ReadString();
                    Console.WriteLine(receive);
                }
                catch
                {

                }
            }
        }
    }
}
