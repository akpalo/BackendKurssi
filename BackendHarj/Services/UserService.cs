using BackendHarj.Middleware;
using BackendHarj.Models;
using BackendHarj.Repositories;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BackendHarj.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IUserAuthenticationService _authenticationService;

        public UserService(IUserRepository repository, IUserAuthenticationService authenticationService)
        {
            _repository = repository;
            _authenticationService = authenticationService;
        }
        public async Task<bool> DeleteUserAsync(long id)
        {
            User user = await _repository.GetUserAsync(id);
            if(user == null)
            {
                return await _repository.DeleteUserAsync(user);
            }
            return false;
        }

        public async Task<UserDTO> GetUserAsync(long id)
        {
            User user = await _repository.GetUserAsync(id);
            if(user == null)
            {
                return null;
            }
            return UserToDTO(user);
        }

        

        public async Task<IEnumerable<UserDTO>> GetUsersAsync() 
        {
            IEnumerable<User> users = await _repository.GetUsersAsync();
            List<UserDTO> result = new List<UserDTO>();
            foreach (User user in users)
            {
                result.Add(UserToDTO(user));
            }
            return result;
        }

        public async Task<UserDTO?> NewUserAsync(User user)
        {
            User? dbUser = await _repository.GetUserAsync(user.UserName);
            if(dbUser != null)
            {
                return null;
            }

            User? newUser = _authenticationService.CreateUserCredentials(user);
            if(newUser != null)
            {

                return UserToDTO(await _repository.NewUserAsync(newUser));
            }
            return null;
        }

        

        public async Task<bool> UpdateUserAsync(User user) 
        {
            User dbUser = await _repository.GetUserAsync(user.UserName);
            if(dbUser == null)
            {
                return false;
            }
            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            dbUser.Password = user.Password;
            dbUser = _authenticationService.CreateUserCredentials(dbUser);

            return await _repository.UpdateUserAsync(dbUser);
            
        }

        private UserDTO UserToDTO(User user)
        {
           UserDTO dto = new UserDTO();
           dto.UserName = user.UserName;
           dto.FirstName = user.FirstName;
           dto.LastName = user.LastName;
           dto.JoinDate = user.JoinDate;
           dto.LastLogin = user.LastLogin;

            return dto;
        }
      
    }
}
