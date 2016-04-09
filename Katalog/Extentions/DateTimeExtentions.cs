using System;

namespace Katalog
{
    public static class DateTimeExtentions
    {
        public static string GetBackupFileName(this DateTime dt, string ext = ".bak")
        {
            return $"{dt.Year:0000}{dt.Month:00}{dt.Day:00}{dt.Hour:00}{dt.Minute:00}{dt.Second:00}{dt.Millisecond:000}{ext}";
        }
    }
}