using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Users
{
    public class MailingAddressEntity
    {
        public int Id { get; set; }
        public string Address { get; set; } = "";
        public string Department { get; set; } = "";
        public string City { get; set; } = "";
        public string Neighborhood { get; set; } = "";
        public string Complement { get; set; } = "";
        public string ContactPersonName { get; set; } = "";
        public string ContactPersonLastName { get; set; } = "";
        public string Phone { get; set; } = "";
        public int UserId { get; set; }
        public UserEntity User { get; set; } = null;
    }
}
