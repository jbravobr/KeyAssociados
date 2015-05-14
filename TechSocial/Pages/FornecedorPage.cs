using System;
using Xamarin.Forms;

namespace TechSocial
{
	public class FornecedorPage : ContentPage
	{
		FornecedorViewModel model;

		public FornecedorPage()
		{
			Title = "Fornecedores";
			BindingContext = model = new FornecedorViewModel();

			var listViewFornecedores = new ListView
			{
				ItemsSource = model.Fornecedores,
				VerticalOptions = LayoutOptions.StartAndExpand,
				ItemTemplate = new DataTemplate(typeof(FornecedorViewCell)),
				SeparatorVisibility = SeparatorVisibility.Default,
				RowHeight = 77,
				HasUnevenRows = true
			};

			listViewFornecedores.ItemTapped += (sender, e) =>
			{
				if (e.Item == null)
					return; 
				((ListView)sender).SelectedItem = null;
			};

			var layout = new StackLayout { Children = { listViewFornecedores } };

			this.Content = layout;
		}
	}
}

