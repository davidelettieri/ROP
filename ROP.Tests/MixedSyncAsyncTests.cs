using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ROP.Tests
{
    public class MixedSyncAsyncTests
    {
        [Fact(DisplayName = "Sync then async")]
        public async Task FirstFailSync()
        {
            var p = new Customer()
            {
                Age = 15
            };

            var validationResult = await IsNameValid(p).Then(ThrowAsync);

            Assert.True(validationResult.Match(_ => false, _ => true));
        }

        [Fact(DisplayName = "Async then async")]
        public async Task FirstFailAsync()
        {
            var p = new Customer()
            {
                Age = 15
            };

            var validationResult = await IsNameValidAsync(p).Then(Throw);

            Assert.True(validationResult.Match(_ => false, _ => true));
        }

        private static Task<Result<Customer>> IsNameValidAsync(Customer _) => Task.FromResult(new Result<Customer>(new GenericError()));
        private static Result<Customer> IsNameValid(Customer _) => new GenericError();
        private static Task<Result<Customer>> ThrowAsync(Customer _) => throw new Exception();
        private static Result<Customer> Throw(Customer _) => throw new Exception();
    }
}
