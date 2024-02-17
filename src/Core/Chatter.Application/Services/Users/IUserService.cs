using Chatter.Application.Dtos;
using Chatter.Application.Dtos.Users;

namespace Chatter.Application.Services.Users;

public interface IUserService
{
    Task<List<SearchUserShortInfoDto>> GetUsersShortInfoAsync(string searchValue);
}