using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DAL
{
    class EmailValidator:Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.Unfocused += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        private void OnEntryTextChanged(object sender, FocusEventArgs e)
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
             + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
             + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            Regex validPattern = new Regex(validEmailPattern);

            var isValid = validPattern.IsMatch( e.VisualElement.GetValue(Entry.TextProperty).ToString());
            ((Entry)sender).TextColor = isValid ? Color.Black : Color.Red;
           
        }

      
        protected override void OnDetachingFrom(Entry entry)
        {
            entry.Unfocused -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        //void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        //{
        //    string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
        //       + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
        //       + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        //    Regex validPattern = new Regex(validEmailPattern);

        //    var isValid = validPattern.IsMatch(args.NewTextValue);
        //    ((Entry)sender).TextColor = isValid ? Color.Default : Color.Red;
        //}

    }
}
