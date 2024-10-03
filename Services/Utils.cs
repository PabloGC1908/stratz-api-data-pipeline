namespace StratzAPI.Services
{
    public class Utils
    {
        public static DateTime? ConvertUnixToDateTime(long? unix)
        {
            if (unix == null)
                return null;
                 
            return DateTimeOffset.FromUnixTimeSeconds((long)unix).DateTime;
        }
    }
}
