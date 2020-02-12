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
    public partial class EnterStatsPage : ContentPage
    {
        private int _userId;
        private int _charId;

        public EnterStatsPage(int userId, int charId)
        {
            _userId = userId;
            _charId = charId;

            Title = "Encounter Tracker";
            InitializeComponent();
        }

        private void submitButton_Clicked(object sender, EventArgs e)
        {

        }

        private void killCount_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void assistCount_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void dmgDealt_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void dmgTaken_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void expEntry_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}