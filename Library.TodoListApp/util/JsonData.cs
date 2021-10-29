// Miguel Quezada
// Assignment #2
// 10/02/2021
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Library.TodoListApp.Models;

namespace Library.TodoListApp.util
{
    public static class JsonData
    {
        private static readonly StorageFolder LocalFolder = ApplicationData.Current.LocalFolder;
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All};
        public static async Task<List<TodoItem>> Load()
        {
            try
            {
                var folder = await LocalFolder.CreateFolderAsync("TodoListApp", CreationCollisionOption.OpenIfExists);
                var file = await folder.GetFileAsync("data.json");
                var json = await FileIO.ReadTextAsync(file);
                var jsonObj = JsonConvert.DeserializeObject<List<TodoItem>>(json, Settings);
                return jsonObj;
            }
            catch
            {
                return new List<TodoItem>();
            }
        }
        public static async void Save(ObservableCollection<TodoItem> items)
        {
            var folder = await LocalFolder.CreateFolderAsync("TodoListApp", CreationCollisionOption.OpenIfExists);
            var item = await folder.TryGetItemAsync("data.json");
            var file = item == null
                ? await folder.CreateFileAsync("data.json")
                : await folder.GetFileAsync("data.json");
            await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(items.ToList(), Settings));
        }
    }
}
