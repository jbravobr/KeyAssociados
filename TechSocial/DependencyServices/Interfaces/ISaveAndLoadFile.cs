using System;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace TechSocial
{
	public interface ISaveAndLoadFile
	{
		Task<bool> SaveImage(ImageSource img, string imageName);

		string GetImage(string imageName);

		byte[] GetImageArray(string imageName);
	}
}

