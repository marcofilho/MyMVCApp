using AutoMapper;
using DevIO.App.ViewModels;
using DevIO.Business.Models;

namespace DevIO.App.Mappers
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Supplier, SupplierViewModel>();
            CreateMap<Address, AddressViewModel>();
            CreateMap<Product, ProductViewModel>();
        }
    }
}
