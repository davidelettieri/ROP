using System;
using System.Threading.Tasks;

namespace ROP.Tests;

public class MixedSyncAsyncTests
{
    [Test]
    public async Task FirstFailSync()
    {
        var p = new Customer()
        {
            Age = 15
        };

        var validationResult = await IsNameValid(p).Then(ThrowAsync);

        await Assert.That(validationResult.Match(_ => false, _ => true)).IsEqualTo(true);
    }

    [Test]
    public async Task FirstFailAsync()
    {
        var p = new Customer()
        {
            Age = 15
        };

        var validationResult = await IsNameValidAsync(p).Then(Throw);

        await Assert.That(validationResult.Match(_ => false, _ => true)).IsEqualTo(true);
    }

    private static Task<Result<Customer>> IsNameValidAsync(Customer _) => Task.FromResult(new Result<Customer>(new GenericError()));
    private static Result<Customer> IsNameValid(Customer _) => new GenericError();
    private static Task<Result<Customer>> ThrowAsync(Customer _) => throw new Exception();
    private static Result<Customer> Throw(Customer _) => throw new Exception();
}