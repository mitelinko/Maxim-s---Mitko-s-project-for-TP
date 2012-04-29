using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleCommon
{
    [Serializable]
    public class ProfessorDayAndTimeConstraint : IConstraint
    {
        private List<TimeDayRequirement> requirements;
        public string Name { get; set; }
        public ProfessorDayAndTimeConstraint(string aName, List<TimeDayRequirement> aRequirements)
        {
            Name = aName;
            requirements = aRequirements;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is ProfessorDayAndTimeConstraint)) return false;
            var pdtc = (ProfessorDayAndTimeConstraint)obj;
            return (pdtc.requirements == this.requirements);
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Name, "Professor Day&Time off");
        }

        public ConstraintResult Check(Schedule sched)
        {

            bool pass = true;
            StringBuilder errorContainer = new StringBuilder();
            
            foreach (var req in requirements)
            {
                int day = req.Day;

                if (sched[day].Count == 0)
                {
                    continue;
                }
                foreach (var group in Configuration.Instance.Groups)
                {
                    foreach (var classs in sched[day][group])
                    {
                        if (classs.Course.Professor == req.Professor)
                        {
                            TimeSpan classStart = sched.GetStartTimeForClass(day, group, classs);
                            TimeSpan classEnd = classStart + classs.Length;
                            if ((classStart >= req.Start && classStart <= req.End) || (classEnd >= req.Start && classEnd <= req.End))
                            {
                                pass = false;
                                string error = string.Format("Conflict: professor {0} does not want to work on {3} between {1:hh\\:mm}-{2:hh\\:mm}",
                                    req.Professor, classStart, classEnd, ConversionServices.GetDayNameFromDayNumber(req.Day));
                                errorContainer.AppendLine(error);
                            }
                        }
                    }
                }
            }
            return new ConstraintResult(pass, errorContainer.ToString().Trim());
        }
    }
}
