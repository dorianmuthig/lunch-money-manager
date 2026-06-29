using System;
using System.Collections.Generic;
using System.Text;

namespace LunchMoneyManager.Library.Models
{
    public class Setting
    {
        public Guid ID { get; set; }
        public required string Name { get; set; }
        public string? Value { get; set; }
    }
}
