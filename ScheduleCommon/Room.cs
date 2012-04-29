﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleCommon
{
    [Serializable]
    public class Room
    {
        public string Name { get; set; }
        public CourseType Type { get; set; }
        public static Room Empty = new Room();
        private Room()
        {
            Name = "none";
            Type = CourseType.NormalCourse;
        }
        public Room(string aName, CourseType aType)
        {
            Name = aName;
            Type = aType;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
