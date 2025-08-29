
using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using ecommerceAPI.src.EcommerceAPI.Domain.Repositories;
using ecommerceAPI.src.EcommerceAPI.Persistence.Data;
using Microsoft.EntityFrameworkCore;


namespace ecommerceAPI.src.EcommerceAPI.Persistence.Repositories
{

    public class UserRepository : IUserRepository
    {
        private readonly EcommerceDbContext _db;

        public UserRepository(EcommerceDbContext context)
        {
            _db = context;
        } 
 
    }

}