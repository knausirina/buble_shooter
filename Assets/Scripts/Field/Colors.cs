using System.Collections.Generic;
using Data;

namespace Field
{
    public static class MapColors
    {
        private static readonly Dictionary<char, ColorEnum> COLORS = new Dictionary<char, ColorEnum>()
        {
            {'Y', ColorEnum.Yellow},
            {'B', ColorEnum.Blue},
            {'R', ColorEnum.Red},
            {'G', ColorEnum.Green}
        };

        public static bool GetColorEnum(char color, out ColorEnum colorEnum)
        {
            return COLORS.TryGetValue(color, out colorEnum);
        }
    }
}