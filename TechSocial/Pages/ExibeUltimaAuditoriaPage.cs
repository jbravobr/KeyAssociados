using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using Autofac;

namespace TechSocial
{
    public class ExibeUltimaAuditoriaPage : ContentPage
    {
        
        string _q = string.Empty;

        MyButton entObservacoes;
        MyButton entAcoesRequeridas;
        MyButton entryComoEvidenciar;
        Entry entryCriterio;
        MyButton entryDescricaoBaseLegal;
        DatePicker dataPicker;
        QuestaUltimaAuditoria model;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var db = new TechSocialDatabase(false);
            var _r = db.GetRespostaUltimaPorId(_q);
            model = new QuestaUltimaAuditoria(_r);

            this.BindingContext = model.Resposta;

            this.entryCriterio.SetBinding(Entry.TextProperty, "atende");
            this.entObservacoes.entry.SetBinding(Entry.TextProperty, "observacao");
            this.entAcoesRequeridas.entry.SetBinding(Entry.TextProperty, "acaorequerida");
        }

        public ExibeUltimaAuditoriaPage(string questaoId)
        {
            this._q = questaoId;

            #region Critério
            entryCriterio = new Entry();
            entryCriterio.IsEnabled = false;

            var btnCriterio = new Button{ Text = "Critério", IsEnabled = false };

            #endregion

            #region Bases Legais
            entryDescricaoBaseLegal = new MyButton();
            entryDescricaoBaseLegal.entry.HeightRequest = 90;
            entryDescricaoBaseLegal.IsEnabled = false;
            entryDescricaoBaseLegal.entry.IsEnabled = false;

            var btnBaseLegal = new Button{ Text = "Base Legal", IsEnabled = false };
            #endregion

            #region Data
            dataPicker = new DatePicker
            {
                Format = "dd/MM/yyyy",
                IsVisible = true,
                IsEnabled = false
            };
            var btnData = new Button
            {
                Text = "Data Prazo",
                IsEnabled = false
            };
            #endregion

            #region Observações
            entObservacoes = new MyButton();
            entObservacoes.entry.HeightRequest = 90;
            entObservacoes.IsEnabled = false;
            entObservacoes.entry.IsEnabled = false;
            #endregion

            #region Ações Requeridas
            entAcoesRequeridas = new MyButton();
            entAcoesRequeridas.entry.HeightRequest = 90;
            entAcoesRequeridas.IsEnabled = false;
            entAcoesRequeridas.entry.IsEnabled = false;
            #endregion

            #region Como Evidenciar
            entryComoEvidenciar = new MyButton();
            entryComoEvidenciar.entry.HeightRequest = 130;
            entryComoEvidenciar.entry.IsEnabled = false;
            #endregion

            #region Grid Como Evidenciar
            var gridComoEvidenciar = new Grid
            {
                VerticalOptions = LayoutOptions.Start,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(300, GridUnitType.Star) }
                }
            };
            gridComoEvidenciar.Children.Add(new Label { Text = "Como Evidenciar" }, 0, 1);
            gridComoEvidenciar.Children.Add(entryComoEvidenciar, 1, 1);
            #endregion

            #region Grid para Critério
            var gridCriterio = new Grid
            {
                VerticalOptions = LayoutOptions.Start,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(300, GridUnitType.Star) }
                }
            };
            gridCriterio.Children.Add(btnCriterio, 0, 0);
            gridCriterio.Children.Add(entryCriterio, 1, 0);
            #endregion

            #region Grid Bases Legais
            var gridBaseLegal = new Grid
            {
                VerticalOptions = LayoutOptions.Start,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(300, GridUnitType.Star) }
                }
            };
            gridBaseLegal.Children.Add(btnBaseLegal, 0, 1);
            gridBaseLegal.Children.Add(entryDescricaoBaseLegal, 1, 1);
            #endregion

            #region Grid para Data
            var gridData = new Grid
            {
                VerticalOptions = LayoutOptions.Start,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(300, GridUnitType.Star) }
                }
            };
            gridData.Children.Add(btnData, 0, 1);
            gridData.Children.Add(dataPicker, 1, 1);
            #endregion

            #region Grid Observações
            var gridObs = new Grid
            {
                VerticalOptions = LayoutOptions.Start,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(300, GridUnitType.Star) }
                }
            };
            gridObs.Children.Add(new Button{ Text = "Observações" }, 0, 1);
            gridObs.Children.Add(entObservacoes, 1, 1);
            #endregion

            #region Grid Ações Requeridas
            var gridAcoesRequeridas = new Grid
            {
                VerticalOptions = LayoutOptions.Start,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(300, GridUnitType.Star) }
                }
            };
            gridAcoesRequeridas.Children.Add(new Button{ Text = "Ações Requeridas", FontSize = 12 }, 0, 1);
            gridAcoesRequeridas.Children.Add(entAcoesRequeridas, 1, 1);
            #endregion

            var btnClose = new Button();
            btnClose.Text = "Fechar";
            btnClose.Clicked += async (sender, e) => await this.Navigation.PopAsync();
            btnClose.Style = Estilos.buttonDefaultStyle;

            var stack = new StackLayout
            {
                Padding = new Thickness(15, 10, 5, 5),
                Spacing = 10,
                Children =
                { 
                    gridCriterio,
                    gridObs,
                    gridAcoesRequeridas, 
                    btnClose
                },
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            this.Content = new ScrollView{ Content = stack, Orientation = ScrollOrientation.Vertical };
        }
    }
}


