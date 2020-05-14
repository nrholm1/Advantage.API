using Advantage.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Advantage.API
{
    public class Helpers
    {
        private static Random _rand = new Random();
        private static string GetRandom(IList<string> items)
        {
            return items[_rand.Next(items.Count)];
        }

        internal static string MakeUniqueCustomerName(List<string> names)
        {
            var maxNames = bizPrefix.Count * bizSuffix.Count; // expensive operation in prod (maybe)
            
            if (names.Count >= maxNames)
            {
                throw new System.InvalidOperationException("Maximum number of unique names exceeded");
            }

            var prefix = GetRandom(bizPrefix);
            var suffix = GetRandom(bizSuffix);
            var bizName = prefix + suffix;
            
            if (names.Contains(bizName))
            {
                MakeUniqueCustomerName(names);
            }
            return bizName;
        }

        internal static string MakeCustomerEmail(string customerName)
        {
            return $"contact@{customerName.ToLower()}.com";
        }
        internal static string GetRandomState()
        {

            return GetRandom(usStates);
        }

        internal static decimal GetRandomOrderTotal()
        {
            return _rand.Next(100, 5000);
        }

        internal static DateTime GetRandomOrderPlaced()
        {
            var end = DateTime.Now;
            var start = end.AddDays(-90); // at most 3 months ago

            TimeSpan possibleSpan = end - start;
            TimeSpan newSpan = new TimeSpan(0, _rand.Next(0, (int)possibleSpan.TotalMinutes), 0);

            return start + newSpan;
        }

        internal static DateTime? GetRandomOrderCompleted(DateTime orderPlaced)
        {
            var now = DateTime.Now;
            var minLeadTime = TimeSpan.FromDays(7);
            var timePassed = now - orderPlaced;

            if (timePassed < minLeadTime)
            {
                return null;
            }

            return orderPlaced.AddDays(_rand.Next(7, 14));
        }

        internal static Customer GetRandomCustomer(ApiContext ctx)
        {
            var randomId = _rand.Next(1, ctx.Customers.Count());
            return ctx.Customers.First(c => c.Id == randomId);
        }


        private static readonly List<string> usStates = new List<string>()
        {
            "AK", "AL","AZ",  "AR", "CA", "CO", "CT", "DE", "FL", "GA",
            "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD",
            "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ",
            "NM", "NY", "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC",
            "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY"
        };

        private static readonly List<string> bizPrefix = new List<string>()
        {
            "ABC",
            "XYZ",
            "MainSt",
            "Budget",
            "Apollo",
            "Enterprise",
            "Navi",
            "Magic",
            "Peak",
            "Next",
            "Interface",
            "Cool",
            "Comfort"
        };
        private static readonly List<string> bizSuffix = new List<string>()
        {
            "Corporation",
            "Co",
            "Logistics",
            "Transit",
            "Partner",
            "Ltd",
            "GmBH",
            "Goods",
            "Foods",
            "Automotive",
            "ERP",
            "Dynamics",
            "AstroCorp"
        };
    }
}
