class Program
{
    static List<Position> sneak = new List<Position>();
    static int[,] map;
    static int minX, maxX, minY, maxY;
    static public ConsoleKeyInfo key;
    static public bool gamePlay = true;

    static void Main(string[] args)
    {
        char ok;
        do
        {
            gamePlay = true;
            sneak.Clear();
            StartGame();
            while (gamePlay)
            {
                Thread.Sleep(500);
                RunGame();
            }
            System.Console.WriteLine("Game Over");
            System.Console.Write("Play Again: ");
            ok = Console.ReadKey().KeyChar;
        } while (ok == 'y');
    }

    static public void StartGame()
    {
        Random random = new Random();
        map = new int[10, 30];
        minX = 0;
        maxX = map.GetLength(1) - 1;
        minY = 0;
        maxY = map.GetLength(0) - 1;
        CreatePoint();
        Position firstLocation = new Position((minX + maxX) / 2, (minY + maxY) / 2);
        map[firstLocation.y, firstLocation.x] = 1;
        sneak.Add(firstLocation);
        key = new ConsoleKeyInfo(
            (char)0,             // Ký tự
            ConsoleKey.LeftArrow, // Mã phím
            false,               // Alt
            false,               // Ctrl
            false                // Shift
        );
        new Thread(() =>
        {
            while (gamePlay)
            {
                key = Console.ReadKey(true);
            }
        }).Start();
    }
    static public void Display()
    {
        Console.Clear();
        System.Console.WriteLine($"Count : {sneak.Count()}");
        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                if (x == minX || x == maxX || y == minY || y == maxY)
                {
                    System.Console.Write("█");
                }
                else if (map[y, x] == 0)
                {
                    System.Console.Write(" ");
                }
                else if (map[y, x] == 1)
                {
                    System.Console.Write("■");
                }
                else if (map[y, x] == 2)
                {
                    System.Console.Write("@");
                }
            }
            System.Console.WriteLine();
        }
    }

    static public void RunGame()
    {
        switch (key.Key)
        {
            case ConsoleKey.LeftArrow:
                sneak.Insert(0, new Position(sneak[0].x - 1, sneak[0].y));
                break;
            case ConsoleKey.RightArrow:
                sneak.Insert(0, new Position(sneak[0].x + 1, sneak[0].y));
                break;
            case ConsoleKey.UpArrow:
                sneak.Insert(0, new Position(sneak[0].x, sneak[0].y - 1));
                break;
            case ConsoleKey.DownArrow:
                sneak.Insert(0, new Position(sneak[0].x, sneak[0].y + 1));
                break;
        }
        Position sneakHead = sneak[0];

        if ((sneakHead.x <= minX) ||
                    (sneakHead.x >= maxX) ||
                    (sneakHead.y <= minY) ||
                    (sneakHead.y >= maxY) ||
                    (map[sneakHead.y, sneakHead.x] == 1))
        {
            gamePlay = false;
            return;
        }
        else if (map[sneakHead.y, sneakHead.x] != 2)
        {
            Position sneakLast = sneak[sneak.Count() - 1];
            map[sneakLast.y, sneakLast.x] = 0;
            sneak.Remove(sneakLast);
        }
        else CreatePoint();
        map[sneakHead.y, sneakHead.x] = 1;
        Display();
    }

    public static void CreatePoint()
    {
        Random random = new Random();
        map[random.Next(minY, maxY), random.Next(minX, maxX)] = 2;
    }


    class Position
    {
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int x;
        public int y;
    }
}
