using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KsiazkaXAMARIN
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OptionsPage : ContentPage
    {
        public Person person = new Person();

        public OptionsPage()
        {
            InitializeComponent();

            eName.Text = "";
            eSurname.Text = "";
            eNumber.Text = "";
            eEmail.Text = "";
        }

        public OptionsPage(string name, string surname, string number, string email)
        {
            InitializeComponent();

            this.eName.Text = name;
            this.eSurname.Text = surname;
            this.eNumber.Text = number;
            this.eEmail.Text = email;
        }

        private async void OptionsButton_Clicked(object sender, EventArgs e)
        {
            if (eName.Text == "" || eName.Text == " " || eSurname.Text == "" || eSurname.Text == " " || eNumber.Text == "" || eNumber.Text == " " || eEmail.Text == "" || eEmail.Text == " " || RegexVal_Email(eEmail.Text) == false)
            {
                if (eName.Text == "" || eName.Text == " ")
                {
                    eName.BackgroundColor = Color.HotPink;
                }

                if (eSurname.Text == "" || eSurname.Text == " ")
                {
                    eSurname.BackgroundColor = Color.HotPink;
                }

                if (eNumber.Text == "" || eNumber.Text == " ")
                {
                    eNumber.BackgroundColor = Color.HotPink;
                }

                if (eEmail.Text == "" || eEmail.Text == " " || RegexVal_Email(eEmail.Text) == false)
                {
                    eEmail.BackgroundColor = Color.HotPink;
                }
            }
            else
            {
                person.makePerson(eName.Text, eSurname.Text, eNumber.Text, eEmail.Text);
                await Navigation.PopAsync();
            }
        }

        private void eName_TextChanged(object sender, TextChangedEventArgs e)
        {
            eName.BackgroundColor = Color.White;
            var text = eName.Text;
            Regex regex = new Regex("^[A-Za-z]+$");

            if (!regex.IsMatch(text))
            {
                int startIndex = text.Length - 1;
                if (startIndex >= 0)
                {
                    text = text.Remove(startIndex);
                }
            }
        }

        private void eSurname_TextChanged(object sender, TextChangedEventArgs e)
        {
            eSurname.BackgroundColor = Color.White;
            var text = eSurname.Text;
            Regex regex = new Regex("^[A-Za-z]+$");

            if (!regex.IsMatch(text))
            {
                int startIndex = text.Length - 1;
                if (startIndex >= 0)
                {
                    text = text.Remove(startIndex);
                }
            }
        }

        private void eEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            eEmail.BackgroundColor = Color.White;
        }
        
        private void eNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            eNumber.BackgroundColor = Color.White;
        }

        private bool RegexVal_Email(string email)
        {
            bool valide = false;

            if (Regex.IsMatch(email, @"^[A-Za-z\-]+[A-Za-z0-9]*@[A-Za-z0-9]+\.[a-z]+$"))
            {
                valide = true;
            }
            else
            {
                valide = false;
            }

            return valide;
        }
    }
}