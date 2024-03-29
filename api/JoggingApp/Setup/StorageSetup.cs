﻿using JoggingApp.Core;
using JoggingApp.Core.Jogs;
using JoggingApp.Core.Users;
using JoggingApp.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JoggingApp.Setup
{
    public static class StorageSetup
    {
        public static void AddStorage(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<JoggingAppDbContext>(context => context.UseSqlServer(builder.Configuration["ConnectionString:DefaultConnection"]));
            builder.Services.AddScoped<IUserStorage, UserStorage>();
            builder.Services.AddScoped<IJogStorage, JogStorage>();
        }
    }
}
