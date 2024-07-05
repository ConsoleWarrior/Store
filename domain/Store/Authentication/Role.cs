using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Authentication
{
    public class Role
    {
        public string Name { get; set; }
        public RoleId RoleId { get; set; }
        public virtual List<User> Users { get; set; }
    }
}
