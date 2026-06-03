using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Business;

namespace Domain.Ports
{
    public interface IBusinessRepository
    {
        Task<BusinessEntity> CreateBusiness(BusinessEntity business);
        Task<bool> ExistsById(int id);
        Task<BusinessEntity?> GetBusinessById(int id);
        Task<bool> ExistsByNit(string nit);

    }
}
