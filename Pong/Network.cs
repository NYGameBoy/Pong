using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;


namespace Pong
{

    public class Network
    {
        
        public void UpdateOpponent(Game game)
        {
            try
            {
                Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sender.Connect(new IPEndPoint(IPAddress.Parse("192.168.1.200"), 201));
                sender.Send(Encoding.ASCII.GetBytes(game.currentPlayer.ToString() + "~" + game.players[game.currentPlayer - 1].y.ToString() + "~<EOF>"));
                string data = null;
                while (true)
                {
                    byte[] bytes = new byte[1024];
                    int bytesRec = sender.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    if (data.IndexOf("<EOF>") > -1) break;
                }
                string[] splitData = data.Split('~');
                Console.WriteLine(splitData[0] + ";" + splitData[1]);
                game.players[0].y = int.Parse(splitData[0]);
                game.players[1].y = int.Parse(splitData[1]);
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }

}