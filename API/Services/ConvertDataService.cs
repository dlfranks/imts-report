using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Serialization;
using API.Helper;
using OfficeOpenXml;

namespace API.Services
{
    public static class ConvertDataService
    {
        public static DataTable ConvertToTable<T>(List<T> data, PropertyDescriptorCollection props, PropertyDescriptorCollection rowProps)
        {
            DataTable table = new DataTable();
            //PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(FieldConcreteTestFlattenJsonDataset));
            //PropertyDescriptorCollection rowProps = TypeDescriptor.GetProperties(typeof(FieldConcreteTestRowJsonDataset));
            int totalProps = (props.Count - 1) + rowProps.Count;
            object[] propNames = new object[totalProps];

            int propCount = 0;
            for (int i = 0; i < props.Count - 1; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                propNames[propCount] = prop.Name;
                propCount++;
            }
            for (int j = 0; j < rowProps.Count; j++)
            {
                PropertyDescriptor prop = rowProps[j];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                propNames[propCount] = prop.Name;
                propCount++;
            }

            foreach (var test in data)
            {
                var actualRow = table.NewRow();
                var copyRow = table.NewRow();

                int valueCount = 0;

                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];

                    if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        dynamic testRows = prop.GetValue(test) ?? DBNull.Value;
                        Type type = prop.PropertyType.GetGenericArguments()[0];
                        if (testRows is IEnumerable && testRows.Count > 0)
                        {
                            var tRows = Enumerable.ToList<dynamic>(testRows);
                            int tRowCount = 0;
                            foreach (var r in tRows)
                            {
                                if (tRowCount != 0)
                                {
                                    var cloneRow = table.NewRow();
                                    cloneRow.ItemArray = copyRow.ItemArray;

                                    for (int j = 0; j < rowProps.Count; j++)
                                    {
                                        PropertyDescriptor rProp = rowProps[j];

                                        cloneRow[rProp.Name] = rProp.GetValue(r) ?? DBNull.Value;
                                        valueCount++;
                                    }
                                    table.Rows.Add(cloneRow);

                                }
                                else
                                {
                                    for (int j = 0; j < rowProps.Count; j++)
                                    {
                                        PropertyDescriptor rProp = rowProps[j];

                                        actualRow[rProp.Name] = rProp.GetValue(r) ?? DBNull.Value;
                                        valueCount++;
                                    }
                                    table.Rows.Add(actualRow);
                                }
                                tRowCount++;
                            }
                        }
                        else
                        {
                            table.Rows.Add(actualRow);
                        }
                    }
                    else
                    {
                        actualRow[prop.Name] = prop.GetValue(test) ?? DBNull.Value;
                        copyRow[prop.Name] = prop.GetValue(test) ?? DBNull.Value;
                        valueCount++;
                    }
                }
            }
            return table;
        }

        public static byte[] FromTableToExcel(DataTable table)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage package = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet ws = package.Workbook.Worksheets.Add("Concrete");
                ws.Cells.LoadFromDataTable(table, true);
                var dPos = new List<int>();
                for (var i = 0; i < table.Columns.Count; i++)
                    if (table.Columns[i].DataType.Name.Equals("DateTime"))
                        dPos.Add(i);
                foreach (var pos in dPos)
                {
                    ws.Column(pos + 1).Style.Numberformat.Format = "mm/dd/yyyy";
                }
                package.Save();
            }

            return memoryStream.ToArray();
        }

        public static byte[] FromListToXml<T>(List<T> data)
        {
            //const string xmlFilename = "FitTClassics.xml";
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));
            MemoryStream writer = new MemoryStream();
            //var file = new StreamWriter(xmlFilename);

            //xmlSerializer.Serialize(file, data);
            xmlSerializer.Serialize(writer, data);
            return writer.ToArray();



        }

        public static void SerializeToXml<T>(T anyobject, string xmlFilePath)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(anyobject.GetType());

            using (StreamWriter writer = new StreamWriter(xmlFilePath))
            {
                xmlSerializer.Serialize(writer, anyobject);
            }
        }

        public static string DataTableToJSON(DataTable table)
        {
            var options = new JsonSerializerOptions()
            {
                Converters = { new DataTableConverter(), new DataSetConverter(), new TimeSpanToStringConverter() }
            };

            string jsonDataTable = JsonSerializer.Serialize(table, options);

            return jsonDataTable;
        }


    }
}