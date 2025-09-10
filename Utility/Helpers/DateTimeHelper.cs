using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Helpers
{
    public class DateTimeHelper
    {

        /// <summary>
        /// Kiểm tra 2 khoảng ngày có giao nhau không
        /// </summary>
        public static bool IsDateRangeOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            return start1 <= end2 && start2 <= end1;
        }

        /// <summary>
        /// Tính số ngày giữa 2 mốc
        /// </summary>
        public static int GetDayBetween(DateTime from, DateTime to)
        {
            return (to.Date - from.Date).Days;
        }

        /// <summary>
        /// Lấy tuổi từ ngày sinh
        /// </summary>
        public static int GetAge(DateTime birthDay)
        {
            var today = DateTime.Today;

            if (birthDay.Year > today.Year) return 0;

            int age = (today.Year - birthDay.Year);
            if (birthDay.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }

    }
}
