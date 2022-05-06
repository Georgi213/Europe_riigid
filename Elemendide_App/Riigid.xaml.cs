using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Elemendide_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Riigid : ContentPage
    {
        public ObservableCollection<Europe> europe { get; set; }
        Label lbl_list;
        ListView listitem;
        Button lisa;
        Button kustuta;
        MediaFile file;
        string imageName;
        string filePath;
        public Riigid()
        {
            europe = new ObservableCollection<Europe>
            {
                new Europe {Nimetus="Eesti", Pealinn="Tallinn",Elanikkond=1367716, Pilt="eesti.jpg"},
                new Europe {Nimetus="Luksemburg", Pealinn="Luksemburg",Elanikkond=632275, Pilt="luksem.jpg"},
                new Europe {Nimetus="Rootsi", Pealinn="Stockholm",Elanikkond=10409248 , Pilt="rootsi.jpg"},
                new Europe {Nimetus="Saksamaa", Pealinn="Berliin",Elanikkond=83149300, Pilt="saksamaa.jpg"}
            };
            lbl_list = new Label
            {
                Text = "Euroopa riigid",
                HorizontalOptions = LayoutOptions.Center,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            };
            listitem = new ListView
            {


                HasUnevenRows = true,
                ItemsSource = europe,
                ItemTemplate = new DataTemplate(() => {

                    ImageCell imageCell = new ImageCell { TextColor = Color.Moccasin, DetailColor = Color.White };
                    imageCell.SetBinding(ImageCell.TextProperty, "Nimetus");
                    Binding companyBinding = new Binding { Path = "Pealinn", StringFormat = "Pealinn on: {0}" };
                    imageCell.SetBinding(ImageCell.DetailProperty, companyBinding);
                    Binding a = new Binding { Path = "Elanikkonnd", StringFormat = "Elanikkonnd: {0}" };
                    imageCell.SetBinding(ImageCell.DetailProperty, a);
                    imageCell.SetBinding(ImageCell.ImageSourceProperty, "Pilt");
                    return imageCell;
                })
            };
            lisa = new Button
            {
                Text = "Lisa riik",
                VerticalOptions = LayoutOptions.Center,
            };
            lisa.Clicked += Lisa_Clicked;
            kustuta = new Button
            {
                Text = "Kustuta riik",
                VerticalOptions = LayoutOptions.Center,
            };
            kustuta.Clicked += Kustuta_Clicked;
            listitem.ItemTapped += Listitem_ItemTapped;
            this.Content = new StackLayout { Children = { lbl_list, listitem, lisa, kustuta } };
            this.BackgroundColor = Color.Teal;
        }
        private ObservableCollection<MediaFile> _images;
        public ObservableCollection<MediaFile> Images
        {
            get { return _images ?? (_images = new ObservableCollection<MediaFile>()); }
            set
            {
                if (_images != value)
                {
                    _images = value;
                    OnPropertyChanged();
                }
            }
        }
        private void Kustuta_Clicked(object sender, EventArgs e)
        {
            Europe euriik = listitem.SelectedItem as Europe;
            if (euriik != null)
            {
                europe.Remove(euriik);

            }
        }
        
        private async void Lisa_Clicked(object sender, EventArgs e)
        {
            string nimetuseu = await DisplayPromptAsync("Lisage riik", "Nimetus riik: ", initialValue: "", keyboard: Keyboard.Chat);
            string pealinneu = await DisplayPromptAsync("Lisage pealinn", $"{nimetuseu}" + " pealinna nimi: ", initialValue: "", keyboard: Keyboard.Chat);
            string elanikoodeu = await DisplayPromptAsync("Lisage elanikkond", "Kui palju inimesi elab " + $"{nimetuseu}" + ": ", initialValue: "", keyboard: Keyboard.Numeric);
            string pilteue= await DisplayPromptAsync("Lisa foto flagi", "Palun sisesta", keyboard: Keyboard.Text);
            int elanikuperedelen = Convert.ToInt32(elanikoodeu);
            if (nimetuseu == "" || pealinneu == "" || elanikoodeu == "" || pilteue == "")
                return;
            Europe uue = new Europe { Nimetus = nimetuseu, Pealinn = pealinneu, Elanikkond = elanikuperedelen, Pilt = pilteue };
            foreach (Europe asju in europe)
            {
                if (asju.Nimetus == uue.Nimetus)
                    return;
            }
           europe.Add(item: uue);
        }

        private async void Listitem_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Europe selectedeu = e.Item as Europe;
            if (selectedeu != null)
            {
                await DisplayAlert("Euroopa riigid: ", $"{selectedeu.Nimetus}: {selectedeu.Pealinn} " + "\nElanikkond: " + $"{selectedeu.Elanikkond}", "OK");
            }
        }
    }
}