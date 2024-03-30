﻿namespace Auth.Mongo.Models
{
    public class UsersDatabaseSettings
    {
        public string? ConnectionString { get; set; } = null;
        public string? DatabaseName { get; set; } = null;
        public string? CollectionName { get; set; } = null;
    }
}
