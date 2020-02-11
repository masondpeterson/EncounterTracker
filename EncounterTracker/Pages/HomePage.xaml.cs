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
    public partial class HomePage : ContentPage
    {
        private int userId;

        public HomePage(int id)
        {
            userId = id;

            Title = "Encounter Tracker";

            InitializeComponent();
        }

        #region Button Action Methods

        async void CharCreateButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateCharacterPage());
        }

        async void newStatsButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EnterStatsPage());
        }

        async void histButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StatsPage());
        }

        #endregion
    }
}