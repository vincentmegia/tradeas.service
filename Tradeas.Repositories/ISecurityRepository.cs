﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public interface ISecurityRepository
    {
        Task<Result> GetAll();
    }
}