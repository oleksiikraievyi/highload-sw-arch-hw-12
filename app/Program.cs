using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Beanstalk.Core;
using StackExchange.Redis;

const string testTubeName = "test";
const string testRdbKey = "testRdbKey";
const string testAofKey = "testAofKey";

var rdbRedisConnectionMultiplexer = await ConnectionMultiplexer.ConnectAsync("redis-rdb:6379");
var aofRedisConnectionMultiplexer = await ConnectionMultiplexer.ConnectAsync("redis-aof:6379");

WebHost.CreateDefaultBuilder(args)
    .Configure(c => c
        .UseRouting()
        .UseEndpoints(e => 
        {
            e.MapGet("/beanstalkd/produce", async ctx =>
            {
                using var client = new BeanstalkConnection("beanstalkd", 11300);

                await client.Use(testTubeName);
                await client.Put("Test message");
            });

            e.MapGet("/beanstalkd/consume", async ctx => 
            {
                using var client = new BeanstalkConnection("beanstalkd", 11300);

                await client.Watch(testTubeName);

                var message = await client.Reserve(TimeSpan.FromMinutes(5));
                await client.Delete(message.Id);

                await ctx.Response.WriteAsync(message.Data);
            });

            e.MapGet("/redis-rdb/produce", async ctx =>
            {
                var db = rdbRedisConnectionMultiplexer.GetDatabase();
                await db.ListRightPushAsync(testRdbKey, "testMessage");
            });

            e.MapGet("/redis-rdb/consume", async ctx =>
            {
                var db = rdbRedisConnectionMultiplexer.GetDatabase();
                var message = await db.ListRightPopAsync(testRdbKey);

                await ctx.Response.WriteAsync(message);
            });

            e.MapGet("/redis-aof/produce", async ctx =>
            {
                var db = rdbRedisConnectionMultiplexer.GetDatabase();
                await db.ListRightPushAsync(testAofKey, "testMessage");
            });

            e.MapGet("/redis-aof/consume", async ctx =>
            {
                var db = rdbRedisConnectionMultiplexer.GetDatabase();
                var message = await db.ListRightPopAsync(testAofKey);

                await ctx.Response.WriteAsync(message);
            });
        }))
    .Build()
    .Run();