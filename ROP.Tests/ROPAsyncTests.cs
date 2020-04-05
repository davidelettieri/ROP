using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static ROP.AsyncFunctions;

namespace ROP.Tests
{
    public class ROPAsyncTests
    {
        [Fact(DisplayName = "First validation fails, throw is not reached")]
        public async Task FirstFail()
        {
            var p = new Customer()
            {
                Age = 15
            };

            var validationResult = await IsNameValid(p).Then(Throw);

            Assert.Equal(Errors.NameIsEmpty, validationResult.Match(p => Errors.None, p => p));
        }

        [Fact(DisplayName = "First validation pass, throw is reached")]
        public async Task FirstSuccessThenThrows()
        {
            var p = new Customer()
            {
                Age = 15,
                Name = "test"
            };

            await Assert.ThrowsAsync<Exception>(() => IsNameValid(p).Then(Throw));
        }

        [Fact(DisplayName = "Second validation fails, throw is not reached")]
        public async Task SecondFail()
        {
            var p = new Customer()
            {
                Name = "test",
                Age = 15
            };

            var validationResult = await IsNameValid(p).Then(IsEmailValid).Then(Throw);

            Assert.Equal(Errors.InvalidEmail, validationResult.Match(p => Errors.None, p => p));
        }

        [Fact(DisplayName = "All success")]
        public async Task AllSuccess()
        {
            var p = new Customer()
            {
                Name = "test",
                Email = "test@test.it",
                Age = 15
            };

            var validationResult = await IsNameValid(p).Then(IsEmailValid).Then(SendEmail);

            Assert.True(validationResult.Match(p => true, p => false));
        }

        [Fact(DisplayName = "Action is performed")]
        public async Task InsertAction()
        {
            var p = new Customer()
            {
                Name = "test",
                Email = "test@test.it",
                Age = 15
            };

            var validationResult = await IsNameValid(p).Then(IsEmailValid).Then(SendEmail).Then(EmitEmailSentEvent);

            Assert.True(validationResult.Match(p => true, p => false));
            Assert.True(validationResult.Match(p => p.EventEmitted, p => false));
        }

        [Fact(DisplayName = "Func is performed")]
        public async Task InsertFunc()
        {
            var p = new Customer()
            {
                Name = "test",
                Email = "test@test.it",
                Age = 15,
                LastEmailSent = new DateTime(2020, 04, 01)
            };

            var finalDate = new DateTime(2020, 04, 03);

            var validationResult = await IsNameValid(p)
                                         .Then(IsEmailValid)
                                         .Then(SendEmail)
                                         .Then(EmitEmailSentEvent)
                                         .Then(es => UpdateCustomer(es, finalDate));

            Assert.True(validationResult.Match(p => true, p => false));
            Assert.True(validationResult.Match(p => p.Customer.LastEmailSent == finalDate, p => false));
        }

        private static Task<Result<Customer, Errors>> Throw(Customer customer)
        {
            throw new Exception("Reached!");
        }
        private static Task<Result<Customer, Errors>> IsNameValid(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.Name))
                return Task.FromResult(new Result<Customer, Errors>(Errors.NameIsEmpty));

            return Task.FromResult(new Result<Customer, Errors>(customer));
        }
        private static Task<Result<Customer, Errors>> IsEmailValid(Customer customer)
        {
            if (customer?.Email?.IndexOf('@') > -1)
                return Task.FromResult(new Result<Customer, Errors>(customer));

            return Task.FromResult(new Result<Customer, Errors>(Errors.InvalidEmail));
        }
        private static Task<Result<EmailSent, Errors>> SendEmail(Customer customer)
        {
            return Task.FromResult(new Result<EmailSent, Errors>(new EmailSent() { Customer = customer }));
        }
        private static Task EmitEmailSentEvent(EmailSent es)
        {
            es.EventEmitted = true;
            return Task.CompletedTask;
        }
        private static Task<EmailSent> UpdateCustomer(EmailSent es, DateTime date)
        {
            es.Customer.LastEmailSent = date;
            return Task.FromResult(es);
        }
    }
}
