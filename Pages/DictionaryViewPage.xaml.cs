using ReCallVocabulary.Data_Access;

namespace ReCallVocabulary.Pages;

public partial class DictionaryViewPage : ContentPage
{
    List<Phrase> PhraseList { get; set; } = App.ActiveContext.Phrases.ToList();

    public DictionaryViewPage()
    {
        InitializeComponent();
        int wordNumber = Model.GetTotalNumber();
        this.Title = $"{Path.GetFileNameWithoutExtension(File.ReadAllText(App.fileWithCurrentDBName))} ({wordNumber} words)";
        dictView.ItemsSource = PhraseList;
    }

    private async void dictView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Phrase item = (Phrase)e.CurrentSelection;
        await Navigation.PushAsync(new PhraseViewPage(item.Id));
    }
}