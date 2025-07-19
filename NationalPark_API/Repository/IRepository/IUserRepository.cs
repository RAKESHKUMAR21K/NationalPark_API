using NationalPark_API.Models;

namespace NationalPark_API.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string UserName);
        User Authenticate(string UserName,string Password);
        User Register(string UserName,string Password);
    }
}
