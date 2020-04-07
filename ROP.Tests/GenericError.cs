namespace ROP.Tests
{

    public class GenericError : Failure
    {
        public GenericError() : base("500", "Unknown error")
        {
        }
    }
}
