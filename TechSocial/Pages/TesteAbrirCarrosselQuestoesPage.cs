using System;

using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;

namespace TechSocial
{
	public class TesteAbrirCarrosselQuestoesPage : ContentPage
	{
		public TesteAbrirCarrosselQuestoesPage(string audi = "4222")
		{
			var data = new TechSocialDatabase(false);
			var questoes = new List<Questoes>();
			var viewsQuestaoProblema = new List<QuestaoProblemaView>();

			var respostas = data.GetRespostaPorAuditoria(Convert.ToInt32(audi))
				.Where(r => (r.atende != "2" && r.atende != "Sim" && r.atende != "NA")
				                && !String.IsNullOrEmpty(r.atende));

			foreach (var resposta in respostas)
			{
				questoes.AddRange(data.GetQuestoes()
					.Where(q => q.questao.ToString() == resposta.questao));
			}

			foreach (var questao in questoes)
			{
				viewsQuestaoProblema.Add(new QuestaoProblemaView(questao, audi, questao.modulo.ToString()));
			}

			var btnCarregaCarrossel = new Button
			{
				Text = "Carregar carrossel"
			};
			btnCarregaCarrossel.Clicked += async (sender, e) =>
			{
				await this.Navigation.PushModalAsync(new QuestoesComProblemaCarrosselPage(viewsQuestaoProblema));
			};

			this.Content = new StackLayout{ HorizontalOptions = LayoutOptions.CenterAndExpand, Children = { btnCarregaCarrossel } };
		}
	}
}


