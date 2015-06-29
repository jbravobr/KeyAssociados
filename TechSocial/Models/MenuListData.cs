using System;

using Xamarin.Forms;
using System.Collections.Generic;

namespace TechSocial
{
	public class MenuListData : List<MenuMasterItem>
	{
		public MenuListData()
		{
			this.Add(new MenuMasterItem()
				{ 
					Title = "Configura Acesso de Testes", 
					IconSource = "teste.png", 
					TargetType = typeof(TestePage)
				});

			this.Add(new MenuMasterItem()
				{ 
					Title = "Configura Acesso de Produção", 
					IconSource = "producao.png", 
					TargetType = typeof(ProducaoPage)
				});

//			this.Add(new MenuMasterItem()
//				{
//					Title = "Atualizar dados", 
//					IconSource = "refresh.png", 
//					TargetType = typeof(RefreshPage)
//				});
		}
	}
}


