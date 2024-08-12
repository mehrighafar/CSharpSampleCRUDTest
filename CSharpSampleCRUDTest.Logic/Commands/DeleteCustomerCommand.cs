﻿using CSharpSampleCRUDTest.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSampleCRUDTest.Logic.Commands;

public record DeleteCustomerCommand(int Id) : IRequest<int>;