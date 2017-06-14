using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Video
    {
        public byte[] content { get; set; }
        public string name { get; set; }
        public Boolean isLast { get; set; }
        public double currentBlob { get; set; }
        public string extension { get; set; }

        public Video(byte[] content, string name, Boolean isLast, double currentBlob, string extension ) {
            this.content = content;
            this.name = name;
            this.isLast = isLast;
            this.currentBlob = currentBlob;
            this.extension = extension;
        }
    }
}
