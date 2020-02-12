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
    public partial class CreateCharacterPage : ContentPage
    {
        //private variables
        private int _userId;
        private SQLConn _conn = new SQLConn();
        private List<CharClass> _charClasses;
        private int _selInd = -1;
        private CharClass _selClass;
        private bool _nameCheck;


        //public variables
        public ObservableCollection<string> ClassNames { get; } = new ObservableCollection<string>();

        public CreateCharacterPage(int id)
        {
            _userId = id;
            Title = "Create Character";
            InitializeComponent();

            Content = new ScrollView
            {
                Content = CreateContent()
            };
        }

        #region Layout Methods

        public StackLayout CreateContent()
        {
            SetCharClasses();
            classPicker.ItemsSource = ClassNames;
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

        #endregion

        #region Action Methods

        async void cancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HomePage(_userId));
        }

        async void createButton_Clicked(object sender, EventArgs e)
        {
            if (CheckClassNotNull())
            {
                await DisplayAlert("Alert", "Please Select a Character Class", "OK");
            }
            else if (_nameCheck == false)
            {
                await DisplayAlert("Alert", "Must enter a unique Character Name", "OK");
            }
            else
            {
                var character = new Character();
                character.CharName = nameEntry.Text;
                character.UserId = _userId;
                _conn.InsertCharacter(character);
                await DisplayAlert("Notification", "Character Created Successfullly", "OK");
                await Navigation.PushAsync(new HomePage(_userId), true);
            }
        }

        public void classPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;
            _selInd = picker.SelectedIndex;
            _selClass = _charClasses[_selInd];
        }

        #endregion

        #region Support Methods

        private void SetCharClasses()
        {
            _conn.PopulateCharClassTable();
            _charClasses = _conn.GetCharClasses();
            foreach (var cc in _charClasses)
            {
                ClassNames.Add(cc.ClassName);
            }
        }

        private void nameEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var check = false;
            var characters = _conn.GetUserCharacters(_userId);
            foreach (var c in characters)
            {
                if (c.CharName == nameEntry.Text)
                {
                    check = true;
                }
            }
            if (check)
            {
                nameValidate.TextColor = Color.Red;
                nameValidate.Text = "Character Name Unavailable";
                nameValidate.IsVisible = true;
                _nameCheck = false;
            }
            else
            {
                nameValidate.TextColor = Color.Green;
                nameValidate.Text = "Character Name Available";
                nameValidate.IsVisible = true;
                _nameCheck = true;
            }
        }

        private bool CheckClassNotNull()
        {
            if (_selInd == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}