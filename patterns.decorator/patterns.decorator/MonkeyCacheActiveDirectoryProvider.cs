using MonkeyCache;
using MonkeyCache.LiteDB;
using System;

namespace patterns.decorator
{
    public sealed class MonkeyCacheActiveDirectoryProvider : DecoratedActiveDirectoryProvider
    {
        private IBarrel _barrel;
        private readonly double _expiryLengthDays;

        public MonkeyCacheActiveDirectoryProvider(IActiveDirectoryProvider activeDirectoryProvider, string applicationId, double expiryLengthDays = 1.0, bool refresh = false) : base(activeDirectoryProvider)
        {
            Barrel.ApplicationId = applicationId;
            _barrel = Barrel.Current;
            _expiryLengthDays = expiryLengthDays;

            if (refresh)
                _barrel.EmptyAll();
            else
                _barrel.EmptyExpired();
        }

        public override User GetUser(string userPid)
        {
            User user = null;

            if (!_barrel.IsExpired(userPid))
            {
                user = _barrel.Get<User>(userPid);
            }
            else
            {
                user = base.GetUser(userPid);
                _barrel.Add(userPid, user, TimeSpan.FromDays(_expiryLengthDays));
            }

            return user;
        }
    }
}
