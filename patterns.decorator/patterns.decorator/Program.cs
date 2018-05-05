using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace patterns.decorator
{
    class Program
    {
        static void Main(string[] args)
        {
            IActiveDirectoryProvider adProvider = new MonkeyCacheActiveDirectoryProvider(new ActiveDirectoryProvider(), "patterns.decorator");
            var user = adProvider.GetUser("pooja");
            user = adProvider.GetUser("vish");
            Console.WriteLine(user.DisplayName);
            Console.WriteLine($"Cache is stored at {Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}");
        }
    }
}
