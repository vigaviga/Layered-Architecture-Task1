using Avro;
using Avro.Specific;

namespace Carting.Kafka.Models
{
    public class ItemUpdateEvent : ISpecificRecord
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public ItemUpdateEvent(int id, string name, double price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
        public ItemUpdateEvent() { }

        public Schema Schema => Schema.Parse(@"{
              ""doc"": ""Item whose data should be updated."",
              ""fields"": [
                {
                  ""name"": ""Id"",
                  ""type"": ""int""
                },
                {
                  ""name"": ""Name"",
                  ""type"": ""string""
                },
                {
                  ""name"": ""Price"",
                  ""type"": ""double""
                }
              ],
              ""name"": ""ItemToUpdate"",
              ""namespace"": ""Catalog.ApplicationModels"",
              ""type"": ""record""
            }");



        public object Get(int fieldPos)
        {
            switch (fieldPos)
            {
                case 0: return Id;
                case 1: return Name;
                case 2: return Price;
                default: throw new ArgumentException("Bad index " + fieldPos);
            }
        }

        public void Put(int fieldPos, object fieldValue)
        {
            switch (fieldPos)
            {
                case 0: Id = (int)fieldValue; break;
                case 1: Name = (string)fieldValue; break;
                case 2: Price = (double)fieldValue; break;
                default: throw new AvroRuntimeException("Bad index " + fieldPos);
            }
        }

    }
}
