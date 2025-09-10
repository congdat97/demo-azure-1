using Application.Dto;
using Domain.Contracts;

namespace Application.Logic.CategoryService;

public interface ICategoryService
{
    Task<int> Create(CategoryDto categoryDto);
    Task CreateTestTransaction(CategoryDto categoryDto, ProductDto productDto, CancellationToken cancellationToken = default);
    Task<int> AddCategoryAsync(CategoryDto categoryDto, CancellationToken cancellationToken = default); 
}