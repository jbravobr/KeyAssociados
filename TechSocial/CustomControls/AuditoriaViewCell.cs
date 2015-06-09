using System;
using Xamarin.Forms;
using System.IO;
using System.Threading.Tasks;
using Autofac;

namespace TechSocial
{
	public class AuditoriaViewCell : ViewCell
	{
		public AuditoriaViewCell()
		{
			var lblIdAuditoria = new Label
			{
				FontFamily = "ArialHebrew-Bold",
				FontSize = 18,
				TextColor = Color.FromHex("#333333")
			};
			lblIdAuditoria.SetBinding(Label.TextProperty, "audi");

			var lblAuditoria = new Label()
			{
				FontFamily = "ArialHebrew-Bold",
				FontSize = 18,
				TextColor = Color.FromHex("#333333"),
				Text = "Auditoria: Manutencao"
			};

			var lblData = new Label
			{
				FontFamily = "HelveticaNeue-Thin",
				FontSize = 14,
				TextColor = Color.FromHex("#666")
			};
			lblData.SetBinding(Label.TextProperty, new Binding("data"){ StringFormat = "Data: {0:dd/MM/yyyy}" });

			var lnlNota = new Label
			{
				FontFamily = "GeezaPro-Bold",
				FontSize = 20,
				TextColor = Color.FromHex("#333333")
			};
			lnlNota.SetBinding(Label.TextProperty, new Binding("nota"){ StringFormat = "Nota: {0}" });

			var imgSeta = new Image
			{ 
				VerticalOptions = LayoutOptions.Center, 
				HorizontalOptions = LayoutOptions.End
			};
			imgSeta.SetBinding(Image.SourceProperty, "Imagem");

			var rotalLayout = new StackLayout
			{
				Padding = new Thickness(10, 0, 0, 0),
				Spacing = 3,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = { lblAuditoria, lblData, lnlNota, lblIdAuditoria }
			};

			var cellLayout = new StackLayout
			{
				Spacing = 0,
				Padding = new Thickness(10, 5, 10, 5),
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = { rotalLayout, imgSeta },
			};
			cellLayout.SetBinding(StackLayout.IsEnabledProperty, "Ativo");

			var revisarAction = new MenuItem { Text = "Enviar", IsDestructive = true };
			revisarAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("audi"));
			revisarAction.Clicked += async (sender, e) =>
			{
				var dialog = DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>();
				var alert = DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>();
				dialog.ShowLoading("Enviando...");

				var model = App.Container.Resolve<ChecklistViewModel>();
				var audi = ((int)((MenuItem)sender).CommandParameter);
				var result = await model.EnviarRespostas(audi);

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
			};

			var enviarAction = new MenuItem { Text = "Revisar" };
			enviarAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("audi"));
			enviarAction.Clicked += (sender, e) =>
			{
				var audi = ((int)((MenuItem)sender).CommandParameter);
				MessagingCenter.Send<AuditoriaViewCell,int>(this, "finalizar", audi);
			};

			var assinarAction = new MenuItem { Text = "Assinar" };
			assinarAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("audi"));
			assinarAction.Clicked += (sender, e) =>
			{
				var audi = ((int)((MenuItem)sender).CommandParameter);
				MessagingCenter.Send<AuditoriaViewCell,int>(this, "assinar", audi);
			};

			ContextActions.Add(revisarAction);
			ContextActions.Add(assinarAction);
			ContextActions.Add(enviarAction);

			this.View = cellLayout;
		}
	}
}

