using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VegoCityManagment.Shared.Domain.Models;

namespace VegoCityManagment.Shared.Domain.VegoAPI
{
    internal class VegoAPI : IVegoAPI
    {
        private readonly HttpClient _httpClient;

        public VegoAPI()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://26.254.208.125:3000");
        }

        public async Task AddCategoryAsync(AddCategoryRequest addCategoryRequest)
            => await _httpClient.PostAsJsonAsync("managment/add-category", addCategoryRequest);

        public async Task AddProductAsync(AddProductRequest addProductRequest)
            => await _httpClient.PostAsJsonAsync("managment/add-product", addProductRequest);

        public async Task EditProductInfoAsync(EditEntityRequest editProductRequest)
            => await _httpClient.PutAsJsonAsync("managment/edit-product-info", editProductRequest);

        public async Task<CategoryResponse[]> FetchAllCategoriesAsync()
            => await _httpClient.GetFromJsonAsync<CategoryResponse[]>("categories/get-all");

        public async Task<ProductShortResponse[]> FetchAllProductsAsync()
            => await _httpClient.GetFromJsonAsync<ProductShortResponse[]>("products/get-all");

        public async Task<ProductDetailResponse> FetchProductDetailsAsync(int id)
            => await _httpClient.GetFromJsonAsync<ProductDetailResponse>($"products/get/{id}");

        public async Task<ProductShortResponse[]> FetchProductsWithFilterAsync(FilteredProductsRequest filteredProductsRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("products/get-with-filter", filteredProductsRequest);
            var jsonString = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;

            return JsonSerializer.Deserialize<ProductShortResponse[]>(jsonString, options);
        }
    }
}
