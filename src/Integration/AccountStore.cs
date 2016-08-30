using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Integration
{
    public class AccountStore
    {
        private IMongoCollection<Account> _collection;

        public AccountStore(IMongoDatabase mongoDatabase)
        {
            _collection = mongoDatabase.GetCollection<Account>("accounts");
        }

        public async Task Create(Account account, CancellationToken cancellationToken)
        {
            await _collection.InsertOneAsync(account, cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }
}
