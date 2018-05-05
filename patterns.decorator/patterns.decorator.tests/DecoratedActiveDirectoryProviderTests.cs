using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace patterns.decorator.tests
{
    [TestClass]
    public class DecoratedActiveDirectoryProviderTests
    {
        private static Mock<IActiveDirectoryProvider> _adProvider;
        private static MonkeyCacheActiveDirectoryProvider _cachedActiveDirectoryProvider;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            _adProvider = new Mock<IActiveDirectoryProvider>();
            _cachedActiveDirectoryProvider = new MonkeyCacheActiveDirectoryProvider(_adProvider.Object, "patterns.decorator.tests", 1.0, true);
        }

        [TestMethod]
        public void ShouldCallActiveDirectoryFetchWhenCalledFirstTime()
        {
            _adProvider.Setup(p => p.GetUser("vish")).Returns(new User() { DisplayName = "vish", PrimaryPID = "vish" });

            Assert.AreEqual("vish", _cachedActiveDirectoryProvider.GetUser("vish").DisplayName);

            _adProvider.Verify();
        }

        [TestMethod]
        public void ShouldReturnCachedValueWhenCalledMoreThanOnce()
        {
            _adProvider.Setup(p => p.GetUser("user2")).Returns(new User() { DisplayName = "user2", PrimaryPID = "user2" });

            Assert.AreEqual("user2", _cachedActiveDirectoryProvider.GetUser("user2").DisplayName);
            Assert.AreEqual("user2", _cachedActiveDirectoryProvider.GetUser("user2").DisplayName);

            _adProvider.Verify(p => p.GetUser("user2"), Times.Once());
        }
    }
}
