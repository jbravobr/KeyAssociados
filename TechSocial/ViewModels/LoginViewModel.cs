using System;
using System.Linq;
using System.Threading.Tasks;

using PropertyChanged;
using Xamarin.Forms;
using System.Collections.Generic;
using Connectivity.Plugin;

namespace TechSocial
{
    [ImplementPropertyChanged]
    public class LoginViewModel
    {
        readonly ILoginService service;
        readonly IAuditoriaService serviceAuditoria;
        readonly ICheckListService serviceChecklist;
        readonly IQuestoesService serviceQuestoes;
        readonly IRespostaService serviceRespostas;
        readonly IBaseService serviceBaseLegal;
        readonly IServiceUltimaAuditoria serviceUltimaAuditoria;

        public ICollection<Respostas> listaRespostas { get; set; }

        public LoginViewModel(ILoginService service, IAuditoriaService serviceAuditoria, ICheckListService serviceChecklist,
                              IQuestoesService serviceQuestoes, IRespostaService serviceRespostas, IBaseService serviceBaseLegal,
                              IServiceUltimaAuditoria srvUltima)
        {
            this.service = service;
            this.serviceAuditoria = serviceAuditoria;
            this.serviceChecklist = serviceChecklist;
            this.serviceQuestoes = serviceQuestoes;
            this.serviceRespostas = serviceRespostas;
            this.serviceBaseLegal = serviceBaseLegal;
            this.serviceUltimaAuditoria = srvUltima;
        }

        public async Task<bool> ExecutarLogin(string user, string pass)
        {
            if (String.IsNullOrEmpty(user) || String.IsNullOrEmpty(pass))
                throw new ArgumentException("Usuário ou senha em branco!");

            if (CrossConnectivity.Current.IsConnected &&
                await CrossConnectivity.Current.IsReachable("http://www.google.com.br"))
            {
                var dadosFromServer = await service.ExecutarLogin(user, pass);

                if (dadosFromServer != null && !String.IsNullOrEmpty(dadosFromServer.Auditor.nome))
                {
                    var db = new TechSocialDatabase(false);

                    // Gravando Auditor logado.
                    dadosFromServer.Auditor.id = Guid.NewGuid();
                    db.InsertAuditor(dadosFromServer.Auditor);

                    // Gravando Rotas recebidas.
                    db.InsertRotas(dadosFromServer.Rotas);

                    //var _rotas = db.GetRotas();
//					foreach (var rota in _rotas) {
//						rota.auditorId = _auditor.id;
//						db.AtualizarRota (rota);
//					}

                    // Gravando Semanas.
                    db.InsertSemanas(dadosFromServer.Semanas);
                    var _semanas = db.GetSemanas();
//                    foreach (var semana in _semanas)
//                    {
//                        semana.auditorId = _auditor.id;
//                        db.AtualizarSemana(semana);
//                    }

                    // Gravando Fornecedores recebidos.
                    var fornecedores = dadosFromServer.Rotas.Select(x => x.Fornecedores).ToList();
                    db.InsertFornecedores(fornecedores);

                    // Gravando Auditorias.
                    foreach (var fornecedor in fornecedores)
                    {
                        dadosFromServer = await this.serviceAuditoria.RetornarAuditorias(fornecedor.fornecedor);
                        if (dadosFromServer != null && dadosFromServer.Auditorias != null && dadosFromServer.Auditorias.Any())
                        {
                            db.InsertAuditorias(dadosFromServer.Auditorias);
                        }
                        db.AtualizaFornecedor(fornecedor);

                        // Gravando última Auditoria
                        var ultimas = await this.serviceUltimaAuditoria.GetUltimaAuditoria(fornecedor.fornecedor);

                        db.InserirRespostaUltima(ultimas.Ultima_Auditoria.RespostaUltima);
                    }

                    // Gravando Módulos.
                    var modulo = new JsonObjectModulo();
                    var auditorias = db.GetAuditorias();
                    foreach (var auditoria in auditorias)
                    {
                        modulo = await this.serviceChecklist.RetornaChecklist(auditoria.audi.ToString());

                        if (modulo != null && modulo.Auditorias != null)
                        {
                            foreach (var item in modulo.Modulos)
                            {
                                item.audi = auditoria.audi;
                            }

                            db.InsertModulos(modulo.Modulos);
                        }

                        db.AtualiarAuditoria(auditoria);
                    }

                    var auditoriasNecessarias = db.GetAuditorias().Where(x => x.resultado == 3).Select(c => c.audi).ToList();
                    var _modulosValidos = db.GetModulos();

                    // Gravando Questões.
                    foreach (var mod in _modulosValidos)
                    {
                        if (auditoriasNecessarias.Contains(mod.audi))
                        {
                            var questoes = await this.serviceQuestoes.RetornarQuestoes(mod.modulo);

                            if (questoes != null && questoes.Questoes != null && questoes.Questoes.Any())
                            {
                                foreach (var item in questoes.Questoes)
                                {
                                    item.audi = mod.audi;
                                }

                                db.InsertQuestao(questoes.Questoes);
                            }   

                            mod.qtdeQuestoesNoModulo = questoes.Questoes.Count;
                            db.AtualizarModulo(mod);
                        }
                    }

                    // Gravando Bases Legais.
                    var _questoes = db.GetQuestoes();
                    var maxPont = 0;
                    foreach (var _mods in modulo.Modulos)
                    {
                        if (maxPont > 0)
                            db.AtualizarModulo(_mods);
						
                        maxPont = 0;
                        if (_mods.compile == "1")
                        {
                            foreach (var qq in _questoes.Where(q=>q.modulo == _mods.modulo))
                            {
                                maxPont += qq.peso * 2;
                                _mods.valorMaxPontuacao = maxPont;
                            }
                        }
                    }
                    var listaBases = new List<BaseLegal>();

                    foreach (var item in _questoes)
                    {
                        var dbMod = new TechSocialDatabase(false);
                        var checkList = dbMod.GetModuloById(item.modulo).checklist;
                        var baseLegal = await this.serviceBaseLegal.RetornarBasesLegaisPorChecklistQuestao(item.questao.ToString(), checkList);

                        if (baseLegal != null && baseLegal.BaseLegal != null && baseLegal.BaseLegal.Any())
                        {
                            foreach (var baselegalItem in baseLegal.BaseLegal)
                            {
                                listaBases.Add(new BaseLegal
                                    {
                                        descricao = baselegalItem.descricao,
                                        id_baselegal = baselegalItem.id_baselegal,
                                        nome = baselegalItem.nome,
                                        questao = item.questao.ToString()
                                    });
                            }
                        }
                    }
                    db.InsertBaseLegal(listaBases);
                    //db.InsertQuestao(_questoes); // Atualiza questões com bases legais

                    // Gravando Respostas.
//                foreach (var auditoria in auditorias)
//                {
//                    var respostas = await this.serviceRespostas.RetornarRespostasParaAuditoria(auditoria.audi);
//
//                    if (respostas != null && respostas.Modulos != null && respostas.Modulos.Any())
//                    {
//                        foreach (var _mods in modulo.Modulos)
//                        {
//                            if (_mods.Respostas != null && _mods.Respostas.Any())
//                            {
//                                foreach (var resposta in _mods.Respostas)
//                                {
//                                    resposta.modulo = _mods.modulo.ToString();
//                                    resposta.audi = _mods.audi.ToString();
//
//                                    db.InsertResposta(resposta);
//                                }
//                            }
//                        }
//                    }
//                }

                    // Atualizando Módulos com atende
                    if (db.ExistemRespostas())
                    {
                        var mods = db.GetModulos();
                        var q = db.GetQuestoes();
                        this.listaRespostas = db.GetRespostas();

                        foreach (var mod in mods)
                        {
                            int? operacao;

                            foreach (var questao in q.Where(x=>x.modulo == mod.modulo))
                            {
                                var peso = questao.peso;
                                if (this.listaRespostas.Any(b => b.questao == questao.questao.ToString()))
                                {
                                    var atende = this.listaRespostas.First(x => x.questao == questao.questao.ToString()).atende;
                                    operacao += atende == null ? 0 : Convert.ToInt32(atende) * peso;
                                }
                            }
                            mod.atende = operacao;
                        }

                        // Atualizando Módulos com imagem do semáforo.
//                    foreach (var mod in mods)
//                    {
//                        ImageSource imgSrc = null;
//                        var pontuacao = (mod.atende * 100) / mod.valor_meta;
//
//                        if (pontuacao < mod.meta)
//                            imgSrc = ImageSource.FromResource("TechSocial.Content.Images.circuloVermelho.png");
//                        else if (pontuacao == mod.meta)
//                            imgSrc = ImageSource.FromResource("TechSocial.Content.Images.circuloLaranja.png");
//                        else if (pontuacao > mod.meta)
//                            imgSrc = ImageSource.FromResource("TechSocial.Content.Images.circuloVerde.png");
//                        else
//                            imgSrc = ImageSource.FromResource("TechSocial.Content.Images.circuloCinza.png");
//
//                        mod.Image = imgSrc;
//                    }
                        db.InsertModulos(mods);
                    }

                    return true;
                }
            }
            else if ((!CrossConnectivity.Current.IsConnected && !await CrossConnectivity.Current.IsReachable("http://www.google.com.br"))
                     && await this.VerificaDados())
            {

                return await Task.FromResult(true);
            }
            else
            {
                DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>().HideLoading();
                await DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>().AlertAsync("Sem Conexão", "Não existe carga na aplicação, efetue o login online", "OK");
            }

            return false;
        }

        public async Task<bool> VerificaDados()
        {
            var db = new TechSocialDatabase(false);
            var existeDados = db.GetAuditorias().Any();

            return await Task.FromResult(existeDados);
        }

        public async Task Refresh()
        {
            var user = Application.Current.Properties["usuario"].ToString();
            var pass = Application.Current.Properties["senha"].ToString();

            var dadosFromServer = await service.ExecutarLogin(user, pass);

            if (dadosFromServer != null && !String.IsNullOrEmpty(dadosFromServer.Auditor.nome))
            {
                var db = new TechSocialDatabase(false);

                // Gravando Auditor logado.
                db.InsertAuditor(dadosFromServer.Auditor);

                // Gravando Rotas recebidas.
                db.InsertRotas(dadosFromServer.Rotas);

                // Gravando Semanas.
                db.InsertSemanas(dadosFromServer.Semanas);

                // Gravando Fornecedores recebidos.
                var fornecedores = dadosFromServer.Rotas.Select(x => x.Fornecedores).ToList();
                db.InsertFornecedores(fornecedores);

                // Gravando Auditorias.
                foreach (var fornecedor in fornecedores)
                {
                    dadosFromServer = await this.serviceAuditoria.RetornarAuditorias(fornecedor.fornecedor);

                    if (dadosFromServer != null && dadosFromServer.Auditorias != null && dadosFromServer.Auditorias.Any())
                    {
                        db.InsertAuditorias(dadosFromServer.Auditorias);
                    }
                }

                // Gravando Módulos.
                var modulo = new JsonObjectModulo();
                var auditorias = db.GetAuditorias();
                foreach (var auditoria in auditorias)
                {
                    modulo = await this.serviceChecklist.RetornaChecklist(auditoria.audi.ToString());

                    if (modulo != null && modulo.Auditorias != null)
                    {
                        foreach (var item in modulo.Modulos)
                        {
                            item.audi = auditoria.audi;
                        }

                        db.InsertModulos(modulo.Modulos);
                    }
                }

                // Gravando Questões.
                foreach (var mod in modulo.Modulos)
                {
                    var questoes = await this.serviceQuestoes.RetornarQuestoes(mod.modulo);

                    if (questoes != null && questoes.Questoes != null && questoes.Questoes.Any())
                    {
                        db.InsertQuestao(questoes.Questoes);
                    }   
                }

                // Gravando Bases Legais.
                var _questoes = db.GetQuestoes();
                var maxPont = 0;
                foreach (var _mods in modulo.Modulos)
                {
                    if (maxPont > 0)
                        db.AtualizarModulo(_mods);

                    maxPont = 0;
                    if (_mods.compile == "1")
                    {
                        foreach (var qq in _questoes.Where(q=>q.modulo == _mods.modulo))
                        {
                            maxPont += qq.peso * 2;
                            _mods.valorMaxPontuacao = maxPont;
                        }
                    }
                }
                var listaBases = new List<BaseLegal>();

                foreach (var item in _questoes)
                {
                    var dbMod = new TechSocialDatabase(false);
                    var checkList = dbMod.GetModuloById(item.modulo).checklist;
                    var baseLegal = await this.serviceBaseLegal.RetornarBasesLegaisPorChecklistQuestao(item.questao.ToString(), checkList);

                    if (baseLegal != null && baseLegal.BaseLegal != null && baseLegal.BaseLegal.Any())
                    {
                        foreach (var baselegalItem in baseLegal.BaseLegal)
                        {
                            listaBases.Add(new BaseLegal
                                {
                                    descricao = baselegalItem.descricao,
                                    id_baselegal = baselegalItem.id_baselegal,
                                    nome = baselegalItem.nome,
                                    questao = item.questao.ToString()
                                });
                        }
                    }
                }
                db.InsertBaseLegal(listaBases);
                //db.InsertQuestao(_questoes); // Atualiza questões com bases legais

                // Atualizando Módulos com atende
                if (db.ExistemRespostas())
                {
                    var mods = db.GetModulos();
                    var q = db.GetQuestoes();
                    this.listaRespostas = db.GetRespostas();

                    foreach (var mod in mods)
                    {
                        int? operacao;

                        foreach (var questao in q.Where(x=>x.modulo == mod.modulo))
                        {
                            var peso = questao.peso;
                            if (this.listaRespostas.Any(b => b.questao == questao.questao.ToString()))
                            {
                                var atende = this.listaRespostas.First(x => x.questao == questao.questao.ToString()).atende;
                                operacao += atende == null ? 0 : Convert.ToInt32(atende) * peso;
                            }
                        }
                        mod.atende = operacao;
                    }

                    // Atualizando Módulos com imagem do semáforo.
                    db.InsertModulos(mods);
                }
            }
        }
    }
}

