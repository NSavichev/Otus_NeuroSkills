using SchulteTable.Core.Enums;

namespace SchulteTable.Data.Entities;

public class UserProfileEntity
{
    public int Id { get; set; }
    public string UserName { get; set; } = "Player";
    public int PreferredGridSize { get; set; } = 5;
    public ThemeMode PreferredTheme { get; set; } = ThemeMode.Auto;
    public List<GameResultEntity> GameResults { get; set; } = new();
}
