using SchulteTable.Core.Models;
using SchulteTable.Data.Entities;

namespace SchulteTable.Data.Repositories;

public interface IUserProfileRepository
{
    Task<UserProfileEntity> GetOrCreateAsync(string userName);
    Task<UserProfileEntity> UpdateAsync(UserProfileEntity userProfile);
    Task<UserProfileEntity?> GetByNameAsync(string userName);
}
