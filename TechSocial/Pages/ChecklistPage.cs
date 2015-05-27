using System;
using Xamarin.Forms;
using Autofac;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace TechSocial
{
	public class ChecklistPage : ContentPage
	{
		ChecklistViewModel model = null;
		string audi = String.Empty;
		ListView listViewModulos;

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			model = App.Container.Resolve<ChecklistViewModel>();
			await model.MontarModulos(audi);

			BindingContext = model.Modulos;
			listViewModulos.ItemsSource = model.Modulos;
			listViewModulos.ItemTemplate = new DataTemplate(typeof(CheckListViewCell));
		}

		public ChecklistPage(string audi)
		{
			Title = "Módulos";
			this.audi = audi;
			this.BackgroundColor = Color.FromHex("#EEEEEE");

			listViewModulos = new ListView
			{
				VerticalOptions = LayoutOptions.StartAndExpand,
				SeparatorVisibility = SeparatorVisibility.Default,
				RowHeight = 77,
				HasUnevenRows = true
			};

			listViewModulos.ItemTapped += async (sender, e) => await ExibePerguntaDoModulo(e.Item);

			var layout = new StackLayout { Children = { listViewModulos } };

			this.Content = layout;
		}

		async Task ExibePerguntaDoModulo(object item)
		{
			await Navigation.PushAsync(new QuestoesPage(((Modulos)item).modulo, ((Modulos)item).audi.ToString(), ((Modulos)item).checklist));
		}
	}
}

