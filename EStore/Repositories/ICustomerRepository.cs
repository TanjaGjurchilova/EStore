using EStore.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Repositories
{
    interface ICustomerRepository
    {
        Customer FindCustomerById(long id);
        IEnumerable<Customer> GetAllCustomers();
        void SaveCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
    }
}
