using System;
using System.Collections.Generic;
using System.Text;

namespace LunchMoneyManager.Library.Models
{
    public class Debtor
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public override string ToString()
        {
            if (Name != null)
            {
                return Name.Trim();
            }
            else
            {
                return "";
            }
        }
    }
}
