﻿using System;
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


namespace PongServer
{

    public partial class ServerConfig : Window
    {

        public ServerConfig()
        {
            InitializeComponent();
        }


        private void btnStart_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }


        private void btnStart_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Label)sender).Foreground = Brushes.LightBlue;
        }


        private void btnStart_MouseLeave(object sender, MouseEventArgs e)
        {

            ((Label)sender).Foreground = Brushes.Blue;
        }
    }

}
