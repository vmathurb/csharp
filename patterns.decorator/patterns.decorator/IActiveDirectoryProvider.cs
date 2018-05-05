namespace patterns.decorator
{
    public interface IActiveDirectoryProvider
    {
        User GetUser(string userPid);
    }
}
