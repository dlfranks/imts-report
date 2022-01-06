using System;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace API.Helper
{
    public class TimeSpanToStringConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value=reader.GetString();
            return TimeSpan.Parse(value);
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }

    public class DataTableConverter : JsonConverter<DataTable>
    {
        public override DataTable Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, DataTable value,
            JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (DataRow row in value.Rows)
            {
                writer.WriteStartObject();
                foreach (DataColumn column in row.Table.Columns)
                {
                    object columnValue = row[column];

                    // If necessary:
                    if (options.IgnoreNullValues)
                    {
                        // Do null checks on the values here and skip writing.
                    }

                    writer.WritePropertyName(column.ColumnName);
                    JsonSerializer.Serialize(writer, columnValue, options);
                }
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }
    }

    public class DataSetConverter : JsonConverter<DataSet>
    {
        public override DataSet Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, DataSet value,
            JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            foreach (DataTable table in value.Tables)
            {
                writer.WritePropertyName(table.TableName);
                JsonSerializer.Serialize(writer, table, options);
            }
            writer.WriteEndObject();
        }
    }
}