﻿using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;

namespace TechSocial
{
	public class QuestoesComProblemaCarrosselPage : ContentPage
	{
		public QuestoesComProblemaCarrosselPage(List<QuestaoProblemaView> view)
		{
			var slider = new SliderView(view.First(), App.ScreenHeight * 0.5, App.ScreenWidth)
			{ 
				BackgroundColor = Color.Transparent,
				TransitionLength = 200,
				StyleId = "SliderView",
				MinimumSwipeDistance = 50
			};

			for (int i = 1; i < view.Count; i++)
			{
				slider.Children.Add(view.ElementAt(i));
			}

			this.Content = new StackLayout
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = { slider }
			};
		}
	}
}


