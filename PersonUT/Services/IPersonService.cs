using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PersonUT.Models;
using Microsoft.AspNetCore.Mvc;

namespace PersonUT.Services
{
    public interface IPersonService
    {
        List<Person> GetList();
        void Create(Person member);
        void Edit(Person member);
        void Delete(string email);
        Person Detail(string email);

    }

}