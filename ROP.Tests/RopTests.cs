using System;
using System.Threading.Tasks;

namespace ROP.Tests;

public class RopTests
{
    [Test]
    public async Task FirstFail()
    {
        var p = new Customer()
        {
            Age = 15
        };

        var validationResult = IsNameValid(p).Then(Throw);

        await Assert.That(validationResult.Match(_ => Errors.None, e => e)).IsEqualTo(Errors.NameIsEmpty);
    }

    [Test]
    public void FirstSuccessThenThrows()
    {
        var p = new Customer()
        {
            Age = 15,
            Name = "test"
        };

        Assert.Throws<Exception>(() => IsNameValid(p).Then(Throw));
    }

    [Test]
    public async Task SecondFail()
    {
        var p = new Customer()
        {
            Name = "test",
            Age = 15
        };

        var validationResult = IsNameValid(p).Then(IsEmailValid).Then(Throw);

        await Assert.That(validationResult.Match(_ => Errors.None, e => e)).IsEqualTo(Errors.InvalidEmail);
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

        var validationResult = IsNameValid(p).Then(IsEmailValid).Then(SendEmail);

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

        var validationResult = IsNameValid(p).Then(IsEmailValid).Then(SendEmail).Then(EmitEmailSentEvent);

        await Assert.That(validationResult.Match(_ => true, _ => false)).IsEqualTo(true);
        await Assert.That(validationResult.Match(r => r.EventEmitted, _ => false)).IsEqualTo(true);
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

        var validationResult = IsNameValid(p)
            .Then(IsEmailValid)
            .Then(SendEmail)
            .Then(EmitEmailSentEvent)
            .Then(es => UpdateCustomer(es, finalDate));

        await Assert.That(validationResult.Match(_ => true, _ => false)).IsEqualTo(true);
        await Assert.That(validationResult.Match(e => e.Customer.LastEmailSent == finalDate, _ => false)).IsEqualTo(true);
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