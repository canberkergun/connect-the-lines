public class GridObject<T>
{
    private GridSystem2D<GridObject<T>> grid;
    private int x;
    private int y;
    private T content;

    public GridObject(GridSystem2D<GridObject<T>> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        content = default;
    }

    public int X => x;
    public int Y => y;
    
    public T Content
    {
        get => content;
        set => content = value;
    }

    public void SetContent(T content)
    {
        this.content = content;
        grid.SetValue(x, y, this); // Update the grid with the new content
    }

    public void ClearContent()
    {
        content = default;
        grid.SetValue(x, y, this); // Update the grid to reflect the cleared content
    }

    public override string ToString()
    {
        return $"GridObject({x}, {y}): {content}";
    }
}