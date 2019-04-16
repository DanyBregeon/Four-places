using System;
using System.Collections.Generic;
using System.Text;

namespace Fourplaces.Modele
{
    class RestServiceSingleton
    {
        private static RestService singletonRS;

        private RestServiceSingleton()
        {

        }

        public static RestService SingletonRS
        {
            get
            {
                Console.WriteLine("\nRestServiceSingl\n");
                if (singletonRS == null)
                {
                    singletonRS = new RestService();
                }
                return singletonRS;
            }
        }
    }
}
