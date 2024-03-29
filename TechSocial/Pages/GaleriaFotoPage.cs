﻿using System;
using System.Linq;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Acr.XamForms.Mobile.Media;
using System.IO;

namespace TechSocial
{
    public class GaleriaFotoPage : ContentPage
    {
        string Imagem;
        Image imgCapturada;
        Image imgSelecionada;
        StackLayout areaFotosCapturadasThumb;
        string auditoria;
        string modulo;
        string questao;
        Image icoAdicionar;
        Image icoExcluir;
        bool carregou;
        Respostas resp;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var GetPhoto = new TapGestureRecognizer();
            GetPhoto.Tapped += async (sender, e) =>
            {
                var mediaPickerService = DependencyService.Get<IMediaPicker>();

                if (mediaPickerService.IsCameraAvailable || mediaPickerService.IsPhotoGalleryAvailable)
                {
                    var options = new CameraOptions
							{ Camera = CameraDevice.Rear };

                    var photo = await mediaPickerService.TakePhoto(options);
                    ImageSource imgSource;

                    if (photo != null)
                    {
                        imgSource = ImageSource.FromStream(() =>
                            {
                                var stream = photo.GetStream();
                                return stream;
                            });

                        var imgNome = String.Concat(Path.GetRandomFileName(), ".jpeg");

                        var salvarImagem = await DependencyService.Get<ISaveAndLoadFile>().SaveImage(imgSource, imgNome);

                        if (salvarImagem)
                        {
                            var db = new TechSocialDatabase(false);
                            db.InserirFotoAuditoria(this.auditoria, this.modulo, questao, imgNome);

                            this.Imagem = DependencyService.Get<ISaveAndLoadFile>().GetImage(imgNome);
                            areaFotosCapturadasThumb.Children.Add(new Image{ Source = ImageSource.FromFile(this.Imagem), ClassId = imgNome });
                            imgCapturada.Source = ImageSource.FromFile(this.Imagem);
                            this.imgSelecionada = imgCapturada;

                            if (areaFotosCapturadasThumb.Children.Any())
                            {
                                foreach (var item in areaFotosCapturadasThumb.Children)
                                {
                                    var img = (Image)item;
                                    var img_Click = new TapGestureRecognizer();
                                    img_Click.Tapped += (s, elem) =>
                                    {
                                        DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>().ShowLoading("Carregando");

                                        imgCapturada.Source = img.Source;
                                        this.imgSelecionada = img;

                                        DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>().HideLoading();

                                    };
                                    img.GestureRecognizers.Add(img_Click);
                                }
                            }
                            this.Imagem = string.Empty;
                        }
                    }
                    else
                        return;
                }
            };
            icoAdicionar.GestureRecognizers.Add(GetPhoto);

            var excluir_click = new TapGestureRecognizer();
            excluir_click.Tapped += (sender, e) =>
            {
                if (this.imgSelecionada == null)
                    return;

                DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>().ShowLoading("Excluindo");
                var index = areaFotosCapturadasThumb.Children.IndexOf(this.imgSelecionada);
                areaFotosCapturadasThumb.Children.RemoveAt(index);
                DependencyService.Get<ISaveAndLoadFile>().RemoveImage(this.imgSelecionada.ClassId);
                this.imgCapturada.Source = ((Image)areaFotosCapturadasThumb.Children[index - 1 > 0 ? index - 1 : 0]).Source;
                this.imgSelecionada = this.imgCapturada;
                DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>().HideLoading();
            };
            icoExcluir.GestureRecognizers.Add(excluir_click);


            var imagens = this.CarregaImagensSeHouver();

            if (imagens != null && imagens.Any())
            {
                carregou = true;
                foreach (var imagem in imagens)
                {
                    var imgNome = DependencyService.Get<ISaveAndLoadFile>().GetImage(imagem);
                    var imgSrc = new Image { Source = ImageSource.FromFile(imgNome), ClassId = imagem };

                    if (areaFotosCapturadasThumb.Children.All(c => c.ClassId != imagem))
                        areaFotosCapturadasThumb.Children.Add(imgSrc);
                }

                if (areaFotosCapturadasThumb.Children.Any())
                {
                    foreach (var item in areaFotosCapturadasThumb.Children)
                    {
                        var img = (Image)item;
                        var img_Click = new TapGestureRecognizer();
                        img_Click.Tapped += (s, elem) =>
                        {
                            DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>().ShowLoading("Carregando");
                            imgCapturada.Source = img.Source;
                            this.imgSelecionada = img;
                            DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>().HideLoading();

                        };
                        img.GestureRecognizers.Add(img_Click);
                    }
                }

                var imgNomePrincipal = imagens.First();
                var imgPrincipal = DependencyService.Get<ISaveAndLoadFile>().GetImage(imgNomePrincipal);
                imgCapturada.Source = ImageSource.FromFile(imgPrincipal);
            }
        }

        public GaleriaFotoPage(string audi, string modulo, string questao, Respostas resposta = null)
        {
            this.auditoria = audi;
            this.modulo = modulo;
            this.resp = resposta;
            this.questao = questao;
							
            var menu = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(5, Device.OnPlatform(20, 0, 0), 5, 0),
                BackgroundColor = Color.FromHex("#EEEEEE")
            };

            icoExcluir = new Image
            {
                Source = ImageSource.FromResource("TechSocial.Content.Images.excluirFoto.png"),
                HorizontalOptions = LayoutOptions.Start,
                AnchorX = 0.15
            };
            /*var excluir_click = new TapGestureRecognizer();
            excluir_click.Tapped += (sender, e) =>
            {
                if (this.imgSelecionada == null)
                    return;

                DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>().ShowLoading("Excluindo");
                var index = areaFotosCapturadasThumb.Children.IndexOf(this.imgSelecionada);
                areaFotosCapturadasThumb.Children.RemoveAt(index);
                DependencyService.Get<ISaveAndLoadFile>().RemoveImage(this.imgSelecionada.ClassId);
                this.imgCapturada.Source = ((Image)areaFotosCapturadasThumb.Children[index - 1 > 0 ? index - 1 : 0]).Source;
                this.imgSelecionada = this.imgCapturada;
                DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>().HideLoading();
//                if (this.imgSelecionada == null)
//                    return;
//
//                var index = areaFotosCapturadasThumb.Children.IndexOf(this.imgSelecionada);
//                areaFotosCapturadasThumb.Children.RemoveAt(index);
//                DependencyService.Get<ISaveAndLoadFile>().RemoveImage(this.imgSelecionada.ClassId);
//
//                if (areaFotosCapturadasThumb.Children.Count >= 0)
//                {
//                    this.imgCapturada.Source = ((Image)areaFotosCapturadasThumb.Children[index - 1 > 0 ? index - 1 : 0]).Source;
//                    this.imgSelecionada = this.imgCapturada;
//                }
            };
            icoExcluir.GestureRecognizers.Add(excluir_click);*/
			
            icoAdicionar = new Image
            {
                Source = ImageSource.FromResource("TechSocial.Content.Images.adicionarFoto.png"),
                AnchorX = 1.75
            };
			
            var lblConcluir = new Label
            {
                Text = "Concluir",
                FontSize = 22,
                TextColor = Color.Blue,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };
            var encerrar_Click = new TapGestureRecognizer();
            encerrar_Click.Tapped += (sender, e) => this.Navigation.PopModalAsync();
            lblConcluir.GestureRecognizers.Add(encerrar_Click);

            imgCapturada = new Image();
            imgCapturada.Aspect = Aspect.AspectFit;

            menu.Children.Add(icoExcluir);
            menu.Children.Add(icoAdicionar);
            menu.Children.Add(lblConcluir);

            var areaFoto = new ContentView
            {
                Content = 
						new StackLayout
                {
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Orientation = StackOrientation.Horizontal,
                    Children =
                    {
                        imgCapturada
                    },
                    BackgroundColor = Color.Transparent
                },
                HeightRequest = DependencyService.Get<Acr.XamForms.Mobile.IDeviceInfo>().ScreenHeight - 30,
                WidthRequest = DependencyService.Get<Acr.XamForms.Mobile.IDeviceInfo>().ScreenWidth
            };

            areaFotosCapturadasThumb = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal,
                HeightRequest = 120,
                BackgroundColor = Color.FromHex("#EEEEEE")
            };

            var scrollAreaFotosCapturadas = new ScrollView { Content = areaFotosCapturadasThumb, Orientation = ScrollOrientation.Horizontal };

            var mainLayout = new StackLayout
            { 
                Orientation = StackOrientation.Vertical, 
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 8)
            };

            mainLayout.Children.Add(menu);
            mainLayout.Children.Add(areaFoto);
            mainLayout.Children.Add(scrollAreaFotosCapturadas);

            this.Content = mainLayout;
        }

        public List<string> CarregaImagensSeHouver()
        {
            var db = new TechSocialDatabase(false);

            return db.GetImagensAuditoriaModulos(this.auditoria, this.modulo, this.questao);
        }
    }
}


