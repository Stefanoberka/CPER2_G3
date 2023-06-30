using CPER2G3.Earth4Sport.AzureFunction.Models;
using System;
using System.Threading.Tasks;

namespace CPER2G3.Earth4Sport.AzureFunction.Service {
    public interface IUserService {
        Task<int> Login(string username, string password);
        Task<string> Register(User user, string clockUuid);
    }
}
