using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PongNetworkServer
{

    class Server
    {

        private State state;


        public Server()
        {
            state = new State();
        }


        public void StartServer()
        {
            try
            {
                Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(new IPEndPoint(IPAddress.Parse("192.168.1.200"), 201));
                listener.Listen(100);
                while (true)
                {
                    Socket handler = listener.Accept();
                    string data = null;
                    while (true)
                    {
                        byte[] bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (data.IndexOf("<EOF>") > -1) break;
                    }
                    string[] splitData = data.Split('~');
                    state.yLocation[int.Parse(splitData[0]) - 1] = int.Parse(splitData[1]);
                    handler.Send(Encoding.ASCII.GetBytes(state.yLocation[0].ToString() + "~" + state.yLocation[1].ToString() + "~<EOF>"));
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }



        private class State
        {
            public int[] yLocation = { 0, 0 };
        }

    }

}