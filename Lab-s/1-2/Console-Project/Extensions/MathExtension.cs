using OpenTK.Mathematics;

namespace Console_Project
{
    static class MathExtension
    {
        public const float SmallStep = 0.001f,
            MediumStep = 0.004f,
            BigStep = 0.01f,
            KiloStep = 0.08f,
            MegaStep = 0.24f,
            GigaStep = 0.72f;

        public static string ToFormattedString(this float number) =>
            number.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);

        public static string ToFormattedString(this Matrix4 mat, string separator = ", ")
        {
            var res = "";

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    res += mat[i, j].ToFormattedString() + separator;
                }
            }

            return res[..^separator.Length];
        }
    }
}
