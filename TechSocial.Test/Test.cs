using NUnit.Framework;
using System;
using System.Collections.Generic;
using Autofac;
using Xamarin.Forms;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Linq;

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
        //
        //        [Test()]
        //        public async void TestaEnviarImagem()
        //        {
        //            const string auditoria = "3792";
        //            const string questao = "4";
        //            //const string urlFoto = "foto.json/";
        //            const string urlFoto = "assinatura.json/";
        //            var img = File.ReadAllBytes("IMG_14052015_152040.png");
        //            const string url = "http://techsocial.com.br/hering/webservices/app/api/";
        //
        //            using (var m = new MemoryStream())
        //            {
        //                string base64String = Convert.ToBase64String(img);
        //
        //                var client = new HttpClient();
        //                client.BaseAddress = new Uri(url);
        //                client.DefaultRequestHeaders.Accept.Clear();
        //                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //
        //                var json = JsonConvert.SerializeObject(new ImagemPOST{ audi = auditoria, foto = base64String });
        //                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
        //
        //                try
        //                {
        //                    var result = await client.PostAsync(urlFoto, content);
        //
        //                    Assert.IsTrue(result.IsSuccessStatusCode);
        //                }
        //                catch (Exception ex)
        //                {
        //                    throw ex;
        //                }
        //            }
        //        }
        //
        //        [Test]
        //        public async void Testa_Enviar_Colecao_Respostas()
        //        {
        //            const string URL = "http://techsocial.com.br/hering/webservices/app/api/";
        //            const string metodoEnvio = "Answer.json";
        //
        //            var respostas = new List<Respostas>();
        //
        //            #region -- Adicionando Perguntas na coleção --
        //
        //            respostas.Add(new Respostas
        //                {
        //                    acoesRequeridadas = "Teste",
        //                    atende = "Teste",
        //                    atualizacao = "Teste",
        //                    audi = "Teste",
        //                    baseLegalTexto = "Teste",
        //                    dt_prazo = "Teste",
        //                    evidencia = "Teste",
        //                    id_baselegal = "Teste",
        //                    imagem = "Teste",
        //                    modulo = "Teste",
        //                    observacao = "Teste",
        //                    questao = "Teste",
        //                    respondida = true,
        //                    tp_prazo = "Teste"
        //                });
        //            respostas.Add(new Respostas
        //                {
        //                    acoesRequeridadas = "Teste",
        //                    atende = "Teste",
        //                    atualizacao = "Teste",
        //                    audi = "Teste",
        //                    baseLegalTexto = "Teste",
        //                    dt_prazo = "Teste",
        //                    evidencia = "Teste",
        //                    id_baselegal = "Teste",
        //                    imagem = "Teste",
        //                    modulo = "Teste",
        //                    observacao = "Teste",
        //                    questao = "Teste",
        //                    respondida = true,
        //                    tp_prazo = "Teste"
        //                });
        //            respostas.Add(new Respostas
        //                {
        //                    acoesRequeridadas = "Teste",
        //                    atende = "Teste",
        //                    atualizacao = "Teste",
        //                    audi = "Teste",
        //                    baseLegalTexto = "Teste",
        //                    dt_prazo = "Teste",
        //                    evidencia = "Teste",
        //                    id_baselegal = "Teste",
        //                    imagem = "Teste",
        //                    modulo = "Teste",
        //                    observacao = "Teste",
        //                    questao = "Teste",
        //                    respondida = true,
        //                    tp_prazo = "Teste"
        //                });
        //            respostas.Add(new Respostas
        //                {
        //                    acoesRequeridadas = "Teste",
        //                    atende = "Teste",
        //                    atualizacao = "Teste",
        //                    audi = "Teste",
        //                    baseLegalTexto = "Teste",
        //                    dt_prazo = "Teste",
        //                    evidencia = "Teste",
        //                    id_baselegal = "Teste",
        //                    imagem = "Teste",
        //                    modulo = "Teste",
        //                    observacao = "Teste",
        //                    questao = "Teste",
        //                    respondida = true,
        //                    tp_prazo = "Teste"
        //                });
        //            respostas.Add(new Respostas
        //                {
        //                    acoesRequeridadas = "Teste",
        //                    atende = "Teste",
        //                    atualizacao = "Teste",
        //                    audi = "Teste",
        //                    baseLegalTexto = "Teste",
        //                    dt_prazo = "Teste",
        //                    evidencia = "Teste",
        //                    id_baselegal = "Teste",
        //                    imagem = "Teste",
        //                    modulo = "Teste",
        //                    observacao = "Teste",
        //                    questao = "Teste",
        //                    respondida = true,
        //                    tp_prazo = "Teste"
        //                });
        //            respostas.Add(new Respostas
        //                {
        //                    acoesRequeridadas = "Teste",
        //                    atende = "Teste",
        //                    atualizacao = "Teste",
        //                    audi = "Teste",
        //                    baseLegalTexto = "Teste",
        //                    dt_prazo = "Teste",
        //                    evidencia = "Teste",
        //                    id_baselegal = "Teste",
        //                    imagem = "Teste",
        //                    modulo = "Teste",
        //                    observacao = "Teste",
        //                    questao = "Teste",
        //                    respondida = true,
        //                    tp_prazo = "Teste"
        //                });
        //            respostas.Add(new Respostas
        //                {
        //                    acoesRequeridadas = "Teste",
        //                    atende = "Teste",
        //                    atualizacao = "Teste",
        //                    audi = "Teste",
        //                    baseLegalTexto = "Teste",
        //                    dt_prazo = "Teste",
        //                    evidencia = "Teste",
        //                    id_baselegal = "Teste",
        //                    imagem = "Teste",
        //                    modulo = "Teste",
        //                    observacao = "Teste",
        //                    questao = "Teste",
        //                    respondida = true,
        //                    tp_prazo = "Teste"
        //                });
        //            respostas.Add(new Respostas
        //                {
        //                    acoesRequeridadas = "Teste",
        //                    atende = "Teste",
        //                    atualizacao = "Teste",
        //                    audi = "Teste",
        //                    baseLegalTexto = "Teste",
        //                    dt_prazo = "Teste",
        //                    evidencia = "Teste",
        //                    id_baselegal = "Teste",
        //                    imagem = "Teste",
        //                    modulo = "Teste",
        //                    observacao = "Teste",
        //                    questao = "Teste",
        //                    respondida = true,
        //                    tp_prazo = "Teste"
        //                });
        //            respostas.Add(new Respostas
        //                {
        //                    acoesRequeridadas = "Teste",
        //                    atende = "Teste",
        //                    atualizacao = "Teste",
        //                    audi = "Teste",
        //                    baseLegalTexto = "Teste",
        //                    dt_prazo = "Teste",
        //                    evidencia = "Teste",
        //                    id_baselegal = "Teste",
        //                    imagem = "Teste",
        //                    modulo = "Teste",
        //                    observacao = "Teste",
        //                    questao = "Teste",
        //                    respondida = true,
        //                    tp_prazo = "Teste"
        //                });
        //            respostas.Add(new Respostas
        //                {
        //                    acoesRequeridadas = "Teste",
        //                    atende = "Teste",
        //                    atualizacao = "Teste",
        //                    audi = "Teste",
        //                    baseLegalTexto = "Teste",
        //                    dt_prazo = "Teste",
        //                    evidencia = "Teste",
        //                    id_baselegal = "Teste",
        //                    imagem = "Teste",
        //                    modulo = "Teste",
        //                    observacao = "Teste",
        //                    questao = "Teste",
        //                    respondida = true,
        //                    tp_prazo = "Teste"
        //                });
        //            respostas.Add(new Respostas
        //                {
        //                    acoesRequeridadas = "Teste",
        //                    atende = "Teste",
        //                    atualizacao = "Teste",
        //                    audi = "Teste",
        //                    baseLegalTexto = "Teste",
        //                    dt_prazo = "Teste",
        //                    evidencia = "Teste",
        //                    id_baselegal = "Teste",
        //                    imagem = "Teste",
        //                    modulo = "Teste",
        //                    observacao = "Teste",
        //                    questao = "Teste",
        //                    respondida = true,
        //                    tp_prazo = "Teste"
        //                });
        //            respostas.Add(new Respostas
        //                {
        //                    acoesRequeridadas = "Teste",
        //                    atende = "Teste",
        //                    atualizacao = "Teste",
        //                    audi = "Teste",
        //                    baseLegalTexto = "Teste",
        //                    dt_prazo = "Teste",
        //                    evidencia = "Teste",
        //                    id_baselegal = "Teste",
        //                    imagem = "Teste",
        //                    modulo = "Teste",
        //                    observacao = "Teste",
        //                    questao = "Teste",
        //                    respondida = true,
        //                    tp_prazo = "Teste"
        //                });
        //            respostas.Add(new Respostas
        //                {
        //                    acoesRequeridadas = "Teste",
        //                    atende = "Teste",
        //                    atualizacao = "Teste",
        //                    audi = "Teste",
        //                    baseLegalTexto = "Teste",
        //                    dt_prazo = "Teste",
        //                    evidencia = "Teste",
        //                    id_baselegal = "Teste",
        //                    imagem = "Teste",
        //                    modulo = "Teste",
        //                    observacao = "Teste",
        //                    questao = "Teste",
        //                    respondida = true,
        //                    tp_prazo = "Teste"
        //                });
        //            respostas.Add(new Respostas
        //                {
        //                    acoesRequeridadas = "Teste",
        //                    atende = "Teste",
        //                    atualizacao = "Teste",
        //                    audi = "Teste",
        //                    baseLegalTexto = "Teste",
        //                    dt_prazo = "Teste",
        //                    evidencia = "Teste",
        //                    id_baselegal = "Teste",
        //                    imagem = "Teste",
        //                    modulo = "Teste",
        //                    observacao = "Teste",
        //                    questao = "Teste",
        //                    respondida = true,
        //                    tp_prazo = "Teste"
        //                });
        //            respostas.Add(new Respostas
        //                {
        //                    acoesRequeridadas = "Teste",
        //                    atende = "Teste",
        //                    atualizacao = "Teste",
        //                    audi = "Teste",
        //                    baseLegalTexto = "Teste",
        //                    dt_prazo = "Teste",
        //                    evidencia = "Teste",
        //                    id_baselegal = "Teste",
        //                    imagem = "Teste",
        //                    modulo = "Teste",
        //                    observacao = "Teste",
        //                    questao = "Teste",
        //                    respondida = true,
        //                    tp_prazo = "Teste"
        //                });
        //            respostas.Add(new Respostas
        //                {
        //                    acoesRequeridadas = "Teste",
        //                    atende = "Teste",
        //                    atualizacao = "Teste",
        //                    audi = "Teste",
        //                    baseLegalTexto = "Teste",
        //                    dt_prazo = "Teste",
        //                    evidencia = "Teste",
        //                    id_baselegal = "Teste",
        //                    imagem = "Teste",
        //                    modulo = "Teste",
        //                    observacao = "Teste",
        //                    questao = "Teste",
        //                    respondida = true,
        //                    tp_prazo = "Teste"
        //                });
        //            #endregion
        //
        //            var client = new HttpClient();
        //            client.BaseAddress = new Uri(URL);
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //
        //            var json = JsonConvert.SerializeObject(respostas);
        //            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
        //
        //            try
        //            {
        //                var result = await client.PostAsync(String.Format("{0}{1}", URL, metodoEnvio), content);
        //
        //                Assert.IsTrue(result.IsSuccessStatusCode);
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }

        [Test]
        public async void Testa_Get_Ultima_Auditoria_Respostas()
        {
            const string URL = "http://compliance.ciahering.com.br/hering/webservices/app/api/";
            const string metodoEnvio = "last_auditing.json/hering/";

            var client = new HttpClient();
            client.BaseAddress = new Uri(URL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync(String.Format("{0}{1}{2}", URL, metodoEnvio, "17903106000106"));

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var ultimasFromJson = JsonConvert.DeserializeObject<JsonObjectUltimaAuditoria>(jsonString);

                    Assert.IsNotNull(ultimasFromJson.Ultima_Auditoria);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

