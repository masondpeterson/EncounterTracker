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

namespace EncounterTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        //private variables
        private int _userId;
        private SQLConn _conn = new SQLConn();
        private List<Character> Characters;
        private List<Encounter> _eList;
        private int _selIndex = -1;
        private Character _selChar;

        //public variables
        public ObservableCollection<string> CharacterNames { get; } = new ObservableCollection<string>();

        public HomePage(int id)
        {
            _userId = id;

            Title = "Encounter Tracker";
            InitializeComponent();

            Content = new ScrollView
            {
                Content = CreateContent()
            };
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
            if(_selIndex == -1)
            {
                await DisplayAlert("Alert", "Please select a character", "OK");
            }
            else
            {
                await Navigation.PushAsync(new EnterStatsPage(_userId, _selChar.CharacterId));
            } 
        }

        private void charPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;
            _selIndex = picker.SelectedIndex;
            _selChar = Characters[_selIndex];
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

        #region Reporting Section Methods

        async void searchButton_Clicked(object sender, EventArgs args)
        {
            //pupulate the eList with ALL the players known encounters
            _eList = _conn.GetEncountersByPlayer(_userId);
            //Search the list for names that contain the search term
            var elist = SearchEncounters(searchInput.Text);
            if (elist.Count == 0)
            {
                await DisplayAlert("Alert", "No Matching Encounters were found \r\nso an unfiltered list was returned", "OK");
                BuildResultFrames(_eList);
            }
            else
            {
                BuildResultFrames(elist);
            }
        }

        private void refreshButton_Clicked(object sender, EventArgs e)
        {
            BuildResultFrames(_eList);
        }

        private List<Encounter> SearchEncounters(string searchTerm)
        {
            var elist = new List<Encounter>();
            foreach (var e in _eList)
            {
                //check if whole search term is contained in the encounter name or if they start with the same letter. 
                if (e.EncounterName.Contains(searchTerm))
                {
                    elist.Add(e);
                }
            }

            return elist;
        }

        private void BuildResultFrames(List<Encounter> encounters)
        {
            foreach(var e in encounters)
            {
                var frame = new Frame
                {
                    BorderColor = Color.Gray,
                    HasShadow = true,
                    Content = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            new Label
                            {
                                Text = e.EncounterName,
                                HorizontalOptions = LayoutOptions.CenterAndExpand
                                
                            },
                            new Label
                            {
                                Text = _conn.GetCharacterById(e.CharId).CharName,
                                HorizontalOptions = LayoutOptions.CenterAndExpand
                            },
                            new Button
                            {
                                Text = "Stats",
                                TabIndex = e.EncounterId,
                                HorizontalOptions = LayoutOptions.CenterAndExpand
                            }
                        }
                    }
                };
                resultsSection.Children.Add(frame);
            }
        }
        #endregion


    }
}