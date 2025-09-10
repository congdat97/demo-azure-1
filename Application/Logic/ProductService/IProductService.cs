using Application.Dto;
using Domain.Contracts;

namespace Application.Logic.ProductService;

public interface IProductService
{
    Task<int> AddProductAsync(ProductDto productDto, CancellationToken cancellationToken = default);
}