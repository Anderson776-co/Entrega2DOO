using Domain.Services.Publications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Publications.UseCase
{
    public class DeletePublicationByIdUseCase
    {
        private readonly PublicationService _publicationService;

        public DeletePublicationByIdUseCase(PublicationService publicationService)
        {
            _publicationService = publicationService;
        }

        public async Task Delete(int id, int userId)
        {
            await _publicationService.Delete(id, userId);
        }
    }
}
