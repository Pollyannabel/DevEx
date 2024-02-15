using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Addresses
{
    public class Address : BaseAddresses
    {
        // for data coming from the database

        public int SuiteNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool IsActive { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }

        public static implicit operator Address(List<Address> v)
        {
            throw new NotImplementedException();
        }
    }
}


































