using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using TechSocial.iOS;
using System.IO;
using Foundation;
using UIKit;

[assembly: Dependency(typeof(SaveAndLoadFile_IOS))]
namespace TechSocial.iOS
{
	public class SaveAndLoadFile_IOS : ISaveAndLoadFile
	{
		UIImage image;

		#region ISaveAndLoadFile implementation

		public async Task<bool> SaveImage(ImageSource img, string imageName)
		{
			try
			{
				var render = new StreamImagesourceHandler();
				NSData imgData = null;

				var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				var nomeImagem = Path.Combine(path, imageName);
				NSError erro = null;

				image = await render.LoadImageAsync(img);
				imgData = image.AsJPEG();
						
				return imgData.Save(nomeImagem, false, out erro);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public string GetImage(string imageName)
		{
			var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var nomeImagem = Path.Combine(path, imageName);

			return nomeImagem;
		}

		public byte[] GetImageArray(string imageName)
		{
			var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var nomeImagem = Path.Combine(path, imageName);

			return File.ReadAllBytes(nomeImagem);
		}


		public void RemoveImage(string imageName)
		{
			var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var nomeImagem = Path.Combine(path, imageName);

			File.Delete(nomeImagem);
		}

		#endregion
        
	}
}

