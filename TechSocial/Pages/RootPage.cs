using System;

using Xamarin.Forms;

namespace TechSocial
{
	public class RootPage : MasterDetailPage
	{
		public RootPage()
		{
			var menuPage = new MenuPage();

			//menuPage.Menu.ItemSelected += (sender, e) => NavigateTo(e.SelectedItem as MenuMasterItem);

			Master = menuPage;
			Detail = new NavigationPage(new SemanaPage());
		}

		void NavigateTo(MenuMasterItem menu)
		{
			Page displayPage = (Page)Activator.CreateInstance(menu.TargetType);

			Detail = new NavigationPage(displayPage);

			IsPresented = false;
		}
	}
}


