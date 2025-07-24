using DevIO.Business.Interfaces;
using DevIO.Business.Models;

namespace DevIO.Business.Services
{
    public class SupplierService : BaseService, ISupplierService
    {
        public Task<IEnumerable<Supplier>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Supplier> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task Add(Supplier supplier)
        {
            throw new NotImplementedException();
        }
        public Task Update(Supplier supplier)
        {
            throw new NotImplementedException();
        }

        public Task Remove(Guid id)
        {
            throw new NotImplementedException();
        }
        public Task UpdateAddress(Address address)
        {
            throw new NotImplementedException();
        }
    }
}
