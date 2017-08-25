using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Gro.Core.DataModels.Boka.DateTimeDto
{
    public class DateTimeObj
    {
        public DateObj Date { get; set; }
        public TimeObj Time { get; set; }
    }

    public class DateObj
    {
        [XmlElement]
        public string Year { get; set; }
        [XmlElement]
        public string Month { get; set; }
        [XmlElement]
        public string Day { get; set; }
    }

    public class TimeObj
    {
        [XmlElement]
        public string Hour { get; set; }
        [XmlElement]
        public string Minute { get; set; }
    }
}
