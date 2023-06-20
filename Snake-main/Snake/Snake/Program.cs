using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;

namespace Snake
{
    struct Position
    {
        public int row;
        public int col;

        public Position(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            byte right = 0;
            byte left = 1;
            byte down = 2;
            byte up = 3;
            int lastFoodTime = 0;
            int foodDisappearTime = 8000;
            int negativePoints = 0;

            Position[] directions = new Position[]
            {
                new Position(0, 1), // right
                new Position(0, -1), // left
                new Position(1, 0), // down
                new Position(-1, 0), // up
            };

            double sleepTime = 100;
            int direction = right;
            Random randomNumbersGenerator = new Random();
            Console.BufferHeight = Console.WindowHeight;
            lastFoodTime = Environment.TickCount;

            // Apply console appearance changes
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Clear();

            Console.WriteLine("Welcome to the Snake game!");
            Console.WriteLine("Rules:");
            Console.WriteLine("- Control the snake using arrow keys.");
            Console.WriteLine("- Eat '*' to earn points.");
            Console.WriteLine("- Avoid obstacles ('#') and do not collide with yourself.");
            Console.WriteLine("- Each eaten '*' increases the snake's speed.");
            Console.WriteLine("- You have limited time to eat each '*'.");
            Console.WriteLine("Press any key to start!");

            Console.ReadKey();
            Console.Clear();

            List<Position> obstacles = GenerateObstacles();

            foreach (Position obstacle in obstacles)
            {
                Console.SetCursorPosition(obstacle.col, obstacle.row);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("#");
            }

            Queue<Position> snakeElements = new Queue<Position>();
            for (int i = 0; i <= 5; i++)
            {
                snakeElements.Enqueue(new Position(0, i));
            }

            Position food;
            do
            {
                food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                    randomNumbersGenerator.Next(0, Console.WindowWidth));
            }
            while (snakeElements.Contains(food) || obstacles.Contains(food));
            Console.SetCursorPosition(food.col, food.row);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("*");

            foreach (Position position in snakeElements)
            {
                Console.SetCursorPosition(position.col, position.row);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("O");
            }

            while (true)
            {
                negativePoints++;

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo userInput = Console.ReadKey();
                    if (userInput.Key == ConsoleKey.LeftArrow && direction != right)
                    {
                        direction = left;
                    }
                    if (userInput.Key == ConsoleKey.RightArrow && direction != left)
                    {
                        direction = right;
                    }
                    if (userInput.Key == ConsoleKey.UpArrow && direction != down)
                    {
                        direction = up;
                    }
                    if (userInput.Key == ConsoleKey.DownArrow && direction != up)
                    {
                        direction = down;
                    }
                }

                Position snakeHead = snakeElements.Last();
                Position nextDirection = directions[direction];

                Position snakeNewHead = new Position(snakeHead.row + nextDirection.row,
                    snakeHead.col + nextDirection.col);

                if (snakeNewHead.col < 0)
                {
                    snakeNewHead.col = Console.WindowWidth - 1;
                }
                if (snakeNewHead.row < 0)
                {
                    snakeNewHead.row = Console.WindowHeight - 1;
                }
                if (snakeNewHead.row >= Console.WindowHeight)
                {
                    snakeNewHead.row = 0;
                }
                if (snakeNewHead.col >= Console.WindowWidth)
                {
                    snakeNewHead.col = 0;
                }

                if (snakeElements.Contains(snakeNewHead) || obstacles.Contains(snakeNewHead))
                {
                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Game over!");
                    int userPoints = (snakeElements.Count - 6) * 100 - negativePoints;
                    userPoints = Math.Max(userPoints, 0);
                    Console.WriteLine("Your points: {0}", userPoints);
                    Console.WriteLine("Press any key to play again.");
                    Console.ReadKey();
                    return;
                }

                Console.SetCursorPosition(snakeHead.col, snakeHead.row);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("O");

                snakeElements.Enqueue(snakeNewHead);
                Console.SetCursorPosition(snakeNewHead.col, snakeNewHead.row);
                Console.ForegroundColor = ConsoleColor.Gray;
                if (direction == right) Console.Write(">");
                if (direction == left) Console.Write("<");
                if (direction == up) Console.Write("^");
                if (direction == down) Console.Write("v");

                if (snakeNewHead.col == food.col && snakeNewHead.row == food.row)
                {
                    do
                    {
                        food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                            randomNumbersGenerator.Next(0, Console.WindowWidth));
                    }
                    while (snakeElements.Contains(food) || obstacles.Contains(food));
                    lastFoodTime = Environment.TickCount;
                    Console.SetCursorPosition(food.col, food.row);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("*");
                    sleepTime--;

                    obstacles = GenerateObstacles();
                    foreach (Position obstacle in obstacles)
                    {
                        Console.SetCursorPosition(obstacle.col, obstacle.row);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("#");
                    }
                }
                else
                {
                    Position last = snakeElements.Dequeue();
                    Console.SetCursorPosition(last.col, last.row);
                    Console.Write(" ");
                }

                if (Environment.TickCount - lastFoodTime >= foodDisappearTime)
                {
                    negativePoints = negativePoints + 50;
                    Console.SetCursorPosition(food.col, food.row);
                    Console.Write(" ");
                    do
                    {
                        food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                            randomNumbersGenerator.Next(0, Console.WindowWidth));
                    }
                    while (snakeElements.Contains(food) || obstacles.Contains(food));
                    lastFoodTime = Environment.TickCount;
                }

                Console.SetCursorPosition(food.col, food.row);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("*");

                sleepTime -= 0.01;

                Thread.Sleep((int)sleepTime);
            }
        }

        static List<Position> GenerateObstacles()
        {
            Random random = new Random();
            List<Position> obstacles = new List<Position>();

            int obstacleCount = random.Next(5, 11); // Generate random obstacle count between 5 and 10

            for (int i = 0; i < obstacleCount; i++)
            {
                Position obstacle = new Position(random.Next(0, Console.WindowHeight),
                    random.Next(0, Console.WindowWidth));
                obstacles.Add(obstacle);
            }

            return obstacles;
        }
    }
}
