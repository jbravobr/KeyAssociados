using System;
using SQLite.Net;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;

namespace TechSocial
{
	public class TechSocialDatabase
	{
		readonly SQLiteConnection database;

		public TechSocialDatabase(bool checaExistenciaBase)
		{
			database = DependencyService.Get<ISQLite>().GetConnection();

			if (checaExistenciaBase)
				CriaSchema();
		}

		public bool ExisteSchema()
		{
			if (database.TableMappings.All(b => b.TableName != "Auditor"
				    || b.TableName != "Rotas"
				    || b.TableName != "Fornecedores"
				    || b.TableName != "Auditorias"
				    || b.TableName != "Modulos"
				    || b.TableName != "Questoes"
				    || b.TableName != "Respostas"
				    || b.TableName != "Semana"
				    || b.TableName != "BaseLegal"))
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Rotina para verificar se o Schema do banco já foi criado
		/// </summary>
		public void CriaSchema()
		{
			if (database.TableMappings.All(b => b.TableName != "Auditor"
				    || b.TableName != "Rotas"
				    || b.TableName != "Fornecedores"
				    || b.TableName != "Auditorias"
				    || b.TableName != "Modulos"
				    || b.TableName != "Questoes"
				    || b.TableName != "Respostas"
				    || b.TableName != "Semana"
				    || b.TableName != "BaseLegal"))
			{
				database.CreateTable<Auditor>();
				database.CreateTable<Rotas>();
				database.CreateTable<Fornecedores>();
				database.CreateTable<Auditorias>();
				database.CreateTable<Modulos>();
				database.CreateTable<Questoes>();
				database.CreateTable<Respostas>();
				database.CreateTable<Semana>();
				database.CreateTable<BaseLegal>();
				database.CreateTable<ImagemAuditoriaModulo>();
			}
		}

		/// <summary>
		/// Insere as bases legais.
		/// </summary>
		public void InsertBaseLegal(ICollection<BaseLegal> bases)
		{
			database.InsertOrReplaceAll(bases);
		}

		/// <summary>
		/// Insere um auditor na base de dados.
		/// </summary>
		/// <param name="auditor">Auditor.</param>
		public void InsertAuditor(Auditor auditor)
		{
			database.InsertOrReplace(auditor);
		}

		/// <summary>
		/// Insere uma coleção de Rotas na base de dados.
		/// </summary>
		/// <param name="rotas">Rotas.</param>
		public void InsertRotas(ICollection<Rotas> rotas)
		{
			foreach (var rota in rotas)
			{
				if (!this.database.Table<Rotas>().Any(x => x.IdRota == rota.IdRota && x.fornecedor == rota.fornecedor))
					this.database.Insert(rota);
				else
					this.database.Update(rota);
			}
		}

		/// <summary>
		/// Insere os fornecedores na base de dados.
		/// </summary>
		/// <param name="fornecedores">Fornecedores.</param>
		public void InsertFornecedores(ICollection<Fornecedores> fornecedores)
		{
			foreach (var fornecedor in fornecedores)
			{
				if (!this.database.Table<Fornecedores>().Any(x => x.fornecedor == fornecedor.fornecedor))
					this.database.Insert(fornecedor);
				else
					this.database.Update(fornecedor);
			}
		}

		/// <summary>
		/// Insere uma nova semama ou atualiza na base de dados.
		/// </summary>
		public void InsertSemanas(ICollection<Semana> Semanas)
		{
			foreach (var semana in Semanas)
			{
				if (!this.database.Table<Semana>().Any(x => x.semana == semana.semana && x.idRota == semana.idRota))
					this.database.Insert(semana);
				else
					this.database.Update(semana);
			}
		}

		/// <summary>
		/// Insere todas as auditorias no banco de dados.
		/// </summary>
		public void InsertAuditorias(ICollection<Auditorias> auditorias)
		{
			foreach (var auditoria in auditorias)
			{
				if (!this.database.Table<Auditorias>().Any(x => x.audi == auditoria.audi && x.fornecedor == auditoria.fornecedor))
					this.database.Insert(auditoria);
				else
					this.database.Update(auditoria);
			}
		}

		/// <summary>
		/// Insere todos os módulos de uma Avalição para um determinado fornecedor.
		/// </summary>
		public void InsertModulos(ICollection<Modulos> modulos)
		{
			foreach (var modulo in modulos)
			{
				if (!this.database.Table<Modulos>().Any(x => x.modulo == modulo.modulo && x.audi == modulo.audi))
					this.database.Insert(modulo);
				else
					this.database.Update(modulo);
			}
		}

		/// <summary>
		/// Insere uma questão no banco de dados
		/// </summary>
		/// <param name="questoes">Questões.</param>
		public void InsertQuestao(ICollection<Questoes> questoes)
		{
			foreach (var questao in questoes)
			{
				if (!this.database.Table<Questoes>().Any(x => x.questao == questao.questao && x.modulo == questao.modulo))
					this.database.Insert(questao);
				else
					this.database.Update(questao);
			}
		}

		/// <summary>
		/// Insere resposta para questão.
		/// </summary>
		/// <param name="respostas">Respostas.</param>
		public void InsertRespostas(ICollection<Respostas> respostas)
		{
			foreach (var resposta in respostas)
			{
				if (!this.database.Table<Respostas>().Any(x => x.audi == resposta.audi && x.modulo == resposta.modulo && x.questao == resposta.questao))
					this.database.Insert(resposta);
				else
					this.database.Update(resposta);
			}
		}

		/// <summary>
		/// Insere uma única resposta.
		/// </summary>
		/// <param name="resposta">Resposta.</param>
		public void InsertResposta(Respostas resposta)
		{
			if (!this.database.Table<Respostas>().Any(x => x.audi == resposta.audi && x.modulo == resposta.modulo && x.questao == resposta.questao))
				this.database.Insert(resposta);
			else
				this.database.Update(resposta);
		}

		/// <summary>
		/// Retorna todas as rotas cadastradas na base de dados
		/// </summary>
		/// <returns>The rotas.</returns>
		public ICollection<Rotas> GetRotas()
		{
			return database.Table<Rotas>().ToList();
		}

		/// <summary>
		/// Retorna uma rota pelo seu ID.
		/// </summary>
		public ICollection<Rotas> GetRotasById(string IdRota)
		{
			return this.database.Table<Rotas>().Where(x => x.IdRota == IdRota).ToList();
		}

		/// <summary>
		/// Retorna um fornecedor.
		/// </summary>
		/// <returns>The fornecedor.</returns>
		public ICollection<Fornecedores> GetFornecedores()
		{
			return database.Table<Fornecedores>().ToList();
		}

		/// <summary>
		/// Retornar um fornecedor de uma rota pelo seu ID.
		/// </summary>
		public Fornecedores GetFornecedorById(string fornecedorId)
		{
			return database.Table<Fornecedores>().FirstOrDefault(x => x.fornecedor == fornecedorId);
		}

		/// <summary>
		/// Retorna todos os fornecedores de uma rota específica.
		/// </summary>
		public IEnumerable<Fornecedores> GetFornecedoresByRotaId(string IdRota)
		{
			var fornecedoresID = this.database.Table<Rotas>().Where(x => x.IdRota == IdRota).Select(x => x.fornecedor);

			foreach (var fornecedor in fornecedoresID)
			{
				yield return this.GetFornecedorById(fornecedor);
			}
		}

		/// <summary>
		/// Retornar um módulo do checklist pelo seu ID.
		/// </summary>
		public Modulos GetModuloById(int moduloId)
		{
			return database.Table<Modulos>().First(x => x.modulo == moduloId);
		}

		/// <summary>
		/// Retornar uma questão pelo seu ID.
		/// </summary>
		public Questoes GetQuestaoById(int questaoId)
		{
			return database.Table<Questoes>().First(x => x.questao == questaoId);
		}

		/// <summary>
		/// Retorna uma coleção de Auditorias.
		/// </summary>
		/// <returns>The auditorias.</returns>
		public ICollection<Auditorias> GetAuditorias()
		{
			return database.Table<Auditorias>().ToList();
		}

		/// <summary>
		/// Retorna os módulos de uma auditoria.
		/// </summary>
		/// <returns>The modulos.</returns>
		public ICollection<Modulos> GetModulos()
		{
			return database.Table<Modulos>().ToList();
		}

		/// <summary>
		/// Retorna módulos de uma determinada auditoria.
		/// </summary>
		public ICollection<Modulos> GetModulosByAuditoria(string audi)
		{
			var auditoria = Convert.ToInt32(audi);
			return database.Table<Modulos>().Where(x => x.audi == auditoria).ToList();
		}

		/// <summary>
		/// Retorna uma coleção de todas as questões.
		/// </summary>
		public ICollection<Questoes> GetQuestoes()
		{
			return database.Table<Questoes>().ToList();
		}

		/// <summary>
		/// Retorna todas as respostas.
		/// </summary>
		public ICollection<Respostas> GetRespostas()
		{
			return database.Table<Respostas>().ToList();
		}

		/// <summary>
		/// Retorna todas as respostas de uma auditoria.
		/// </summary>
		public ICollection<Respostas> GetRespostaPorAuditoria(int audi)
		{
			var audiToString = audi.ToString();
			return database.Table<Respostas>().Where(x => x.audi == audiToString).ToList();
		}

		/// <summary>
		/// Retorna todas as respostas de uma auditoria e de uma módulo.
		/// </summary>
		/// <param name="audi">Audi.</param>
		/// <param name="modulo">Modulo.</param>
		public ICollection<Respostas> GetRespostaPorAuditoriaModulo(int audi, int modulo)
		{
			var auditoria = audi.ToString();
			var mod = modulo.ToString();
			return database.Table<Respostas>().Where(x => x.audi == auditoria && x.modulo == mod).ToList();
		}

		/// <summary>
		/// Retornar a resposta de uma questão.
		/// </summary>
		/// <param name="audi">Audi.</param>
		/// <param name="modulo">Modulo.</param>
		/// <param name="questao">Questao.</param>
		public Respostas GetRespostaProAuditoriaModuloQuestao(string audi, string modulo, int questao)
		{
			if (database.Table<Respostas>().Any(x => x.audi == audi && x.modulo == modulo && x.questao == questao.ToString()))
				return database.Table<Respostas>().First(x => x.audi == audi && x.modulo == modulo && x.questao == questao.ToString());

			return new Respostas();
		}

		/// <summary>
		/// Verifica se existem respostas resgatadas do servidor.
		/// </summary>
		public bool ExistemRespostas()
		{
			return database.Table<Respostas>().Any();
		}

		/// <summary>
		/// Retorna uma semana por Id da rota
		/// </summary>
		public IEnumerable<Semana> GetSemanaByRota(string rotaId)
		{
			return this.database.Table<Semana>().Where(x => x.idRota == rotaId);
		}

		/// <summary>
		/// Retorna todas as semanas.
		/// </summary>
		/// <returns>The semanas.</returns>
		public ICollection<Semana> GetSemanas()
		{
			return this.database.Table<Semana>().ToList();
		}

		/// <summary>
		/// Retorna todas as bases legais.
		/// </summary>
		public ICollection<BaseLegal> GetBaseLegal()
		{
			return this.database.Table<BaseLegal>().ToList();
		}

		/// <summary>
		/// Retorna o Id da base legal selecionada.
		/// </summary>
		public string GetIdBaseLegalByNome(string nome)
		{
			return this.database.Table<BaseLegal>().First(x => x.nome == nome).id_baselegal;
		}

		/// <summary>
		/// Retorna uma resposta pelo seu ID.
		/// </summary>
		public Respostas GetRespostaById(int respostaId)
		{
			return this.database.Table<Respostas>().First(x => x._id == respostaId);
		}

		public Auditor GetAuditor()
		{
			return this.database.Table<Auditor>().First();
		}

		/// <summary>
		/// Retorna a indicação se todas as respostas de um módulo já foram respondidas.
		/// </summary>
		/// <param name="audi">Audi.</param>
		/// <param name="modulo">Modulo.</param>
		public bool TrocaStatusModuloCompleto(int audi, int modulo)
		{
			if (this.database.Table<Modulos>().Any(x => x.audi == audi && x.modulo == modulo))
			{
				List<Respostas> _respostas = null;

				if (this.database.Table<Respostas>().Any(x => x.audi == audi.ToString() && x.modulo == modulo.ToString()))
				{
					var _audi = audi.ToString();
					var _modulo = modulo.ToString();
					_respostas = this.database.Table<Respostas>().Where(x => x.audi == _audi && x.modulo == _modulo).ToList();
				}

				if (_respostas != null && _respostas.Any())
				{
					return _respostas.All(x => x.respondida);
				}
			}

			return false;
		}

		/// <summary>
		/// Atualiza a pontuação do módulo de acordo com uma resposta de sua questão.
		/// </summary>
		/// <param name="nota">Nota.</param>
		/// <param name="audi">Audi.</param>
		/// <param name="modulo">Modulo.</param>
		public void AtualizaPontuacaoModuloAuditoria(int nota, int audi, int modulo)
		{
			var mod = this.database.Table<Modulos>().First(x => x.audi == audi && x.modulo == modulo);
			mod.pontuacao += nota;
			var modCompleto = this.TrocaStatusModuloCompleto(audi, modulo);
			mod.completo = modCompleto;
			database.Update(mod);


			var mods = this.database.Table<Modulos>().Where(m => m.audi == audi);
			var sumNotas = 0.0;
			var sumPesos = 0.0;

			foreach (var m in mods)
			{
				sumNotas += m.pontuacao;
				sumPesos += m.somaPesos;
			}
        
			var auditoria = this.database.Table<Auditorias>().First(x => x.audi == audi);
			auditoria.nota = sumNotas <= 0 ? 0 : 100 * (sumNotas / sumPesos);
			database.Update(auditoria);
		}

		public void SubtraiPontuacaoAntesDeAtualizar(int pontuacao, int audi, int modulo)
		{
			var mod = this.database.Table<Modulos>().First(x => x.audi == audi && x.modulo == modulo);
			mod.pontuacao -= pontuacao;
			var modCompleto = this.TrocaStatusModuloCompleto(audi, modulo);
			mod.completo = modCompleto;
			database.Update(mod);

			var auditoria = this.database.Table<Auditorias>().First(x => x.audi == audi);
			auditoria.nota = auditoria.nota == null ? pontuacao : auditoria.nota - pontuacao;
			database.Update(auditoria);
		}

		/// <summary>
		/// Atualiza o valor da pontuação da questão.
		/// </summary>
		/// <param name="questao">Questao.</param>
		/// <param name="pontuacao">Pontuacao.</param>
		/// <param name="modulo">Modulo.</param>
		public void AtualizaPontuacaoQuestao(int questao, int pontuacao, int modulo, int audi, int SomaPeso)
		{
			var q = this.database.Table<Questoes>().First(x => x.questao == questao && x.modulo == modulo);
			q.pontuacao = pontuacao;

			this.AtualizaSomaPesoModulo(modulo, SomaPeso, audi);

			// Atualizando a pontuação do Módulo.
			this.AtualizaPontuacaoModuloAuditoria(pontuacao, audi, modulo);
		}

		public void AtualizaSomaPesoModulo(int modulo, int SomaPeso, int audi)
		{
			var mod = this.database.Table<Modulos>().First(x => x.modulo == modulo && x.audi == audi);
			mod.somaPesos += SomaPeso;
			database.Update(mod);
		}

		public void SubtraiSomaPesoModulo(int modulo, int SomaPeso)
		{
			var mod = this.database.Table<Modulos>().First(x => x.modulo == modulo);
			mod.somaPesos -= SomaPeso;
			database.Update(mod);
		}

		public bool CarregaOffline()
		{
			if (this.database.Table<Auditorias>().Any())
			{
				return this.database.Table<Auditorias>().Any(x => x.resultado == 3);
			}

			return false;
		}

		public void SalvarAssinatura(string imagem, int audi)
		{
			var auditoria = this.database.Table<Auditorias>().First(x => x.audi == audi);
			auditoria.assinatura = imagem;

			this.database.Update(auditoria);
		}

		public void InserirFotoAuditoria(string auditoria, string modulo, string questao, string nomeimagem)
		{
			this.database.Insert(new ImagemAuditoriaModulo{ AuditoriaId = auditoria, ModoloId = modulo, Questao = questao, NomeImagem = nomeimagem });
		}

		public List<ImagemAuditoriaModulo> GetImagensAuditoria(string auditoria)
		{
			if (this.database.Table<ImagemAuditoriaModulo>().Any(a => a.AuditoriaId == auditoria))
				return this.database.Table<ImagemAuditoriaModulo>().Where(a => a.AuditoriaId == auditoria).ToList();

			return null;
		}

		public List<string> GetImagensAuditoriaModulos(string audi, string mod, string questao)
		{
			if (this.database.Table<ImagemAuditoriaModulo>().Any(a => a.AuditoriaId == audi && a.ModoloId == mod))
				return this.database.Table<ImagemAuditoriaModulo>().Where(a => a.AuditoriaId == audi && a.ModoloId == mod
					&& a.Questao == questao)
					.Select(a => a.NomeImagem)
					.ToList();

			return null;
		}
	}
}

