using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starukhin_KDZ
{
    [Serializable]
    public class Galaxy
    {

        public string _name { get; set; }
        public double _distance { get; set; }
        public double _radius { get; set; }
        public string _type { get; set; }

        public Galaxy(string name, double distance, double radius, string type)
        {
            _distance = distance;
            _name = name;
            _radius = radius;
            _type = type;

        }


    }

}
