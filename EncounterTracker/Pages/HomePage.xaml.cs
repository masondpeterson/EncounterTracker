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



        #endregion

        private void searchButton_Clicked(object sender, EventArgs e)
        {

        }

        private void refreshButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}