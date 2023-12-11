using System.Collections.Concurrent;

namespace ApiCatalogoController.Logging
{
    public class CustomLoggerProvider : ILoggerProvider
    {
        private readonly CustomLoggerProviderConfiguration loggerConfig;
        private readonly ConcurrentDictionary<string, CustomerLogger> loggers =
            new ConcurrentDictionary<string, CustomerLogger>();

        public CustomLoggerProvider(CustomLoggerProviderConfiguration configuration)
        {
            loggerConfig = configuration;
        }

        public ILogger CreateLogger(string categoryName)
        {
            // cria uma instância do logger customizado
            return loggers.GetOrAdd(categoryName, name => new CustomerLogger(name, loggerConfig));
        }

        public void Dispose()
        {
            loggers.Clear();
        }
    }
}
