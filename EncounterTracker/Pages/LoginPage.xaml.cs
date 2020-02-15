using EncounterTracker.DataBase;
using EncounterTracker.DBObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EncounterTracker.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private SQLConn _conn = new SQLConn();
        private Simple3Des _secure = new Simple3Des();
        private List<string> _usernames;
        private bool _nameCheck;
        private bool _passCheck;
        private int _userId;

        public LoginPage()
        {
            Title = "Encounter Tracker";
            InitializeComponent();
            title.Text = "    Welcome To\r\nEncounter Tracker";
        }

        #region Button Methods

        private void regButton_Clicked(object sender, EventArgs e)
        {
            //Alter page text to show registration
            title.Text = "Create Account";
            nameEntry.Placeholder = "Enter New Username";
            passEntry.Placeholder = "Enter New Password";
            lowerTitleLabel.IsVisible = false;
            logButton.IsVisible = false;
            regButton.IsVisible = false;
            createButton.IsVisible = true;
            nameEntry.TextChanged += createUsername_TextChanged;
            passEntry.TextChanged += createPassword_TextChanged;
            _usernames = _conn.GetUserNames();
        }

        async void logButton_Clicked(object sender, EventArgs e)
        {
            var check = ValidateUser(nameEntry.Text, passEntry.Text);
            if (check)
            {
                await Navigation.PushAsync(new HomePage(_userId));
            }
            else
            {
                await DisplayAlert("Alert", "Username and Password are not valid.", "OK");
            }
        }

        async void createButton_Clicked(object sender, EventArgs e)
        {
            if (_nameCheck && _passCheck)
            {
                var user = new User();
                var newPass = _secure.EncryptData(passEntry.Text);
                user.Username = nameEntry.Text;
                user.Password = newPass;
                _conn.InsertUser(user);
                await Navigation.PushAsync(new LoginPage(), true);
            }
            else
            {
                await DisplayAlert("Alert", "Please Enter a Valid Username and Password", "OK");
            }
        }

        #endregion

        #region Validation Methods

        private bool ValidateUser(string username, string password)
        {
            var check = false;
            //Get the user and validate the password matches the DB
            var user = _conn.GetUserByName(username);
            if (user != null)
            {
                var pass = _secure.DecryptData(user.Password);
                if (pass == password)
                {
                    check = true;
                    _userId = user.UserId;
                }
            }

            return check;
        }

        private void createUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            var check = false;
            foreach (var name in _usernames)
            {
                if (name == nameEntry.Text)
                {
                    check = true;
                }
            }
            if (check)
            {
                nameValidate.TextColor = Color.Red;
                nameValidate.Text = "Username Unavailable";
                nameValidate.IsVisible = true;
                _nameCheck = false;
            }
            else
            {
                nameValidate.TextColor = Color.Green;
                nameValidate.Text = "Username Available";
                nameValidate.IsVisible = true;
                _nameCheck = true;
            }
        }

        private void createPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (passEntry.Text.Length < 4)
            {
                passValidate.TextColor = Color.Red;
                passValidate.Text = "Password must be more than 4 Characters";
                passValidate.IsVisible = true;
                _passCheck = false;
            }
            else
            {
                passValidate.TextColor = Color.Green;
                passValidate.Text = "Valid Password";
                passValidate.IsVisible = true;
                _passCheck = true;
            }
        }

        #endregion

        #region Test Methods

        async void testButton_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Are You Sure?", "This action will clear and repopulate \r\nthe database with demo data.", "Yes", "No");
            if (answer)
            {
                //Add action to Populate the DB with Demo Data
                testLabel.Text = "DataBase reset to Demo Values";
                testLabel.IsVisible = true;
                testLabel.TextColor = Color.Green;
            }
            else
            {
                testLabel.Text = "No Action Taken";
                testLabel.IsVisible = true;
                testLabel.TextColor = Color.Blue;
            }
        }

        #endregion
    }
}