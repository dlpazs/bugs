using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BugsApi.Models
{
    public class Bug
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Opened { get; set; }
        public bool IsOpen { get; set; }
        public int? UserId { get; set; }
        public UsersModel User { get; set; }
    }
}
