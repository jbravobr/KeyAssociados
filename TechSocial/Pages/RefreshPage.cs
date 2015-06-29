using System;

using Xamarin.Forms;

namespace TechSocial
{
	public class RefreshPage : ContentPage
	{
		public RefreshPage()
		{
			Content = new StackLayout
			{ 
				Children =
				{
					new Label { Text = "Hello ContentPage" }
				}
			};
		}
	}
}


