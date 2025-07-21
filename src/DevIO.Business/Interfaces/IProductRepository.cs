using DevIO.Business.Models;

namespace DevIO.Business.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        public Task<IEnumerable<Product>> GetProductsBySupplier(Guid id);
        public Task<IEnumerable<Product>> GetProductsSuppliers();
        public Task<Product> GetProductSupplier(Guid supplierID);
    }
}
