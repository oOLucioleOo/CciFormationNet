using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Video
    {
        public byte[] size { get; set; }
        public string name { get; set; }
        public double count { get; set; }
        public double current { get; set; }

        public Video(byte[] size, string name, double count, double current ) {
            this.size = size;
            this.name = name;
            this.count = count;
            this.current = current;
        }
    }
}
