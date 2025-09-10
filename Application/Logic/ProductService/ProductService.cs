using Application.Dto;
using AutoMapper;
using Domain.Contracts;
using Infrastructure.GenericRepository;
using Infrastructure.UnitOfWork;

namespace Application.Logic.ProductService;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork; 
    private readonly IMapper _mapper;
    
    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper     = mapper;
    }
    
    public async Task<int> AddProductAsync(ProductDto productDto, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var product = _mapper.Map<Product>(productDto);
            product.CreatedAt = DateTime.UtcNow;

            var productRepo = _unitOfWork.GetRepository<Product>();
            int result = await productRepo.AddAsync(product, cancellationToken);

            if (result > 0)
            {
                await _unitOfWork.CommitAsync(cancellationToken);
            }
            else
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
            }

            return result;
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }


}