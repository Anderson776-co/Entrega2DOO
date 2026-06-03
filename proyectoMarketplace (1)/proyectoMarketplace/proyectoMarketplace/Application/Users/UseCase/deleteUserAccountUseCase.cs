using Domain.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UseCase
{
    public class deleteUserAccountUseCase
    {
        private readonly UserService _userService;

        public deleteUserAccountUseCase(UserService userService)
        {
            _userService = userService;
        }

        public async Task DeleteUserAccount(int authenticatedUserId)
        {
            await _userService.DeleteUser(authenticatedUserId);
        }
    }
}
