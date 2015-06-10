using System;
using System.IO;

namespace TechSocial
{
	public interface IFileAccess
	{
		bool Exists(string filename);

		string FullPath(string filename);

		bool WriteStream(string filename, Stream streamIn);
	}
}

