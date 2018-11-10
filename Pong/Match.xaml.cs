using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Pong
{

    public partial class Match : Window
    {

        public Game pongGame;
        public Network network;

        public Match()
        {
            InitializeComponent();
            pongGame = new Game();
            network = new Network();
        }

    }

}