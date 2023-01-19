namespace SonOfBlogUpdater.Utils
{
    public static class Singleton<T> where T : class
    {
        private static T? _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Activator.CreateInstance<T>();
                }
                return _instance ?? throw new InvalidOperationException("Instance is null");
            }
        }
    }
}
