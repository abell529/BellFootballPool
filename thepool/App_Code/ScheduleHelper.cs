using System;
using System.Globalization;
using nflgames;

/// <summary>
/// Date helpers shared by the pick pages. Dates from the feed are converted to
/// Eastern time so a late Sunday game never slips into Monday.
/// </summary>
public static class ScheduleHelper
{
    private static readonly TimeZoneInfo EasternTimeZone = InitializeEasternTimeZone();

    /// <summary>Games played before Sunday (Wednesday through Saturday) are picked on the early page.</summary>
    public static bool IsEarlyGame(Gameentry entry)
    {
        var date = ParseDate(entry?.date);
        if (!date.HasValue)
        {
            return false;
        }

        var day = date.Value.DayOfWeek;
        return day == DayOfWeek.Wednesday || day == DayOfWeek.Thursday || day == DayOfWeek.Friday || day == DayOfWeek.Saturday;
    }

    /// <summary>The current time on the Eastern clock, whatever zone the server runs in.</summary>
    public static DateTime NowEastern => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, EasternTimeZone);

    /// <summary>Converts a wall-clock time on the Eastern clock to UTC.</summary>
    public static DateTime EasternToUtc(DateTime eastern)
    {
        return TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(eastern, DateTimeKind.Unspecified), EasternTimeZone);
    }

    /// <summary>Converts a UTC time to the Eastern clock.</summary>
    public static DateTime UtcToEastern(DateTime utc)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utc, DateTimeKind.Utc), EasternTimeZone);
    }

    /// <summary>True once the Eastern clock is past the given "yyyy-MM-dd HH:mm" cutoff. A blank or bad value never closes.</summary>
    public static bool IsPastEastern(string cutoff)
    {
        var parsed = ParseCutoff(cutoff);
        return parsed.HasValue && NowEastern >= parsed.Value;
    }

    /// <summary>"Thursday, September 17 at 8:30 PM ET" for showing the cutoff on a page.</summary>
    public static string DescribeEastern(string cutoff)
    {
        var parsed = ParseCutoff(cutoff);
        return parsed.HasValue ? parsed.Value.ToString("dddd, MMMM d 'at' h:mm tt", CultureInfo.GetCultureInfo("en-US")) + " ET" : string.Empty;
    }

    private static DateTime? ParseCutoff(string cutoff)
    {
        if (DateTime.TryParseExact((cutoff ?? string.Empty).Trim(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
        {
            return parsed;
        }

        return null;
    }

    public static string DayName(Gameentry entry)
    {
        var date = ParseDate(entry?.date);
        return date.HasValue ? date.Value.ToString("dddd", CultureInfo.InvariantCulture) : string.Empty;
    }

    public static DateTime? ParseDate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var trimmed = value.Trim();
        bool hasTimeComponent = trimmed.IndexOf('T') >= 0 || trimmed.IndexOf(':') >= 0;

        if (hasTimeComponent && DateTimeOffset.TryParse(trimmed, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal, out var parsedOffset))
        {
            return TimeZoneInfo.ConvertTime(parsedOffset, EasternTimeZone).DateTime;
        }

        if (DateTime.TryParse(trimmed, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var parsedInvariant))
        {
            return parsedInvariant;
        }

        if (DateTime.TryParse(trimmed, CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.AllowWhiteSpaces, out var parsedUs))
        {
            return parsedUs;
        }

        return null;
    }

    private static TimeZoneInfo InitializeEasternTimeZone()
    {
        foreach (var id in new[] { "Eastern Standard Time", "America/New_York" })
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(id);
            }
            catch (TimeZoneNotFoundException)
            {
            }
            catch (InvalidTimeZoneException)
            {
            }
        }

        return TimeZoneInfo.Utc;
    }
}
