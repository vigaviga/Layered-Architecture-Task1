using Carting.Kafka.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;

namespace Carting.Kafka.Services
{
    public class ItemWorker : BackgroundService
    {
        private IConsumer<string, ItemUpdateEvent> _consumer;
        private string topicName = "ItemsUpdates";
        public ItemWorker(IConsumer<string, ItemUpdateEvent> consumer) 
        {
            _consumer = consumer;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(topicName);

            while(!stoppingToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(stoppingToken);
                await HandleMessage(result.Message.Value, stoppingToken);
                _consumer.StoreOffset(result);
                _consumer.Commit();
            }
        }

        private async Task HandleMessage(ItemUpdateEvent item, CancellationToken stoppingToken)
        {
            Console.WriteLine(item.Name);
            await Task.CompletedTask;
        }
    }
}
