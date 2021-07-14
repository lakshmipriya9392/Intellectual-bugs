﻿using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace TrainingLab.Extensions
{
    public static class DistributedCacheExtensions
    {
        //Setting data in cache
        public static async Task SetRecordAsync<T> (this IDistributedCache cache,string recordId,T data,TimeSpan? absoluteExpireTime=null,TimeSpan? unusedExpireTime=null)
        {
            try
            {
                var options = new DistributedCacheEntryOptions();
                options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
                options.SlidingExpiration = unusedExpireTime;
                var jsonData = JsonSerializer.Serialize(data);
                await cache.SetStringAsync(recordId, jsonData, options);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        //Getting data from cache
        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
        {
            try
            {
                var jsonData = await cache.GetStringAsync(recordId);
                if (jsonData is null)
                {
                    return default(T);
                }

                return JsonSerializer.Deserialize<T>(jsonData);
            }
            catch(Exception e)
            {
                return default(T);
            }
        }
    }
}
