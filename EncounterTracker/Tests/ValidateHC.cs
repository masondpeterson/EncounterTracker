using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EncounterTracker.Tests
{
    public class ValidateHC
    {
        public bool ValidateNameField(string name)
        {
            var check = false;
            if (name == null || name == "")
            {
                check = true;
            }
            return check;
        }
    }
}
