using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VegoAPI.Domain.Models;

namespace VegoAPI.Domain
{
    public class VegoAPI : IVegoAPI
    {
        private const string IMAGE_API_KEY = "6d207e02198a847aa98d0a2a901485a5";
        private const string IMAGE_API_URL = "https://freeimage.host/api/1/upload";
        private readonly HttpClient _httpClient;

        public VegoAPI()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://26.254.208.125:3000")
            };
        }

        public async Task AddCategoryAsync(AddCategoryRequest addCategoryRequest)
            => await _httpClient.PostAsJsonAsync("managment/add-category", addCategoryRequest);

        public async Task<Guid> AddProductAsync(AddProductRequest addProductRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("managment/add-product", addProductRequest);

            var jsonString = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<ProductAddedResponse>(jsonString, options)
                .ProductId;
        }

        public async Task<Guid> AddProductPhotoAsync(AddProductPhotoRequest addProductPhotoRequest)
        {
            var form = new MultipartFormDataContent();

            var key = new StringContent(IMAGE_API_KEY);
            var action = new StringContent("upload");
            var format = new StringContent("json");
            var source = new StringContent(addProductPhotoRequest.Source);

            form.Add(key, "key");
            form.Add(action, "action");
            form.Add(format, "format");
            form.Add(source, "source");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var imageResponse = await _httpClient.PostAsync(IMAGE_API_URL, form);
            var jsonImageString = await imageResponse.Content.ReadAsStringAsync();
            var imageResponseObject = JsonSerializer.Deserialize<ImageApiResponse>(jsonImageString, options);

            var addPhotoRequest = new
            {
                ProductId = addProductPhotoRequest.ProductId,
                LowImagePath = imageResponseObject.image.file.resource.chain.medium,
                HighImagePath = imageResponseObject.image.file.resource.chain.image
            };

            var idResponse = await _httpClient.PostAsJsonAsync("managment/add-product-photo", addPhotoRequest);
            var idResponseJsonString = await idResponse.Content.ReadAsStringAsync();
            var idResponseObject = JsonSerializer.Deserialize<PhotoAddedResponse>(idResponseJsonString, options);

            return idResponseObject.PhotoId;
        }

        public Task DeleteProductPhotoAsync()
        {
            throw new NotImplementedException();
        }

        public async Task EditCategoryInfoAsync(EditEntityWithIntIdRequest editCategoryRequest)
            => await _httpClient.PutAsJsonAsync("managment/edit-category", editCategoryRequest);

        public async Task EditProductInfoAsync(EditEntityWithGuidRequest editProductRequest)
            => await _httpClient.PutAsJsonAsync("managment/edit-product-info", editProductRequest);

        public async Task<CategoryResponse[]> FetchAllCategoriesAsync()
            => await _httpClient.GetFromJsonAsync<CategoryResponse[]>("categories/get-all");

        public async Task<ProductShortResponse[]> FetchAllProductsAsync()
            => await _httpClient.GetFromJsonAsync<ProductShortResponse[]>("products/get-all");

        public async Task<CategoryResponse> FetchCategoryAsync(int id)
            => await _httpClient.GetFromJsonAsync<CategoryResponse>($"categories/get/{id}");

        public async Task<ProductDetailResponse> FetchProductDetailsAsync(Guid id)
            => await _httpClient.GetFromJsonAsync<ProductDetailResponse>($"products/get/{id}");

        public async Task<ProductShortResponse[]> FetchProductsWithFilterAsync(FilteredProductsRequest filteredProductsRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("products/get-with-filter", filteredProductsRequest);
            var jsonString = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<ProductShortResponse[]>(jsonString, options);
        }








        public async Task LoadProductMainPhotoAsync(LoadProductPhotoRequest loadProductPhotoRequest)
        {
            var form = new MultipartFormDataContent();

            

            var productId = new StringContent($"{loadProductPhotoRequest.ProductId}");
            //productId.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            //{
            //    Name = "ProductId",
            //};
            //
            var photoFile = new ByteArrayContent(loadProductPhotoRequest.PhotoBytes);
            //photoFile.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            //{
            //    Name = "ImageFile",
            //};

            form.Add(productId, "ProductId");
            form.Add(photoFile, "ImageFile", "name");

            await _httpClient.PostAsync("managment/load-product-main-photo", form);
        }

        public async Task SetProductMainPhotoAsync(SetProductMainPhotoRequest setProductMainPhotoRequest)
            => _httpClient.PostAsJsonAsync("managment/set-product-main-photo", setProductMainPhotoRequest);
    }
}
