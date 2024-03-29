using System;
using Xunit;

namespace ROP.Tests
{
    public class RopTests
    {
        [Fact(DisplayName = "First validation fails, throw is not reached")]
        public void FirstFail()
        {
            var p = new Customer()
            {
                Age = 15
            };

            var validationResult = IsNameValid(p).Then(Throw);

            Assert.Equal(Errors.NameIsEmpty, validationResult.Match(_ => Errors.None, e => e));
        }

        [Fact(DisplayName = "First validation pass, throw is reached")]
        public void FirstSuccessThenThrows()
        {
            var p = new Customer()
            {
                Age = 15,
                Name = "test"
            };

            Assert.Throws<Exception>(() => IsNameValid(p).Then(Throw));
        }

        [Fact(DisplayName = "Second validation fails, throw is not reached")]
        public void SecondFail()
        {
            var p = new Customer()
            {
                Name = "test",
                Age = 15
            };

            var validationResult = IsNameValid(p).Then(IsEmailValid).Then(Throw);

            Assert.Equal(Errors.InvalidEmail, validationResult.Match(_ => Errors.None, e => e));
        }

        [Fact(DisplayName = "All success")]
        public void AllSuccess()
        {
            var p = new Customer()
            {
                Name = "test",
                Email = "test@test.it",
                Age = 15
            };

            var validationResult = IsNameValid(p).Then(IsEmailValid).Then(SendEmail);

            Assert.True(validationResult.Match(_ => true, _ => false));
        }

        [Fact(DisplayName = "Action is performed")]
        public void InsertAction()
        {
            var p = new Customer()
            {
                Name = "test",
                Email = "test@test.it",
                Age = 15
            };

            var validationResult = IsNameValid(p).Then(IsEmailValid).Then(SendEmail).Then(EmitEmailSentEvent);

            Assert.True(validationResult.Match(_ => true, _ => false));
            Assert.True(validationResult.Match(r => r.EventEmitted, _ => false));
        }

        [Fact(DisplayName = "Func is performed")]
        public void InsertFunc()
        {
            var p = new Customer()
            {
                Name = "test",
                Email = "test@test.it",
                Age = 15,
                LastEmailSent = new DateTime(2020, 04, 01)
            };

            var finalDate = new DateTime(2020, 04, 03);

            var validationResult = IsNameValid(p)
                                   .Then(IsEmailValid)
                                   .Then(SendEmail)
                                   .Then(EmitEmailSentEvent)
                                   .Then(es => UpdateCustomer(es, finalDate));

            Assert.True(validationResult.Match(_ => true, _ => false));
            Assert.True(validationResult.Match(e => e.Customer.LastEmailSent == finalDate, _ => false));
        }

        private static Result<Customer, Errors> Throw(Customer customer)
        {
            throw new Exception("Reached!");
        }
        private static Result<Customer, Errors> IsNameValid(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.Name))
                return Errors.NameIsEmpty;

            return customer;
        }
        private static Result<Customer, Errors> IsEmailValid(Customer customer)
        {
            if (customer?.Email?.IndexOf('@') > -1)
                return customer;

            return Errors.InvalidEmail;
        }
        private static Result<EmailSent, Errors> SendEmail(Customer customer)
        {
            return new EmailSent() { Customer = customer };
        }
        private static void EmitEmailSentEvent(EmailSent es) { es.EventEmitted = true; }
        private static EmailSent UpdateCustomer(EmailSent es, DateTime date)
        {
            es.Customer.LastEmailSent = date;
            return es;
        }
    }
}
