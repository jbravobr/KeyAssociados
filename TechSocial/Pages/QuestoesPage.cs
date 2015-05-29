using System;
using System.Linq;

using Xamarin.Forms;
using Acr.XamForms.Mobile.Media;
using Autofac;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace TechSocial
{
	public class QuestoesPage : ContentPage
	{
		QuestoesViewModel model;
		ICollection<Questoes> _questoes;
		int modulo = 0;
		string checklistId;
		string audi = string.Empty;
		string criterioQuestao;
		Label lblRequisito;
		Label lblPeso;
		MyButton entObservacoes;
		MyButton entAcoesRequeridas;
		Entry entryCriterio;
		MyButton entryDescricaoBaseLegal;
		DatePicker dataPicker;
		Image thumbImagem;
		int page = 1;
		string prazo = string.Empty;
		string prazoData = string.Empty;
		string imagem = string.Empty;
		Questoes questao = null;
		Respostas resposta = null;
		string baseLegalId = string.Empty;

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			if (model != null)
				return;
            
			model = App.Container.Resolve<QuestoesViewModel>();
			await model.MontarQuestao(modulo);
			_questoes = model.Questao;
			BindingContext = questao = _questoes.First();
			resposta = model.GetQuestaoResposta(questao.questao, audi, modulo.ToString());
			lblRequisito.SetBinding(Label.TextProperty, "Pergunta");
			lblPeso.SetBinding(Label.TextProperty, "PesoPergunta");
			entObservacoes.SetBinding(Label.TextProperty, "Texto");

			// Exibe os paginadores.
			if (model.Questao.Count > 1 && !this.ToolbarItems.Any())
			{
				this.ToolbarItems.Add(new ToolbarItem("anterior", "less.png", () =>
                    MessagingCenter.Send<QuestoesPage, ICollection<Questoes>>(this, "anterior", _questoes), ToolbarItemOrder.Primary));
                
				this.ToolbarItems.Add(new ToolbarItem("próxima", "more.png", () => 
                        MessagingCenter.Send<QuestoesPage, ICollection<Questoes>>(this, "proximo", _questoes), ToolbarItemOrder.Default));
			}

			// Se para a questão carregada houver resposta preenche os campos na tela.
			if (resposta != null)
			{
				entryCriterio.Text = resposta.atende == "0" ? "Não" : resposta.atende == "1" ? "Sim" : resposta.atende;
				this.criterioQuestao = resposta.atende;
				entryDescricaoBaseLegal.Text = resposta.baseLegalTexto;

				if (!String.IsNullOrEmpty(resposta.dt_prazo))
				{
					dataPicker.Date = Convert.ToDateTime(resposta.dt_prazo);
					dataPicker.IsVisible = true;
				}

				entAcoesRequeridas.Text = resposta.acoesRequeridadas;
				entObservacoes.Text = resposta.observacao;

				if (!String.IsNullOrEmpty(resposta.evidencia))
				{
					var imgSrc = DependencyService.Get<ISaveAndLoadFile>().GetImage(resposta.evidencia);
					thumbImagem.Source = imgSrc;
				}
			}
		}

		public QuestoesPage(int modulo, string audi, string checklistId)
		{
			this.checklistId = checklistId;
			this.modulo = modulo;
			this.audi = audi;
			this.Title = "Questão";

			entryCriterio = new Entry
			{
				HeightRequest = 20, 
				WidthRequest = 20, 
				IsEnabled = false,
			};

			var btnCriterio = new Button
			{
				Text = "Critério"
			};
			btnCriterio.Clicked += (sender, e) =>
			{
				var dialogService = DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>();
				var config = new Acr.XamForms.UserDialogs.ActionSheetConfig();	
                   
				config.SetTitle("Critério");
                
				foreach (var valorCriterio in model.Questao.First().criterio.Split('/'))
				{
					Action _selecionaResposta = () => this.criterioQuestao = entryCriterio.Text = valorCriterio;
					config.Options.Add(new Acr.XamForms.UserDialogs.ActionSheetOption(valorCriterio, _selecionaResposta));
				}
				dialogService.ActionSheet(config);
			};

			entryDescricaoBaseLegal = new MyButton
			{
				HeightRequest = 90,
				WidthRequest = 140,
			};
            
			var btnBaseLegal = new Button
			{
				Text = "Base Legal"
			};
			btnBaseLegal.Clicked += async (sender, e) =>
			{
				var dialogService = DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>();
				var config = new Acr.XamForms.UserDialogs.ActionSheetConfig();

				// Retornando Bases Legais.
				var dadosBasesLegais = await new BaseLegalService().RetornarBasesLegaisPorChecklistQuestao(this.questao.questao.ToString(), this.checklistId);

				config.SetTitle("Base Legal");

				foreach (var bl in dadosBasesLegais.BaseLegal)
				{
					Action _selecionaBase = () =>
					{
						entryDescricaoBaseLegal.Text = bl.descricao;
						this.baseLegalId = bl.id_baselegal;
						entryDescricaoBaseLegal.IsEnabled = true;
					};
					config.Options.Add(new Acr.XamForms.UserDialogs.ActionSheetOption(bl.nome, _selecionaBase));
				}
				dialogService.ActionSheet(config);
			};

			dataPicker = new DatePicker
			{
				Format = "dd/MM/yyyy",
				IsVisible = false
			};
			var btnData = new Button
			{
				Text = "Data Prazo"
			};
			btnData.Clicked += (sender, e) =>
			{
				var dialogService = DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>();
				var config = new Acr.XamForms.UserDialogs.ActionSheetConfig();

				config.SetTitle("Data Prazo");
				Action _imediato = () =>
				{
					this.prazo = "1";
					this.dataPicker.Date = DateTime.Now;
					this.dataPicker.IsVisible = true;
				};
				Action _pedeData = () =>
				{
					this.prazo = "2";
					this.dataPicker.IsVisible = true;
				};
				config.Options.Add(new Acr.XamForms.UserDialogs.ActionSheetOption("Imediato", _imediato));
				config.Options.Add(new Acr.XamForms.UserDialogs.ActionSheetOption("Pede Data", _pedeData));
				dialogService.ActionSheet(config);
			};

			entObservacoes = new MyButton
			{
				HeightRequest = 90,
				WidthRequest = 130
			};

			entAcoesRequeridas = new MyButton
			{
				HeightRequest = 90,
				WidthRequest = 130,
				VerticalOptions = LayoutOptions.Center
			};

			var btnSalvar = new Button
			{
				Text = "Salvar"
			};
                       
			lblRequisito = new Label
			{
				FontFamily = "ArialHebrew-Bold",
				FontSize = 18,
				TextColor = Color.FromHex("#333333"),
				LineBreakMode = LineBreakMode.WordWrap,
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.Start
			};
            
			lblPeso = new Label
			{
				FontFamily = "ArialHebrew-Bold",
				FontSize = 24,
				TextColor = Color.FromHex("#333333"),
				LineBreakMode = LineBreakMode.WordWrap
			};

			btnSalvar.Clicked += async (sender, e) =>
			{
				var obs = entObservacoes.Text;
				var evidencia = string.Empty;
				var criterio = this.criterioQuestao;
				var baseLegalId = this.baseLegalId;
				var baseLegalTexto = entryDescricaoBaseLegal.Text;
				var data = dataPicker.Date.ToString("yyyy-MM-dd");
				var imagemEvidencia = this.imagem;
				var acoesRequeridas = entAcoesRequeridas.Text;
				var peso = this.lblPeso.Text;

				// Salva uma nova resposta.
				if (this.resposta == null)
					await SalvarResposta(obs, evidencia, criterio, baseLegalId, baseLegalTexto, data, 
						imagemEvidencia, audi, acoesRequeridas, peso);
				else // Atualiza uma resposta dada.
                    await SalvarResposta(obs, evidencia, criterio, baseLegalId, baseLegalTexto, data, 
						imagemEvidencia, audi, acoesRequeridas, peso, this.resposta._id);
			};

			// Paginação de próximo.
			MessagingCenter.Subscribe<QuestoesPage,ICollection<Questoes>>(this, "proximo", (sender, questoes) =>
				{
					try
					{
						if (_questoes.Count > 1 && page < _questoes.Count)
						{
							this.entObservacoes.Text = string.Empty;
							this.criterioQuestao = string.Empty;
							this.entryCriterio.Text = string.Empty;
							this.entryDescricaoBaseLegal.Text = string.Empty;
							this.prazo = string.Empty;
							this.dataPicker = new DatePicker{ Format = "dd/MM/yyyy" };
							this.dataPicker.IsVisible = false;
							this.entAcoesRequeridas.Text = string.Empty;
							this.thumbImagem.Source = null;
							this.imagem = String.Empty;

							questao = questoes.Skip(page).Take(1).First();
							BindingContext = questao;
							resposta = model.GetQuestaoResposta(questao.questao, audi, modulo.ToString());

							if (resposta != null)
							{
								entryCriterio.Text = resposta.atende == "0" ? "Não" : resposta.atende == "1" ? "Sim" : resposta.atende;
								entryDescricaoBaseLegal.Text = resposta.baseLegalTexto;
								this.criterioQuestao = resposta.atende;

								if (!String.IsNullOrEmpty(resposta.dt_prazo))
								{
									dataPicker.Date = Convert.ToDateTime(resposta.dt_prazo);
									dataPicker.IsVisible = true;
								}

								entAcoesRequeridas.Text = resposta.acoesRequeridadas;
								entObservacoes.Text = resposta.observacao;

								if (!String.IsNullOrEmpty(resposta.evidencia))
								{
									var imgSrc = DependencyService.Get<ISaveAndLoadFile>().GetImage(resposta.evidencia);
									thumbImagem.Source = ImageSource.FromFile(imgSrc);
								}
								this.imagem = resposta.evidencia;
							}

							lblRequisito.SetBinding(Label.TextProperty, "Pergunta");
							lblPeso.SetBinding(Label.TextProperty, "PesoPergunta");
							entObservacoes.SetBinding(Label.TextProperty, "Texto");

							page++;
						}
						else
						{
							DisplayAlert("Aviso", "Não existem mais questões para este módulo", "OK");
							return;
						}
					}
					catch
					{
						return;
					}

				});

			// Paginação de anterior.
			MessagingCenter.Subscribe<QuestoesPage,ICollection<Questoes>>(this, "anterior", (sender, questoes) =>
				{
					try
					{
						page--;
                        
						questao = questoes.Skip(page).Take(1).First();
						BindingContext = questao;
						resposta = model.GetQuestaoResposta(questao.questao, audi, modulo.ToString());

						if (resposta != null)
						{
							entryCriterio.Text = resposta.atende == "0" ? "Não" : resposta.atende == "1" ? "Sim" : resposta.atende;
							this.criterioQuestao = resposta.atende;
							entryDescricaoBaseLegal.Text = resposta.baseLegalTexto;

							if (!String.IsNullOrEmpty(resposta.dt_prazo))
							{
								dataPicker.Date = Convert.ToDateTime(resposta.dt_prazo);
								dataPicker.IsVisible = true;
							}

							entAcoesRequeridas.Text = resposta.acoesRequeridadas;
							entObservacoes.Text = resposta.observacao;

							if (!String.IsNullOrEmpty(resposta.evidencia))
							{
								var imgSrc = DependencyService.Get<ISaveAndLoadFile>().GetImage(resposta.evidencia);
								thumbImagem.Source = ImageSource.FromFile(imgSrc);
							}
							this.imagem = resposta.evidencia;
						}

						lblRequisito.SetBinding(Label.TextProperty, "Pergunta");
						lblPeso.SetBinding(Label.TextProperty, "PesoPergunta");
						entObservacoes.SetBinding(Label.TextProperty, "Texto");
					}
					catch
					{
						return;
					}
				});

			var gridCriterio = new Grid();

			gridCriterio.ColumnSpacing = -170;
			gridCriterio.VerticalOptions = LayoutOptions.Start;
			gridCriterio.Children.Add(btnCriterio, 0, 1);
			gridCriterio.Children.Add(entryCriterio, 1, 1);

			var gridBaseLegal = new Grid();
            
			gridBaseLegal.ColumnSpacing = -170;
			gridBaseLegal.VerticalOptions = LayoutOptions.Start;
			gridBaseLegal.Children.Add(btnBaseLegal, 0, 1);
			gridBaseLegal.Children.Add(entryDescricaoBaseLegal, 1, 1);

			var gridData = new Grid();
            
			gridData.ColumnSpacing = -170;
			gridData.VerticalOptions = LayoutOptions.Start;
			gridData.Children.Add(btnData, 0, 1);
			gridData.Children.Add(dataPicker, 1, 1);

			var btnAnexo = new Button
			{
				Text = "Capturar Foto"
			};
			btnAnexo.Clicked += async (sender, e) =>
			{
				var obs = entObservacoes.Text;
				var criterio = this.criterioQuestao;
				var baseLegalId = this.baseLegalId;
				var baseLegalTexto = entryDescricaoBaseLegal.Text;
				var data = dataPicker.Date.ToString("yyyy-MM-dd");
				var imagemEvidencia = this.imagem;
				var acoesRequeridas = entAcoesRequeridas.Text;

				await TrataFoto(obs, criterio, baseLegalId, baseLegalTexto, data, 
					imagemEvidencia, audi, acoesRequeridas);
			};

			thumbImagem = new Image
			{
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,
				HeightRequest = 100,
				WidthRequest = 100
			};

			var stack = new StackLayout
			{
				Padding = new Thickness(10, 20, 10, 10),
				Spacing = 5,
				Children =
				{ 
					lblRequisito, 
					lblPeso, 
					gridCriterio,    
					gridBaseLegal,
					gridData,
					entAcoesRequeridas,
					entObservacoes,
					btnAnexo,
					thumbImagem,
					btnSalvar 
				},
				VerticalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Vertical
			};

			this.Content = new ScrollView{ Content = stack, Orientation = ScrollOrientation.Vertical };
		}

		// Salva a resposta.
		private async Task SalvarResposta(string obs, string evidencia, string criterio, string base_id,
		                                  string baseLegalTexto, string data, string imagemEvidencia, 
		                                  string _audi, string acoesRequeridas, string peso, int? _id = null)
		{
			if (!String.IsNullOrEmpty(criterio))
			{
				if (model.SalvarQuestao(obs, evidencia, criterio, baseLegalId, baseLegalTexto
                , data, imagemEvidencia, audi, this.modulo.ToString(), this.questao.questao.ToString(), this.prazo, acoesRequeridas, peso
                , _id))
				{
					await DisplayAlert("Sucesso", "Resposta Salva com sucesso!", "OK");

					// Troca de questão em caso de haver mais de 1 e não estar na última.
					if (_questoes.Count > 1 && page < _questoes.Count)
						MessagingCenter.Send<QuestoesPage, ICollection<Questoes>>(this, "proximo", _questoes);
					else
						await this.Navigation.PopAsync();
				}
				else
					await DisplayAlert("Erro", "Erro ao salvar questão", "OK");
			}
			else
				await DisplayAlert("Erro", "Você precisa informar um critério", "OK");
		}

		// Tira foto.
		private async Task TrataFoto(string obs, string criterio, string base_id,
		                             string baseLegalTexto, string data, string imagemEvidencia, 
		                             string _audi, string acoesRequeridas, int? _id = null)
		{
			this.entObservacoes.Text = obs;
			this.entryCriterio.Text = criterio;
			this.baseLegalId = base_id;
			this.entryDescricaoBaseLegal.Text = baseLegalTexto;
			this.dataPicker.Date = Convert.ToDateTime(data);
			this.audi = _audi;
			this.entAcoesRequeridas.Text = acoesRequeridas;
			var mediaPickerService = DependencyService.Get<IMediaPicker>();

			if (mediaPickerService.IsCameraAvailable || mediaPickerService.IsPhotoGalleryAvailable)
			{
				var options = new CameraOptions
                    { Camera = CameraDevice.Rear };

				var photo = await mediaPickerService.TakePhoto(options);
				ImageSource imgSource;

				if (photo != null)
				{
					imgSource = ImageSource.FromStream(() =>
						{
							var stream = photo.GetStream();
							//photo.Dispose();
							return stream;
						});

					//var imgSource = ImageSource.FromResource("TechSocial.Content.Images.imagemteste.jpg");

					var imgNome = String.Concat(Path.GetRandomFileName(), ".jpg");
					var salvarImagem = await DependencyService.Get<ISaveAndLoadFile>().SaveImage(imgSource, imgNome);

					this.imagem = imgNome;
					this.thumbImagem.Source = imgSource;

					if (!salvarImagem)
						await DisplayAlert(String.Empty, "Erro ao salvar a imagem, tente novamente!", "OK");
					else
						await DisplayAlert(String.Empty, "Imagem salva com sucesso!", "OK");
				}
				else
					return;
			}

			return;
		}
	}
}

