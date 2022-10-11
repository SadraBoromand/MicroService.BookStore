using System;
using System.Globalization;

namespace Microservice.Accounting.Models
{
    public static class DateConvertor
    {
        public static string ToShamsi(this DateTime date)
        {
            PersianCalendar pc = new();

            return $"{pc.GetYear(date)}/{pc.GetMonth(date).ToString("#,0")}/{pc.GetDayOfMonth(date).ToString("#,0")}";
        }
    }
}
