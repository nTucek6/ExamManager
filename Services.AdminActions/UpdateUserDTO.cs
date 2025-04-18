using Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AdminActions
{
    public class UpdateUserDTO
    {
       public int UserId { get; set; }
       public UserRoleEnum UserRole { get; set; }

    }
}
