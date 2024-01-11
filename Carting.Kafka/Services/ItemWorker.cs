using Carting.Carting.Domain.Models;
using Carting.Kafka.Models;
using Carting.Shared.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace Carting.Kafka.Services
{
    public class ItemWorker : BackgroundService
    {
        private IConsumer<string, ItemUpdateEvent> _consumer;
        private HttpClient _httpClient;
        private string topicName = "ItemsUpdates";
        private string itemUpdatePath = "items";
        public ItemWorker(IConsumer<string, ItemUpdateEvent> consumer, IHttpClientFactory httpClientFactory) 
        {
            _consumer = consumer;
            _httpClient = httpClientFactory.CreateClient(HttpNamedClients.CartingClient);

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(topicName);

            while (!stoppingToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(stoppingToken);
                await HandleMessage(result.Message.Value, stoppingToken);
                _consumer.StoreOffset(result);
                _consumer.Commit();
            }
        }

        private async Task HandleMessage(ItemUpdateEvent item, CancellationToken stoppingToken)
        {
            var itemUpdateInfo = new ItemUpdateInfo()
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
            };
            var httpContent = JsonContent.Create(itemUpdateInfo);

            var response = await _httpClient.PutAsync("items", httpContent);
            await Task.CompletedTask;
        }
    }
}
