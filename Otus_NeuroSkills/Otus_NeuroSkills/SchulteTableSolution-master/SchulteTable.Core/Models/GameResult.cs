namespace SchulteTable.Core.Models;

// Результат игры
public class GameResult
{
    public int Id { get; set; }
    public int GridSize { get; set; }
    public long TimeElapsedTicks { get; set; }
    public DateTime DateCompleted { get; set; }
    public int ErrorCount { get; set; }
    public string UserName { get; set; } = string.Empty;

    // Время в нормальном виде
    public TimeSpan TimeElapsed => TimeSpan.FromTicks(TimeElapsedTicks);
}
