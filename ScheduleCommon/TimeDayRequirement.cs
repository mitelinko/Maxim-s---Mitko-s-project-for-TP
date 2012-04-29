using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ScheduleCommon
{
    [Serializable]
    public class TimeDayRequirement:INotifyPropertyChanged
    {
        Professor professor;
        int day;
        TimeSpan start;
        TimeSpan end;
        public Professor Professor
        {
            get
            {
                return professor;
            }
            set
            {
                professor = value;
                OnPropertyChanged("Professor");
            }
        }
        public int Day
        {
            get
            {
                return day;
            }
            set
            {
                day = value;
                OnPropertyChanged("Day");
            }
        }
        public TimeSpan Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value;
                OnPropertyChanged("Start");
            }
        }
        public TimeSpan End
        {
            get
            {
                return end;
            }
            set
            {
                end = value;
                OnPropertyChanged("End");
            }
        }

        public TimeDayRequirement(Professor aProf, int aDay, TimeSpan aStart, TimeSpan aEnd)
        {
            Professor = aProf;
            Day = aDay;
            Start = aStart;
            End = aEnd;
            //Professor.Requierments.Add(this);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is TimeDayRequirement)) return false;
            var tdrq = (TimeDayRequirement)obj;
            return (tdrq.Professor == this.Professor) && (tdrq.Start == this.Start) && (tdrq.End == this.End) && (tdrq.Day == this.Day);
        }

        public override string ToString()
        {
            return string.Format("Professor {0} does not want to work on {1} between {2:hh\\:mm}-{3:hh\\:mm}", Professor, ConversionServices.GetDayNameFromDayNumber(Day), Start, End);
        }
        void OnPropertyChanged(string aName)
        {
            if(PropertyChanged!=null)
                PropertyChanged(this, new PropertyChangedEventArgs(aName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
