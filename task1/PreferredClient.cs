using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task1
{
    class PreferredClient : Client
    {

        private bool discount = false;
        private int limit = 10;
        public double percent = 0.1;
        public double dicount_sum = 0;

        public PreferredClient(string name, string password) :
            base (name, password)
        {
           
        }
    }
}
