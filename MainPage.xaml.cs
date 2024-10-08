﻿using ReCallVocabulary.Data_Access;
using ReCallVocabulary.Pages;
using CommunityToolkit.Maui.Views;
namespace ReCallVocabulary
{
    public partial class MainPage : ContentPage
    {
        private DictionaryContext activeContext = App.ActiveContext ??
        throw new ArgumentNullException(nameof(activeContext));
        
        private StatsContext statsContext = App.statsContext ??
                                            throw new ArgumentNullException(nameof(statsContext));
        public MainPage()
        {
            InitializeComponent();
            if (!Directory.Exists(Path.GetDirectoryName(activeContext.MyPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(activeContext.MyPath)!);
            }

            if (!File.Exists(activeContext.MyPath))
            {
                var myFile = File.Create(activeContext.MyPath);
                myFile.Close();
            }

            if (!Directory.Exists(Path.GetDirectoryName(statsContext.MyPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(statsContext.MyPath)!);
            }

            if (!File.Exists(statsContext.MyPath))
            {
                var myFile = File.Create(statsContext.MyPath);
                myFile.Close();
            }

            activeContext.Database.EnsureCreated();
            statsContext.Database.EnsureCreated();
        }
        private async void Recall_Clicked(object sender, EventArgs e)
        {
            Popup popup = new ChoosePriorityPopup(false);
            await Application.Current.MainPage.ShowPopupAsync(popup);

            popup.Close();
        }
        private async void RecallRecent_Clicked(object sender, EventArgs e)
        {
            Popup popup = new ChoosePriorityPopup(true);
            await Application.Current.MainPage.ShowPopupAsync(popup);

            popup.Close();
        }
        private async void Addwords_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Pages.AddWordsPage());
        }
        private async void SeeDictionary_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(DictionaryViewPage));
        }
    }
}
