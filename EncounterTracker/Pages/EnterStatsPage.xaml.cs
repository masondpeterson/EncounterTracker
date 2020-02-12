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
    }
}