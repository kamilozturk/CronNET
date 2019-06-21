using System;

namespace CronNET
{
    public interface ICronSchedule
    {
        bool IsValid(string expression);
        bool IsTime(DateTime dateTime);
    }
}
