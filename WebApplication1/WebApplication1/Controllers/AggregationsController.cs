using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Helpers;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AggregationsController : ControllerBase
    {

        //private TestDbContext _dbContext;
        //private QueryAggrRepo queryAggr;
        private IQueryAggr _queryAggr;

        public AggregationsController(IQueryAggr queryAggr)
        {
            _queryAggr = queryAggr;
        }


        [HttpPost("CreateRecord")]
        public IActionResult Create([FromBody] TableAggregations request)
        {

            TableAggregations record = new TableAggregations();
            record.Project = request.Project;
            record.Employee = request.Employee;
            record.Date = request.Date;
            record.Hours = request.Hours;

            try
            {
                _queryAggr.CreateRecord(record);
            }
            catch (Exception e)
            {
                return StatusCode(500, "An error occured!: " + e);
            }

            return Ok();

        }

        [HttpPost("SelectAggregations")]
        [AllowAnonymous]
        public IActionResult SelectAggregations([FromBody] BodyRequest request)
        {
            if (request.Second != "none")
            {
                if (request.Third != "none")
                {
                    var aggr3 = _queryAggr.GetAggrByThree(request.First,request.Second,request.Third);
                    Console.WriteLine("Sono in tre");
                    return Ok(aggr3);
                }
                else
                {
                    var aggr2 = _queryAggr.GetAggrByTwo(request.First, request.Second);
                    Console.WriteLine("Sono in due");
                    return Ok(aggr2);
                }
            }
            else
            {
                var aggr1 = _queryAggr.GetAggrByOne(request.First);
                Console.WriteLine("Sono in uno");
                return Ok(aggr1);

            }
        }

        [HttpPut("UpdateUser")]
        public IActionResult UpdateUser([FromBody] TableAggregations request)
        {

            try
            {
                _queryAggr.UpdateRecord(request);
            }
            catch (Exception e)
            {
                return StatusCode(500, "An error occured!: " + e);
            }

            return Ok();
        }

        [HttpDelete("DeleteUser")]

        public IActionResult DeleteUser([FromBody] TableAggregations request)
        {
            try
            {
                _queryAggr.DeleteRecord(request);
            }
            catch (Exception e)
            {
                return StatusCode(500, "An error occured!: " + e);
            }

            return Ok();
        }

    }
}
