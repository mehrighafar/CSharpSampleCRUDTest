using BadRequestException = CSharpSampleCRUDTest.Domain.Exceptions.BadRequestException;

namespace CSharpSampleCRUDTest.Domain.Exceptions;

public sealed class CustomerExistsException : BadRequestException
{
    public CustomerExistsException(int id)
         : base($"The customer with the identifier {id} already exists.")
    {
    }
}
