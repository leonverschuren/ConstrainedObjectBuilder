using System;

namespace ConstrainedObjectBuilder
{
  public static class DateHelper
  {
    private static readonly DateTime UNIX_EPOCH = new DateTime(1970, 1, 1);

    public static int ToInt(DateTime date)
    {
      var timeSpan = date.Subtract(UNIX_EPOCH);

      return timeSpan.Days;
    }

    public static DateTime ToDateTime(int days)
    {
      var timeSpan = TimeSpan.FromDays(days);

      return UNIX_EPOCH.Add(timeSpan);
    }
  }
}
