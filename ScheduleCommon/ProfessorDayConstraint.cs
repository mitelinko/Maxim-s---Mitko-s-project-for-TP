using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleCommon
{
    [Serializable]
    public class ProfessorDayConstraint : IConstraint
    {
        private Professor prof;
        private List<int> off;
        public string Name { get; set; }
        public ProfessorDayConstraint(string aName, Professor professor, List<int> daysOff)
        {
            Name = aName;
            prof = professor;
            off = daysOff;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is ProfessorDayConstraint)) return false;
            var pdc = (ProfessorDayConstraint)obj;
            return (pdc.prof == this.prof) && (pdc.off == this.off);
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Name, "Professor Days off");
        }

        public ConstraintResult Check(Schedule sched)
        {
            bool pass = true;
            StringBuilder errorContainer = new StringBuilder();

            foreach (int day in off)
            {
                if (sched[day].Count == 0)
                {
                    continue;
                }
                foreach(var group in Configuration.Instance.Groups)
                {
                    foreach (var classs in sched[day][group])
                    {
                        if (classs.Course.Professor == prof)
                        {
                            pass = false;
                            string error = string.Format("Conflict: professor {0} does not want to work on {1}",
                                prof, ConversionServices.GetDayNameFromDayNumber(day));
                            errorContainer.AppendLine(error);
                        }
                    }
                }
            }
            return new ConstraintResult(pass, errorContainer.ToString().Trim());
        }
    }
}
