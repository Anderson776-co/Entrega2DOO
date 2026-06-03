using Application.Users.DTO_s;
using Domain.Entities.Users;
using Domain.Services.Users;

namespace Application.Users.UseCase
{
    public class GetPersonalInfoUseCase
    {
        private readonly UserService _userService;
        public GetPersonalInfoUseCase(UserService userService)
        {
            _userService = userService;
        }
        public async Task<UserInfoDTO> GetPersonalInfo(int userId)
        {
            UserEntity user = await _userService.GetUserById(userId);
            return UserInfoDTO.FromEntityToDTO(user);
        }
    }
}
