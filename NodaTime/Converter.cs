using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NodaTime.Serialization.SystemTextJson;
using NodaTime.Text;
using System.ComponentModel;

namespace NodaTime.Api
{
    public static class Converter
    {
        public static RouteGroupBuilder MapConvertApis(this RouteGroupBuilder group)
        {
            group.MapGet("/UnixToUtc/{unix}", UnixToUtc).WithDisplayName("UnixToUtc");
            group.MapGet("/UnixToLocal/{unix}", UnixToLocal).WithDisplayName("UnixToLocal");
            group.MapGet("/AddDayToDateOnly/{local}", AddDayToDateOnly).WithDisplayName("AddDayToDateOnly");
            group.MapGet("/addDayToLocal/{localString}", addDayToLocal).WithDisplayName("addDayToLocal").WithOpenApi(x =>
            {
                x.Parameters[0].Schema = new OpenApiSchema { Type = "string", Format = "date", Example = new OpenApiString("2024-05-11") };
                return x;
            });

            return group;
        }

        //does not work
        public static Ok<LocalDate> AddDayToDateOnly([TypeConverter(typeof(NodaConverters))][FromRoute] LocalDate local, IClock clock)
        {
            return TypedResults.Ok(local.PlusDays(1));
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
