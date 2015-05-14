using NUnit.Framework;
using System;
using System.Collections.Generic;
using Autofac;

namespace TechSocial.Test
{
    [TestFixture()]
    public class Test
    {
        [Test()]
        public async void TestaLogin()
        {
            const string user = "helton";
            const string pass = "zzz";
        }

        [Test()]
        public async void TestaAuditorias()
        {
            const string fornecedor = "9968253411322";

            var auditoriaService = new AuditoriaService();
            var retorno = await auditoriaService.RetornarAuditorias(fornecedor);

            Assert.IsNotNull(retorno);
            Assert.IsInstanceOf <JsonObject>(retorno);
        }
    }
}

