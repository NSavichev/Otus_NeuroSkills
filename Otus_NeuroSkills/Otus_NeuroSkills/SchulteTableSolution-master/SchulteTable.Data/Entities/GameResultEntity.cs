using SchulteTable.Core.Enums;

namespace SchulteTable.Data.Entities;

public class GameResultEntity
{
    public int Id { get; set; }
    public int GridSize { get; set; }
    public long TimeElapsedTicks { get; set; }
    public DateTime DateCompleted { get; set; }
    public int ErrorCount { get; set; }

    // внешний ключ
    public int UserProfileId { get; set; }
    public UserProfileEntity UserProfile { get; set; } = null!;
}
