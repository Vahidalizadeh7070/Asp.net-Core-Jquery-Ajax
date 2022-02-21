using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDOperationUsingJqueryAjax.Models
{
    public class FeedBack
    {
        [Key]
        public int Id { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; }
    }
}
