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
				entryDescricaoBaseLegal.entry.Text = resposta.baseLegalTexto;

				if (!String.IsNullOrEmpty(resposta.dt_prazo))
				{
					dataPicker.Date = Convert.ToDateTime(resposta.dt_prazo);
					dataPicker.IsVisible = true;
				}

				entAcoesRequeridas.entry.Text = resposta.acoesRequeridas;
				entObservacoes.entry.Text = resposta.observacao;

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

			#region Critério
			entryCriterio = new Entry{ IsEnabled = false };
			
			var btnCriterio = new Button{ Text = "Critério" };
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
			#endregion

			#region Bases Legais
			entryDescricaoBaseLegal = new MyButton();
			entryDescricaoBaseLegal.entry.HeightRequest = 90;
            
			var btnBaseLegal = new Button{ Text = "Base Legal" };
			btnBaseLegal.Clicked += async (sender, e) =>
			{
				var dialogService = DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>();
				var config = new Acr.XamForms.UserDialogs.ActionSheetConfig();
				
				config.SetTitle("Base Legal");
				
				Action _selecionaBase = () =>
				{
					entryDescricaoBaseLegal.entry.Text = questao.BaseLegalDescricao;
					this.baseLegalId = questao.BaseLegalId;
					entryDescricaoBaseLegal.IsEnabled = true;
				};
				config.Options.Add(new Acr.XamForms.UserDialogs.ActionSheetOption(questao.BaseLegalNome, _selecionaBase));
				dialogService.ActionSheet(config);
			};
			#endregion

			#region Data
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
			#endregion

			#region Observações
			entObservacoes = new MyButton();
			entObservacoes.entry.HeightRequest = 90;
			#endregion

			#region Ações Requeridas
			entAcoesRequeridas = new MyButton();
			entAcoesRequeridas.entry.HeightRequest = 90;
			#endregion

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
			#endregion
            
			#region Peso
			lblPeso = new Label
			{
				FontFamily = "ArialHebrew-Bold",
				FontSize = 24,
				TextColor = Color.FromHex("#333333"),
				LineBreakMode = LineBreakMode.WordWrap
			};
			#endregion

			var btnSalvar = new Button
			{
				Text = "Salvar",
				Style = Estilos.buttonDefaultStyle
			};
			btnSalvar.Clicked += async (sender, e) =>
			{
				var obs = entObservacoes.entry.Text;
				var evidencia = string.Empty;
				var criterio = this.criterioQuestao;
				var baseLegalId = this.baseLegalId;
				var baseLegalTexto = entryDescricaoBaseLegal.entry.Text;
				var data = dataPicker.Date.ToString("yyyy-MM-dd");
				var imagemEvidencia = this.imagem;
				var acoesRequeridas = entAcoesRequeridas.entry.Text;
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
							this.entObservacoes.entry.Text = string.Empty;
							this.criterioQuestao = string.Empty;
							this.entryCriterio.Text = string.Empty;
							this.entryDescricaoBaseLegal.entry.Text = string.Empty;
							this.prazo = string.Empty;
							this.dataPicker = new DatePicker{ Format = "dd/MM/yyyy" };
							this.dataPicker.IsVisible = false;
							this.entAcoesRequeridas.entry.Text = string.Empty;
							this.thumbImagem.Source = null;
							this.imagem = String.Empty;

							questao = questoes.Skip(page).Take(1).First();
							BindingContext = questao;
							resposta = model.GetQuestaoResposta(questao.questao, audi, modulo.ToString());

							if (resposta != null)
							{
								entryCriterio.Text = resposta.atende == "0" ? "Não" : resposta.atende == "1" ? "Sim" : resposta.atende;
								entryDescricaoBaseLegal.entry.Text = resposta.baseLegalTexto;
								this.criterioQuestao = resposta.atende;

								if (!String.IsNullOrEmpty(resposta.dt_prazo))
								{
									dataPicker.Date = Convert.ToDateTime(resposta.dt_prazo);
									dataPicker.IsVisible = true;
								}

								entAcoesRequeridas.entry.Text = resposta.acoesRequeridas;
								entObservacoes.entry.Text = resposta.observacao;

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
							entryDescricaoBaseLegal.entry.Text = resposta.baseLegalTexto;

							if (!String.IsNullOrEmpty(resposta.dt_prazo))
							{
								dataPicker.Date = Convert.ToDateTime(resposta.dt_prazo);
								dataPicker.IsVisible = true;
							}

							entAcoesRequeridas.entry.Text = resposta.acoesRequeridas;
							entObservacoes.entry.Text = resposta.observacao;

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

			#region Grid para Critério
			var gridCriterio = new Grid
			{
				VerticalOptions = LayoutOptions.Start,
				RowDefinitions =
				{
					new RowDefinition { Height = GridLength.Auto }
				},
				ColumnDefinitions =
				{
					new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(300, GridUnitType.Star) }
				}
			};
			gridCriterio.Children.Add(btnCriterio, 0, 0);
			gridCriterio.Children.Add(entryCriterio, 1, 0);
			#endregion

			#region Grid Bases Legais
			var gridBaseLegal = new Grid
			{
				VerticalOptions = LayoutOptions.Start,
				RowDefinitions =
				{
					new RowDefinition { Height = GridLength.Auto }
				},
				ColumnDefinitions =
				{
					new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(300, GridUnitType.Star) }
				}
			};
			gridBaseLegal.Children.Add(btnBaseLegal, 0, 1);
			gridBaseLegal.Children.Add(entryDescricaoBaseLegal, 1, 1);
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
					new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(300, GridUnitType.Star) }
				}
			};
			gridData.Children.Add(btnData, 0, 1);
			gridData.Children.Add(dataPicker, 1, 1);
			#endregion

			#region Grid Observações
			var gridObs = new Grid
			{
				VerticalOptions = LayoutOptions.Start,
				RowDefinitions =
				{
					new RowDefinition { Height = GridLength.Auto }
				},
				ColumnDefinitions =
				{
					new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(300, GridUnitType.Star) }
				}
			};
			gridObs.Children.Add(new Button{ Text = "Observações" }, 0, 1);
			gridObs.Children.Add(entObservacoes, 1, 1);
			#endregion

			#region Grid Ações Requeridas
			var gridAcoesRequeridas = new Grid
			{
				VerticalOptions = LayoutOptions.Start,
				RowDefinitions =
				{
					new RowDefinition { Height = GridLength.Auto }
				},
				ColumnDefinitions =
				{
					new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(300, GridUnitType.Star) }
				}
			};
			gridAcoesRequeridas.Children.Add(new Button{ Text = "Ações Requeridas", FontSize = 12 }, 0, 1);
			gridAcoesRequeridas.Children.Add(entAcoesRequeridas, 1, 1);
			#endregion

			var btnAnexo = new Button
			{
				Text = "Capturar Foto", 
				Style = Estilos.buttonDefaultStyle
			};
			btnAnexo.Clicked += async (sender, e) =>
			{
				var obs = entObservacoes.entry.Text;
				var criterio = this.criterioQuestao;
				var baseLegalId = this.baseLegalId;
				var baseLegalTexto = entryDescricaoBaseLegal.entry.Text;
				var data = dataPicker.Date.ToString("yyyy-MM-dd");
				var imagemEvidencia = this.imagem;
				var acoesRequeridas = entAcoesRequeridas.entry.Text;

				await TrataFoto(obs, criterio, baseLegalId, baseLegalTexto, data, 
					imagemEvidencia, audi, acoesRequeridas, questao.questao.ToString(), resposta);
			};

			thumbImagem = new Image
			{
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,
				HeightRequest = 100,
				WidthRequest = 100
			};
			
			#region Grid Foto
			var gridFoto = new Grid
			{
				RowDefinitions =
				{
					new RowDefinition { Height = GridLength.Auto }
				},
				ColumnDefinitions =
				{
					new ColumnDefinition { Width = GridLength.Auto },
					new ColumnDefinition { Width = GridLength.Auto }
				},
				ColumnSpacing = 110
			};
			gridFoto.Children.Add(btnSalvar, 0, 1);
			gridFoto.Children.Add(btnAnexo, 1, 1);
			#endregion
		
			var stack = new StackLayout
			{
				Padding = new Thickness(15, 10, 5, 5),
				Spacing = 10,
				Children =
				{ 
					lblRequisito, 
					lblPeso, 
					gridCriterio,    
					gridBaseLegal,
					//gridData,
					gridObs,
					gridAcoesRequeridas, 
					gridFoto,
					//btnSalvar 
				},
				Orientation = StackOrientation.Vertical,
				VerticalOptions = LayoutOptions.FillAndExpand
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
		                             string _audi, string acoesRequeridas, string q, Respostas _resposta, int? _id = null)
		{
			this.entObservacoes.entry.Text = obs;
			this.entryCriterio.Text = criterio;
			this.baseLegalId = base_id;
			this.entryDescricaoBaseLegal.entry.Text = baseLegalTexto;
			this.dataPicker.Date = Convert.ToDateTime(data);
			this.audi = _audi;
			this.entAcoesRequeridas.entry.Text = acoesRequeridas;
			var mediaPickerService = DependencyService.Get<IMediaPicker>();

			if (_resposta != null)
				await this.Navigation.PushModalAsync(new GaleriaFotoPage(audi, modulo.ToString(), q, _resposta));
			else
				await this.Navigation.PushModalAsync(new GaleriaFotoPage(audi, modulo.ToString(), q, null));


//			if (mediaPickerService.IsCameraAvailable || mediaPickerService.IsPhotoGalleryAvailable)
//			{
//				var options = new CameraOptions
//                    { Camera = CameraDevice.Rear };
//
//				var photo = await mediaPickerService.TakePhoto(options);
//				ImageSource imgSource;
//
//				if (photo != null)
//				{
//					imgSource = ImageSource.FromStream(() =>
//						{
//							var stream = photo.GetStream();
//							//photo.Dispose();
//							return stream;
//						});
//
//					//var imgSource = ImageSource.FromResource("TechSocial.Content.Images.imagemteste.jpg");
//
//					var imgNome = String.Concat(Path.GetRandomFileName(), ".jpg");
//					var salvarImagem = await DependencyService.Get<ISaveAndLoadFile>().SaveImage(imgSource, imgNome);
//
//					this.imagem = imgNome;
//					this.thumbImagem.Source = imgSource;
//
//					if (!salvarImagem)
//						await DisplayAlert(String.Empty, "Erro ao salvar a imagem, tente novamente!", "OK");
//					else
//						await DisplayAlert(String.Empty, "Imagem salva com sucesso!", "OK");
//				}
//				else
//					return;
//			}

			return;
		}
	}
}

