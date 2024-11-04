using System.Threading.Tasks;

namespace ROP.Tests;

public class ThenTests
{
    [Test]
    public async Task Test()
    {
        var request = new SearchRequest();
        var repo = new ProjectRepository();
        var r = await GetUser("fake_id")
            .Then(IsAdmin)
            .Then(_ => repo.Search(request))
            .ConfigureAwait(false);

        await Assert.That(r.Match(p => true, p => false)).IsEqualTo(true);
    }

    public class User { }
    public class SearchRequest { }
    public class SearchResult { }
    public class ProjectRepository
    {
        public Task<SearchResult> Search(SearchRequest _) => Task.FromResult(new SearchResult());
    }

    private Task<Result<User>> GetUser(string _) => Task.FromResult(new Result<User>(new User()));
    private Task<Result<User>> IsAdmin(User u) => Task.FromResult(new Result<User>(u));
}