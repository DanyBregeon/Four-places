using System;
using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace Fourplaces.Modele
{
    class MyMap : Map
    {
        public List<MyPin> CustomPins { get; set; }
    }
}
