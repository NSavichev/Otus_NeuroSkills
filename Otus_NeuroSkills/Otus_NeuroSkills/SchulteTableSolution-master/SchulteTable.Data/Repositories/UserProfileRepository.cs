using Microsoft.EntityFrameworkCore;
using SchulteTable.Data.Entities;

namespace SchulteTable.Data.Repositories;

public class UserProfileRepository : IUserProfileRepository
{
    private readonly AppDbContext _context;

    public UserProfileRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfileEntity> GetOrCreateAsync(string userName)
    {
        var user = await _context.UserProfiles
            .FirstOrDefaultAsync(u => u.UserName == userName);

        if (user == null)
        {
            user = new UserProfileEntity
            {
                UserName = userName,
                PreferredGridSize = 5,
                PreferredTheme = Core.Enums.ThemeMode.Auto
            };

            _context.UserProfiles.Add(user);
            await _context.SaveChangesAsync();
        }

        return user;
    }

    public async Task<UserProfileEntity> UpdateAsync(UserProfileEntity userProfile)
    {
        _context.UserProfiles.Update(userProfile);
        await _context.SaveChangesAsync();
        return userProfile;
    }

    public async Task<UserProfileEntity?> GetByNameAsync(string userName)
    {
        return await _context.UserProfiles
            .FirstOrDefaultAsync(u => u.UserName == userName);
    }
}
