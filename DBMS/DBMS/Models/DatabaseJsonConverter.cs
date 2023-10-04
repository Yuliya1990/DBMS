using DBMS.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DBMS.Models
{
    public static class DatabaseJsonConverter
    {
        public static void SaveDatabaseTo(string path, DataBase database)
        {
            var databaseDto = new DatabaseDto
            {
                Name = database.Name,
                Tables = database.Tables.Select(table =>
                    new TableDto
                    {
                        Columns = table.Attributes.ToList(),
                        Name = table.Name,
                        Rows = table.Rows.ToList()
                    }).ToList()
            };
            string jsonDatabase = JsonConvert.SerializeObject(databaseDto);
            File.WriteAllText(Path.Combine(path, $"{database.Name}.json"), jsonDatabase);
        }

        public static DataBase GetDatabaseFrom(string path)
        {
            if (!path.EndsWith(".json") || !File.Exists(path))
            {
                throw new ArgumentException("Such database does not exist");
            }
            string jsonFromFile = File.ReadAllText(path);
            DatabaseDto deserializedDatabase = JsonConvert.DeserializeObject<DatabaseDto>(jsonFromFile);
            DataBase database = new DataBase(deserializedDatabase.Name);
            foreach (var tableDto in deserializedDatabase.Tables)
            {
                var table = new Table(tableDto.Name, tableDto.Columns);
                foreach (var row in tableDto.Rows)
                {
                    table.AddRow(row);
                }
                database.Tables.Add(table);
            }
            return database;
        }
    }
}
