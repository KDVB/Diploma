using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerForVoice
{
    class Station
    {
        private string numb;
        public List<Telephone> telephones;

        public Station()
        {

        }

        public Station(string numbStation)
        {
            numb = numbStation;
            telephones = new List<Telephone>();
        }
        public string Numb
        {
            get
            {
                return numb;
            }
            set
            {
                numb = value;
            }
        }

        public void AddTelephone(string phoneNumb)
        {
            telephones.Add(new Telephone(phoneNumb));
        }

    }
}
