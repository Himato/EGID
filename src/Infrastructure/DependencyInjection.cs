﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using EGID.Application.Common.Interfaces;
using EGID.Domain.Entities;
using EGID.Infrastructure.Auth;
using EGID.Infrastructure.Data;
using EGID.Infrastructure.Security;
using EGID.Infrastructure.Security.Cryptography;
using EGID.Infrastructure.Security.DigitalSignature;
using EGID.Infrastructure.Security.Hash;
using EGID.Infrastructure.Security.Jwt;
using EGID.Infrastructure.Time;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace EGID.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<EgidDbContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<IEgidDbContext>(provider => provider.GetService<EgidDbContext>());

            services.AddIdentity<Card, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = true;
                    options.Lockout.DefaultLockoutTimeSpan = new TimeSpan(0, 5, 0);
                })
                .AddEntityFrameworkStores<EgidDbContext>()
                .AddDefaultTokenProviders();

            // remove default claims
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var jwtSettings = new JwtSettings(
                configuration["Jwt:Key"],
                new TimeSpan(0, 30, 0),
                configuration["Jwt:Issuer"],
                configuration["Jwt:Issuer"]
            );

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(config =>
                {
                    config.RequireHttpsMetadata = false;
                    config.SaveToken = true;
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddSingleton(_ => jwtSettings);
            services.AddSingleton<IJwtTokenService, JwtTokenService>();

            services.AddTransient<IDateTime, UtcDateTime>();

            services.AddTransient<IKeysGeneratorService, KeysGeneratorService>();
            services.AddTransient<IDigitalSignatureService, DigitalSignatureService>();
            services.AddSingleton<ISymmetricCryptographyService>(_ =>
                new SymmetricCryptographyService(configuration["PrivateKey"]));
            services.AddSingleton<IHashService>(_ => new HashService(10000, 128));

            services.AddScoped<ICardManagerService, CardManagerService>();

            return services;
        }
    }
}