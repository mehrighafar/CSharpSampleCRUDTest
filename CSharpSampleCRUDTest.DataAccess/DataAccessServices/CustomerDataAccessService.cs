﻿using AutoMapper;
using CSharpSampleCRUDTest.DataAccess.Entities;
using CSharpSampleCRUDTest.DataAccess.MapperProfiles;
using CSharpSampleCRUDTest.DataAccess.Repositories;
using CSharpSampleCRUDTest.Domain.Interfaces.DataAccess;
using CSharpSampleCRUDTest.Domain.Interfaces.Services;
using CSharpSampleCRUDTest.Domain.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URF.Core.Abstractions;

namespace CSharpSampleCRUDTest.DataAccess.DataAccessServices;

public class CustomerDataAccessService : ICustomerDataAccessService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CustomerDataAccessService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new CustomerEntityANDCustomerModelMapperProfile());
        });
        _mapper = new Mapper(config);
    }

    public async Task<IEnumerable<CustomerModel>> GetAllAsync()
    {
        try
        {
            var result = await _customerRepository.GetAllAsync();
            if (result is null || result.Count() == 0) { return null; }

            return _mapper.Map<IEnumerable<CustomerModel>>(result);
        }
        catch (Exception ex)
        {
            // log with optional message

            throw new Exception(ex.Message);
        }
    }

    public async Task<CustomerModel> GetByIdAsync(int id)
    {
        try
        {
            var result = await _customerRepository.GetByIdAsync(id);
            if (result is null) { return null; }

            return _mapper.Map<CustomerModel>(result);
        }
        catch (Exception ex)
        {
            // log with optional message

            throw new Exception(ex.Message);
        }

    }
    public async Task<CustomerModel> AddAsync(CustomerModel model)
    {
        try
        {
            if (!await IsEmailUniqueInDb(model.Id, model.Email))
                return null;
            else if (!await IsFirstnameLastnameDateofbirthUnique
                (model.Id, model.FirstName, model.LastName, model.DateOfBirth))
                return null;

            var result = await _customerRepository.AddAsync(_mapper.Map<CustomerEntity>(model));
            if (result is null) { return null; }

            return _mapper.Map<CustomerModel>(result);
        }
        catch (Exception ex)
        {
            // log with optional message

            throw new Exception(ex.Message);
        }
    }
    public async Task<CustomerModel> UpdateAsync(CustomerModel model)
    {
        try
        {
            if (!await IsEmailUniqueInDb(model.Id, model.Email))
                return null;
            else if (!await IsFirstnameLastnameDateofbirthUnique
                (model.Id, model.FirstName, model.LastName, model.DateOfBirth))
                return null;

            var result = await _customerRepository.UpdateAsync(_mapper.Map<CustomerEntity>(model));
            if (result is null) { return null; }

            return _mapper.Map<CustomerModel>(result);
        }
        catch (Exception ex)
        {
            // log with optional message

            throw new Exception(ex.Message);
        }
    }
    public async Task<int> DeleteAsync(int id)
    {
        try
        {
            return await _customerRepository.RemoveAsync(id);
        }
        catch (Exception ex)
        {
            // log with optional message

            throw new Exception(ex.Message);
        }
    }

    private async Task<bool> IsEmailUniqueInDb(int id, string email)
    {
        var customers = await _customerRepository.GetAllAsync();
        var emails = customers.Where(item => item.Id != id).Select(x => x.Email).ToList();
        if (emails.Contains(email))
            return false;
        return true;
    }

    private async Task<bool> IsFirstnameLastnameDateofbirthUnique(int id, string firstName, string lastName, DateOnly dateOfBirth)
    {
        var customers = await _customerRepository.GetAllAsync();
        var list = customers.Where(item => item.Id != id).Select(item => new
        {
            firstName = item.FirstName,
            lastName = item.LastName,
            dateOfBirth = item.DateOfBirth
        }).ToList();

        if (list.Contains(new { firstName, lastName, dateOfBirth }))
            return false;

        return true;
    }
}