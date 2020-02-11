using EncounterTracker.DataBase;
using EncounterTracker.DBObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EncounterTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private SQLConn _conn = new SQLConn();
        private Simple3Des secure = new Simple3Des();
        private List<string> Usernames;
        private bool nameCheck;
        private bool passCheck;

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
            Usernames = _conn.GetUserNames();
        }

        async void logButton_Clicked(object sender, EventArgs e)
        {
            var check = ValidateUser(nameEntry.Text, passEntry.Text);
            if (check)
            {
                await Navigation.PushAsync(new HomePage(), true);
            }
            else
            {
                await DisplayAlert("Alert", "Username and Password are not valid.", "OK");
            }
        }

        async void createButton_Clicked(object sender, EventArgs e)
        {
            if (nameCheck && passCheck)
            {
                var user = new User();
                var newPass = secure.EncryptData(passEntry.Text);
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

        private void createUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            var check = false;
            foreach (var name in Usernames)
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
                nameCheck = false;
            }
            else
            {
                nameValidate.TextColor = Color.Green;
                nameValidate.Text = "Username Available";
                nameValidate.IsVisible = true;
                nameCheck = true;
            }
        }

        private void createPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (passEntry.Text.Length < 4)
            {
                passValidate.TextColor = Color.Red;
                passValidate.Text = "Password must be more than 4 Characters";
                passValidate.IsVisible = true;
                passCheck = false;
            }
            else
            {
                passValidate.TextColor = Color.Green;
                passValidate.Text = "Valid Password";
                passValidate.IsVisible = true;
                passCheck = true;
            }
        }

        #endregion

        #region Support Methods

        private bool ValidateUser(string username, string password)
        {
            var check = false;
            //Get the user and validate the password matches the DB
            var user = _conn.GetUserByName(username);
            if (user != null)
            {
                var pass = secure.DecryptData(user.Password);
                if (pass == password)
                {
                    check = true;
                }
            }

            return check;
        }

        #endregion
    }
}