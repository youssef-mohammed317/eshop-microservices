using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(DiscountContext _dbContext, ILogger<DiscountService> _logger)
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _dbContext.Coupons
            .FirstOrDefaultAsync(c => c.ProductName == request.ProductName)
            ?? new Coupon
            {
                ProductName = "No Discount",
                Amount = 0,
                Description = "No Discount Description"
            };

        // Fixed the missing semicolon here
        _logger.LogInformation("Discount retrieved for ProductName: {ProductName}, Amount: {Amount}", coupon.ProductName, coupon.Amount);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon == null)
        {
            _logger.LogWarning("CreateDiscount failed: Invalid request payload.");
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request payload."));
        }

        await _dbContext.Coupons.AddAsync(coupon);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Discount successfully created. ProductName: {ProductName}, Amount: {Amount}", coupon.ProductName, coupon.Amount);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon == null)
        {
            _logger.LogWarning("UpdateDiscount failed: Invalid request payload.");
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request payload."));
        }

        _dbContext.Coupons.Update(coupon);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Discount successfully updated. ProductName: {ProductName}", coupon.ProductName);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _dbContext.Coupons
             .FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

        if (coupon == null)
        {
            _logger.LogWarning("DeleteDiscount failed: Discount with ProductName = {ProductName} was not found.", request.ProductName);

            // Fixed the exception message to actually include the product name
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName = {request.ProductName} was not found."));
        }

        _dbContext.Coupons.Remove(coupon);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Discount successfully deleted for ProductName: {ProductName}", request.ProductName);

        return new DeleteDiscountResponse { Success = true };
    }
}