using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TechSocial
{
	public class GaleriaFotoPage : ContentPage
	{
		public GaleriaFotoPage()
		{
			var menu = new StackLayout
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
				Padding = new Thickness(5, Device.OnPlatform(20, 0, 0), 5, 0),
				BackgroundColor = Color.FromHex("#EEEEEE")
			};

			var icoExcluir = new Image
			{
				Source = ImageSource.FromResource("TechSocial.Content.Images.excluirFoto.png"),
				HorizontalOptions = LayoutOptions.Start
			};

			var icoAdicionar = new Image
			{
				Source = ImageSource.FromResource("TechSocial.Content.Images.adicionarFoto.png"),
				HorizontalOptions = LayoutOptions.Center,
				AnchorX = 1
			};

			var lblConcluir = new Label
			{
				Text = "Concluir",
				FontSize = 22,
				TextColor = Color.Blue,
				HorizontalOptions = LayoutOptions.EndAndExpand
			};

			var imgCapturada = new Image();
			imgCapturada.Source = ImageSource.FromResource("TechSocial.Content.Images.imagemteste.jpg"); 
			imgCapturada.Aspect = Aspect.AspectFit;

			menu.Children.Add(icoExcluir);
			menu.Children.Add(icoAdicionar);
			menu.Children.Add(lblConcluir);

			var areaFoto = new ContentView
			{
				Content = 
						new StackLayout
				{
					VerticalOptions = LayoutOptions.CenterAndExpand,
					HorizontalOptions = LayoutOptions.CenterAndExpand,
					Orientation = StackOrientation.Horizontal,
					Children =
					{
						imgCapturada
					},
					BackgroundColor = Color.Transparent
				},
				HeightRequest = DependencyService.Get<Acr.XamForms.Mobile.IDeviceInfo>().ScreenHeight - 30,
				WidthRequest = DependencyService.Get<Acr.XamForms.Mobile.IDeviceInfo>().ScreenWidth
			};

			var areaFotosCapturadasThumb = new StackLayout
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
				HeightRequest = 120,
				BackgroundColor = Color.FromHex("#EEEEEE")
			};

			foreach (var imagem in ImagensCapturadas ())
			{
				areaFotosCapturadasThumb.Children.Add(imagem);
			}

			var scrollAreaFotosCapturadas = new ScrollView { Content = areaFotosCapturadasThumb, Orientation = ScrollOrientation.Horizontal };

			var mainLayout = new StackLayout
			{ 
				Orientation = StackOrientation.Vertical, 
				VerticalOptions = LayoutOptions.FillAndExpand,
				Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 8)
			};

			mainLayout.Children.Add(menu);
			mainLayout.Children.Add(areaFoto);
			mainLayout.Children.Add(scrollAreaFotosCapturadas);

			this.Content = mainLayout;
		}

		private static ObservableCollection<Image> ImagensCapturadas()
		{
			var img1 = new Image { Source = ImageSource.FromResource("TechSocial.Content.Images.imagemteste.jpg") };
			var img2 = new Image { Source = ImageSource.FromResource("TechSocial.Content.Images.imagemteste.jpg") };
			var img3 = new Image { Source = ImageSource.FromResource("TechSocial.Content.Images.imagemteste.jpg") };
			var img4 = new Image { Source = ImageSource.FromResource("TechSocial.Content.Images.imagemteste.jpg") };
			var img5 = new Image { Source = ImageSource.FromResource("TechSocial.Content.Images.imagemteste.jpg") };
			var img6 = new Image { Source = ImageSource.FromResource("TechSocial.Content.Images.imagemteste.jpg") };
			var img7 = new Image { Source = ImageSource.FromResource("TechSocial.Content.Images.imagemteste.jpg") };

			var lista = new List<Image>();
			lista.Add(img1);
			lista.Add(img2);
			lista.Add(img3);
			lista.Add(img4);
			lista.Add(img5);
			lista.Add(img6);
			lista.Add(img7);

			return new ObservableCollection<Image>(lista);
		}
	}
}


