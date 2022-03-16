using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ROP.Tests
{
    public class ThenTests
    {
        [Fact]
        public async Task Test()
        {
            var request = new SearchRequest();
            var repo = new ProjectRepository();
            var r = await GetUser("fake_id")
                          .Then(IsAdmin)
                          .Then(_ => repo.Search(request))
                          .ConfigureAwait(false);

            Assert.True(r.Match(p => true, p => false));
        }

        public class User { }
        public class SearchRequest { }
        public class SearchResult { }
        public class ProjectRepository
        {
            public Task<SearchResult> Search(SearchRequest _) => Task.FromResult(new SearchResult());
        }

        public Task<Result<User>> GetUser(string _) => Task.FromResult(new Result<User>(new User()));
        public Task<Result<User>> IsAdmin(User u) => Task.FromResult(new Result<User>(u));


    }
}
