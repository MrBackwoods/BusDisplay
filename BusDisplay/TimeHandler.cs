using System;

namespace BusDisplay
{
    public static class TimeHandler
    {
        // Helper function for converting datetime to UTC timestamp
        public static long ConvertToUTCTimestamp(DateTime value)
        {
            long epoch = (value.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            return epoch;
        }

        // Helper function for converting local datetime from timestamp
        public static DateTime ConvertToLocalTimeDateTime(int timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(timestamp).ToLocalTime();
        }
    }
}
