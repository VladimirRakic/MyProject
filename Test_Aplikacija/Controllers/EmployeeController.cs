using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web.Http;
using Test_Aplikacija.Models;

namespace Test_Aplikacija.Controllers
{
    
    public class EmployeeController : ApiController
    {
        List<Employee> employees;
        public EmployeeController()
        {
            employees = new List<Employee>();
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\vraki\Desktop\APP\Test_Aplikacija\EDIt\text\emp.txt");

            foreach (var line in lines)
            {
                var props = line.Split(',');
                employees.Add(new Employee()
                {
                    Id = Convert.ToInt32(props[0]),
                    FirstName = props[1],
                    LastName = props[2],
                    Email = props[3],
                    Image = props[4]
                });
            }
        }
        [HttpGet]
        [Route("Employees/getAll")]
        public IEnumerable<Employee> GetAllEmployees()
        {
            return employees;
        }
      
        public HttpResponseMessage GetEmployee(int id)
        {
            var employee = employees.FirstOrDefault((p) => p.Id == id);
            if (employee == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id " + id.ToString() + " is not found");
            }

            return Request.CreateResponse(HttpStatusCode.OK, employee);
        }
        [HttpDelete]
        [ActionName("deleteObject")]
        public HttpResponseMessage EraseEmployee(int id) {

           var emp = employees.FirstOrDefault((p) => p.Id == id);
           if (emp == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "employee with Id = " + id.ToString() + "not found to delete");
            }
            else {

                employees.Remove(emp);
            }

            List<string>lines= new List<string>();

            foreach (var e in employees) {
               var line = ""+e.Id+","+e.FirstName+","+e.LastName+","+ e.Email+","+ e.Image;
                lines.Add(line);

            }

            System.IO.File.WriteAllLines(@"C:\Users\vraki\Desktop\APP\Test_Aplikacija\EDIt\text\emp.txt", lines.ToArray());

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        [ActionName("insertObject")]
        public HttpResponseMessage AddNewEmployee(Employee emp)
        {
            Random rnd = new Random();
            emp.Id = rnd.Next(1, 100000);
            employees.Add(emp);
            List<string> lines = new List<string>();

            try
            {
                foreach (var e in employees)
                {
                    var line = "" + e.Id + "," + e.FirstName + "," + e.LastName + "," + e.Email + "," + e.Image;
                    lines.Add(line);
                }
                System.IO.File.WriteAllLines(@"C:\Users\vraki\Desktop\APP\Test_Aplikacija\EDIt\text\emp.txt", lines.ToArray());
            }

            catch (IOException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, emp.Id);
        }

        [HttpPatch]
        [ActionName("udpadteObject")]
        public HttpResponseMessage EditEmployees(Employee emp)
        {
            List<string> lines = new List<string>();

            foreach (var e in employees)
            {
                if (e.Id == emp.Id)
                {
                    e.FirstName = emp.FirstName;
                    e.LastName = emp.LastName;
                    e.Email = emp.Email;
                    e.Image = emp.Image;
                }
                var line = "" + e.Id + "," + e.FirstName + "," + e.LastName + "," + e.Email + "," + e.Image;
                lines.Add(line);
            }

            System.IO.File.WriteAllLines(@"C:\Users\vraki\Desktop\APP\Test_Aplikacija\EDIt\text\emp.txt", lines.ToArray());

            return Request.CreateResponse(HttpStatusCode.OK, "User successfilly updated");
        }


    }
}
