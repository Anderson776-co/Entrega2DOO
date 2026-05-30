using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports
{
    public interface IUserRepository
    {
        Task<UserEntity> CreateUser(UserEntity user);
        void AssignBusiness(int userId, int businessId, int roleId);
        Task<(bool emailExists, bool usernameExists, bool phoneExists)> ExistsDuplicate(string email, string username, string phone);
        Task<UserEntity?> Login(string identificador, string password);
        int? GetBusinessIdByUserId(int userId);
    }
}
