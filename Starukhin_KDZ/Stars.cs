﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starukhin_KDZ
{
    [Serializable]
    public class Stars
    {

        public string _name { get; set; }
        public double _distance { get; set; }
        public double _radius { get; set; }
        public double _brightness { get; set; }

        public Stars(string name, double distance, double radius, double brightness)
        {
            _distance = distance;
            _name = name;
            _radius = radius;
            _brightness = brightness;

        }
    }
}
