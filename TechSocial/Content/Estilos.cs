using System;
using Xamarin.Forms;

namespace TechSocial
{
	public static class Estilos
	{
		public static Style buttonDefaultStyle = new Style(typeof(Button))
		{
			Setters =
			{
				new Setter{ Property = Button.BackgroundColorProperty, Value = Color.FromHex("#32419A") },
				new Setter { Property = Button.TextColorProperty, Value = Color.White },
				new Setter{ Property = Button.BorderRadiusProperty, Value = 2 },
				new Setter
				{
					Property = Button.FontFamilyProperty,
					Value = Device.OnPlatform(
						iOS: "Helvetica",
						Android: "Roboto",
						WinPhone: "Segoe"
					)
				},
				new Setter { Property = Button.FontAttributesProperty, Value = FontAttributes.Bold },
				new Setter
				{
					Property = Button.HorizontalOptionsProperty,
					Value = LayoutOptions.EndAndExpand
				},
				new Setter
				{
					Property = Button.WidthRequestProperty,
					Value = 140
				},
				new Setter
				{
					Property = Button.VerticalOptionsProperty,
					Value = LayoutOptions.Center
				},
				new Setter
				{
					Property = Button.HeightRequestProperty,
					Value = 50
				}
			}
		};
	}
}

