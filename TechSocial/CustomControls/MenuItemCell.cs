using System;
using Xamarin.Forms;

namespace TechSocial
{
	public class MenuItemCell : ViewCell
	{
		public MenuItemCell()
		{
			var ico = new Image();
			ico.SetBinding(Image.SourceProperty, "IconSource");
			ico.HorizontalOptions = LayoutOptions.Start;

			var lblIco = new Label();
			lblIco.SetBinding(Label.TextProperty, "Title");
			lblIco.TextColor = Color.White;
			lblIco.FontSize = 14;
			lblIco.HorizontalOptions = LayoutOptions.FillAndExpand;

			var mainLayout = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = 8,
				Padding = 10,
				Children = { ico, lblIco }
			};

			this.View = mainLayout;
		}
	}
}

