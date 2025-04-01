namespace BlazorApp1;

public abstract class AWinner
{
    protected abstract string? DiagonalWinner2();
    protected abstract string? HorizontalWinner();
    protected abstract string? VerticalWinner();
    protected abstract string? DiagonalWinner1();

    protected virtual string? CheckWinner()
    {
        string? w = HorizontalWinner();
        if (w == null) w = VerticalWinner();
        if (w == null) w = DiagonalWinner1();
        if (w == null) w = DiagonalWinner2();
        /* if (w == null)
             return "No Winner / Draw";*/

        return w;
    }
}
