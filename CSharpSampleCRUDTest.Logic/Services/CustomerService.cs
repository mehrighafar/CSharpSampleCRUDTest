using CSharpSampleCRUDTest.Domain.Interfaces.DataAccess;
using CSharpSampleCRUDTest.Domain.Interfaces.Services;
using CSharpSampleCRUDTest.Domain.Models;
using PhoneNumbers;

namespace CSharpSampleCRUDTest.Logic.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerDataAccessService _customerDataAccessService;

    public CustomerService(ICustomerDataAccessService customerDataAccessService)
    {
        _customerDataAccessService = customerDataAccessService;
    }

    public async Task<IEnumerable<CustomerModel>> GetAllAsync()
    {
        try
        {
            var result = await _customerDataAccessService.GetAllAsync();
            return result;
        }
        catch (Exception ex)
        {
            // log 

            throw new Exception(ex.Message);
        }
    }

    public async Task<CustomerModel> GetByIdAsync(int id)
    {
        try
        {
            var result = await _customerDataAccessService.GetByIdAsync(id);
            return result;
        }
        catch (Exception ex)
        {
            // log 

            throw new Exception(ex.Message);
        }
    }

    public async Task<CustomerModel> AddAsync(CustomerModel model)
    {
        try
        {
            if (!IsValidNumber(model.PhoneNumber))
                return null;

            // TODO: some logic for bank account number validation

            var result = await _customerDataAccessService.AddAsync(model);

            return result;
        }
        catch (Exception ex)
        {
            // log 

            throw new Exception(ex.Message);
        }
    }
    public async Task<CustomerModel> UpdateAsync(CustomerModel model)
    {
        try
        {
            if (!IsValidNumber(model.PhoneNumber))
                return null;

            // TODO: some logic for bank account number validation

            var result = await _customerDataAccessService.UpdateAsync(model);

            return result;
        }
        catch (Exception ex)
        {
            // log 

            throw new Exception(ex.Message);
        }

    }
    public async Task<int> DeleteAsync(int id)
    {
        try
        {
            return await _customerDataAccessService.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            // log 

            throw new Exception(ex.Message);
        }
    }
    private bool IsValidNumber(string number)
    {
        if (!number.All(char.IsDigit))
            return false;

        PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();
        PhoneNumber phoneNumber = phoneNumberUtil.Parse(number, "NL");
        var result = phoneNumberUtil.IsValidNumber(phoneNumber);
        return result;
    }
}
