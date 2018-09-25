using DataAccess.PrimaryTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ExtendedTypes
{
    public class CustomerWithTransaction
    {
        public Customer customer { get; set; }
        public int Balance { get; set; }


    }
}
