using Application.Dto;
using Application.Logic.ProductService;
using AutoMapper;
using Domain.Contracts;
using Infrastructure.GenericRepository;
using Infrastructure.UnitOfWork;
using System.Threading;

namespace Application.Logic.CategoryService;

public class CategoryService : ICategoryService
{
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IProductService _service;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IProductService service)
    {
        _unitOfWork = unitOfWork;
        _mapper     = mapper;
        _service = service;
    }

    public async Task<int> AddCategoryAsync(CategoryDto categoryDto, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var category = _mapper.Map<Category>(categoryDto);
            category.CreatedAt = DateTime.UtcNow;

            var caterogyRepo = _unitOfWork.GetRepository<Category>();
            int result = await caterogyRepo.AddAsync(category, cancellationToken);

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

    public async Task<int> Create(CategoryDto categoryDto)
    {
        var category       = _mapper.Map<Category>(categoryDto);

        var categoryRepo   = _unitOfWork.GetRepository<Category>();

        category.CreatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        int result         = await categoryRepo.AddAsync(category);

        if (result > 0) { await _unitOfWork.CommitAsync(); }

        return result;
    }


    public async Task CreateTestTransaction(CategoryDto categoryDto, ProductDto productDto, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var category = _mapper.Map<Category>(categoryDto);
            var product = _mapper.Map<Product>(productDto);

            var categoryRepo = _unitOfWork.GetRepository<Category>();
            var productRepo = _unitOfWork.GetRepository<Product>();

            category.CreatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            var resultCate = await categoryRepo.AddAsync(category);

            product.CreatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            var resultPro = await productRepo.AddAsync(product);

            if (resultPro < 0 || resultCate < 0)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
            }
            
            await _unitOfWork.CommitAsync();

        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}