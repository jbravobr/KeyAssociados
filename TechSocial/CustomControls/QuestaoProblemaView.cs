using System;
using System.Linq;

using Xamarin.Forms;
using System.Threading.Tasks;
using Autofac;

namespace TechSocial
{
	public class QuestaoProblemaView : ContentView
	{
		Label lblRequisito;
		MyButton entAcoesRequeridas;
		MyButton entryDescricaoBaseLegal;
		MyButton entObservacoes;
		public DatePicker dataPicker;
		const string prazo = "1";
		QuestoesViewModel model;
		Questoes q;
		string audi;
		string modulo;

		public QuestaoProblemaView(Questoes _questao, string audi, string modulo)
		{
			this.q = _questao;
			this.audi = audi;
			this.modulo = modulo;
			this.BindingContext = model = App.Container.Resolve<QuestoesViewModel>();

			var db = new TechSocialDatabase(false);
			var resp = db.GetRespostaPorAuditoria(Convert.ToInt32(this.audi)).First(x => x.questao == _questao.questao.ToString());

			#region Requisito (Título)
			lblRequisito = new Label
			{
				FontFamily = "ArialHebrew-Bold",
				FontSize = 18,
				TextColor = Color.FromHex("#333333"),
				LineBreakMode = LineBreakMode.WordWrap,
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.Start
			};
			lblRequisito.Text = _questao.Pergunta;
			#endregion

			#region Atende/Critério

			var entCriterio = new Entry();
			entCriterio.IsEnabled = false;
			entCriterio.Text = resp.atende;
			#endregion

			#region Grid Atende
			var gridAtende = new Grid
			{
				RowDefinitions =
				{
					new RowDefinition { Height = GridLength.Auto }
				},
				ColumnDefinitions =
				{
					new ColumnDefinition { Width = GridLength.Auto },
					new ColumnDefinition { Width = new GridLength(500, GridUnitType.Absolute) }
				}
			};
			gridAtende.Children.Add(new Button{ Text = "Critério" }, 0, 1);
			gridAtende.Children.Add(entCriterio, 1, 1);
			#endregion

			#region Ações Requeridas
			entAcoesRequeridas = new MyButton();
			entAcoesRequeridas.entry.HeightRequest = 130;
			entAcoesRequeridas.entry.IsEnabled = false;
			if (!String.IsNullOrEmpty(resp.acoesRequeridas))
				entAcoesRequeridas.entry.Text = resp.acoesRequeridas;
			#endregion

			#region Grid Ações Requeridas
			var gridAcoesRequeridas = new Grid
			{
				RowDefinitions =
				{
					new RowDefinition { Height = GridLength.Auto }
				},
				ColumnDefinitions =
				{
					new ColumnDefinition { Width = GridLength.Auto },
					new ColumnDefinition { Width = new GridLength(500, GridUnitType.Absolute) }
				}
			};
			gridAcoesRequeridas.Children.Add(new Button{ Text = "Ações Requeridas" }, 0, 1);
			gridAcoesRequeridas.Children.Add(entAcoesRequeridas, 1, 1);
			#endregion

			#region Bases Legais
			entryDescricaoBaseLegal = new MyButton();
			entryDescricaoBaseLegal.entry.HeightRequest = 130;
			entryDescricaoBaseLegal.entry.IsEnabled = false;
			if (!String.IsNullOrEmpty(resp.baseLegalTexto))
				entryDescricaoBaseLegal.entry.Text = resp.baseLegalTexto;
			#endregion

			#region Grid Bases Legais
			var gridBaseLegal = new Grid
			{
				RowDefinitions =
				{
					new RowDefinition { Height = GridLength.Auto }
				},
				ColumnDefinitions =
				{
					new ColumnDefinition { Width = GridLength.Auto },
					new ColumnDefinition { Width = new GridLength(500, GridUnitType.Absolute) }
				}
			};
			gridBaseLegal.Children.Add(new Label { Text = "Base Legal" }, 0, 1);
			gridBaseLegal.Children.Add(entryDescricaoBaseLegal, 1, 1);
			#endregion

			#region Observações
			entObservacoes = new MyButton();
			entObservacoes.entry.HeightRequest = 130;
			entObservacoes.entry.IsEnabled = false;
			if (!String.IsNullOrEmpty(resp.observacao))
				entObservacoes.entry.Text = resp.observacao;
			#endregion

			#region Grid Observações
			var gridObs = new Grid
			{
				RowDefinitions =
				{
					new RowDefinition { Height = GridLength.Auto }
				},
				ColumnDefinitions =
				{
					new ColumnDefinition { Width = GridLength.Auto },
					new ColumnDefinition { Width = new GridLength(500, GridUnitType.Absolute) }
				}
			};
			gridObs.Children.Add(new Button{ Text = "Observações" }, 0, 1);
			gridObs.Children.Add(entObservacoes, 1, 1);
			#endregion

			#region Data
			dataPicker = new DatePicker
			{
				Format = "dd/MM/yyyy",
			};
			dataPicker.DateSelected += (sender, e) => Application.Current.Properties["DataAtende"] = e.NewDate;

			if (App.Current.Properties.ContainsKey("DataAtende"))
			{
				dataPicker.Date = (DateTime)Application.Current.Properties["DataAtende"];
				dataPicker.IsEnabled = false;
			}
			#endregion

			#region Grid para Data
			var gridData = new Grid
			{
				VerticalOptions = LayoutOptions.Start,
				RowDefinitions =
				{
					new RowDefinition { Height = GridLength.Auto }
				},
				ColumnDefinitions =
				{
					new ColumnDefinition { Width = GridLength.Auto },
					new ColumnDefinition { Width = new GridLength(300, GridUnitType.Absolute) }
				}
			};
			gridData.Children.Add(new Label { Text = "Data" }, 0, 1);
			gridData.Children.Add(dataPicker, 1, 1);
			#endregion

			var btnSalvar = new Button
			{
				Text = "Salvar",
				Style = Estilos.buttonDefaultStyle
			};
			btnSalvar.Clicked += (sender, e) =>
			{
				var obs = entObservacoes.entry.Text;
				DateTime data;

				if (App.Current.Properties.ContainsKey("DataAtende"))
					data = (DateTime)Application.Current.Properties["DataAtende"];
				else
					data = dataPicker.Date;
					
				var acoesRequeridas = entAcoesRequeridas.entry.Text;
				var tp_prazo = prazo;


				var dbResposta = new TechSocialDatabase(false);
				var resposta = dbResposta.GetRespostaPorAuditoria(Convert.ToInt32(this.audi)).First(x => x.questao == _questao.questao.ToString());
				
				SalvarResposta(resposta._id, tp_prazo, data.ToString("yyyy-MM-dd"), obs, acoesRequeridas);
			};

			var stack = new StackLayout
			{
				Padding = new Thickness(10, 10, 10, 0),
				Spacing = 5,
				HeightRequest = DependencyService.Get<Acr.XamForms.Mobile.IDeviceInfo>().ScreenHeight,
				WidthRequest = DependencyService.Get<Acr.XamForms.Mobile.IDeviceInfo>().ScreenWidth,
				Children =
				{ 
					lblRequisito, 
					gridAtende,
					gridData,
					gridObs,
					gridAcoesRequeridas, 
					gridBaseLegal,
					btnSalvar 
				},
				Orientation = StackOrientation.Vertical,
			};

			this.Content = new ScrollView{ Content = stack, Orientation = ScrollOrientation.Vertical };
		}

		private void SalvarResposta(int idResposta, string tp_prazo, string data, string obs, string acoesRequeridas)
		{
			if (model.AtualizarRespostaQuestaoComProblema(idResposta, tp_prazo, data, obs, acoesRequeridas))
				DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>().Alert("Sucesso", "Resposta gravada!", "OK");
			else
				DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>().Alert("Erro", "Erro ao salvar questão", "OK");
		}
	}
}


