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
        void AssignBusiness(UserEntity user);
        Task<UserEntity> UpdateUser(UserEntity user);
        Task DeleteUser(UserEntity user);
        Task<UserEntity?> GetUserById(int userId);
        Task<(bool emailExists, bool phoneExists)> ExistsDuplicate(string email, string phone);
        Task<UserEntity?> Login(string identificador, string password);
        Task<int?> GetBusinessIdByUserId(int userId);
    }
}
