using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Advantage.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Advantage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApiContext _ctx;

        public OrderController(ApiContext ctx)
        {
            _ctx = ctx;
        }


        // GET api/order/pageNumber/pageSize
        [HttpGet("{pageIndex:int}/{pageSize:int}")]
        public IActionResult Get(int pageIndex, int pageSize)
        {
            var data = _ctx.Orders.Include(o => o.Customer)
                .OrderByDescending(c => c.Placed);


            var page = new PaginatedResponse<Order>(data, pageIndex, pageSize);

            var totalCount = data.Count();
            var totalPages = Math.Ceiling((double)totalCount / pageSize);

            var response = new
            {
                Page = page,
                TotalPages = totalPages
            };

            return Ok(response);
        }

        [HttpGet("ByState")]
        public IActionResult ByState()
        {
            var orders = _ctx.Orders.Include(o => o.Customer).ToList(); // needs include to use the customer members

            var groupedResult = orders.GroupBy(o => o.Customer.State)
                                      .ToList()
                                      .Select(grp => new
                                      {
                                          State = grp.Key, // state is .Key, as it just grouped by state
                                          Total = grp.Sum(x => x.OrderTotal)
                                      }).OrderByDescending(res => res.Total)
                                      .ToList();
            return Ok(groupedResult);
        }

        [HttpGet("ByCustomer/{n}")]
        public IActionResult ByCustomer(int n)
        {
            var orders = _ctx.Orders.Include(o => o.Customer).ToList(); // needs include to use the customer members

            var groupedResult = orders.GroupBy(o => o.Customer.Id)
                                      .ToList()
                                      .Select(grp => new
                                      {
                                          _ctx.Customers.Find(grp.Key).Name, // Id is .Key, and allows to search for customers directly, and retrieve the name
                                          Total = grp.Sum(x => x.OrderTotal)
                                      }).OrderByDescending(res => res.Total)
                                      .Take(n)
                                      .ToList();
            return Ok(groupedResult);
        }

        [HttpGet("GetOrder/{id}", Name ="GetOrder")]
        public IActionResult GetOrder(int id)
        {
            var order = _ctx.Orders.Include(o => o.Customer) // Include Customer on order
                                   .First(o => o.Id == id);
            return Ok(order);
        }
    }
}