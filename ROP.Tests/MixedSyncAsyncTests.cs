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

            Assert.True(validationResult.Match(p => false, p => true));
        }

        [Fact(DisplayName = "Async then async")]
        public async Task FirstFailAsync()
        {
            var p = new Customer()
            {
                Age = 15
            };

            var validationResult = await IsNameValidAsync(p).Then(Throw);

            Assert.True(validationResult.Match(p => false, p => true));
        }

        public static Task<Result<Customer>> IsNameValidAsync(Customer c) => Task.FromResult(new Result<Customer>(new GenericError()));
        public static Result<Customer> IsNameValid(Customer c) => new GenericError();
        public static Task<Result<Customer>> ThrowAsync(Customer c) => throw new Exception();
        public static Result<Customer> Throw(Customer c) => throw new Exception();
    }
}
