using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleCommon
{
    [Serializable]
    public class AllProfessorsDayLimitConstraint : IConstraint
    {
        private int classLimit;
        public string Name { get; set; } 

        public AllProfessorsDayLimitConstraint(string aName, int aClassLimit)
        {
            Name = aName;
            classLimit = aClassLimit;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is AllProfessorsDayLimitConstraint)) return false;
            var apdlc = (AllProfessorsDayLimitConstraint)obj;
            return (apdlc.classLimit == this.classLimit);
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Name, "Professors classes per day");
        }

        public ConstraintResult Check(Schedule sched)
        {
            bool pass = true;
            StringBuilder errorContainer = new StringBuilder();
            int classCounter;

            foreach (var prof in Configuration.Instance.Professors)
            {
                for (int day = 0; day < 6; day++)
                {
                    classCounter = 0;
                    if (sched[day].Count == 0)
                    {
                        continue;
                    }

                    foreach (var group in Configuration.Instance.Groups)
                    {
                        foreach (var classs in sched[day][group])
                        {
                            if (prof == classs.Course.Professor)
                            {
                                classCounter++;
                            }
                        }
                    }

                    if (classCounter > classLimit)
                    {
                        pass = false;
                        string error = string.Format("Conflict: professor {0} has {1} classes on {2}, instead of <= {3}",
                            prof, classCounter, ConversionServices.GetDayNameFromDayNumber(day), classLimit );
                        errorContainer.AppendLine(error);
                    }
                }
            }
            return new ConstraintResult(pass, errorContainer.ToString().Trim());
        }


    }
}
