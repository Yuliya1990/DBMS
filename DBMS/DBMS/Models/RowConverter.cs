using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DBMS.Models
{
    public class RowConverter:JsonConverter<Row>
    {
        public override Row ReadJson(JsonReader reader, Type objectType, Row existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jArray = JArray.Load(reader);

            // Создайте список объектов, который будет содержать элементы строки
            var records = new List<object>();

            foreach (var jToken in jArray)
            {
                // Добавьте элементы строки в список
                records.Add(jToken.ToObject<object>());
            }

            return new Row(records);
        }
        public override void WriteJson(JsonWriter writer, Row value, JsonSerializer serializer)
        {
            var jArray = new JArray(value.Records);
            jArray.WriteTo(writer);
        }

    }
}
