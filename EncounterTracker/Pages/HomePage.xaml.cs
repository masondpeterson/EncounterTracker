using EncounterTracker.DataBase;
using EncounterTracker.DBObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EncounterTracker.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        //private variables
        private int _userId;
        private SQLConn _conn = new SQLConn();
        private List<Character> Characters;
        private int _selIndex;
        private Character _selChar;
        private bool _first = false;

        //public variables
        public ObservableCollection<string> CharacterNames { get; } = new ObservableCollection<string>();
        public enum Stats { Kill, Assist, Dealt, Taken, Heal, Drop }

        public HomePage(int userId, int index = -1, Character selChar = null, bool first = true)
        {
            _userId = userId;
            _selIndex = index;
            _selChar = selChar;
            _first = first;
            Title = "Encounter Tracker";
            InitializeComponent();

            Content = new ScrollView
            {
                Content = CreateContent()
            };

            GenerateStatsLayouts();
            charPicker.SelectedIndex = _selIndex;
        }

        public StackLayout CreateContent()
        {
            SetCharLists();
            charPicker.ItemsSource = CharacterNames;
            var stack = new StackLayout
            {
                Spacing = 10,
                Children =
                {
                    mainContent
                }
            };
            return stack;
        }

        #region Tracking Section Methods

        async void CharCreateButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateCharacterPage(_userId));
        }

        async void newStatsButton_Clicked(object sender, EventArgs e)
        {
            if (_selIndex == -1)
            {
                await DisplayAlert("Alert", "Please select a character", "OK");
            }
            else
            {
                await Navigation.PushAsync(new EnterStatsPage(_userId, _selChar.CharacterId));
            }
        }

        async void charPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_first)
            {
                Picker picker = (Picker)sender;
                _selIndex = picker.SelectedIndex;
                _selChar = Characters[_selIndex];
                await Navigation.PushAsync(new HomePage(_userId, _selIndex, _selChar, false));
            }
            else
            {
                _first = true;
            }
        }

        //Support Methods
        private void SetCharLists()
        {
            Characters = _conn.GetUserCharacters(_userId);
            foreach (var character in Characters)
            {
                CharacterNames.Add(character.CharName);
            }
        }

        #endregion

        #region Search Section Methods

        async void searchButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchResultsPage(searchInput.Text, _userId));
        }

        #endregion

        #region Reports Layout Methods

        private void GenerateStatsLayouts()
        {
            if (_conn.GetEncountersByPlayer(_userId).Count == 0)
            {
                var label = new Label
                {
                    Text = "Enter Encounter to See Stats",
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
                dynamicSection.Children.Add(label);
                Grid.SetColumnSpan(label, 2);
            }
            else
            {
                killStats.Children.Add(CreateStatsLayout(Stats.Kill));
                assistStats.Children.Add(CreateStatsLayout(Stats.Assist));
                dealtStats.Children.Add(CreateStatsLayout(Stats.Dealt));
                takenStats.Children.Add(CreateStatsLayout(Stats.Taken));
                healStats.Children.Add(CreateStatsLayout(Stats.Heal));
                dropStats.Children.Add(CreateStatsLayout(Stats.Drop));
                var button = new Button
                {
                    Text = "Player Stats",
                    HorizontalOptions = LayoutOptions.StartAndExpand
                };
                button.Clicked += resetButton_Clicked;
                resetButton.Children.Add(button);
            }
        }

        private StackLayout CreateStatsLayout(Stats stat)
        {
            if (_selIndex != -1)
            {
                statsTitle.Text = _selChar.CharName + "'s Stats (" + _conn.GetEncountersByCharacter(_selChar.CharacterId).Count + " Encounters)";
            }
            else
            {
                statsTitle.Text = "Player Stats (" + _conn.GetEncountersByPlayer(_userId).Count + " Encounters)";
            }
            var sList = GetStatStrings(stat);
            var title = CreateTitleLabel(stat);
            var stack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Children =
                {
                    title,
                    new Label
                    {
                        Text = sList[0]
                    },
                    new Label
                    {
                        Text = sList[1]
                    },
                    new Label
                    {
                        Text = sList[2]
                    },
                    new Label
                    {
                        Text = sList[3]
                    }
                }
            };
            return stack;
        }

        private Label CreateTitleLabel(Stats stat)
        {
            var title = SetStatTitle(stat);
            var label = new Label
            {
                Text = title,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                TextDecorations = TextDecorations.Underline,
                FontAttributes = FontAttributes.Bold
            };
            return label;
        }

        private string SetStatTitle(Stats stat)
        {
            string title;
            switch (stat)
            {
                case Stats.Kill:
                    title = "Kills";
                    break;
                case Stats.Assist:
                    title = "Assists";
                    break;
                case Stats.Dealt:
                    title = "Dmg Dealt";
                    break;
                case Stats.Taken:
                    title = "Dmg Taken";
                    break;
                case Stats.Heal:
                    title = "Healing";
                    break;
                case Stats.Drop:
                    title = "Dropped";
                    break;
                default:
                    title = "Something went wrong";
                    break;
            }
            return title;
        }

        private List<string> GetStatStrings(Stats stat)
        {
            //Create containers
            var phrases = new List<string>();
            //Get the stats
            var statList = GetStatsList(stat);
            //Find the Max, Avg, and Min
            var maxPhrase = "Max: " + statList.Max();
            var avgPhrase = "Avg: " + Math.Round(statList.Average());
            var minPhrase = "Min: " + statList.Min();
            var totalPhrase = "Total: " + statList.Sum();
            //Add to the return list
            phrases.Add(maxPhrase);
            phrases.Add(avgPhrase);
            phrases.Add(minPhrase);
            phrases.Add(totalPhrase);

            return phrases;
        }

        private List<int> GetStatsList(Stats stat)
        {
            List<Encounter> elist;
            if (_selIndex != -1)
            {
                elist = _conn.GetEncountersByCharacter(_selChar.CharacterId);
            }
            else
            {
                elist = _conn.GetEncountersByPlayer(_userId);
            }
            var statList = new List<int>();
            foreach (var e in elist)
            {
                switch (stat)
                {
                    case Stats.Kill:
                        statList.Add(e.Kills);
                        break;
                    case Stats.Assist:
                        statList.Add(e.Assist);
                        break;
                    case Stats.Dealt:
                        statList.Add(e.DmgDealt);
                        break;
                    case Stats.Taken:
                        statList.Add(e.DmgTaken);
                        break;
                    case Stats.Heal:
                        statList.Add(e.Healing);
                        break;
                    case Stats.Drop:
                        statList.Add(e.Dropped);
                        break;
                    default:
                        statList.Add(0);
                        break;
                }
            }
            return statList;
        }

        async void resetButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HomePage(_userId));
        }

        #endregion
    }
}