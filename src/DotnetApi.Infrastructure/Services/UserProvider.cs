﻿using System;
using System.Security.Claims;
using DotnetApi.Common.Interfaces;
using DotnetApi.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DotnetApi.Infrastructure.Services
{
    public class UserProvider(
        IHttpContextAccessor httpContextAccessor, 
        DotnetApiDbContext dbContext) : IUserProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
        private readonly DotnetApiDbContext dbContext = dbContext;

        public async Task<string> GetCurrentUserIdAsync()
        {
            if (httpContextAccessor.HttpContext == null)
            {
                throw new ApplicationException("HttpContext not available");
            }

            var userClaimes = httpContextAccessor.HttpContext.User.Claims;
            var userId = userClaimes.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                throw new UnauthorizedAccessException($"Invalid token. {nameof(ClaimTypes.NameIdentifier)} claim not found.");
            }

            var user = await dbContext.Users.FirstOrDefaultAsync(user => user.ExternalId == userId.Value);

            if (user is null)
            {
                throw new UnauthorizedAccessException($"Invalid token. User with id {userId.Value} not found.");
            }

            return user.Id;
        }

        public async Task<string?> GetCurrentUserIdOrDefaultAsync()
        {
            if (httpContextAccessor.HttpContext == null)
                return null;

            var userClaimes = httpContextAccessor.HttpContext.User.Claims;
            var userId = userClaimes.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);

            if (userId is null)
                return null;

            var user = await dbContext.Users.FirstOrDefaultAsync(user => user.ExternalId == userId.Value);

            if (user is null)
                return null;

            return user.Id;
        }
    }
}

