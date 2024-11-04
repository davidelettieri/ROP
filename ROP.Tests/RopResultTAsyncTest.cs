using System;
using System.Threading.Tasks;

namespace ROP.Tests;

public class RopResultTAsyncTest
{
    [Test]
    public async Task FirstFail()
    {
        var p = new Customer()
        {
            Age = 15
        };

        var validationResult = await IsNameValid(p).Then(Throw);

        await Assert.That(validationResult.Match(_ => false, e => e.ReasonId == nameof(NameIsEmpty))).IsEqualTo(true);
    }

    [Test]
    public async Task FirstSuccessThenThrows()
    {
        var p = new Customer()
        {
            Age = 15,
            Name = "test"
        };

        await Assert.ThrowsAsync<Exception>(() => IsNameValid(p).Then(Throw));
    }

    [Test]
    public async Task SecondFail()
    {
        var p = new Customer()
        {
            Name = "test",
            Age = 15
        };

        var validationResult = await IsNameValid(p).Then(IsEmailValid).Then(Throw);

        await Assert.That(validationResult.Match(_ => false, e => e.ReasonId == nameof(InvalidEmail))).IsEqualTo(true);
    }

    [Test]
    public async Task AllSuccess()
    {
        var p = new Customer()
        {
            Name = "test",
            Email = "test@test.it",
            Age = 15
        };

        var validationResult = await IsNameValid(p).Then(IsEmailValid).Then(SendEmail);

        await Assert.That(validationResult.Match(_ => true, _ => false)).IsEqualTo(true);
    }

    [Test]

    public async Task InsertAction()
    {
        var p = new Customer()
        {
            Name = "test",
            Email = "test@test.it",
            Age = 15
        };

        var validationResult = await IsNameValid(p).Then(IsEmailValid).Then(SendEmail).Then(EmitEmailSentEvent);
        
        await Assert.That(validationResult.Match(_ => true, _ => false)).IsEqualTo(true);
        await Assert.That(validationResult.Match(u => u.EventEmitted, _ => false)).IsEqualTo(true);
    }

    [Test]
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

        await Assert.That(validationResult.Match(_ => true, _ => false)).IsEqualTo(true);
        await Assert.That(validationResult.Match(e => e.Customer.LastEmailSent == finalDate, _ => false)).IsEqualTo(true);
    }

    private static Task<Result<Customer>> Throw(Customer customer)
    {
        throw new Exception("Reached!");
    }

    private static Task<Result<Customer>> IsNameValid(Customer customer)
    {
        if (string.IsNullOrWhiteSpace(customer.Name))
            return Task.FromResult(new Result<Customer>(new NameIsEmpty()));

        return Task.FromResult(new Result<Customer>(customer));
    }
    private static Task<Result<Customer>> IsEmailValid(Customer customer)
    {
        if (customer?.Email?.IndexOf('@') > -1)
            return Task.FromResult(new Result<Customer>(customer));

        return Task.FromResult(new Result<Customer>(new InvalidEmail()));
    }
    private static Task<Result<EmailSent>> SendEmail(Customer customer)
    {
        return Task.FromResult(new Result<EmailSent>(new EmailSent() { Customer = customer }));
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

    public class NameIsEmpty() : Failure(nameof(NameIsEmpty), "Name of the user is empty");

    public class InvalidEmail() : Failure(nameof(InvalidEmail), "Email is not valid");
}