using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NodaTime.Text;

namespace NodaTime.Api
{
    public static class Converter
    {
        public static RouteGroupBuilder MapConvertApis(this RouteGroupBuilder group)
        {
            group.MapGet("/UnixToUtc/{unix}", UnixToUtc).WithDisplayName("UnixToUtc");
            group.MapGet("/UnixToLocal/{unix}", UnixToLocal).WithDisplayName("UnixToLocal");
            group.MapGet("/addDayToLocal/{localString}", addDayToLocal).WithDisplayName("addDayToLocal");

            return group;
        }

        public static Ok<LocalDate> addDayToLocal([FromRoute] string localString, IClock clock)
        {
            var local = LocalDatePattern.Iso.Parse(localString);
            return TypedResults.Ok(local.Value.PlusDays(1));
        }

        public static Ok<ZonedDateTime> UnixToUtc(long unix, IClock clock)
        {
            var instant = Instant.FromUnixTimeTicks(unix);
            return TypedResults.Ok(instant.InUtc());
        }

        public static Ok<ZonedDateTime> UnixToLocal(long unix, IClock clock)
        {
            DateTimeZone tz = DateTimeZoneProviders.Tzdb.GetSystemDefault();
            var instant = Instant.FromUnixTimeTicks(unix);
            return TypedResults.Ok(instant.InZone(tz));
        }
    }
}
