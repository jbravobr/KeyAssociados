using System;
using System.Linq;

using Xamarin.Forms;
using System.Collections.Generic;
using Autofac;
using System.Threading.Tasks;

namespace TechSocial
{
	public class QuestoesComProblema : ContentPage
	{
		List<Questoes> questoes;
		string audi;
		int pagina = 0;
		QuestaoProblemaView questaoProblemaView;
		Questoes questaoParaExibicao;
		StackLayout mainLayout;
		string Data;

		public QuestoesComProblema(string auditoriaId)
		{
			this.audi = auditoriaId;
			var data = new TechSocialDatabase(false);
			questoes = new List<Questoes>();

			var respostas = data.GetRespostaPorAuditoria(Convert.ToInt32(audi))
				.Where(r => (r.atende != "2" && r.atende != "Sim" && r.atende != "NA")
				                && !String.IsNullOrEmpty(r.atende));

			foreach (var resposta in respostas)
			{
				questoes.AddRange(data.GetQuestoes()
					.Where(q => q.questao.ToString() == resposta.questao));
			}

			mainLayout = new StackLayout
			{
				Orientation = StackOrientation.Vertical,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = 2,
				Padding = new Thickness(15, Device.OnPlatform(20, 0, 0), 5, 0),
				HeightRequest = DependencyService.Get<Acr.XamForms.Mobile.IDeviceInfo>().ScreenHeight,
				WidthRequest = DependencyService.Get<Acr.XamForms.Mobile.IDeviceInfo>().ScreenWidth,
			};

			#region ToolbarItem

			if (respostas != null && respostas.Any())
			{
				this.ToolbarItems.Add(new ToolbarItem(String.Empty, "less.png", () =>
						{
							MessagingCenter.Send<QuestoesComProblema>(this, "anterior");
						}, ToolbarItemOrder.Secondary));

				this.ToolbarItems.Add(new ToolbarItem(String.Empty, "more.png", () =>
						{
							MessagingCenter.Send<QuestoesComProblema>(this, "proxima");
						}, ToolbarItemOrder.Secondary));
			}
			#endregion

			#region Message Subscribe >> Próxima

			MessagingCenter.Subscribe<QuestoesComProblema>
			(
				this,
				"proxima",
				(sender) =>
				{
					if (this.pagina + 1 >= questoes.Count)
					{
						DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>().Alert(String.Empty, "Não há mais questões com problema", "OK");
						return;
					}

					this.pagina++;

					questaoParaExibicao = questoes.Skip(this.pagina).Take(1).First();
					questaoProblemaView = new QuestaoProblemaView(questaoParaExibicao, auditoriaId, questaoParaExibicao.modulo.ToString());
					mainLayout.Children.Clear();
					mainLayout.Children.Add(questaoProblemaView);
				}
			);

			#endregion

			#region Message Subscribe >> Anterior

			MessagingCenter.Subscribe<QuestoesComProblema>
			(
				this,
				"anterior",
				(sender) =>
				{
					if (this.pagina < 1)
					{
						DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>().Alert(String.Empty, "Não há mais questões com problema", "OK");
						return;
					}

					this.pagina--;

					questaoParaExibicao = questoes.Skip(this.pagina).Take(1).First();
					questaoProblemaView = new QuestaoProblemaView(questaoParaExibicao, auditoriaId, questaoParaExibicao.modulo.ToString());
					mainLayout.Children.Clear();
					mainLayout.Children.Add(questaoProblemaView);
				}
			);

			#endregion

			if (questoes != null && questoes.Any())
			{
				questaoParaExibicao = questoes.Skip(pagina).Take(1).First();

				questaoProblemaView = new QuestaoProblemaView(questaoParaExibicao, auditoriaId, questaoParaExibicao.modulo.ToString());
				mainLayout.Children.Add(questaoProblemaView);

			}
			else
			{
				mainLayout.Children.Add(new Label
					{
						Text = "Não há problemas!",
						FontSize = 33,
						XAlign = TextAlignment.Center,
						VerticalOptions = LayoutOptions.Center
					});
			}

			this.Content = mainLayout;
		}

		public QuestoesComProblema()
		{
			
		}

		private async Task Enviar()
		{
			var dialog = DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>();
			var alert = DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>();
			dialog.ShowLoading("Enviado");
			
			var model = App.Container.Resolve<ChecklistViewModel>();
			var result = await model.EnviarRespostas(Convert.ToInt32(this.audi));
			
			if (result == ExceptionEnvioRespostas.Enviado)
				dialog.HideLoading();
			else if (result == ExceptionEnvioRespostas.RespostasPendentes)
			{
				dialog.HideLoading();
				await alert.AlertAsync("Existem questões pendentes de respostas!", "Aviso", "OK");
			}
			else
			{
				dialog.HideLoading();
				await alert.AlertAsync("Houve um erro ao enviar, tente novamente!", "Erro", "OK");	
			}
		}
	}
}


