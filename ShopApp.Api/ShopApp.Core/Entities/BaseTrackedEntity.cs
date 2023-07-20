using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Core.Entities
{
    public class BaseTrackedEntity:BaseEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now.AddHours(4);
        public DateTime ModifiedAt { get; set;} = DateTime.Now.AddHours(4);
    }
}
