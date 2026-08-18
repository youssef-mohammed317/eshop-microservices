namespace Shopping.Web.Services;

public interface ICatalogService
{
    [Get("/catalog-api/products?pageNumber={pageNumber}&pageSize={pageSize}")]
    Task<GetProductsResponse> GetProducts(int? pageNumber = 1, int? pageSize = 10);
    
    [Get("/catalog-api/products/{id}")]
    Task<GetProductByIdResponse> GetProduct(Guid id);
    
    [Get("/catalog-api/products/category/{category}")]
    Task<GetProductByCategoryResponse> GetProductsByCategory(string category);
}
