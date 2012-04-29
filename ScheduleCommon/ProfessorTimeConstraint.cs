using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleCommon
{
    [Serializable]
    public class ProfessorTimeConstraint : IConstraint
    {
        private Professor prof;
        private TimeSpan start;
        private TimeSpan end;
        public string Name { get; set; }
        public ProfessorTimeConstraint(string aName, Professor professor, TimeSpan start, TimeSpan end)
        {
            Name = aName;
            this.prof = professor;
            this.start = start;
            this.end = end;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is ProfessorTimeConstraint)) return false;
            var ptc = (ProfessorTimeConstraint)obj;
            return (ptc.prof == this.prof) && (ptc.start == this.start) && (ptc.end == this.end);
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Name, "Professor Time off");
        }

        public ConstraintResult Check(Schedule sched)
        {
            bool pass = true;
            StringBuilder errorContainer = new StringBuilder();

            for (int day = 0; day < 6; day++)
            {
                if (sched[day].Count == 0)
                {
                    continue;
                }
                foreach (var group in Configuration.Instance.Groups)
                {
                    foreach (var classs in sched[day][group])
                    {
                        if (classs.Course.Professor == prof)
                        {
                            TimeSpan classStart = sched.GetStartTimeForClass(day, group, classs);
                            TimeSpan classEnd = classStart + classs.Length;
                            if( (classStart>=start && classStart<=end) || (classEnd>=start && classEnd<=end) ){
                                pass = false;
                                string error = string.Format("Conflict: professor {0} does not want to work between {1:hh\\:mm}-{2:hh\\:mm}",
                                    prof, classStart, classEnd);
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
