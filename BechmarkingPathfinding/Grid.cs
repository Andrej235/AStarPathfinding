using System;

public class Grid<T>
{
    public class OnGridCellValueChangedEventArgs : EventArgs
    {
        public OnGridCellValueChangedEventArgs(int x, int y, T newValue)
        {
            X = x;
            Y = y;
            NewValue = newValue;
        }

        public int X { get; }
        public int Y { get; }
        public T NewValue { get; }
    }
    public event EventHandler<OnGridCellValueChangedEventArgs>? OnCellValueChanged;

    public int Width { get; }
    public int Height { get; }
    public int CellSize { get; }

    private readonly T[,] gridArray;

    public Grid(int width, int height, int cellSize, Func<Grid<T>, int, int, T> createGridObject)
    {
        Width = width;
        Height = height;
        CellSize = cellSize;

        gridArray = new T[width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                gridArray[x, y] = createGridObject(this, x, y);
    }

    public virtual T this[int x, int y]
    {
        get
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
                return default;

            return gridArray[x, y];
        }

        set
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
                return;

            gridArray[x, y] = value;
            OnCellValueChanged?.Invoke(this, new(x, y, value));
        }
    }
}
