namespace Shopping.Web.Services;

public interface IOrderingService
{
    [Get("/ordering-api/orders?pageIndex={pageIndex}&pageSize={pageSize}")]
    Task<GetOrdersResponse> GetOrders(int? pageIndex = 1, int? pageSize = 10);

    [Get("/ordering-api/orders/{orderName}")]
    Task<GetOrdersByNameResponse> GetOrdersByName(string orderName);

    [Get("/ordering-api/orders/customer/{customerId}")]
    Task<GetOrdersByCustomerResponse> GetOrdersByCustomer(Guid customerId);
}
