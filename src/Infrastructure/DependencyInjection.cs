﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using EGID.Application;
using EGID.Common.Interfaces;
using EGID.Infrastructure.Auth;
using EGID.Infrastructure.Auth.Services;
using EGID.Infrastructure.Security;
using EGID.Infrastructure.Security.Cryptography;
using EGID.Infrastructure.Security.DigitalSignature;
using EGID.Infrastructure.Security.Hash;
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

            services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(connectionString));

            services.AddIdentity<Card, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = true;
                    options.Lockout.DefaultLockoutTimeSpan = new TimeSpan(0, 0, 5, 0);
                })
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            // remove default claims
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var jwtOpts = new JwtOptions(configuration);

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
                        // TODO: set expiration
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOpts.JwtKey)),
                        ValidIssuer = jwtOpts.JwtIssuer,
                        ValidAudience = jwtOpts.JwtIssuer,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddTransient<IDateTime, UtcDateTime>();

            services.AddTransient<IKeysGeneratorService, KeysGeneratorService>();
            services.AddTransient<IDigitalSignatureService, DigitalSignatureService>();
            services.AddSingleton<ISymmetricCryptographyService>(_ => new SymmetricCryptographyService(configuration["PrivateKey"]));
            services.AddSingleton<IHashService>(_ => new HashService(10000, 128));

            services.AddScoped<ICardManagerService, CardManagerService>();
            services.AddScoped<IRoleManagerService, RoleManagerService>();

            return services;
        }

        private class JwtOptions
        {
            public JwtOptions(IConfiguration config)
            {
                config.GetSection("JWT").Bind(this);
            }

            public string JwtKey { get; set; }
            public string JwtIssuer { get; set; }
            public string JwtExpireMinutes { get; set; }
        }
    }
}