using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Helpers
{
    public class QueryAggrRepo : IQueryAggr
    {
        private readonly Data.TestDbContext _context;

        public QueryAggrRepo(Data.TestDbContext context)
        {
            _context = context;
        }

        public void CreateRecord(TableAggregations newRecord)
        {
            _context.TableAggregations.Add(newRecord);
            _context.SaveChanges();
        }


        public List<TableAggregations> GetAggrByOne(string param1)
        {
            List<TableAggregations> list = new List<TableAggregations>();

            switch (param1)
            {
                case "project":
                    var resultP = _context.TableAggregations.AsEnumerable()
                         .GroupBy(x => x.Project)
                         .Select(x => new
                         {
                             Project = x.Key,
                             Hours = x.Sum(y => y.Hours)

                         }).ToList();

                    foreach (var r in resultP)
                    {
                        TableAggregations element = new TableAggregations();
                        element.Project = r.Project;
                        element.Hours = r.Hours;

                        list.Add(element);
                    }
                    break;
                case "employee":
                    var resultE = _context.TableAggregations.AsEnumerable()
                         .GroupBy(x => x.Employee)
                         .Select(x => new
                         {
                             Employee = x.Key,
                             Hours = x.Sum(y => y.Hours)

                         }).ToList();

                    foreach (var r in resultE)
                    {
                        TableAggregations element = new TableAggregations();
                        element.Employee = r.Employee;
                        element.Hours = r.Hours;

                        list.Add(element);
                    }
                    break;
                case "date":
                    var resultD = _context.TableAggregations.AsEnumerable()
                         .GroupBy(x => x.Date)
                         .Select(x => new
                         {
                             Date = x.Key,
                             Hours = x.Sum(y => y.Hours)

                         }).ToList();

                    foreach (var r in resultD)
                    {
                        TableAggregations element = new TableAggregations();
                        element.Date = r.Date;
                        element.Hours = r.Hours;

                        list.Add(element);
                    }
                    break;
            }

            return list;
        }

        public List<TableAggregations> GetAggrByTwo(string param1, string param2)
        {
            switch (param1)
            {
                case "project":
                    if (param2 == "employee")
                    {
                        return GetProjectEmployee();
                    }
                    else
                    {
                        return GetProjectDate();
                    }
                case "employee":
                    if (param2 == "date")
                    {
                        return EmployeeDate();
                    }
                    else
                    {
                        return GetProjectEmployee();
                    }
                case "date":
                    if (param2 == "employee")
                    {
                        return EmployeeDate();
                    }
                    else
                    {
                        return GetProjectDate();
                    }
            }
            return null;
        }

        public List<TableAggregations> GetAggrByThree(string param1, string param2, string param3)
        {
            var result = _context.TableAggregations
                         .Join(_context.TableAggregations
                         .GroupBy(x => x.Project)
                         .Select(x => new
                         {
                             Project = x.Key,
                             Hours = x.Sum(y => y.Hours)

                         }), o => o.Project, u => u.Project, (o, u) => new { BigTable = o, FirstJoin = u })
                        .Join(_context.TableAggregations
                        .GroupBy(x => x.Employee)
                        .Select(x => new
                        {
                            Employee = x.Key,
                            Hours = x.Sum(y => y.Hours)

                        }), o => o.BigTable.Employee, u => u.Employee, (o, u) => new { SecondTable = o, SecondJoin = u })
                        .Join(_context.TableAggregations
                        .GroupBy(x => x.Date)
                        .Select(x => new
                        {
                            Date = x.Key,
                            Hours = x.Sum(y => y.Hours)

                        }), o => o.SecondTable.BigTable.Date, u => u.Date, (o, u) => new { ThirdJoin = o, EndJoin = u })
                        .Select(x => new
                        {
                            Project = x.ThirdJoin.SecondTable.FirstJoin.Project,
                            Employee = x.ThirdJoin.SecondJoin.Employee,
                            Date = x.EndJoin.Date,
                            Hours = x.EndJoin.Hours

                        }).ToList().GroupBy(x => new { x.Project, x.Employee, x.Date }).ToList();

            List<TableAggregations> list = new List<TableAggregations>();

            foreach (var r in result)
            {
                TableAggregations element = new TableAggregations();
                element.Project = r.First().Project;
                element.Employee = r.First().Employee;
                element.Date = r.First().Date;
                element.Hours = r.Sum(y => y.Hours);

                list.Add(element);
            }

            return list;
        }
        
        private List<TableAggregations> GetProjectEmployee()
        {
            var result = _context.TableAggregations
                         .Join(_context.TableAggregations
                         .GroupBy(x => x.Project)
                         .Select(x => new
                         {
                             Project = x.Key,
                             Hours = x.Sum(y => y.Hours)

                         }), o => o.Project, u => u.Project, (o, u) => new { BigTable = o, FirstJoin = u })
                        .Join(_context.TableAggregations
                        .GroupBy(x => x.Employee)
                        .Select(x => new
                        {
                            Employee = x.Key,
                            Hours = x.Sum(y => y.Hours)

                        }), o => o.BigTable.Employee, u => u.Employee, (o, u) => new { SecondJoin = o, EndJoin = u }).Select(x => new
                        {
                            Project = x.SecondJoin.FirstJoin.Project,
                            Employee = x.EndJoin.Employee,
                            Hours = x.SecondJoin.BigTable.Hours

                        }).ToList().GroupBy(x => new { x.Project, x.Employee }).ToList();

            List<TableAggregations> list = new List<TableAggregations>();

            foreach (var r in result)
            {
                TableAggregations element = new TableAggregations();
                element.Project = r.First().Project;
                element.Employee = r.First().Employee;
                element.Hours = r.Sum(y => y.Hours);

                list.Add(element);
            }

            return list;
        }
        private List<TableAggregations> GetProjectDate()
        {
            var result = _context.TableAggregations
                         .Join(_context.TableAggregations
                         .GroupBy(x => x.Project)
                         .Select(x => new
                         {
                             Project = x.Key,
                             Hours = x.Sum(y => y.Hours)

                         }), o => o.Project, u => u.Project, (o, u) => new { BigTable = o, FirstJoin = u })
                        .Join(_context.TableAggregations
                        .GroupBy(x => x.Date)
                        .Select(x => new
                        {
                            Date = x.Key,
                            Hours = x.Sum(y => y.Hours)

                        }), o => o.BigTable.Date, u => u.Date, (o, u) => new { SecondJoin = o, EndJoin = u }).Select(x => new
                        {
                            Project = x.SecondJoin.FirstJoin.Project,
                            Date = x.EndJoin.Date,
                            Hours = x.SecondJoin.BigTable.Hours

                        }).ToList().GroupBy(x => new { x.Project, x.Date }).ToList();

            List<TableAggregations> list = new List<TableAggregations>();

            foreach (var r in result)
            {
                TableAggregations element = new TableAggregations();
                element.Project = r.First().Project;
                element.Date = r.First().Date;
                element.Hours = r.Sum(y => y.Hours);

                list.Add(element);
            }

            return list;
        }
        private List<TableAggregations> EmployeeDate()
        {
            var result = _context.TableAggregations
                         .Join(_context.TableAggregations
                         .GroupBy(x => x.Employee)
                         .Select(x => new
                         {
                             Employee = x.Key,
                             Hours = x.Sum(y => y.Hours)

                         }), o => o.Employee, u => u.Employee, (o, u) => new { BigTable = o, FirstJoin = u })
                        .Join(_context.TableAggregations
                        .GroupBy(x => x.Date)
                        .Select(x => new
                        {
                            Date = x.Key,
                            Hours = x.Sum(y => y.Hours)

                        }), o => o.BigTable.Date, u => u.Date, (o, u) => new { SecondJoin = o, EndJoin = u }).Select(x => new
                        {
                            Employee = x.SecondJoin.FirstJoin.Employee,
                            Date = x.EndJoin.Date,
                            Hours = x.SecondJoin.BigTable.Hours

                        }).ToList().GroupBy(x => new { x.Employee, x.Date }).ToList();

            List<TableAggregations> list = new List<TableAggregations>();

            foreach (var r in result)
            {
                TableAggregations element = new TableAggregations();
                element.Date = r.First().Date;
                element.Employee = r.First().Employee;
                element.Hours = r.Sum(y => y.Hours);

                list.Add(element);
            }

            return list;
        }

        public void UpdateRecord(TableAggregations newRecord)
        {
            //TO DO
        }

        public void DeleteRecord(TableAggregations record)
        {
            //var user = _context.TableAggregations.Where(p => p.Employee == record.Employee && p.Project == record.Project && p.Date == record.Date).FirstOrDefault();

            //_context.Entry(user).State = EntityState.Deleted;
            //_context.SaveChanges();

            //TO DO
        }
    }
}
