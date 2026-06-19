using Mapster;
using ecommerceAPI.src.EcommerceAPI.Application.DTOs;
using ecommerceAPI.src.EcommerceAPI.Application.Interfaces;
using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Domain.Repositories;

namespace ecommerceAPI.src.EcommerceAPI.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;

        public PaymentService(IPaymentRepository paymentRepository, IOrderRepository orderRepository)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
        }

        public async Task<PaymentDto> CreatePaymentAsync(CreatePaymentDto paymentDto)
        {
            if (!await _orderRepository.ExistsAsync(paymentDto.OrderId))
            {
                throw new ArgumentException("Invalid OrderId");
            }

            var payment = paymentDto.Adapt<Payment>();
            payment.Id = Guid.NewGuid();
            payment.PaymentDate = DateTime.UtcNow;
            payment.CreatedAt = DateTime.UtcNow;
            payment.UpdatedAt = DateTime.UtcNow;
            payment.IsActive = true;

            var createdPayment = await _paymentRepository.AddAsync(payment);
            return createdPayment.Adapt<PaymentDto>();
        }

        public async Task<bool> DeletePaymentAsync(Guid id)
        {
            if (!await _paymentRepository.ExistsAsync(id))
            {
                return false;
            }

            await _paymentRepository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();
            return payments.Adapt<IEnumerable<PaymentDto>>();
        }

        public async Task<PaymentDto?> GetPaymentByIdAsync(Guid id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            return payment == null ? null : payment.Adapt<PaymentDto>();
        }

        public async Task<bool> UpdatePaymentAsync(Guid id, UpdatePaymentDto paymentDto)
        {
            var existingPayment = await _paymentRepository.GetByIdAsync(id);
            if (existingPayment == null || !existingPayment.IsActive)
            {
                return false;
            }

            paymentDto.Adapt(existingPayment);
            existingPayment.UpdatedAt = DateTime.UtcNow;
            await _paymentRepository.UpdateAsync(existingPayment);
            return true;
        }
    }
}
