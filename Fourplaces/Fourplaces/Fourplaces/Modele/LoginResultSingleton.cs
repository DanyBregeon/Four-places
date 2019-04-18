using System;
using System.Collections.Generic;
using System.Text;
using TD.Api.Dtos;

namespace Fourplaces.Modele
{
    class LoginResultSingleton
    {
        private static LoginResult singletonLR;

        private LoginResultSingleton()
        {

        }

        public static LoginResult SingletonLR
        {
            get
            {
                return singletonLR;
            }
            set
            {
                if (singletonLR == null)
                {
                    singletonLR = value;
                }
            }
        }
    }
}
