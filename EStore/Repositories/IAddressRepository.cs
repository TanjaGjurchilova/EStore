using EStore.Models.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Repositories
{
    interface IAddressRepository
    {
        Address FindAddressById(long id);
        IEnumerable<Address> GetAllAddresss();
        void SaveAddress(Address Address);
        void UpdateAddress(Address Address);
        void DeleteAddress(Address Address);
    }
}
