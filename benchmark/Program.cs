using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

WebHost.CreateDefaultBuilder(args)
    .Configure(c => c
        .UseRouting()
        .UseEndpoints(e => 
        {
            e.MapGet("/hello", async ctx =>
            {
                await ctx.Response.WriteAsync("Hello, World");
            });
        }))
    .Build()
    .Run();