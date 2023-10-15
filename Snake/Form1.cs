using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace Snake
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
    }

}

public partial class SnakeGame : Form
{
    private List<Point> snake;
    private Point food;
    private int directionX = 0;
    private int directionY = 1;
    private int gridSize = 20;
    private int snakeSize = 5;
    private Random random = new Random();
    private Timer gameTimer;

    public SnakeGame()
    {
        InitializeComponent();
        snake = new List<Point>();
        snake.Add(new Point(5, 5)); // Initial snake position
        food = GenerateFood();

        gameTimer = new Timer();
        gameTimer.Interval = 100; // Adjust the speed of the game
        gameTimer.Tick += new EventHandler(Update);
        gameTimer.Start();

        this.KeyDown += new KeyEventHandler(OnKeyDown);
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        // Handle arrow key input to change the snake's direction
        switch (e.KeyCode)
        {
            case Keys.Left:
                if (directionX != 1)
                {
                    directionX = -1;
                    directionY = 0;
                }
                break;
            case Keys.Right:
                if (directionX != -1)
                {
                    directionX = 1;
                    directionY = 0;
                }
                break;
            case Keys.Up:
                if (directionY != 1)
                {
                    directionX = 0;
                    directionY = -1;
                }
                break;
            case Keys.Down:
                if (directionY != -1)
                {
                    directionX = 0;
                    directionY = 1;
                }
                break;
        }
    }

    private void Update(object sender, EventArgs e)
    {
        // Update snake's position
        Point newHead = new Point(snake[0].X + directionX, snake[0].Y + directionY);

        // Check if the snake eats the food
        if (newHead == food)
        {
            snake.Add(food);
            food = GenerateFood();
        }
        else
        {
            // Remove the last segment of the snake
            snake.RemoveAt(snake.Count - 1);
        }

        // Check for collisions (e.g., wall or snake itself)
        if (newHead.X < 0 || newHead.X >= ClientSize.Width / gridSize ||
            newHead.Y < 0 || newHead.Y >= ClientSize.Height / gridSize ||
            snake.Contains(newHead))
        {
            gameTimer.Stop();
            MessageBox.Show("Game Over");
            return;
        }

        // Add the new head to the snake
        snake.Insert(0, newHead);

        // Invalidate the form to trigger the Paint event
        Invalidate();
    }

    private Point GenerateFood()
    {
        // Generate random coordinates for food
        int maxX = ClientSize.Width / gridSize;
        int maxY = ClientSize.Height / gridSize;
        int x, y;
        do
        {
            x = random.Next(maxX);
            y = random.Next(maxY);
        } while (snake.Contains(new Point(x, y)));

        return new Point(x, y);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        // Draw the snake
        foreach (Point segment in snake)
        {
            e.Graphics.FillRectangle(Brushes.Green, segment.X * gridSize, segment.Y * gridSize, gridSize, gridSize);
        }

        // Draw the food
        e.Graphics.FillEllipse(Brushes.Red, food.X * gridSize, food.Y * gridSize, gridSize, gridSize);
    }

    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new SnakeGame());
    }
}




