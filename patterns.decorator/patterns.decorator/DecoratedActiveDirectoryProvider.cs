namespace patterns.decorator
{
    /// <summary>
    /// Why an abstract decorator? So that the client code need not change when you use a different cache
    /// Why pass the interface into the decorator constructor? So that the client code need not change when you use a different underling AD provider
    /// </summary>
    public abstract class DecoratedActiveDirectoryProvider : IActiveDirectoryProvider
    {
        private IActiveDirectoryProvider _activeDirectoryProvider;

        public DecoratedActiveDirectoryProvider(IActiveDirectoryProvider activeDirectoryProvider)
        {
            _activeDirectoryProvider = activeDirectoryProvider;
        }

        public virtual User GetUser(string userPid)
        {
            return _activeDirectoryProvider.GetUser(userPid);
        }
    }
}
