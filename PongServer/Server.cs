using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;


namespace PongServer
{

    public class Server
    {

        public Socket socket;
        public Socket listener;


        public Server()
        {
            
        }


        public void StartServer()
        {
            
        }


        public void UpdateOpponent()
        {
        }


        public static void GetOpponentAction(Socket server, String action)
        {
            try
            {
                server.Send(Encoding.UTF8.GetBytes(action));
            }
            catch (SocketException e) { }
        }

    }

}