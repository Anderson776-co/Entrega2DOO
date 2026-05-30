using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Publications.Interfaces
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(Stream imageStream, string fileName);
    }
}
