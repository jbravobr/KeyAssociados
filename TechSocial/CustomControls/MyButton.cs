using System;

using Xamarin.Forms;

namespace TechSocial
{
	public class MyButton : ContentView
	{
		public int Height { get; set; }

		public int Width { get; set; }

		public string Text { get; set; }

		public MyButton()
		{
			var entry = new Editor
			{
				HeightRequest = Height,
				WidthRequest = Width,
				Text = Text
			};

			var frame = new Frame
			{
				HasShadow = false,
				OutlineColor = Color.Gray,
				HeightRequest = Height,
				WidthRequest = Width,
				Content = entry
			};

			this.Content = frame;
		}
	}
}


