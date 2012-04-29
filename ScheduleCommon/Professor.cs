using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleCommon
{
    [Serializable]
    public class Professor
    {
        public string Name { get; set; }
    //    public List<TimeDayRequirement> Requierments { get; set; }
        //private bool empty;
        public static readonly Professor Empty = new Professor();

        private Professor()
        {
            Name = string.Empty;
       //     Requierments = new List<TimeDayRequirement>();
        }

        public Professor(string aName)
        {
            if (aName == string.Empty)
            {
                throw new ArgumentException("Professor name is empty!");
            }
            Name = aName;
         //   Requierments = new List<TimeDayRequirement>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
