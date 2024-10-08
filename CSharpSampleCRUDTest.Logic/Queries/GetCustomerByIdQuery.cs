﻿using CSharpSampleCRUDTest.Domain.Models;
using MediatR;

namespace CSharpSampleCRUDTest.Logic.Queries;

public record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerModel>;
