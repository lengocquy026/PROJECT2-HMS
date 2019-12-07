using System;
using System.Collections.Generic;

namespace HMS.Entities
{
    public class AccomodationType
    {
        public AccomodationType()
        {
            IsStatus = true;
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsStatus { get; set; }

        public List<AccomodationPackage> accomodationPackages { get; set; }
    }
}
