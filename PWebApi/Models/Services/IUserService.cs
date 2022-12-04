using PWebApi.Models.Request;
using PWebApi.Models.Response;

namespace PWebApi.Models.Services
{
    public interface IUserService
    {
        Usuario Auth(Usuario model);
    }
}
