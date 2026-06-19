using ecommerceAPI.src.EcommerceAPI.Application.DTOs;

namespace ecommerceAPI.src.EcommerceAPI.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync();
        Task<PaymentDto?> GetPaymentByIdAsync(Guid id);
        Task<PaymentDto> CreatePaymentAsync(CreatePaymentDto paymentDto);
        Task<bool> UpdatePaymentAsync(Guid id, UpdatePaymentDto paymentDto);
        Task<bool> DeletePaymentAsync(Guid id);
    }
}
