using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameOfLife
{
    class Grid
    {
        private int SizeX;
        private int SizeY;
        private Cell[,] cells;
        private Cell[,] nextGenerationCells;
        private static Random rnd;
        private Canvas drawCanvas;
        private Ellipse[,] cellsVisuals;

        public bool GraphicsNeedsUpdate { get; private set; }

        // Before
        //public Grid(Canvas c)
        //{
        //    drawCanvas = c;
        //    rnd = new Random();
        //    SizeX = (int) (c.Width / 5);
        //    SizeY = (int)(c.Height / 5);
        //    cells = new Cell[SizeX, SizeY];
        //    nextGenerationCells = new Cell[SizeX, SizeY];
        //    cellsVisuals = new Ellipse[SizeX, SizeY];

        //    for (int i = 0; i < SizeX; i++)
        //    {
        //        for (int j = 0; j < SizeY; j++)
        //        {
        //            cells[i, j] = new Cell(i, j, 0, false);
        //            nextGenerationCells[i, j] = new Cell(i, j, 0, false);
        //        }
        //    }

        //    SetRandomPattern();
        //    InitCellsVisuals();
        //    UpdateGraphics();
        //}

        // After
        public Grid(Canvas c)
        {
            drawCanvas = c;
            rnd = new Random();
            SizeX = (int)(c.Width / 5);
            SizeY = (int)(c.Height / 5);
            cells = new Cell[SizeX, SizeY];
            nextGenerationCells = new Cell[SizeX, SizeY];
            cellsVisuals = new Ellipse[SizeX, SizeY];

            GoThroughCells((i, j) =>
            {
                cells[i, j] = new Cell(i, j, 0, false);
                nextGenerationCells[i, j] = new Cell(i, j, 0, false);
            });

            SetRandomPattern();
            InitCellsVisuals();

            GraphicsNeedsUpdate = true;
            UpdateGraphics();
        }

        // Before
        //public void Clear()
        //{
        //    for (int i = 0; i < SizeX; i++)
        //    {
        //        for (int j = 0; j < SizeY; j++)
        //        {
        //            cells[i, j] = new Cell(i, j, 0, false);
        //            nextGenerationCells[i, j] = new Cell(i, j, 0, false);
        //            cellsVisuals[i, j].Fill = Brushes.Gray;
        //        }
        //    }
        //}

        // After
        public void Clear()
        {
            GoThroughCells((i, j) =>
            {
                cells[i, j] = new Cell(i, j, 0, false);
                nextGenerationCells[i, j] = new Cell(i, j, 0, false);
                cellsVisuals[i, j].Fill = Brushes.Gray;
            });
        }

        // Before
        //void MouseMove(object sender, MouseEventArgs e)
        //{
        //    var cellVisual = sender as Ellipse;

        //    int i = (int)cellVisual.Margin.Left / 5;
        //    int j = (int)cellVisual.Margin.Top / 5;

        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        if (!cells[i, j].IsAlive)
        //        {
        //            cells[i, j].IsAlive = true;
        //            cells[i, j].Age = 0;
        //            cellVisual.Fill = Brushes.White;
        //        }
        //    }
        //}

        // After 
        void MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var cellVisual = sender as Ellipse;

                var cell = cells[(int)cellVisual.Margin.Left / 5, (int)cellVisual.Margin.Top / 5];

                if (!cell.IsAlive)
                {
                    cell.IsAlive = true;
                    cell.Age = 0;
                    cellVisual.Fill = Brushes.White;
                }
            }
        }

        // Before
        // public void UpdateGraphics()
        // {
        //     for (int i = 0; i < SizeX; i++)
        //     {
        //         for (int j = 0; j < SizeY; j++)
        //         {
        //             cellsVisuals[i, j].Fill = cells[i, j].IsAlive
        //                 ? (cells[i, j].Age < 2 ? Brushes.White : Brushes.DarkGray)
        //                 : Brushes.Gray;
        //         }
        //     }
        //}

        // After
        public void UpdateGraphics()
        {
            GoThroughCells((i, j) => {
                cellsVisuals[i, j].Fill = cells[i, j].IsAlive
                    ? (cells[i, j].Age < 2
                        ? Brushes.White
                        : Brushes.DarkGray)
                    : Brushes.Gray;
            });
        }

        // Before
        //public void InitCellsVisuals()
        //{
        //    for (int i = 0; i < SizeX; i++)
        //    {
        //        for (int j = 0; j < SizeY; j++)
        //        {
        //            cellsVisuals[i, j] = new Ellipse();
        //            cellsVisuals[i, j].Width = cellsVisuals[i, j].Height = 5;
        //            double left = cells[i, j].PositionX;
        //            double top = cells[i, j].PositionY;
        //            cellsVisuals[i, j].Margin = new Thickness(left, top, 0, 0);
        //            cellsVisuals[i, j].Fill = Brushes.Gray;
        //            drawCanvas.Children.Add(cellsVisuals[i, j]);

        //            cellsVisuals[i, j].MouseMove += MouseMove;
        //            cellsVisuals[i, j].MouseLeftButtonDown += MouseMove;
        //         }
        //    }

        //    UpdateGraphics();
        //}

        // After
        //public void InitCellsVisuals()
        //{
        //    GoThroughCells((i, j) =>
        //    {
        //        var ellipse = new Ellipse();

        //        ellipse.Width = ellipse.Height = 5;
        //        double left = cells[i, j].PositionX;
        //        double top = cells[i, j].PositionY;
        //        ellipse.Margin = new Thickness(left, top, 0, 0);
        //        ellipse.Fill = Brushes.Gray;
        //        drawCanvas.Children.Add(ellipse);

        //        ellipse.MouseMove += MouseMove;
        //        ellipse.MouseLeftButtonDown += MouseMove;

        //        cellsVisuals[i, j] = ellipse;
        //    });

        //    UpdateGraphics();
        //}

        public void InitCellsVisuals()
        {
            GoThroughCells((i, j) =>
            {
                var ellipse = new Ellipse();

                ellipse.Width = ellipse.Height = 5;
                double left = cells[i, j].PositionX;
                double top = cells[i, j].PositionY;
                ellipse.Margin = new Thickness(left, top, 0, 0);
                ellipse.Fill = Brushes.Gray;
                drawCanvas.Children.Add(ellipse);

                ellipse.MouseMove += MouseMove;
                ellipse.MouseLeftButtonDown += MouseMove;

                cellsVisuals[i, j] = ellipse;
            });

            UpdateGraphics();
        }

        public static bool GetRandomBoolean()
        {
            return rnd.NextDouble() > 0.8;
        }

        // Before
        //public void SetRandomPattern()
        //{
        //    for (int i = 0; i < SizeX; i++)
        //    {
        //        for (int j = 0; j < SizeY; j++)
        //        {
        //            cells[i, j].IsAlive = GetRandomBoolean();
        //        }
        //    }
        //}

        // After
        public void SetRandomPattern()
        {
            GoThroughCells((i, j) => cells[i, j].IsAlive = GetRandomBoolean());
        }

        // Before
        //public void UpdateToNextGeneration()
        //{
        //    for (int i = 0; i < SizeX; i++)
        //    {
        //        for (int j = 0; j < SizeY; j++)
        //        {
        //            cells[i, j].IsAlive = nextGenerationCells[i, j].IsAlive;
        //            cells[i, j].Age = nextGenerationCells[i, j].Age;
        //        }
        //    }

        //    UpdateGraphics();
        //}

        // After
        public void UpdateToNextGeneration()
        {
            GoThroughCells((i, j) =>
            {
                cells[i, j].IsAlive = nextGenerationCells[i, j].IsAlive;
                cells[i, j].Age = nextGenerationCells[i, j].Age;
            });

            UpdateGraphics();
        }

        // Before
        //        public void Update()
        //        {
        //            bool alive = false;
        //            int age = 0;

        //            for (int i = 0; i < SizeX; i++)
        //            {
        //                for (int j = 0; j < SizeY; j++)
        //                {
        ////                    nextGenerationCells[i, j] = CalculateNextGeneration(i,j);          // UNOPTIMIZED
        //                    CalculateNextGeneration(i, j, ref alive, ref age);   // OPTIMIZED
        //                    nextGenerationCells[i, j].IsAlive = alive;  // OPTIMIZED
        //                    nextGenerationCells[i, j].Age = age;  // OPTIMIZED
        //                }
        //            }
        //            UpdateToNextGeneration();
        //        }

        // After
        public void Update()
        {
            bool alive = false;
            int age = 0;

            GoThroughCells((i, j) =>
            {
                CalculateNextGeneration(i, j, ref alive, ref age);
                nextGenerationCells[i, j].IsAlive = alive;
                nextGenerationCells[i, j].Age = age;

                //cells[i, j].IsAlive = nextGenerationCells[i, j].IsAlive;
                //cells[i, j].Age = nextGenerationCells[i, j].Age;
            });

            UpdateToNextGeneration();
        }

        public Cell CalculateNextGeneration(int row, int column)    // UNOPTIMIZED
        {
            bool alive;
            int count, age;

            alive = cells[row, column].IsAlive;
            age = cells[row, column].Age;
            count = CountNeighbors(row, column);

            if (alive && count < 2)
                return new Cell(row, column, 0, false);

            if (alive && (count == 2 || count == 3))
            {
                cells[row, column].Age++;
                return new Cell(row, column, cells[row, column].Age, true);
            }

            if (alive && count > 3)
                return new Cell(row, column, 0, false);
            
            if (!alive && count == 3)
                return new Cell(row, column, 0, true);

            return new Cell(row, column, 0, false);
        }

        // Before
        //public void CalculateNextGeneration(int row, int column, ref bool isAlive, ref int age)     // OPTIMIZED
        //{
        //    isAlive = cells[row, column].IsAlive;
        //    age = cells[row, column].Age;

        //    int count = CountNeighbors(row, column);

        //    if (isAlive && count < 2)
        //    {
        //        isAlive = false;
        //        age = 0;
        //    }

        //    if (isAlive && (count == 2 || count == 3))
        //    {
        //        cells[row, column].Age++;
        //        isAlive = true;
        //        age = cells[row, column].Age;
        //    }

        //    if (isAlive && count > 3)
        //    {
        //        isAlive = false;
        //        age = 0;
        //    }

        //    if (!isAlive && count == 3)
        //    {
        //        isAlive = true;
        //        age = 0;
        //    }
        //}

        // After
        public void CalculateNextGeneration(int row, int column, ref bool isAlive, ref int age)     // OPTIMIZED
        {
            var currentCell = cells[row, column];
            isAlive = currentCell.IsAlive;
            age = currentCell.Age;

            int count = CountNeighbors(row, column);

            if (isAlive)
            {
                if (count < 2)
                {
                    isAlive = false;
                    age = 0;
                }
                else if (count > 3)
                {
                    isAlive = false;
                    age = 0;
                }
                else if (count == 2 || count == 3)
                {
                    currentCell.Age++;
                    isAlive = true;
                    age = currentCell.Age;
                }
            }
            else
            {
                if (count == 3)
                {
                    isAlive = true;
                    age = 0;
                }
            }
        }

        // Before
        //public int CountNeighbors(int i, int j)
        //{
        //    int count = 0;

        //    if (i != SizeX - 1 && cells[i + 1, j].IsAlive) count++;
        //    if (i != SizeX - 1 && j != SizeY - 1 && cells[i + 1, j + 1].IsAlive) count++;
        //    if (j != SizeY - 1 && cells[i, j + 1].IsAlive) count++;
        //    if (i != 0 && j != SizeY - 1 && cells[i - 1, j + 1].IsAlive) count++;
        //    if (i != 0 && cells[i - 1, j].IsAlive) count++;
        //    if (i != 0 && j != 0 && cells[i - 1, j - 1].IsAlive) count++;
        //    if (j != 0 && cells[i, j - 1].IsAlive) count++;
        //    if (i != SizeX - 1 && j != 0 && cells[i + 1, j - 1].IsAlive) count++;

        //    return count;
        //}

        // After
        public int CountNeighbors(int i, int j)
        {
            int count = 0;
            var isMinI = i == 0;
            var isMaxI = i == SizeX - 1;
            var isMinJ = j == 0;
            var isMaxJ = j == SizeY - 1;

            if (isMinI)
            {
                if (isMinJ)
                {
                    if (cells[i + 1, j].IsAlive) count++;
                    if (cells[i + 1, j + 1].IsAlive) count++;
                    if (cells[i, j + 1].IsAlive) count++;

                    return count;
                }
                else if (isMaxJ)
                {
                    if (cells[i + 1, j].IsAlive) count++;
                    if (cells[i, j - 1].IsAlive) count++;
                    if (cells[i + 1, j - 1].IsAlive) count++;

                    return count;
                }

                if (cells[i + 1, j].IsAlive) count++;
                if (cells[i + 1, j + 1].IsAlive) count++;
                if (cells[i, j + 1].IsAlive) count++;
                if (cells[i, j - 1].IsAlive) count++;
                if (cells[i + 1, j - 1].IsAlive) count++;

                return count;
            }
            else if (isMaxI)
            {
                if (isMinJ)
                {
                    if (cells[i, j + 1].IsAlive) count++;
                    if (cells[i - 1, j + 1].IsAlive) count++;
                    if (cells[i - 1, j].IsAlive) count++;

                    return count;
                }
                else if (isMaxJ)
                {
                    if (cells[i - 1, j].IsAlive) count++;
                    if (cells[i - 1, j - 1].IsAlive) count++;
                    if (cells[i, j - 1].IsAlive) count++;

                    return count;
                }

                if (cells[i, j + 1].IsAlive) count++;
                if (cells[i - 1, j + 1].IsAlive) count++;
                if (cells[i - 1, j].IsAlive) count++;
                if (cells[i - 1, j - 1].IsAlive) count++;
                if (cells[i, j - 1].IsAlive) count++;

                return count;
            }
            else if (isMinJ)
            {
                if (cells[i + 1, j].IsAlive) count++;
                if (cells[i + 1, j + 1].IsAlive) count++;
                if (cells[i, j + 1].IsAlive) count++;
                if (cells[i - 1, j + 1].IsAlive) count++;
                if (cells[i - 1, j].IsAlive) count++;

                return count;
            }
            else if (isMaxJ)
            {
                if (cells[i + 1, j].IsAlive) count++;
                if (cells[i - 1, j].IsAlive) count++;
                if (cells[i - 1, j - 1].IsAlive) count++;
                if (cells[i, j - 1].IsAlive) count++;
                if (cells[i + 1, j - 1].IsAlive) count++;

                return count;
            }

            if (cells[i + 1, j].IsAlive) count++;
            if (cells[i + 1, j + 1].IsAlive) count++;
            if (cells[i, j + 1].IsAlive) count++;
            if (cells[i - 1, j + 1].IsAlive) count++;
            if (cells[i - 1, j].IsAlive) count++;
            if (cells[i - 1, j - 1].IsAlive) count++;
            if (cells[i, j - 1].IsAlive) count++;
            if (cells[i + 1, j - 1].IsAlive) count++;

            return count;
        }

        private void GoThroughCells(Action<int, int> action)
        {
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    action(i, j);
                }
            }
        }
    }
}