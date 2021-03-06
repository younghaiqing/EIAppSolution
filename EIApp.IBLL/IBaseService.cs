﻿using EIApp.IDAL;
using EIApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIApp.IBLL
{
    public interface IBaseService<T> : IBaseRepository<T> where T : BaseEntity
    {
    }
}