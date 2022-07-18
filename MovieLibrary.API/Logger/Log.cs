using System.Text;

namespace MovieLibrary.API.Logger
{
    public class Log : ILog
    {
        private static readonly Lazy<Log> Instance = new Lazy<Log>(() => new Log());

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
            string fileName = string.Format("{0}_{1}.log",
                "Exception", DateTime.Now.ToShortDateString());
            string logFilePath = string.Format(@"{0}\{1}", AppDomain.CurrentDomain.BaseDirectory, fileName);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("----------------------------------------");
            sb.AppendLine(DateTime.Now.ToString());
            sb.AppendLine(message);
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.Write(sb.ToString());
                writer.Flush();
            }
        }
    }
}
