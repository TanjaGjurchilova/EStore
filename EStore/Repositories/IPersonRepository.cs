using EStore.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Repositories
{
    interface IPersonRepository
    {
        Person FindPersonById(long id);
        IEnumerable<Person> GetAllPeople();
        void SavePerson(Person person);
        void UpdatePerson(Person person);
        void DeletePerson(Person person);
    }
}
