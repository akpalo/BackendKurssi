using BackendHarj.Models;

namespace BackendHarj.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetUsersAsync();


        Task<UserDTO> GetUserAsync(long id);

        Task<UserDTO> NewUserAsync(User user);

        Task<bool> UpdateUserAsync(User user);

        Task<bool> DeleteUserAsync(long id);
        
    }
}
