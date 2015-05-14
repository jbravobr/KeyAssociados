using System;
using System.Collections.Generic;

namespace TechSocial
{
    public static class StatusVisita
    {
        public static Dictionary<int,string> GeStatusvisita()
        {
            return new Dictionary<int,string>
            {
                { 1,"Planejada" },
                { 2,"Realizada" },
                { 3,"Não Realizada" }
            };
        }
    }
}

