using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace worm
{
    public enum Direction
    {
        up,
        down,
        right,
        left
    }
    public struct Position
    {
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(Object obj)
        {
            Position pos2 = (Position)obj;
            return (this.x == pos2.x && this.y == pos2.y);
        }

        public static bool operator ==(Position pos1, Position pos2)
        {
            return pos1.Equals(pos2);
        }

        public static bool operator !=(Position pos1, Position pos2)
        {
            return !pos1.Equals(pos2);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.x, this.y);
        }
        public int x { get; set; }
        public int y { get; set; }
    }
    public class Worm
    {
        const int initialSize = 3;
        public int size = initialSize;
        public Position currentPosition = new Position(15, 10);
        public Direction direction = Direction.right;
        public Direction wentDirection = Direction.right;
        public Queue<Position> body = new Queue<Position>();

        public Worm()
        {
            body.Enqueue(currentPosition);
            body.Enqueue(currentPosition);
            body.Enqueue(currentPosition);
        }

        public bool Move()
        {
            switch (this.direction)
            {
                case Direction.up:
                    currentPosition.y -= 1;
                    break;
                case Direction.down:
                    currentPosition.y += 1;
                    break;
                case Direction.left:
                    currentPosition.x -= 1;
                    break;
                case Direction.right:
                    currentPosition.x += 1;
                    break;
            }
            if (body.Count == size)
                body.Dequeue();
            foreach (var point in body)
            {
                if (currentPosition == point)
                {
                    return false;
                }
            }
            body.Enqueue(currentPosition);
            wentDirection = direction;
            return true;
        }

        public void Eat()
        {
            size++;
        }

        public void TrocaDirection(Direction newDirection)
        {
            switch (newDirection)
            {
                case Direction.up:
                    if (wentDirection == Direction.down)
                        return;
                    break;
                case Direction.left:
                    if (wentDirection == Direction.right)
                        return;
                    break;
                case Direction.right:
                    if (wentDirection == Direction.left)
                        return;
                    break;
                case Direction.down:
                    if (wentDirection == Direction.up)
                        return;
                    break;
            }
            this.direction = newDirection;

        }
    }

    public class GameScene
    {
        const double verticalFactor = 3.0 / 8.0;
        const int width = 50;
        int height = (int)(width * verticalFactor);

        public Worm worm = new Worm();
        public Random rand = new Random();
        public Position food;

        public GameScene()
        {
            Draw();
            SpawnFood();
        }

        public void SpawnFood()
        {
            do
            {
                food.x = rand.Next(1, width - 1);
                food.y = rand.Next(1, height);
            } while (worm.body.Any(pos => pos == food));

            Console.SetCursorPosition(food.x, food.y);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("O");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void Draw()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(new String('-', width));
            for (int ii = 0; ii < height; ii++)
            {
                Console.Write("|");
                Console.Write(new String(' ', width - 2));
                Console.WriteLine("|");
            }
            Console.WriteLine(new String('-', width));
            Console.Write("Score: 0");
        }

        void DrawWorm()
        {
            Console.SetCursorPosition(worm.currentPosition.x, worm.currentPosition.y);
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("O");
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public void UpdateScore()
        {
            Console.SetCursorPosition(7, height + 2);
            Console.Write(worm.size - 3);
        }


        public void MainLoop()
        {
            Console.CursorVisible = false;
            DrawWorm();
            while (true)
            {
                Position rabo = worm.body.Peek();
                if (worm.Move() == false ||
                    worm.currentPosition.x < 1 ||
                    worm.currentPosition.x > (width - 2) ||
                    worm.currentPosition.y < 1 ||
                    worm.currentPosition.y > (height))
                {
                    break;
                }
                DrawWorm();
                if (worm.currentPosition == food)
                {
                    worm.Eat();
                    UpdateScore();
                    SpawnFood();
                }
                if (rabo != worm.currentPosition)
                    Console.SetCursorPosition(rabo.x, rabo.y);
                Console.Write(" ");
                Console.SetCursorPosition(width - 1, height + 2);
                Console.Write(" ");
                
                System.Threading.Thread.Sleep(100);
            }
            Console.SetCursorPosition(0, height + 3);
            Console.WriteLine("You're dead!");
            Console.ResetColor();
        }
    }
    class Program
    {
        static void ReadKeyboardAsync(Worm worm)
        {
            while (true)
            {
                var tecla = Console.ReadKey(true);

                switch (tecla.Key)
                {
                    case ConsoleKey.UpArrow:
                        worm.TrocaDirection(Direction.up);
                        break;
                    case ConsoleKey.DownArrow:
                        worm.TrocaDirection(Direction.down);
                        break;
                    case ConsoleKey.LeftArrow:
                        worm.TrocaDirection(Direction.left);
                        break;
                    case ConsoleKey.RightArrow:
                        worm.TrocaDirection(Direction.right);
                        break;
                    default:
                        break;
                }
            }
        }

        static void resetColorsCtrlC(object sender, ConsoleCancelEventArgs args)
        {
            Console.ResetColor();
        }
        static void Main(string[] args)
        {
            Console.CancelKeyPress += new ConsoleCancelEventHandler(resetColorsCtrlC);

            // If there is any error reset the colors
            try
            {
                GameScene gameScene = new GameScene();
                var task = Task.Run(() =>
                {
                    ReadKeyboardAsync(gameScene.worm);
                });
                gameScene.MainLoop();
            }
            catch
            {
                Console.ResetColor();
            }
        }


    }
}
