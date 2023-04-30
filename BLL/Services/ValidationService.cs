﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeesService.BLL.Models;
using FeesService.BLL.Services;
using FeesService.DAL.Entities;

namespace FeesService.BLL.Services
{
    public class ValidationService
    {
        private readonly List<IValidator> validators;

        public ValidationService(List<IValidator> validators)
        {
            this.validators = validators;
        }

        public bool Check()
        {
            try 
            {
                foreach (IValidator validator in validators)
                {
                    if (validator.Check() == false) return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            
        }
    }
}
