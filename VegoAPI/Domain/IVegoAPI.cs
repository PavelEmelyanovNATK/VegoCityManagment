using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VegoAPI.Domain.Models;

namespace VegoAPI.Domain
{
    public interface IVegoAPI
    {
        Task<ProductShortResponse[]> FetchAllProductsAsync();
        Task<ProductShortResponse[]> FetchProductsWithFilterAsync(FilteredProductsRequest filteredProductsRequest);
        Task<CategoryResponse[]> FetchAllCategoriesAsync();
        Task<ProductDetailResponse> FetchProductDetailsAsync(Guid id);
        Task<CategoryResponse> FetchCategoryAsync(int id);
        Task<Guid> AddProductAsync(AddProductRequest addProductRequest);
        Task AddCategoryAsync(AddCategoryRequest addCategoryRequest);
        Task EditProductInfoAsync(EditEntityWithGuidRequest editProductRequest);
        Task EditCategoryInfoAsync(EditEntityWithIntIdRequest editCategoryRequest);
        Task<Guid> AddProductPhotoAsync(AddProductPhotoRequest addProductPhotoRequest);
        Task SetProductMainPhotoAsync(SetProductMainPhotoRequest setProductMainPhotoRequest);
        Task DeleteProductPhotoAsync();
    }
}
