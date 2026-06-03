using Domain.Services.Users;

namespace Application.Users.UseCase
{
    public class DeleteMailingAddressUseCase
    {
        private readonly MailingAddressService _mailingAddressService;

        public DeleteMailingAddressUseCase(MailingAddressService mailingAddressService)
        {
            _mailingAddressService = mailingAddressService;
        }

        public async Task DeleteMailingAddress(int id, int userId)
        {
            await _mailingAddressService.DeleteMailingAddress(id, userId);
        }
    }
}
