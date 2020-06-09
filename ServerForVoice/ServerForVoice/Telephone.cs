using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerForVoice
{
    class Telephone
    {
        private string name;
        private bool isTake;
        private string status;
        private string channel;
        private string ipAddress;
        public Telephone()
        {

        }

        public Telephone(string telephoneNumb)
        {
            name = telephoneNumb;
            isTake = false;
            status = "free";
            channel = string.Empty;
            ipAddress = string.Empty;
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public bool IsTake
        {
            get
            {
                return isTake;
            }
            set
            {
                isTake = value;
            }
        }

        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }

        public string Channel
        {
            get
            {
                return channel;
            }
            set
            {
                channel = value;
            }
        }

        public string IPAddress
        {
            get
            {
                return ipAddress;
            }
            set
            {
                ipAddress = value;
            }
        }
    }
}
