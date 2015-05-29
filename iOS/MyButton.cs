using System;
using Xamarin.Forms.Platform.iOS;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;

namespace TechSocial.iOS
{
	public class MyButton : EditorRenderer
	{
		
		protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				{
					Control.Layer.BorderWidth = 5.0f;
					Control.Layer.BorderColor = UIColor.Black.CGColor;
				}
			}
		}
	}
}

