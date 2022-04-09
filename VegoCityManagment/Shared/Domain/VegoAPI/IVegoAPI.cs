using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VegoCityManagment.Shared.Domain.Models;

namespace VegoCityManagment.Shared.Domain.VegoAPI
{
    public interface IVegoAPI
    {
        Task<ProductShortResponse[]> FetchAllProductsAsync();
        Task<ProductShortResponse[]> FetchProductsWithFilterAsync(FilteredProductsRequest filteredProductsRequest);
        Task<CategoryResponse[]> FetchAllCategoriesAsync();
        Task<ProductDetailResponse> FetchProductDetailsAsync(int id);
        Task<CategoryResponse> FetchCategoryAsync(int id);
        Task AddProductAsync(AddProductRequest addProductRequest);
        Task AddCategoryAsync(AddCategoryRequest addCategoryRequest);
        Task EditProductInfoAsync(EditEntityRequest editProductRequest);
    }
}
