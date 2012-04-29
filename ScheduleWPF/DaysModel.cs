﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using GongSolutions.Wpf.DragDrop;
using System.Windows;
using System.Collections;
using ScheduleCommon;
namespace ScheduleWPF
{
    class DaysModel:IDropTarget
    {
        public Schedule CurrentSchedule { get; set; }
        public ObservableCollection<Room> AllRooms
        {
            get
            {
                return ConfigurationInstance.Rooms;
            }
        }
        public Configuration ConfigurationInstance
        {
            get
            {
                return Configuration.Instance;
            }
        }
        public ObservableCollection<ConstraintResult> Conflicts { get; set; }
        void InitializeSchedule()
        {
            Conflicts = new ObservableCollection<ConstraintResult>();

            Configuration.Instance.Groups = new ObservableCollection<StudentGroup>();
            Configuration.Instance.Groups.Add(new StudentGroup("11A"));
            Configuration.Instance.Groups.Add(new StudentGroup("11B"));
            Configuration.Instance.Groups.Add(new StudentGroup("11V"));
            Configuration.Instance.Groups.Add(new StudentGroup("11G"));
            Configuration.Instance.Professors = new ObservableCollection<Professor>{new Professor("Abramowitch"),
                new Professor("Mitova"), new Professor("Mitov")};
            Configuration.Instance.Courses = new ObservableCollection<Course>
                {new Course("Maths", Configuration.Instance.Professors[0], CourseType.NormalCourse),
                new Course("BEL", Configuration.Instance.Professors[1], CourseType.NormalCourse), 
                new Course("TP", Configuration.Instance.Professors[2], CourseType.ComputerCourse),
                new Course("Break", Professor.Empty, CourseType.Break)};
            Configuration.Instance.Rooms = new ObservableCollection<Room>{new Room("42", CourseType.NormalCourse), new Room("21", CourseType.NormalCourse),
                new Room("34", CourseType.ComputerCourse)};

            Configuration.Instance.Constraints.Add(new AllProfessorsDayLimitConstraint("PREDEFINED", 9));
            Configuration.Instance.Constraints.Add(new ClassCountDayLimitConstraint("PREDEFINED", 3));
            Configuration.Instance.Constraints.Add(new ProfessorDayAndTimeConstraint("PREDEFINED", new List<TimeDayRequirement>
                {
                    new TimeDayRequirement(Configuration.Instance.Professors[0], 2, new TimeSpan(8,0,0), new TimeSpan(9,0,0)),
                    new TimeDayRequirement(Configuration.Instance.Professors[1], 3, new TimeSpan(9,0,0), new TimeSpan(10,0,0))
                }
            ));

            Configuration.Instance.Constraints.Add(new ProfessorDayConstraint("PREDEFINED", Configuration.Instance.Professors[0], new List<int> { 0, 1 }));
            Configuration.Instance.Constraints.Add(new ProfessorTimeConstraint("PREDEFINED", Configuration.Instance.Professors[0], new TimeSpan(9, 0, 0), new TimeSpan(10, 0, 0)));
            Configuration.Instance.Constraints.Add(new ProfessorTimeOverlapConstraint("PREDEFINED"));
            Configuration.Instance.Constraints.Add(new RoomTimeOverlapConstraint("PREDEFINED"));
            Configuration.Instance.Constraints.Add(new SingleProfessorDayLimitConstraint("PREDEFINED", Configuration.Instance.Professors[2], 4));

            var groups = Configuration.Instance.Groups;
            var courses = Configuration.Instance.Courses;
            var rooms = Configuration.Instance.Rooms;
            var profs = Configuration.Instance.Professors;
            var classes = Configuration.Instance.Classes;
            CurrentSchedule = new Schedule();
            for(int i = 0;i<7;i++)
            {
                CurrentSchedule[i][groups[0]] = new ObservableCollection<Class>();
                CurrentSchedule[i][groups[1]] = new ObservableCollection<Class>();
                CurrentSchedule[i][groups[2]] = new ObservableCollection<Class>();
                CurrentSchedule[i][groups[3]] = new ObservableCollection<Class>();
                CurrentSchedule.SetStartTime(i, groups[0], TimeSpan.FromHours(8));
                CurrentSchedule.SetStartTime(i, groups[1], TimeSpan.FromHours(8));
                CurrentSchedule.SetStartTime(i, groups[2], TimeSpan.FromHours(8));
                CurrentSchedule.SetStartTime(i, groups[3], TimeSpan.FromHours(8));
            }
            CurrentSchedule.DaysChangedGroupAdded(groups[0]);
            CurrentSchedule.DaysChangedGroupAdded(groups[1]);
            CurrentSchedule.DaysChangedGroupAdded(groups[2]);
            CurrentSchedule.DaysChangedGroupAdded(groups[3]);
            classes.Add(groups[0], new TrulyObservableCollection<ClassContainer>());
            classes.Add(groups[1], new TrulyObservableCollection<ClassContainer>());
            classes.Add(groups[2], new TrulyObservableCollection<ClassContainer>());
            classes.Add(groups[3], new TrulyObservableCollection<ClassContainer>());
            classes[groups[0]].Add(new ClassContainer(new Class(groups[0], courses[3], TimeSpan.FromMinutes(40), Room.Empty), 1000));
            classes[groups[0]].Add(new ClassContainer(new Class(groups[0], courses[0], TimeSpan.FromMinutes(80), rooms[1]), 10));
            classes[groups[0]].Add(new ClassContainer(new Class(groups[0], courses[1], TimeSpan.FromMinutes(80), rooms[0]), 4));
            classes[groups[0]].Add(new ClassContainer(new Class(groups[0], courses[2], TimeSpan.FromMinutes(80), rooms[2]), 5));
            classes[groups[1]].Add(new ClassContainer(new Class(groups[1], courses[0], TimeSpan.FromMinutes(80), rooms[1]), 2));
            classes[groups[1]].Add(new ClassContainer(new Class(groups[1], courses[1], TimeSpan.FromMinutes(80), rooms[0]), 6));
            classes[groups[1]].Add(new ClassContainer(new Class(groups[1], courses[2], TimeSpan.FromMinutes(80), rooms[2]), 4));
            classes[groups[2]].Add(new ClassContainer(new Class(groups[2], courses[0], TimeSpan.FromMinutes(80), rooms[1]), 2));
            classes[groups[2]].Add(new ClassContainer(new Class(groups[2], courses[1], TimeSpan.FromMinutes(80), rooms[0]), 6));
            classes[groups[2]].Add(new ClassContainer(new Class(groups[2], courses[2], TimeSpan.FromMinutes(80), rooms[2]), 4));
            classes[groups[3]].Add(new ClassContainer(new Class(groups[3], courses[0], TimeSpan.FromMinutes(80), rooms[1]), 2));
            classes[groups[3]].Add(new ClassContainer(new Class(groups[3], courses[1], TimeSpan.FromMinutes(80), rooms[0]), 6));
            classes[groups[3]].Add(new ClassContainer(new Class(groups[3], courses[2], TimeSpan.FromMinutes(80), rooms[2]), 4));
        }
        public DaysModel()
        {
            Application.Current.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(Current_DispatcherUnhandledException);
            
            var conf = Configuration.Instance;
            
            InitializeSchedule();
            conf.Groups.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Groups_CollectionChanged);
            EvaluateConstraints();
        }

        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(string.Format("Oh no. A terrible error occured.{1}{0}", e.Exception.Message, Environment.NewLine), "Oooooops.", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }

        void Groups_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateGroups();
        }

        private void UpdateGroups()
        {
            foreach (var g in Configuration.Instance.Groups)
            {
                for (int day = 0; day < CurrentSchedule.Length; day++)
                {
                    if (!CurrentSchedule[day].ContainsKey(g))
                    {
                        CurrentSchedule[day].Add(g, new ObservableCollection<Class>());
                        CurrentSchedule.SetStartTime(day, g, TimeSpan.FromHours(8));
                    }
                    if (!Configuration.Instance.Classes.ContainsKey(g))
                    {
                        Configuration.Instance.Classes.Add(g, new TrulyObservableCollection<ClassContainer>());
                    }
                }
            }

            for (int day = 0; day < CurrentSchedule.Length; day++)
            {
                var groupsToRemove = new List<StudentGroup>();
                foreach (var kv in CurrentSchedule[day])
                {
                    if (!Configuration.Instance.Groups.Contains(kv.Key))
                    {
                        groupsToRemove.Add(kv.Key);
                    }
                }
                groupsToRemove.ForEach(group => CurrentSchedule[day].Remove(group));
                groupsToRemove.ForEach(group => Configuration.Instance.Classes.Remove(group));
            }
        }
        public void RemoveClass(Class aClass)
        {
            for (int day = 0; day < CurrentSchedule.Length; day++)
            {
                foreach (var a in CurrentSchedule[day])
                {
                    if (a.Value.Contains(aClass))
                    {
                        foreach (var kv in Configuration.Instance.Classes)
                        {
                            foreach (var classcont in kv.Value)
                            {
                                if ((classcont.Prototype.Course == aClass.Course) && (classcont.Prototype.Group == aClass.Group))
                                {
                                    classcont.AddClass(aClass, a.Value);
                                }
                            }
                        }
                        EvaluateConstraints();
                        return;
                    }
                }
            }
            
        }
        void IDropTarget.DragOver(DropInfo dropInfo)
        {
            if (dropInfo.Data is Class)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Move;
            }
            else if (dropInfo.Data is ClassContainer)
            {
                if (((ClassContainer)dropInfo.Data).Count > 0)
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                    dropInfo.Effects = DragDropEffects.Copy;
                }
            }
        }

        void IDropTarget.Drop(DropInfo dropInfo)
        {
                if (dropInfo.Data is Class)
                {
                    var daydrop = (Class)dropInfo.Data;
                    var source = ((IList)dropInfo.DragInfo.SourceCollection);
                    var target = ((IList)dropInfo.TargetCollection);
                    int indexmodifier = 0;
                    if ((source.IndexOf(daydrop) < dropInfo.InsertIndex) && (dropInfo.TargetCollection == dropInfo.DragInfo.SourceCollection)) indexmodifier = -1;
                    source.Remove(daydrop);
                    if (target.Count > 0)
                    {
                        target.Insert(dropInfo.InsertIndex + indexmodifier, daydrop);
                    }
                    else
                    {
                        target.Add((Class)daydrop);
                    }
                }
                else if (dropInfo.Data is ClassContainer)
                {
                    var daydrop = (ClassContainer)dropInfo.Data;
                    var target = ((IList)dropInfo.TargetCollection);
                    if (target.Count > 0)
                    {
                        target.Insert(dropInfo.InsertIndex, daydrop.GetClass());
                    }
                    else
                    {
                        target.Add(daydrop.GetClass());
                    }
                }
                EvaluateConstraints();
        }
        public void EvaluateConstraints()
        {
            Conflicts.Clear();
            foreach (var constraint in Configuration.Instance.Constraints)
            {
                var result = constraint.Check(CurrentSchedule);
                if (!result.ConstraintFulfilled) Conflicts.Add(result);
            }
        }
    }
}
