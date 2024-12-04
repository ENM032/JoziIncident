using System;
using System.ComponentModel.DataAnnotations;

namespace ST10091324_PROG7312_Part1.Model
{
    public class ServiceRequest
    {
        // Static counter to generate incremental IDs
        public static int _counter = 0;  

        // Lock object to ensure thread safety
        private static readonly object _lock = new object();

        [Key]
        public string RequestID { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public double Progress { get; set; }
        // Use to emulate the progress bar constantly spinning
        public bool IsIndeterminate { get; set; }

        // New property to indicate if this is a search result or related request
        public string SearchRequestType { get; set; }

        // Foreign key to User
        public Guid UserId { get; set; }
        public User User { get; set; }  // Navigation property

        // Parameterless constructor for Entity Framework
        public ServiceRequest() { }

        // Constructor for easy initialization
        public ServiceRequest(string description, string status, double progress, bool isIndeterminate, Guid userId)
        {
            RequestID = GenerateUniqueID();  // Generate unique ID for each request
            Description = description;
            Status = status;
            Progress = progress;
            IsIndeterminate = isIndeterminate;
            UserId = userId;
        }

        // Method to generate a unique incremental ID with thread safety
        private string GenerateUniqueID()
        {
            lock (_lock)  // Ensure that only one thread can increment the counter at a time
            {
                _counter++;  // Increment counter for each new ID
                return "REQ" + _counter.ToString("D5");  // Format the ID with leading zeros
            }
        }

        // Override Equals and GetHashCode to compare based on RequestID
        public override bool Equals(object obj)
        {
            if (obj is ServiceRequest other)
            {
                return this.RequestID == other.RequestID;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return RequestID.GetHashCode();
        }
    }
}
