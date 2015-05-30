using System;

using Xamarin.Forms;
using PropertyChanged;

namespace TechSocial
{
	[ImplementPropertyChanged]
	public class MyButton : ContentView
	{
		public Editor entry { get; set; }

		public MyButton()
		{
			entry = new Editor();

			var frame = new Frame
			{
				HasShadow = false,
				OutlineColor = Color.FromHex("#e8e8e8"),
				HeightRequest = 200,
				Content = entry,
				BackgroundColor = Color.Transparent
			};

			this.Content = frame;
		}
	}
}


