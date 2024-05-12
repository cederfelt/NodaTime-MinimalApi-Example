using Microsoft.AspNetCore.Http.HttpResults;
using NodaTime.Extensions;

namespace NodaTime.Api
{
    public static class NodaTimeGroup
    {
        public static RouteGroupBuilder MapTimeApis(this RouteGroupBuilder group)
        {
            group.MapGet("/local", Local).WithDisplayName("Local");
            group.MapGet("/utc", Utc).WithDisplayName("Utc");
            group.MapGet("/offset", Offset).WithDisplayName("Offset");
            group.MapGet("/instant", Instant).WithDisplayName("Instant");

            return group;
        }

        public static Ok<Instant> Instant(IClock clock)
        {
            return TypedResults.Ok(clock.GetCurrentInstant());
        }

        public static Ok<ZonedDateTime> Utc(IClock clock)
        {
            return TypedResults.Ok(clock.GetCurrentInstant().InUtc());
        }

        public static Ok<LocalDateTime> Local(IClock clock)
        {
            return TypedResults.Ok(clock.InTzdbSystemDefaultZone().GetCurrentLocalDateTime());
        }

        public static Ok<OffsetDateTime> Offset(IClock clock)
        {
            return TypedResults.Ok(clock.InTzdbSystemDefaultZone().GetCurrentOffsetDateTime());
        }
    }
}
