using NpgsqlTypes;
using Serilog;
using Serilog.Sinks.PostgreSQL;

namespace MySerilogLog
{
    public class MySerilog
    {
        private readonly Serilog.ILogger _logger;

        public MySerilog(string connectionString)
        {
            var columnWriters = new Dictionary<string, ColumnWriterBase>
        {
            { "TimeStamp", new TimestampColumnWriter() },
            { "Level", new LevelColumnWriter() },
            { "Message", new RenderedMessageColumnWriter() },
            { "Exception", new ExceptionColumnWriter() },
            { "Properties", new PropertiesColumnWriter() },
            { "LogStatus", new SinglePropertyColumnWriter("LogStatus", (PropertyWriteMethod)NpgsqlDbType.Varchar) },
     
            { "Method", new SinglePropertyColumnWriter("Method", (PropertyWriteMethod)NpgsqlDbType.Varchar) },
            { "FilePlace", new SinglePropertyColumnWriter("FilePlace", (PropertyWriteMethod)NpgsqlDbType.Varchar) },
        };

            _logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.PostgreSQL(
                    connectionString: connectionString,
                    tableName: "Logs",
                    needAutoCreateTable: true,
                    columnOptions: columnWriters)
                .CreateLogger();
        }

       
        public void Information(string message)
        {
            _logger.ForContext("LogStatus", "Success")
                .Information(message);
            
        }

        public void InformationLogStatus(string message, string logStatus)
        {
            _logger.ForContext("LogStatus", logStatus)
                .Information(message);

            
        }

        public void Information(string message, string methodName)
        {
            _logger.ForContext("LogStatus", "Success")
                .ForContext("Method", methodName)
                .Information(message);

         
        }


        public void Information(string message, string methodName, string filePlace)
        {
            _logger.ForContext("LogStatus", "Success")
                .ForContext("Method", methodName)
               .ForContext("FilePlace", filePlace)
               .Information(message);

           
        }

        public void Error(string message)
        {
            _logger.ForContext("LogStatus", "Error")
                .Error(message);
            
        }

        public void Error(string message, string methodName)
        {
            _logger.ForContext("Method", methodName)
                .ForContext("LogStatus", "Error")
                .Error(message);
           
        }

        public void ErrorLogStatus(string message, string logStatus)
        {
            _logger.ForContext("LogStatus", logStatus)
                .Error(message);
            
        }

        public void Error(string message, string methodName, string filePlace)
        {
            _logger.ForContext("Method", methodName)
                .ForContext("FilePlace", filePlace)
                .ForContext("LogStatus", "Error")
                .Error(message);
            
        }

        public void Error(Exception ex, string message)
        {
            _logger.ForContext("LogStatus", "Error")
                .Error(ex, message);
           
        }


        public void Error(Exception ex, string message, string methodName)
        {
            _logger.ForContext("LogStatus", "Error")
                .ForContext("Method", methodName)
                .Error(ex, message);
           
        }

        public void Error(Exception ex, string message, string methodName, string filePlace)
        {
            _logger.ForContext("LogStatus", "Error")
              .ForContext("Method", methodName)
              .ForContext("FilePlace", filePlace)
              .Error(ex, message);
            
        }

    }
}
