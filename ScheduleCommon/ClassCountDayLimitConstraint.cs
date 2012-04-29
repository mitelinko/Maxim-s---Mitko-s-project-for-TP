using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleCommon
{
    [Serializable]
    public class ClassCountDayLimitConstraint : IConstraint
    {
        private int classLimit;
        public string Name { get; set; } 
        public ClassCountDayLimitConstraint(string aName, int aClassLimit)
        {
            Name = aName;
            classLimit = aClassLimit;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is ClassCountDayLimitConstraint)) return false;
            var ccdlc = (ClassCountDayLimitConstraint)obj;
            return (ccdlc.classLimit == this.classLimit);
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Name, "Class count per day constraint");
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
                    Dictionary<Course, int> courseCounter = new Dictionary<Course, int>();
                    
                    for (int classN=0 ; classN < sched[day][group].Count; classN++){
                        
                        var currentCourse = sched[day][group][classN].Course;

                        if (courseCounter.ContainsKey(currentCourse))
                        {
                            courseCounter[currentCourse]++;
                        }
                        else
                        {
                            courseCounter.Add(currentCourse, 1);
                        }
                    }
                    foreach (var aClass in courseCounter)
                    {
                        if (aClass.Value > classLimit && aClass.Key.Name != "Break")
                        {
                            pass = false;
                            string error = string.Format("Conflict: group {0} has {1} {2} classes on {3}, insted of <= {4}",
                                group.Name, aClass.Value, aClass.Key.Name ,ConversionServices.GetDayNameFromDayNumber(day), classLimit);
                            errorContainer.AppendLine(error);
                        }
                    }
                }
               
                
            }
            return new ConstraintResult(pass, errorContainer.ToString().Trim());
        }
    }
}
