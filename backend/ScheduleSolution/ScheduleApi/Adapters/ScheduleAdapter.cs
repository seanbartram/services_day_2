﻿using ScheduleApi.Controllers;
using System.Text.Json;

namespace ScheduleApi.Adapters;

public class ScheduleAdapter
{
    private readonly Dictionary<string, List<ScheduleItem>> _items;

    public ScheduleAdapter()
    {
        _items = new Dictionary<string, List<ScheduleItem>>();
        var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Schedule", "schedule.json");
        using var sr = new StreamReader(file);

        string json = sr.ReadToEnd();
        var items = JsonSerializer.Deserialize<List<StoredScheduleItem>>(json);
        if (items != null) {
            foreach (var item in items)
            {
                if (!_items.ContainsKey(item.Id))
                {
                    _items.Add(item.Id, new List<ScheduleItem>());
                }
                var newItem = new ScheduleItem
                {
                    StartDate = DateTime.Parse(item.StartDate),
                    EndDate = DateTime.Parse(item.EndDate)
                };
                _items[item.Id].Add(newItem);
            }
        }
    }

    //public async Task<Dictionary<string, List<ScheduleItem>>> GetScheduleAsync()
    //{
    //    return _items;
    //}

    public async Task<List<ScheduleItem>>? GetForClass(string id)
    {
        return _items[id];
    }



public class StoredScheduleItem
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string StartDate { get; set; } = string.Empty;
    public string EndDate { get; set; } = string.Empty;
}
