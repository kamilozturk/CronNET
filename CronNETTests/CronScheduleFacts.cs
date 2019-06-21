using CronNET.Impl;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CronNETTestProject
{
    public class CronScheduleFacts
    {

        [Fact]
        public void is_valid_test()
        {
            var cronSchedule = new CronSchedule();

            Assert.True(cronSchedule.IsValid("*/2"));
            Assert.True(cronSchedule.IsValid("* * * * *"));
            Assert.True(cronSchedule.IsValid("0 * * * *"));
            Assert.True(cronSchedule.IsValid("0,1,2 * * * *"));
            Assert.True(cronSchedule.IsValid("*/2 * * * *"));
            Assert.True(cronSchedule.IsValid("1-4 * * * *"));
            Assert.True(cronSchedule.IsValid("1-55/3 * * * *"));
            Assert.True(cronSchedule.IsValid("1,10,20 * * * *"));
            Assert.True(cronSchedule.IsValid("* 1,10,20 * * *"));
        }

        [Fact]
        public static void divided_array_test()
        {
            var cronSchedule = new CronSchedule("*/2");
            List<int> results = cronSchedule.Minutes.GetRange(0, 5);//("*/2", 0, 10);
            Assert.Equal(results.ToArray(), new int[] { 0, 2, 4, 6, 8 });
        }

        [Fact]
        public static void range_array_test()
        {
            var cronSchedule = new CronSchedule("1-10");
            List<int> results = cronSchedule.Minutes.GetRange(0, 10);//();
            Assert.Equal(results.ToArray(), new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            var cs = new CronSchedule("1-10/3 20-45/4 * * *");
            results = cs.Minutes;
            Assert.Equal(results.ToArray(), new int[] { 3, 6, 9 });
        }

        [Fact]
        public void wild_array_test()
        {
            var cronSchedule = new CronSchedule("*");
            List<int> results = cronSchedule.Minutes.GetRange(0, 10);//("*", 0, 10);
            Assert.Equal(results.ToArray(), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        }

        [Fact]
        public void list_array_test()
        {
            var cronSchedule = new CronSchedule("1,2,3,4,5,6,7,8,9,10");
            List<int> results = cronSchedule.Minutes;
            Assert.Equal(results.ToArray(), new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        }

        [Fact]
        public void generate_values_divided_test()
        {
            var cronSchedule = new CronSchedule("*/2");
            List<int> results = cronSchedule.Minutes.GetRange(0, 5);//(, 0, 10);
            Assert.Equal(results.ToArray(), new int[] { 0, 2, 4, 6, 8 });
        }

        [Fact]
        public void generate_values_range_test()
        {
            var cronSchedule = new CronSchedule("1-10");
            List<int> results = cronSchedule.Minutes.GetRange(0, 10);//(, 0, 10);
            Assert.Equal(results.ToArray(), new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        }

        [Fact]
        public void generate_Minutes_test()
        {
            var cronSchedule = new CronSchedule("1,2,3 * * * *");
            Assert.Equal(cronSchedule.Minutes.ToArray(), new int[] { 1, 2, 3 });
        }

        [Fact]
        public void generate_hours_test()
        {
            var cronSchedule = new CronSchedule("* 1,2,3 * * *");
            Assert.Equal(cronSchedule.Hours.ToArray(), new int[] { 1, 2, 3 });
        }

        [Fact]
        public void generate_days_of_month_test()
        {
            var cronSchedule = new CronSchedule("* * 1,2,3 * *");
            Assert.Equal(cronSchedule.DaysOfMonth.ToArray(), new int[] { 1, 2, 3 });
        }

        [Fact]
        public void generate_months_test()
        {
            var cronSchedule = new CronSchedule("* * * 1,2,3 *");
            Assert.Equal(cronSchedule.Months.ToArray(), new int[] { 1, 2, 3 });
        }

        [Fact]
        public void generate_days_of_weeks()
        {
            var cronSchedule = new CronSchedule("* * * * 1,2,3 ");
            Assert.Equal(cronSchedule.DaysOfWeek.ToArray(), new int[] { 1, 2, 3 });
        }

        [Fact]
        public void is_time_minute_test()
        {
            var cronSchedule = new CronSchedule("0 * * * *");
            Assert.True(cronSchedule.IsTime(DateTime.Parse("8:00 am")));
            Assert.False(cronSchedule.IsTime(DateTime.Parse("8:01 am")));

            cronSchedule = new CronSchedule("0-10 * * * *");
            Assert.True(cronSchedule.IsTime(DateTime.Parse("8:00 am")));
            Assert.True(cronSchedule.IsTime(DateTime.Parse("8:03 am")));

            cronSchedule = new CronSchedule("*/2 * * * *");
            Assert.True(cronSchedule.IsTime(DateTime.Parse("8:00 am")));
            Assert.True(cronSchedule.IsTime(DateTime.Parse("8:02 am")));
            Assert.False(cronSchedule.IsTime(DateTime.Parse("8:03 am")));
        }

        [Fact]
        public void is_time_hour_test()
        {
            var cronSchedule = new CronSchedule("* 0 * * *");
            Assert.True(cronSchedule.IsTime(DateTime.Parse("12:00 am")));

            cronSchedule = new CronSchedule("* 0,12 * * *");
            Assert.True(cronSchedule.IsTime(DateTime.Parse("12:00 am")));
            Assert.True(cronSchedule.IsTime(DateTime.Parse("12:00 pm")));
        }

        [Fact]
        public void is_time_day_of_month_test()
        {
            var cronSchedule = new CronSchedule("* * 1 * *");
            Assert.True(cronSchedule.IsTime(DateTime.Parse("2010/08/01")));
        }

        [Fact]
        public void is_time_month_test()
        {
            var cronSchedule = new CronSchedule("* * * 1 *");
            Assert.True(cronSchedule.IsTime(DateTime.Parse("1/1/2008")));

            cronSchedule = new CronSchedule("* * * 12 *");
            Assert.False(cronSchedule.IsTime(DateTime.Parse("1/1/2008")));

            cronSchedule = new CronSchedule("* * * */3 *");
            Assert.True(cronSchedule.IsTime(DateTime.Parse("3/1/2008")));
            Assert.True(cronSchedule.IsTime(DateTime.Parse("6/1/2008")));
        }

        [Fact]
        public void is_time_day_of_week_test()
        {
            var cronSchedule = new CronSchedule("* * * * 0");
            Assert.True(cronSchedule.IsTime(DateTime.Parse("10/12/2008")));
            Assert.False(cronSchedule.IsTime(DateTime.Parse("10/13/2008")));

            cronSchedule = new CronSchedule("* * * * */2");
            Assert.True(cronSchedule.IsTime(DateTime.Parse("10/14/2008")));
        }

        [Fact]
        public void is_time_test()
        {
            var cronSchedule = new CronSchedule("0 0 12 10 *");
            Assert.True(cronSchedule.IsTime(DateTime.Parse("12:00:00 am 10/12/2008")));
            Assert.False(cronSchedule.IsTime(DateTime.Parse("12:01:00 am 10/12/2008")));
        }

        [Fact]
        public static void ppp()
        {
            var d = new CronDaemon();
            d.Add(new CronJob(() =>
            {
                Console.WriteLine(DateTime.Now.ToString());
                return Task.CompletedTask;
            }, null, "*/1 * * * *"));
            d.Start(CancellationToken.None);
            //Task.Delay(60 * 1000).Wait();
        }
    }
}
