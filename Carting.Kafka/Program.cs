// See https://aka.ms/new-console-template for more information
using Carting.Kafka.Models;
using Carting.Kafka.Services;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Carting.Kafka
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                        .ConfigureServices((hostContext, services) =>
                        {
                            services.Configure<ConsumerConfig>(hostContext.Configuration.GetRequiredSection("KafkaConsumer"));
                            services.Configure<SchemaRegistryConfig>(hostContext.Configuration.GetRequiredSection("SchemaRegistry"));

                            services.AddSingleton<ISchemaRegistryClient>(sp =>
                            {
                                var schemaConfig = sp.GetRequiredService<IOptions<SchemaRegistryConfig>>();
                                return new CachedSchemaRegistryClient(schemaConfig.Value);
                            });

                            services.AddSingleton(sp =>
                            {
                                var config = sp.GetRequiredService<IOptions<ConsumerConfig>>();
                                var schemaRegistry = sp.GetRequiredService<ISchemaRegistryClient>();

                                return new ConsumerBuilder<string, ItemUpdateEvent>(config.Value)
                                       .SetValueDeserializer(new AvroDeserializer<ItemUpdateEvent>(schemaRegistry).AsSyncOverAsync())
                                       .Build();
                            });

                            services.AddHostedService<ItemWorker>();
                        }).Build();

            await host.RunAsync();
        }
    }
}
