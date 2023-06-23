using StackExchange.Redis;

namespace MvcCacheRedixCedex.Helpers
{
    public static class HelperCacheMultiplexer
    {
        private static Lazy<ConnectionMultiplexer> CreateConnection =
            new Lazy<ConnectionMultiplexer>(() =>
            {
                string cnn = "cacheredisproductoscedex.redis.cache.windows.net:6380,password=vfHRzEjHzBzfMlcE0eFXytUTvJ3h5rmGFAzCaNxrteI=,ssl=True,abortConnect=False";
                return ConnectionMultiplexer.Connect(cnn);
            });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return CreateConnection.Value;
            }
        }
    }
}
