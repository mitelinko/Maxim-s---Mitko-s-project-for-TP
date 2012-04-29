using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleCommon
{
    /// <summary>
    /// A schedule constraint.
    /// </summary>

    public interface IConstraint
    {
        string Name { get; set; }
        ConstraintResult Check(Schedule aSchedule);
    }
}
