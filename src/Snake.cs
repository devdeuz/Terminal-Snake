using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

class Snake
{
    private static Queue<ConsoleKey> keyQueue = new Queue<ConsoleKey>();
    private static ConsoleKey lastKey;

    private static List<Point> snakeChain = new List<Point>();
    private static Point foodPosition = new Point();
    private static Point moveDirection = new Point();
    private static Random rnd = new Random();

    private static int score;
    private static string snakeChunk;
    private static bool dead;
    
    private static string snakeChunkAlive = "●";    //■
    private static string snakeChunkDead = "○";     //+
    private static string food = "■";               //♥

    private static string gameOver =
    " ╚═══════════════════════════════════════════════════════════╝\n" +
    "                      Press [F5] for new game                 \n" +
    " ╔═══════════════════════════════════════════════════════════╗";

    private static string gameBoard =
    " ╔═══════════════════════════════════════════════════════════╗    ╔═════════════╗\n" +
    " ║                                                           ║    ║             ║\n" +
    " ║                                                           ║    ║    Score    ║\n" +
    " ║                                                           ║    ║             ║\n" +
    " ║                                                           ║    ║             ║\n" +
    " ║                                                           ║    ║             ║\n" +
    " ║                                                           ║    ╚═════════════╝\n" +
    " ║                                                           ║                   \n" +
    " ║                                                           ║    ╔═════════════╗\n" +
    " ║                                                           ║    ║ [Esc] Close ║\n" +
    " ║                                                           ║    ╚═════════════╝\n" +
    " ║                                                           ║                   \n" +
    " ║                                                           ║    ╔═════════════╗\n" +
    " ║                                                           ║    ║             ║\n" +
    " ║                                                           ║    ║  S N A K E  ║\n" +
    " ║                                                           ║    ║             ║\n" +
    " ╚═══════════════════════════════════════════════════════════╝    ╚═════════════╝\n ";

    private static void Main()
    {
        Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.CursorVisible = false;
        Console.Clear();

        Task KeyListenerThread = new Task(KeyListener);
        KeyListenerThread.Start();
        
        GameReset();
        GameLoop();
    }

    private static void KeyListener()
    {
        ConsoleKey currentKey;

        while (true)
        {
            currentKey = Console.ReadKey(true).Key;
            
            if (currentKey != lastKey)
                keyQueue.Enqueue(currentKey);
            lastKey = currentKey;
        }
    }

    private static void GameReset()
    {
        score = 0;
        moveDirection.X = 1;
        moveDirection.Y = 0;
        snakeChunk = snakeChunkAlive;
        foodPosition = new Point(10, 12);

        snakeChain.Clear();
        snakeChain.Add(new Point(5, 10));  
        snakeChain.Add(new Point(6, 10));
        snakeChain.Add(new Point(7, 10));

        dead = false;
    }

    private static void GameLoop()
    {
        Point head;

        while (true)
        {
            ProcessInputQueue();

            if (!dead)
            {
                head = MoveSnake(snakeChain[snakeChain.Count - 1]);
                dead = CollisionDetected(head);
                CheckForFood(head);
                DrawFrame();

                Thread.Sleep(75);
            }
            else
            {
                Thread.Sleep(750);
                Draw(1, 8, gameOver);
            }
        }
    }

    private static void ProcessInputQueue()
    {
        if (keyQueue.Count > 0)
        {
            ConsoleKey currentKey = keyQueue.Peek();

            if (currentKey == ConsoleKey.W && moveDirection.Y != -1)
            {
                moveDirection.Y = 1;
                moveDirection.X = 0;
            }
            else if (currentKey == ConsoleKey.A && moveDirection.X != 1)
            {
                moveDirection.Y = 0;
                moveDirection.X = -1;
            }
            else if (currentKey == ConsoleKey.S && moveDirection.Y != 1)
            {
                moveDirection.Y = -1;
                moveDirection.X = 0;
            }
            else if (currentKey == ConsoleKey.D && moveDirection.X != -1)
            {
                moveDirection.Y = 0;
                moveDirection.X = 1;
            }
            else if (currentKey == ConsoleKey.F5 && dead)
            {
                GameReset();
            }
            else if (currentKey == ConsoleKey.Escape)
            {
                Console.Clear();
                Console.CursorVisible = true;
                Environment.Exit(0);
            }

            keyQueue.Dequeue();
        }
    }

    private static Point MoveSnake(Point head)
    {
        snakeChain.Add(new Point(head.X + moveDirection.X, head.Y - moveDirection.Y));
        snakeChain.RemoveAt(0);
        return snakeChain[snakeChain.Count - 1];
    }

    private static bool CollisionDetected(Point head)
    {
        if (SnakeOverlaps(head, true) || (head.X <= 2 || head.X >= 31 || head.Y <= 1 || head.Y >= 17))
        {
            snakeChunk = snakeChunkDead;
            lastKey = ConsoleKey.F1;
            return true;
        }
        return false;
    }

    private static void CheckForFood(Point head)
    {
        if (head.Equals(foodPosition))
        {
            do
            {
                foodPosition = new Point(rnd.Next(4, 28), rnd.Next(2, 15));
            }
            while (SnakeOverlaps(foodPosition, false));

            snakeChain.Insert(0, snakeChain[0]);
            score++;
        }
    }

    private static bool SnakeOverlaps(Point point, bool ignoreHead)
    {
        int checkUntil = snakeChain.Count;

        if (ignoreHead)
            checkUntil--;

        for (int i = 0; i < checkUntil; i++)
        {
            if (point.Equals(snakeChain[i]))
                return true;
        }
        return false;
    }

    private static void Draw(int X, int Y, string obj)
    {
        Console.SetCursorPosition((X - 1) * 2, Y - 1);
        Console.Write(obj);
    }

    private static void DrawFrame()
    {
        Console.SetCursorPosition(0, 0);

        Console.Write(gameBoard);

        Draw(36, 5, " $ " + score.ToString());

        Draw(foodPosition.X, foodPosition.Y, food);

        foreach (Point chunkPos in snakeChain)
        {
            Draw(chunkPos.X, chunkPos.Y, snakeChunk);
        }
    }
}

struct Point
{
    public int X;
    public int Y;

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}