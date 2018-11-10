using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Pong
{

    public class Game
    {

        public Rectangle arena;
        public List<Ball> balls;
        public List<Player> players;
        public List<Label> scores;
        public List<Barrier> barriers;
        public Network network;
        public int currentPlayer = 1;
            
        public Game()
        {

            arena = new Rectangle();
            arena.Width = 490;
            arena.Height = 250;
            arena.Stroke = Brushes.White;
            arena.StrokeThickness = 5;
            ((Match)Application.Current.MainWindow).UIContent.Children.Add(arena);

            players = new List<Player>();

            Player player1 = new Player(this, -460, 0, 10, 75);
            player1.playerNumber = 1;
            player1.arena = arena;
            player1.upKey = Key.W;
            player1.downKey = Key.S;
            players.Add(player1);

            Player player2 = new Player(this, 460, 0, 10, 75);
            player2.playerNumber = 2;
            player2.arena = arena;
            player2.upKey = Key.Up;
            player2.downKey = Key.Down;
            players.Add(player2);

            barriers = new List<Barrier>();

            Barrier barrier = new Barrier(this, 0, 180, 10, 60);
            barrier.arena = arena;
            barriers.Add(barrier);
            

            balls = new List<Ball>();

            Ball ball = new Ball(this, 0, 0, 10, 10);
            ball.startKey = Key.Space;
            balls.Add(ball);
            
            scores = new List<Label>();

            Label scoreP1 = new Label();
            scoreP1.Content = 0;
            scoreP1.Foreground = new SolidColorBrush(Colors.LightBlue);
            scoreP1.Margin = new Thickness(30, 0, 0, 0);
            scores.Add(scoreP1);
            player1.score = scoreP1;
            ((Match)Application.Current.MainWindow).UIContent.Children.Add(scoreP1);

            Label scoreP2 = new Label();
            scoreP2.Content = 0;
            scoreP2.Foreground = new SolidColorBrush(Colors.LightBlue);
            scoreP2.Margin = new Thickness(460, 0, 0, 0);
            scores.Add(scoreP2);
            player2.score = scoreP2;
            ((Match)Application.Current.MainWindow).UIContent.Children.Add(scoreP2);

            ((Match)Application.Current.MainWindow).KeyDown += new KeyEventHandler((object e, KeyEventArgs args) => { if(args.Key == Key.M) network = new Network(); });
        }

    }

    public class Barrier : UIObject
    {
        public Game game;

        public Barrier(Game game, int x, int y, int width, int height)
        {
            this.game = game;
            uiElement = new Rectangle();
            uiElement.Margin = new Thickness(x, y, 0, 0);
            uiElement.Width = width;
            uiElement.Height = height;
            uiElement.Fill = new SolidColorBrush(Colors.Orange);
            ((Match)Application.Current.MainWindow).UIContent.Children.Add(uiElement);
        }
    }


    public class Player : UIObject
    {

        public Game game;
        public int playerNumber;
        public Label score;
        public Key upKey;
        public Key downKey;

        public Player(Game game, int x, int y, int width, int height)
        {
            this.game = game;
            uiElement = new Rectangle();
            uiElement.Margin = new Thickness(x, y, 0, 0);
            uiElement.Width = width;
            uiElement.Height = height;
            uiElement.Fill = new SolidColorBrush(Colors.Green);
            ((Match)Application.Current.MainWindow).UIContent.Children.Add(uiElement);
            ((Match)Application.Current.MainWindow).KeyDown += new KeyEventHandler(KeyDownAction);
            new DispatcherTimer(new TimeSpan(60000), DispatcherPriority.Normal, new EventHandler(TimerAction), Application.Current.Dispatcher);
        }
        
        public new void KeyDownAction(object e, KeyEventArgs args)
        {
            Thickness bounds = uiElement.Margin;
            if (args.Key == upKey)
            {
                if (y > arena.Height / 2 * -1) y -= 30;
                game.currentPlayer = playerNumber;
            }
            else if (args.Key == downKey)
            {
                if (y < arena.Height / 2) y += 30;
                game.currentPlayer = playerNumber;
            }
        }

        public new void TimerAction(object e, EventArgs args)
        {
            base.TimerAction(e, args);
            if (this.game.network != null) this.game.network.UpdateOpponent(this.game);
            uiElement.Margin = new Thickness(uiElement.Margin.Left, y, 0, 0);
        }

    }


    public class Ball : UIObject
    {

        public Game game;
        public Player controllingPlayer;
        public Key startKey;

        public Ball(Game game, int x, int y, int width, int height)
        {
            this.game = game;
            uiElement = new Rectangle();
            uiElement.Margin = new Thickness(x, y, 0, 0);
            uiElement.Width = width;
            uiElement.Height = height;
            uiElement.Fill = new SolidColorBrush(Colors.White);
            ((Match)Application.Current.MainWindow).UIContent.Children.Add(uiElement);
            ((Match)Application.Current.MainWindow).KeyDown += new KeyEventHandler(KeyDownAction);
            new DispatcherTimer(new TimeSpan(10000), DispatcherPriority.Normal, new EventHandler(TimerAction), Application.Current.Dispatcher);
        }

        public new void KeyDownAction(object e, KeyEventArgs args)
        {
            if (args.Key == startKey)
            {
                
                game.arena.Stroke = Brushes.White;
                int[] xDirOptions = { -1, 1 };
                xDir = xDirOptions[(new Random()).Next(0, 1)];
                yDir = (new Random()).Next(-30, 30);
                controllingPlayer = game.players[0];
            }
        }

        public new void TimerAction(object e, EventArgs args)
        {
            base.TimerAction(e, args);
            Thickness bounds = uiElement.Margin;
            Thickness p1bounds = game.players[0].uiElement.Margin;
            Thickness p2bounds = game.players[1].uiElement.Margin;
            if (bounds.Left > p2bounds.Left - game.players[1].uiElement.Width
                && bounds.Left < p2bounds.Left + game.players[1].uiElement.Width
                && bounds.Top > p2bounds.Top - game.players[1].uiElement.Height
                && bounds.Top < p2bounds.Top + game.players[1].uiElement.Height)
            {
                controllingPlayer = game.players[1];
                yDir = (int)(bounds.Top - p2bounds.Top);
                xDir = -1;
                uiElement.Margin = new Thickness(bounds.Left + (xDir * 5), bounds.Top + (yDir * 0.2), 0, 0);
            }
            else if (bounds.Left > p1bounds.Left - game.players[0].uiElement.Width
                && bounds.Left < p1bounds.Left + game.players[0].uiElement.Width
                && bounds.Top > p1bounds.Top - game.players[0].uiElement.Height
                && bounds.Top < p1bounds.Top + game.players[0].uiElement.Height)
            {
                controllingPlayer = game.players[0];
                yDir = (int)(bounds.Top - p1bounds.Top);
                xDir = 1;
                uiElement.Margin = new Thickness(bounds.Left + (xDir * 5), bounds.Top, 0, 0);
            }
            else if (bounds.Left > p2bounds.Left + game.players[1].uiElement.Width
                || bounds.Left < p1bounds.Left - game.players[0].uiElement.Width)
            {
                xDir = 0;
                yDir = 0;
                uiElement.Margin = new Thickness(0, 0, 0, 0);
                controllingPlayer.score.Content = int.Parse(controllingPlayer.score.Content.ToString()) + 1;
                game.arena.Stroke = Brushes.Red;
                controllingPlayer.uiElement.Height -= 10;
                if (int.Parse(game.players[0].score.Content.ToString()) == 15 || int.Parse(game.players[1].score.Content.ToString()) == 15)
                {
                    game.players[0].score.Content = 0;
                    game.players[1].score.Content = 0;
                }
            }
            else if (bounds.Top > game.arena.Margin.Top + game.arena.Height || bounds.Top < game.arena.Margin.Top - game.arena.Height)
            {
                yDir *= -1;
                uiElement.Margin = new Thickness(bounds.Left + (xDir * 5), bounds.Top + (yDir * 0.2), 0, 0);
            }
            else
            {
                uiElement.Margin = new Thickness(bounds.Left + (xDir * 5), bounds.Top + (yDir * 0.2), 0, 0);
            }
        }

    }


    public class UIObject
    {
        public int x { get; set; }
        public int y { get; set; }
        public int speed { get; set; }
        public int maxSpeed { get; set; }
        public int xDir { get; set; }
        public int yDir { get; set; }
        public Rectangle uiElement { get; set; }
        public Rectangle arena { get; set; }
        public void KeyDownAction(object e, KeyEventArgs args) { }
        public void TimerAction(object e, EventArgs args) { }
    }

}