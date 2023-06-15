using CPER2G3.Earth4Sport.Auth.Models;

namespace CPER2G3.Earth4Sport.Auth.Service {
    public interface IUserService {
        Task<int> Login(string username, string password);
        Task<string> Register(User user);
    }
}
