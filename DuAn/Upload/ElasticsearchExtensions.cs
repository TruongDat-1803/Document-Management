using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upload.Models;

namespace Upload
{
    public static class ElasticsearchExtensions
    {
        public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IElasticClient>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();

                // Lấy thông tin từ cấu hình
                var cloudId = config["elasticsearch:cloudId"]; // Cloud ID từ Elasticsearch Cloud
                var username = config["elasticsearch:user"]; // Tên người dùng
                var password = config["elasticsearch:password"]; // Mật khẩu
                var defaultIndex = config["elasticsearch:index"]; // Tên chỉ mục mặc định

                // Kiểm tra nếu thiếu thông tin
                if (string.IsNullOrEmpty(cloudId) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    throw new ArgumentException("Elasticsearch configuration is missing required fields (cloudId, user, password).");
                }

                // Cấu hình client
                var settings = new ConnectionSettings(cloudId, new BasicAuthenticationCredentials(username, password))
                    .DefaultIndex(defaultIndex) // Chỉ định chỉ mục mặc định
                    .DefaultMappingFor<File>(m => m.IndexName(defaultIndex)); // Mapping mặc định cho File

                return new ElasticClient(settings);
            });
        }

        private static void AddDefaultMappings(ConnectionSettings settings)
        {
            settings.
                DefaultMappingFor<File>(m => m
                .Ignore(p => p.TenantID)
            );
        }

        private static void CreateIndex(IElasticClient client, string indexName)
        {
            var createIndexResponse = client.Indices.Create(indexName,
                index => index.Map<File>(x => x.AutoMap())
            );
        }
    }
}
