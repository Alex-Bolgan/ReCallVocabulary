using ReCallVocabulary.Data_Access;
using System.ComponentModel;
namespace ReCallVocabulary.Pages;

public partial class RecallGamePage : ContentPage, INotifyPropertyChanged
{
    private readonly bool isOnlyRecent;
    private string term;
    private string definition;
    private int countWithThresholds = 1;
    private int totalCount = 0;
    private int firstPriorityId = Model.GetMinId();
    private int secondPriorityId = 0;
    private int endId = Model.GetMaxId();
    int randomNumber;
    Random random = new Random();

    private Func<int> generatingMethod;

    public event PropertyChangedEventHandler PropertyChanged;

    public string Term
    {
        get => term;
        set
        {
            term = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Term"));
        }
    }
    public string Definition
    {
        get => definition;
        set
        {
            definition = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Definition"));
        }
    }
    public RecallGamePage()
    {

        InitializeComponent();
        BindingContext = this;

        if (Model.IsEmpty())
        {
            Definition = "Oops. It seems you have an empty dictionary";
            definitionLabel.IsVisible = true;
            StopButton.IsVisible = false;
            ToMainMenuButton.IsVisible = true;
        }

        DateTime[] dates = DateFileHandler.GetDates();

        if (dates[0] == DateTime.MinValue)
        {
            firstPriorityId = Model.GetFirstIdWithDate(dates[1]);
            endId = Model.GetMaxIdWithDate(dates[2]);
        }
        else
        {
            firstPriorityId = Model.GetFirstIdWithDate(dates[1]);
            secondPriorityId = Model.GetFirstIdWithDate(dates[0]);
            endId = Model.GetMaxIdWithDate(dates[2]);
        }

        this.isOnlyRecent = MainPage.IsOnlyRecent;
        termLabel.IsVisible = definitionLabel.IsVisible = true;

        generatingMethod = GenerateWith1Threshold;
        if (secondPriorityId != 0)
        {
            generatingMethod = GenerateWith2Thresholds;
        }

        if (this.isOnlyRecent)
        {
            tapRecognizer.Tapped += OnTapGestureRecognizerTappedRecent;
            OnTapGestureRecognizerTappedRecent(this, new TappedEventArgs(""));
        }
        else
        {
            tapRecognizer.Tapped += OnTapGestureRecognizerTapped;
            OnTapGestureRecognizerTapped(this, new TappedEventArgs(""));
        }
    }

    private void StopButton_Clicked(object sender, EventArgs e)
    {
        Definition = $"Congrats! You recalled {totalCount}!";
        Term = "";
        StopButton.IsVisible = false;
        ToMainMenuButton.IsVisible = true;
    }
    void OnTapGestureRecognizerTappedRecent(object sender, TappedEventArgs args)
    {
        if (termLabel.IsVisible)
        {
            do
            {
                randomNumber = random.Next(firstPriorityId, endId + 1);
            } while (!Model.PhraseExists(randomNumber));

            randomNumber = random.Next(firstPriorityId, endId + 1);
            Definition = Model.GetPhraseById(randomNumber).Definition;
            Term = Model.GetPhraseById(randomNumber).Term;
            termLabel.IsVisible = false;
        }
        else
        {
            totalCount++;
            termLabel.IsVisible = true;
        }
    }
    private void OnTapGestureRecognizerTapped(object? sender, TappedEventArgs e)
    {
        if (termLabel.IsVisible)
        {
            do
            {
                countWithThresholds = 1;
                randomNumber = generatingMethod();
            } while (!Model.PhraseExists(randomNumber));

            Definition = Model.GetPhraseById(randomNumber).Definition;
            Term = Model.GetPhraseById(randomNumber).Term;
            termLabel.IsVisible = false;
        }
        else
        {
            totalCount++;
            termLabel.IsVisible = true;
        }
    }
    private int GenerateWith1Threshold()
    {
        if (countWithThresholds == 3)
        {
            randomNumber = random.Next(firstPriorityId, endId);
            countWithThresholds = 1;
        }
        else
        {
            randomNumber = random.Next(1, endId + 1);
            countWithThresholds++;
        }
        return randomNumber;
    }
    private int GenerateWith2Thresholds()
    {
        if (countWithThresholds == 3)
        {
            randomNumber = random.Next(firstPriorityId, endId);
            countWithThresholds = 1;
        }
        else if (countWithThresholds == 2)
        {
            randomNumber = random.Next(secondPriorityId, firstPriorityId);
            countWithThresholds++;
        }
        else
        {
            randomNumber = random.Next(1, endId);
            countWithThresholds++;
        }
        return randomNumber;
    }

    private async void ToMainMenuButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}