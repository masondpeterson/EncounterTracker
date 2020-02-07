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
        public LoginPage()
        {
            Title = "Encounter Tracker";
            InitializeComponent();
        }

        private void regButton_Clicked(object sender, EventArgs e)
        {

        }

        private void logButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}