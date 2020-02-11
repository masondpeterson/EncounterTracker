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
            if (CheckNameNotNull())
            {
                await DisplayAlert("Alert", "Please Enter a Character Name", "OK");
            }
            else if (CheckClassNotNull())
            {
                await DisplayAlert("Alert", "Please Select a Character Class", "OK");
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

        private bool CheckNameNotNull()
        {
            if (nameEntry.Text == null || nameEntry.Text == "")
            {
                return true;
            }
            else
            {
                return false;
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