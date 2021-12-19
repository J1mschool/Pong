using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.Diagnostics;
using System.Media;

namespace Pong
{

    public class Vec2
    {
        // 2 dimensional Vector Data Type used for movement system

        public int X;
        public int Y;

        public Vec2(int newX, int newY)
        {
            X = newX;
            Y = newY;
        }

        public void Zero()
        {
            X = 0;
            Y = 0;
        }
    }

    public class Game
    {
        #region Entry Point
        static void Main(string[] args)
        {
            //Entry Point to the program
            Game game = new Game();
            game.StartGame();
           

            //Gameplay Loop
            while (true)
            {
                //game.Debug();     uncomment to view game variables for debugging
                game.PlayerUpdate();
                game.BallUpdate();
                game.Draw();
            }

        }
        #endregion

        #region Game Variables

        Random randomBounce = new Random();
        Random CPU_failChance = new Random();
        int CPU_fail;
        public int CPUscore;
        public int PlayerScore;
        public bool Play = true;
        public Level level1 = new Level();
        public Player player1 = new Player(36 , 13, "[P]");
        public Player player2 = new Player(76, 13, "[L]");

        public Ball ball = new Ball(56, 13, "0");
        // ∙ small ball icom
        #endregion

        public void StartGame()
        {

            #region Title and instructions

            //Startup messages and instructions

            CursorVisible = false;
            Title = "PONG";
            WriteLine(" -- PONG --");
            System.Threading.Thread.Sleep(500);
            WriteLine("   by JAK  ");
         
            System.Threading.Thread.Sleep(1000);
            WriteLine("");
            WriteLine(" [mild flash warning]");
            WriteLine(" Press ENTER to Continue... ");
            WriteLine("");
            ReadLine();
            Beep();
            Write(" -- Instructions -- ");
            WriteLine("");
            Write(" - Use [W] and [S]  to move vertically");
            ReadLine();
            Beep();
            WriteLine("");
            WriteLine("- The CPU is set to EASY(ish) ;) ");
            Beep();
            ReadLine();
            WriteLine("- You win by not loosing :D");
            WriteLine("- Good Luck!");
            WriteLine("");
            WriteLine("");
            WriteLine("https://en.wikipedia.org/wiki/Pong");
            WriteLine(" (But the actual instructions are here if you really need them ^^ )");
            Beep();
            ReadLine();

            #endregion

            #region GameSetup
            //Initialize Game
            Clear();
            player2.Draw();
            player1.Draw();
            ball.Draw();

            #endregion 

        }

        public void Debug()
        {
            #region Debug Statements
            // Prints stuff out for debugging purposes
            SetCursorPosition(12 , 10);

            for (int i = 0; i < 25; i++)
            {
                WriteLine("");
            }


            WriteLine("--Debug--");
            WriteLine("player 1 pos: " + player1.Pos.X + "," + player1.Pos.Y);
            WriteLine("player 2 pos: " + player2.Pos.X + "," + player2.Pos.Y);
            WriteLine("Ball Pos: " + ball.Pos.X + "," + ball.Pos.Y);
            WriteLine("Ball Y  Vel : " + ball.Velocity.Y);
            WriteLine("Ball Gliding Left?: " + ball.GlideLeft);
            WriteLine("Ball Bounce Anlge " + ball.BounceAngle);

            #endregion
        }

      
        public void PlayerUpdate()
        {
            #region Player and CPU Movement
            
            //Updates player and CPU movement

            System.Threading.Thread.Sleep(10);
            player1.Move();

            //Makes CPU beatable
            CPU_fail = CPU_failChance.Next(1,5);
            if (CPU_fail == 1) // if CPU slips up
                player2.Pos.Y = ball.Pos.Y + 1;

           else
                player2.Pos.Y = ball.Pos.Y;

            #endregion
        }

        public void BallUpdate()
        {

            #region  Player Collision Detexction

            // If hit player 1
            if ((ball.Pos.X - 2) == player1.Pos.X && ball.Pos.Y == player1.Pos.Y 
                || (ball.Pos.X -1) == player1.Pos.X && ball.Pos.Y == player1.Pos.Y
                || (ball.Pos.X ) == player1.Pos.X && ball.Pos.Y == player1.Pos.Y)
            {
                ball.GlideLeft = false;
                Beep();
                ball.BounceAngle = randomBounce.Next(1, 4);
              
            }

            // If hit player 2
            else if (ball.Pos.X == player2.Pos.X && (ball.Pos.Y) == player2.Pos.Y)
            {
                ball.GlideLeft = true;
                Beep();
                ball.BounceAngle = randomBounce.Next(1, 4);

            }
            #endregion

            #region Point Detection
            // enforcing boundaries  

            if (ball.Pos.X <= 30 || ball.Pos.X >= 80) //sides
            {
                ball.GlideLeft = !ball.GlideLeft;
                ball.Pos.X = ball.Start.X;
                ball.Pos.Y = ball.Start.Y;
                ball.BounceAngle = 2;
                ScoreUpdate();
            }

            #endregion 

            ball.SetPos();
        }

        public void ScoreUpdate()
        {
            #region Player Goal
            if (ball.GlideLeft) // if  Player Scored
            {
                PlayerScore += 1;
                Beep();
                Beep();
                Beep();

                Clear();

                SetCursorPosition(50, 13);
                Write(" - You Scored a point :D - ");
                SetCursorPosition(54, 14);
                WriteLine(" Press ENTER to Resume ");
                ReadLine();
            }
            #endregion

            #region CPU Goal
            else if (!ball.GlideLeft) // if CPU Scored
            {

                CPUscore += 1;
                Clear();

                SetCursorPosition(50, 13);
                Write("-  imagine loosing to a bot :P - ");
                SetCursorPosition(54, 14);
                WriteLine(" Press ENTER to Resume ");
                ReadLine();

            }
            #endregion
        }

        public void Draw()
        {
            //refreshes screen every frame
            Clear();

            #region Score Display
            //Display Scores on screen
            SetCursorPosition(10, 3);
            Write("Player: " + PlayerScore);

            SetCursorPosition(95, 3);
            Write("CPU: " + CPUscore);
            #endregion

            #region GamePlay Display
            level1.Draw();
            player1.Draw();
            player2.Draw();
            ball.Draw();
            System.Threading.Thread.Sleep(50);
            #endregion

        }

    }

    public class Level
    {

        #region Level Dimensions

        // Level Dimensions
        public int MaxHeight = 15;
        public int MaxWidth = 51;

        //Padding
        public int OffsetX = 31;
        public int OffsetY = 6;

        //Level Killzones are x <= 50 and x >= 80

        #endregion

        public void Draw()
        {
            #region Render Level Borders

            // Draw Top Border
            for (int i = 1; i <= MaxWidth; i++)
            {
                SetCursorPosition(OffsetX + i , OffsetY);
                Write("■");
            }


            //Draw Bottom Border
            for (int i = 1; i <= MaxWidth; i++)
            {
                SetCursorPosition(OffsetX + i, OffsetY + MaxHeight);
                Write("■");
            }


            //Draw Left Border

            for (int i = 0; i <= MaxHeight; i++)
            {
                SetCursorPosition(OffsetX  , OffsetY + i);
                Write("■");
            }


            //Draw Right Border

            for (int i = 0; i <= MaxHeight; i++)
            {
                SetCursorPosition(OffsetX + MaxWidth, OffsetY + i);
                Write("■");
            }
            #endregion

        }

    }

    public class Player
    {
        #region Movement Variables

        bool canMove;
        //Start Position
        Vec2 start;
        public Vec2 Start
        {
            get { return start; }
            set { start = value; }
        }

        //Current Position
        Vec2 pos;
        public Vec2 Pos
        {
            get { return pos; }
            set { pos = value; }
        }

        // Velocity
        Vec2 velocity = new Vec2(0, 0);
        public Vec2 Veloctity
        {
            get { return velocity; }
            set { velocity = value; }
        }


        #endregion

        #region Display
        //display Char/string
        public string icon;
        #endregion

        public void Move()
        {
             #region Handling Movement Input

            //resets Velocity
            velocity.Zero();

            if (KeyAvailable)
            {
                ConsoleKeyInfo keyinfo = ReadKey(true);
                ConsoleKey key = keyinfo.Key;
                switch (key)
                {

                    case ConsoleKey.W:      //Go Up
                        velocity.Y = -1;
                        break;

                    case ConsoleKey.S:      //Go Down
                        velocity.Y = 1;
                        break;

                    default:
                        break;
                
                }

            }

                  //updates location
                  pos.X += velocity.X;
                  pos.Y += velocity.Y;

                  //Prevents offscreen glitch
                  if (pos.X < 0)
                      pos.X = 0;

                  if (pos.Y < 0)
                      pos.Y = 0;

            #endregion

        }


        public void Draw()
        {
            #region Draw Player

            if (pos.Y < 0) // extra procaution for offscreen glitch
                pos.Y += 1;
                
                SetCursorPosition(pos.X, pos.Y);
                Write(icon);

            #endregion

        }


   
        #region Player Constructor
        //Constructor for ez object creation
        public Player(int initX, int initY, string newIcon)
        {
            start = new Vec2(initX, initY);
            pos = new Vec2(start.X, start.Y);
            icon = newIcon;
        }
        #endregion

    }

    public class Ball
    {
        #region Movement Variables
        public bool GlideLeft = true;
        public int BounceAngle;

        //Start Position
        Vec2 start;
        public Vec2 Start
        {
            get { return start; }
            set { start = value; }
        }

        //Current Position
        Vec2 pos;
        public Vec2 Pos
        {
            get { return pos; }
            set { pos = value; }
        }

        //Velocity 
        Vec2 velocity = new Vec2(0, 0);
        public Vec2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        #endregion

        #region Display
        //Displaying Ball
        string icon;
        #endregion

        public void Bounce()
        {
            #region Bounce Check
            //Checks if Ball needs to bounce 

            // bounce on bottom 
            if (BounceAngle == 1)
                if (pos.Y >= 20)
                {
                    BounceAngle = 3;
                }

            // bounce on top
           if (BounceAngle == 3)
               if (pos.Y <= 7)
               {
                    BounceAngle = 1; 
               }
            #endregion 
        }


        public void SetPos()
        {
            //Responsibile for updating the Ball Position
           
            #region Move Y

            // up hit
            if (BounceAngle == 3)
            {
                velocity.Y = 1;
                pos.Y -= velocity.Y;
            }

            //down hit
            if (BounceAngle == 1)
            {
                velocity.Y = -1;
                pos.Y -= Velocity.Y;
            }

            //straight hit
            if (BounceAngle == 2)
            {
                velocity.Y = 0;
            }

            //Check if ball should Bounce
            Bounce();
            #endregion

            #region Move X
            //Constantly Move Ball left or Right
            if (GlideLeft == false)
                pos.X += 2;

            else if (GlideLeft == true)
                pos.X -= 2;

            #endregion

        }

        public void Draw()
        {
            #region Draw Ball

            if (pos.Y < 0)   //procation against offscreen glitch
                pos.Y += 1;

            SetCursorPosition(pos.X, pos.Y);
            Write(icon);

            #endregion
        }

        #region Ball Constructor
        //Constructor for ez implimentation
        public Ball(int initX, int initY, string newIcon)
        {
            start = new Vec2(initX, initY);
            pos = new Vec2(start.X, start.Y);
            icon = newIcon;  
        }
        #endregion

    }

}
