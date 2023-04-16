using System;
using System.Collections.Generic;
using System.Text;

namespace KsiazkaXAMARIN
{
    public class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Number { get; set; }
        public string Email { get; set; }

        public void makePerson(string name, string surname, string number, string email)
        {
            this.Name = name;
            this.Surname = surname;
            this.Number = number;
            this.Email = email;
        }

        public string Serialize()
        {
            return this.Name + ";" + this.Surname + ";" + this.Number + ";" + this.Email + "\n";
        }

        public static Person Deserialize(string person)
        {
            var t = person.Split(new char[] { Convert.ToChar(";") }, StringSplitOptions.None);

            Person person1 = new Person();

            person1.Name = t[0];
            person1.Surname = t[1];
            person1.Number = t[2];
            person1.Email = t[3];

            return person1;
        }
    }
}
