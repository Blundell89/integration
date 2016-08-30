using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Integration.Tests
{
    [TestFixture]
    public class AccountStoreTests
    {
        private Account _account;
        private IMongoDatabase _database;

        [OneTimeSetUp]
        public void GivenANewlyCreatedAccount()
        {
            var fixture = new Fixture();
            _account = fixture.Create<Account>();

            _database = new MongoClient().GetDatabase(Guid.NewGuid().ToString());
        }

        [SetUp]
        public async Task WhenStoring()
        {
            var accountStore = new AccountStore(_database);

            await accountStore.Create(_account, new CancellationToken());
        }

        [Test]
        public async Task ThenTheAccountIsStored()
        {
            var collection = _database.GetCollection<Account>("accounts");
            var account = await collection.Find(x => x.Id == _account.Id).SingleAsync();

            Assert.That(account.Id, Is.EqualTo(_account.Id));
            Assert.That(account.Number, Is.EqualTo(_account.Number));
        }

        [TearDown]
        public async Task Destroy() => await _database.Client.DropDatabaseAsync(_database.DatabaseNamespace.DatabaseName);
    }
}
