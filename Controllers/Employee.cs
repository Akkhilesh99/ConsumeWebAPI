using ConsumeWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace ConsumeWebAPI.Controllers
{
    public class Employee : Controller
    {
        Uri baseaddress = new Uri("https://localhost:44389/api/values/");
        private readonly HttpClient client;
        public Employee()
        {
            client = new HttpClient();
            client.BaseAddress = baseaddress;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<Models.Employee1> list = new List<Models.Employee1>();
            HttpResponseMessage response=client.GetAsync(client.BaseAddress + "").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<List<Models.Employee1>>(data);
            }
            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee1 model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent Content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(client.BaseAddress + "Post", Content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Product Created successfully";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;

                return View();
            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
                Employee1 emp = new Employee1();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    emp = JsonConvert.DeserializeObject<Employee1>(data);
                }
            return View(emp);
        }
        [HttpPost]
        public IActionResult Edit(int id,Employee1 emp)
        {
            try
            {
                string data = JsonConvert.SerializeObject(emp);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PutAsync(client.BaseAddress + "" + id, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Product Updated successfully";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            return View();
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                HttpResponseMessage response = client.DeleteAsync(client.BaseAddress + "/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Product Deleted successfully";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }
    }
}
