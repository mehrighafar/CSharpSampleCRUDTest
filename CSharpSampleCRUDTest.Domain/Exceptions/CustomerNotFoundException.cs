namespace CSharpSampleCRUDTest.Domain.Exceptions;

public sealed class CustomerNotFoundException : NotFoundException
{
    public CustomerNotFoundException(int id)
        : base($"The customer with the identifier {id} was not found.")
    {
    }
}
