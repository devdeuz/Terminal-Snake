using System;
using System.Collections.Generic;
using System.Threading;


namespace Snake
{
    class Snake
    {
        // Logic //

        public static bool MoveX = true;
        public static bool Death = false;
        public static bool StopGameDraw = false;

        // Values //

        public static int MoveDirection = 1;
        public static int Score;

        // Objects //

        public static List<Point> SnakeChain = new List<Point>();
        public static ConsoleGrid ConsoleGrid = new ConsoleGrid();
        public static Point Monkey = new Point(10, 12);

        public static Queue<ConsoleKey> Key_Queue = new Queue<ConsoleKey>();
        public static ConsoleKey PRESSED;
        public static ConsoleKey LAST_PRESSED;

        // Char Set //

        public static char SnakeAliveSegment = '■';
        public static char SnakeDeathSegment = '+';
        public static char MonkeyChar = 'X';

        public static string gameOver = " ╚═══════════════════════════════════════════════════════════╝\n" +
                    "                      Press [F5] for new game                    \n" +
                    " ╔═══════════════════════════════════════════════════════════╗";

        public static string gameBoard =
        " ╔═══════════════════════════════════════════════════════════╗    ╔════════════╗\n" +
        " ║                                                           ║    ║            ║\n" +
        " ║                                                           ║    ║   Score:   ║\n" +
        " ║                                                           ║    ║            ║\n" +
        " ║                                                           ║    ║            ║\n" +
        " ║                                                           ║    ║            ║\n" +
        " ║                                                           ║    ╚════════════╝\n" +
        " ║                                                           ║                  \n" +
        " ║                                                           ║    ╔════════════╗\n" +
        " ║                                                           ║    ║ [Esc] Exit ║\n" +
        " ║                                                           ║    ╚════════════╝\n" +
        " ║                                                           ║                  \n" +
        " ║                                                           ║    ╔════════════╗\n" +
        " ║                                                           ║    ║            ║\n" +
        " ║                                                           ║    ║  SNAKE NG  ║\n" +
        " ║                                                           ║    ║            ║\n" +
        " ╚═══════════════════════════════════════════════════════════╝    ╚════════════╝\n ";


        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "--help")
            {
                Console.WriteLine("Start with '-a' for ASCII mode");
                Environment.Exit(0);
            }
            if (args.Length > 0 && args[0] == "-a")
            {
                gameBoard =
                " +-----------------------------------------------------------+    +------------+\n" +
                " |                                                           |    |            |\n" +
                " |                                                           |    |   Score:   |\n" +
                " |                                                           |    |            |\n" +
                " |                                                           |    |            |\n" +
                " |                                                           |    |            |\n" +
                " |                                                           |    +------------+\n" +
                " |                                                           |                  \n" +
                " |                                                           |    +------------+\n" +
                " |                                                           |    | [Esc] Exit |\n" +
                " |                                                           |    +------------+\n" +
                " |                                                           |                  \n" +
                " |                                                           |    +------------+\n" +
                " |                                                           |    |            |\n" +
                " |                                                           |    |  SNAKE NG  |\n" +
                " |                                                           |    |            |\n" +
                " +-----------------------------------------------------------+    +------------+\n ";

                gameOver = " +-----------------------------------------------------------+\n" +
                    "                      Press [F5] for new game                    \n" +
                    " +-----------------------------------------------------------+";
            }


            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            Console.Clear();

            Key_Queue.Enqueue(ConsoleKey.D);

            Thread KeyListenerThread = new Thread(KeyListener);
            KeyListenerThread.Start();

            Init();
            GameLoop();
        }


        public static void Init()
        {
            SnakeChain.Add(new Point(5, 10));    //Build first Snake
            SnakeChain.Add(new Point(6, 10));
            SnakeChain.Add(new Point(7, 10));

            LAST_PRESSED = ConsoleKey.D;

            DrawGame();                         //Draw Init Game Board
        }

        public static void KeyListener()
        {
            while (true)
            {
                PRESSED = Console.ReadKey(true).Key;

                if (PRESSED != LAST_PRESSED)
                {
                    if (PRESSED == ConsoleKey.S && LAST_PRESSED == ConsoleKey.W)
                    {
                        PRESSED = LAST_PRESSED;
                    }
                    if (PRESSED == ConsoleKey.W && LAST_PRESSED == ConsoleKey.S)
                    {
                        PRESSED = LAST_PRESSED;
                    }
                    if (PRESSED == ConsoleKey.A && LAST_PRESSED == ConsoleKey.D)
                    {
                        PRESSED = LAST_PRESSED;
                    }
                    if (PRESSED == ConsoleKey.D && LAST_PRESSED == ConsoleKey.A)
                    {
                        PRESSED = LAST_PRESSED;
                    }

                    LAST_PRESSED = PRESSED;
                    Key_Queue.Enqueue(PRESSED);
                }
            }
        }

        public static void GameLoop()
        {
            while (true)
            {
                if (Key_Queue.Count > 0 && Key_Queue.Peek() == ConsoleKey.W)
                {
                    MoveDirection = -1;
                    MoveX = false;
                    Key_Queue.Dequeue();
                }
                else if (Key_Queue.Count > 0 && Key_Queue.Peek() == ConsoleKey.A)
                {
                    MoveDirection = -1;
                    MoveX = true;
                    Key_Queue.Dequeue();
                }
                else if (Key_Queue.Count > 0 && Key_Queue.Peek() == ConsoleKey.S)
                {
                    MoveDirection = 1;
                    MoveX = false;
                    Key_Queue.Dequeue();
                }
                else if (Key_Queue.Count > 0 && Key_Queue.Peek() == ConsoleKey.D)
                {
                    MoveDirection = 1;
                    MoveX = true;
                    Key_Queue.Dequeue();
                }
                else if (PRESSED == ConsoleKey.F5)            //Start New Game with F5
                {
                    Score = 0;
                    SnakeChain.Clear();
                    Key_Queue.Clear();

                    StopGameDraw = false;
                    Death = false;

                    MoveDirection = 1;
                    MoveX = true;

                    Init();
                }
                else if (PRESSED == ConsoleKey.Escape)            //Start New Game with F5
                {
                    Console.CursorVisible = true;
                    Environment.Exit(0);
                }


                if (MoveX)
                {
                    SnakeChain.Add(new Point(SnakeChain[SnakeChain.Count - 1].X + MoveDirection, SnakeChain[SnakeChain.Count - 1].Y));  //Move on X
                    SnakeChain.RemoveAt(0);
                }
                else
                {
                    SnakeChain.Add(new Point(SnakeChain[SnakeChain.Count - 1].X, SnakeChain[SnakeChain.Count - 1].Y + MoveDirection));  //Move on Y
                    SnakeChain.RemoveAt(0);
                }

                for (int i = 0; i < SnakeChain.Count; i++)                              //Check for Snake Self-Collision
                {
                    if (SnakeChain[SnakeChain.Count - 1].Equals(SnakeChain[i]) && i != SnakeChain.Count - 1)
                    {
                        Death = true;
                    }
                }

                if (SnakeChain[SnakeChain.Count - 1].X <= 2 || SnakeChain[SnakeChain.Count - 1].X >= 31 || SnakeChain[SnakeChain.Count - 1].Y <= 1 || SnakeChain[SnakeChain.Count - 1].Y >= 17)     //Check if snake is out of bounds
                {
                    Death = true;
                }


                if (SnakeChain[SnakeChain.Count - 1].Equals(Monkey))                        //If Snake eats Monkey [Food]
                {
                    Random rnd = new Random();
                    Monkey = new Point(rnd.Next(4, 28), rnd.Next(2, 15));                   //Spawn new Monkey

                    for (int i = 0; i < SnakeChain.Count; i++)                              //Spawn-No-Food-On-Snake-Protection
                    {
                        while (Monkey.Equals(SnakeChain[i]))
                        {
                            Monkey = new Point(rnd.Next(4, 28), rnd.Next(2, 15));           //Spawn new Monkey
                        }
                    }

                    SnakeChain.Insert(0, SnakeChain[0]);                                     //Add Snake Segment
                    Score += 8;
                }

                DrawGame();

                Thread.Sleep(75);       //Not the best idea but it works :)
            }
        }



        static void DrawGame()
        {
            if (!StopGameDraw)
            {
                Console.SetCursorPosition(0, 0);

                Console.Write(gameBoard);


                ConsoleGrid.Instantiate(36, 5, "[" + Score.ToString() + "]");

                if (Death)
                {
                    for (int i = 0; i < SnakeChain.Count; i++)
                    {
                        ConsoleGrid.Instantiate(SnakeChain[i].X, SnakeChain[i].Y, SnakeDeathSegment);
                        StopGameDraw = true;
                    }

                    Thread.Sleep(1000);

                    ConsoleGrid.Instantiate(1, 8, gameOver);


                }
                else
                {
                    for (int i = 0; i < SnakeChain.Count; i++)
                    {
                        ConsoleGrid.Instantiate(SnakeChain[i].X, SnakeChain[i].Y, SnakeAliveSegment);
                        ConsoleGrid.Instantiate(Monkey.X, Monkey.Y, 'X');
                    }
                }
            }
        }
    }

    struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }


    public class ConsoleGrid
    {
        public void Instantiate(int X, int Y, char obj)
        {
            string xString = obj.ToString();

            Console.SetCursorPosition((X - 1) * 2, Y - 1);

            Console.WriteLine(xString);
        }

        public void Instantiate(int X, int Y, int obj)
        {
            string xString = obj.ToString();

            Console.SetCursorPosition((X - 1) * 2, Y - 1);

            Console.WriteLine(xString);
        }

        public void Instantiate(int X, int Y, string obj)
        {
            string xString = obj;

            Console.SetCursorPosition((X - 1) * 2, Y - 1);

            Console.WriteLine(xString);
        }
    }
}