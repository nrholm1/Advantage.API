using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Advantage.API.Controllers
{
    public class PaginatedResponse<T>
    {
        public PaginatedResponse(IEnumerable<T> data, int i, int len)
        {
            // [1] page, 10 results => (1 - 1) * 10 = Skip(0) => Take(10).ToList(); takes first 10
            // [2] page, 100 results => (2 - 1) * 100 = Skip(100) => Take(100).ToList(); takes second 100
            Data = data.Skip((i - 1) * len).Take(len).ToList();
            Total = data.Count();
        }

        public int Total { get; set; }
        public IEnumerable<T> Data { get; set; }

    }
}
