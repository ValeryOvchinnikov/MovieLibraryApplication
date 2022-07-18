using System.Text;

namespace MovieLibrary.API.Logger
{
    public class Log : ILog
    {
        private static readonly Lazy<Log> Instance = new (() => new Log());

        private Log()
        {
        }

        public static Log GetInstance
        {
            get
            {
                return Instance.Value;
            }
        }

        public void LogException(string message)
        {
            string fileName = $"{(object)"Exception"}_{(object)DateTime.Now.ToShortDateString()}.log";
            string logFilePath = string.Format(@"{0}\{1}", AppDomain.CurrentDomain.BaseDirectory, fileName);

            StringBuilder sb = new();

            sb.AppendLine("----------------------------------------");
            sb.AppendLine(DateTime.Now.ToString());
            sb.AppendLine(message);
            using StreamWriter writer = new(logFilePath, true);
            writer.Write(sb.ToString());
            writer.Flush();
        }
    }
}
