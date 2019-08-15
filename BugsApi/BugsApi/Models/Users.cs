using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugsApi.Models
{
    public class UsersModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Bug> Bugs { get; set; }

    }
}
