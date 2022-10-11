using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Seles.Models
{
    public static class MonyConvertor
    {
        public static string ToRial(this decimal mony)
        {
            return mony.ToString("#,0")+" تومان ";
        }
    }
}
