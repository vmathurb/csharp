using System;

namespace patterns.decorator
{
    public sealed class ActiveDirectoryProvider : IActiveDirectoryProvider
    {
        User IActiveDirectoryProvider.GetUser(string userPid)
        {
            return new User() { DisplayName = $"Display Name of {userPid}", PrimaryPID = userPid };
        }
    }
}
