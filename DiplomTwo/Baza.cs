using DiplomTwo.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomTwo
{
    public static class Baza
    {
        public static User9Context DbContext { get; set; } = new User9Context();
    }
}
