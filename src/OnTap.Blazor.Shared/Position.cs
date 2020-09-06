namespace OnTap.Blazor.Shared
{
    public enum Position
    {
        Unknown = 0,
        Quarterback,
        RunningBack,
        WideReceiver,
        TightEnd,
        Defense,
        Kicker
    }

    public static class PositionExtensions
    {
        public static string Name(this Position position)
        {
            switch (position)
            {
                case Position.Quarterback:
                    return "Quarterback";

                case Position.RunningBack:
                    return "Running Back";

                case Position.WideReceiver:
                    return "Wide Receiver";

                case Position.TightEnd:
                    return "Tight End";

                case Position.Defense:
                    return "Defense";

                case Position.Kicker:
                    return "Kicker";

                case Position.Unknown:
                default:
                    return "Unknown";
            }
        }

        public static string ShortName(this Position position)
        {
            switch (position)
            {
                case Position.Quarterback:
                    return "QB";

                case Position.RunningBack:
                    return "RB";

                case Position.WideReceiver:
                    return "WR";

                case Position.TightEnd:
                    return "TE";

                case Position.Defense:
                    return "DEF";

                case Position.Kicker:
                    return "K";

                case Position.Unknown:
                default:
                    return "U";
            }
        }
    }
}