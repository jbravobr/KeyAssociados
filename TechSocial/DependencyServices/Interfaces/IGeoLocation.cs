﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace TechSocial
{
    /// <summary>
    ///  Interface para Injeção de Dependência dos serviços de localização
    /// </summary>
    public interface IGeoLocation
    {
        Position GetCurrentPosition();

        void InitLocationService();

        void StopLocationService();

        bool CheckStatusLocationService();
    }
}