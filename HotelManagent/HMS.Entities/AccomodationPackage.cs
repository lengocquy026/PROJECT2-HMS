using System;
using System.Collections.Generic;
using System.Text;

namespace HMS.Entities
{
    public class AccomodationPackage
    {
        public AccomodationPackage()
        {
            IsStatus = true;
        }
        public int ID { get; set; }

        public int AccomodationTypeID { get; set; }
        public virtual AccomodationType AccomodationType { get; set; }

        public string IMGPackage { get; set; }
        public string Name { get; set; }
        public int NoOfRoom { get; set; }
        public decimal FeePerNight { get; set; }
        public bool IsStatus { get; set; }

        //public virtual List<PictureAccomodationPackage> PictureAccomodationPackage { get; set; }
    }
}
