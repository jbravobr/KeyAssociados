using System;

namespace TechSocial
{
	[AttributeUsage(AttributeTargets.All)]
	public class DisplayName : Attribute
	{
		private string _name;

		public DisplayName(string name)
		{
			this._name = name;
		}
	}
}

