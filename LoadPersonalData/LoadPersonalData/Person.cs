using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadPersonalData
{
    public class Person
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; internal set; }
        public string PostalZip { get; internal set; }
        public string Email { get; internal set; }
    }
}
