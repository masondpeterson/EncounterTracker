using EncounterTracker.DataBase;
using EncounterTracker.DBObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EncounterTracker.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnterStatsPage : ContentPage
    {
        private int _userId;
        private int _charId;
        private bool _checkEntry = false;
        private bool _dropped;
        private SQLConn _conn = new SQLConn();

        public EnterStatsPage(int userId, int charId)
        {
            _userId = userId;
            _charId = charId;

            Title = "Let's Track Some Stats!";
            InitializeComponent();
        }

        #region Action Methods

        async void submitButton_Clicked(object sender, EventArgs e)
        {
            if (ValidateNameField(encounterName.Text))
            {
                await DisplayAlert("Alert", "Name Field is required", "OK");
            }
            else if (_checkEntry)
            {
                await DisplayAlert("Alert", "Please use only Valid Whole numbers as responses", "OK");
            }
            else
            {
                SubmitEncounter(CreateEncounter());
                await Navigation.PushAsync(new HomePage(_userId));
            }
        }

        async void cancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HomePage(_userId));
        }

        private void killCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateNumberField(killCount.Text, killValidate);
        }

        private void assistCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateNumberField(assistCount.Text, assistValidate);
        }

        private void dmgDealt_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateNumberField(dmgDealt.Text, dealtValidate);
        }

        private void dmgTaken_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateNumberField(dmgTaken.Text, takenValidate);
        }

        private void healEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateNumberField(healEntry.Text, healValidate);
        }

        private void dropped_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            _dropped = checkbox.IsChecked;
        }

        #endregion

        #region Support Methods

        private void ValidateNumberField(string value, Label label)
        {
            var check = true;
            foreach (char c in value)
            {
                if (c < '0' || c > '9')
                {
                    check = false;
                }
            }
            if (check)
            {
                label.IsVisible = false;
                _checkEntry = false;
            }
            else
            {
                label.IsVisible = true;
                label.TextColor = Color.Red;
                label.Text = "Enter a Valid Whole Number";
                _checkEntry = true;
            }
        }

        private bool ValidateNameField(string name)
        {
            var check = false;
            if(name == null || name == "")
            {
                check = true;
            }
            return check;
        }

        private Encounter CreateEncounter()
        {
            var e = new Encounter();
            e.UserId = _userId;
            e.CharId = _charId;
            e.EncounterName = encounterName.Text;
            e.Kills = Convert.ToInt32(killCount.Text);
            e.Assist = Convert.ToInt32(assistCount.Text);
            e.DmgDealt = Convert.ToInt32(dmgDealt.Text);
            e.DmgTaken = Convert.ToInt32(dmgTaken.Text);
            e.Healing = Convert.ToInt32(healEntry.Text);
            if (_dropped)
            {
                e.Dropped = 1;
            }
            else
            {
                e.Dropped = 0;
            }
            e.Session = DateTime.Now;

            return e;
        }

        private void SubmitEncounter(Encounter encounter)
        {
            _conn.InsertEncounter(encounter);
        }

        #endregion
    }
}