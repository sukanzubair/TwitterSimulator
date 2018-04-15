using log4net;
using Autofac;
using FileReader;
using FileReader.Interfaces;
using TwitterManager;
using TwitterManager.Interfaces;

namespace TwitterSimulator
{
    public static class Configuration
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();
            builder.Register(c => LogManager.GetLogger(typeof(object))).As<ILog>();

            builder.RegisterType<UserFileReader>().As<IUserReader>();
            builder.RegisterType<TweetFileReader>().As<ITweetReader>();
            builder.RegisterType<TweetManager>().As<ITweetManager>();

            return builder.Build();
        }
    }
}
