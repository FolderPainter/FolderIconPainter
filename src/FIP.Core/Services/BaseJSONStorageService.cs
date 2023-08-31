using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FIP.Core.Services
{
    public abstract class BaseJSONStorageService
    {
        private string _filePath;

        public JsonSerializerOptions JsonSerializerOptions { get; set; }

        protected BaseJSONStorageService ()
        {
            JsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };
        }

        public virtual void Initialize(string filePath)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                if (!File.Exists(filePath))
                {
                    // write empty json object to file
                    File.WriteAllText(filePath, "[]");
                }
                _filePath = filePath;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual IEnumerable<TValue> GetAllValues<TValue>() 
        {
            try
            {
                string jsonString = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<IEnumerable<TValue>>(jsonString, JsonSerializerOptions);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual bool SetAllValues<TValue>(IEnumerable<TValue> values)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(values, JsonSerializerOptions);
                if (!String.IsNullOrEmpty(jsonString))
                {
                    File.WriteAllText(_filePath, jsonString);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
