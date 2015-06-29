using System;

using Xamarin.Forms;
using System.Collections.Generic;

namespace TechSocial
{
	public class MenuListView : ListView
	{
		public MenuListView()
		{
			List<MenuMasterItem> data = new MenuListData();

			ItemsSource = data;
			VerticalOptions = LayoutOptions.FillAndExpand;
			BackgroundColor = Color.Transparent;
			this.SeparatorVisibility = SeparatorVisibility.None;

			var cell = new DataTemplate(typeof(MenuItemCell));

			ItemTemplate = cell;
		}
	}
}


