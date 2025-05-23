﻿using Dapper;
using Discount.gRPC.Entities;
using Npgsql;

namespace Discount.gRPC.Repository
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            NpgsqlConnection connection = GetConnectionPostgreSQL();

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

            if (coupon == null)
                return new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };

            return coupon;
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            NpgsqlConnection connection = GetConnectionPostgreSQL();

            var affected = await connection.ExecuteAsync("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)", 
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

            return affected > 0;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            NpgsqlConnection connection = GetConnectionPostgreSQL();

            var affected = await connection.ExecuteAsync("UPDATE Coupon SET ProductName = @ProductName, Description = @Description, Amount = @Amount",
              new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

            return affected > 0;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            NpgsqlConnection connection = GetConnectionPostgreSQL();

            var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

            return affected > 0;
        }

        private NpgsqlConnection GetConnectionPostgreSQL()
        {
            return new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        }
    }
}
