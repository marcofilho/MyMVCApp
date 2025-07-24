using DevIO.Business.Models;

namespace DevIO.Business.Interfaces
{
    public interface IProductService : IDisposable
    {
        Task Add(Product product);
        Task Update(Product product);
        Task Remove(Guid id);
        Task<Product> GetById(Guid id);
        Task<IEnumerable<Product>> GetAll();

        public Task<IEnumerable<Product>> GetProductsSuppliers(Guid id);
        public Task<IEnumerable<Product>> GetProductsSuppliers();
        public Task<Product> GetProductSupplier(Guid supplierId);
    }
}
