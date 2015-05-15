using NUnit.Framework;
using System;
using System.Collections.Generic;
using Autofac;
using Xamarin.Forms;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace TechSocial.Test
{
    [TestFixture()]
    public class Test
    {
        //        [Test()]
        //        public async void TestaLogin()
        //        {
        //            const string user = "helton";
        //            const string pass = "zzz";
        //        }
        //
        //        [Test()]
        //        public async void TestaAuditorias()
        //        {
        //            const string fornecedor = "9968253411322";
        //
        //            var auditoriaService = new AuditoriaService();
        //            var retorno = await auditoriaService.RetornarAuditorias(fornecedor);
        //
        //            Assert.IsNotNull(retorno);
        //            Assert.IsInstanceOf <JsonObject>(retorno);
        //        }

        [Test()]
        public async void TestaEnviarImagem()
        {
            const string auditoria = "3792";
            const string questao = "4";
            const string ulrFoto = "foto.json";
            var img = File.ReadAllBytes("IMG_14052015_152040.png");
            const string url = "http://techsocial.com.br/hering/webservices/app/api/";

            using (var m = new MemoryStream())
            {
                string base64String = Convert.ToBase64String(img);

                var client = new HttpClient();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonConvert.SerializeObject(new ImagemPOST{ audi = auditoria, questao = questao, foto = base64String });
                HttpContent content = new StringContent(json);

                try
                {
                    var result = await client.PostAsync("foto.json/", content);

                    if (result.IsSuccessStatusCode)
                        Assert.IsTrue(1 == 1);
                    else
                        Assert.IsFalse(1 == 1);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }  
        }
    }
}

