using CPER2G3.Earth4Sport.Auth.Models;

namespace CPER2G3.Earth4Sport.Auth.Service {
    public interface IUserService {
        void Login(string username, string password);
        void Register(User user);
    }
}
