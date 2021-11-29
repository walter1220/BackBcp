using BackBCP.Models.Request;
using BackBCP.Models.Response;

namespace BackBCP.Services
{
    public interface IUserService
    {
        UserDTO Auth(AuthRequest model);
    }
}
