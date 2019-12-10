using API.Attributes;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    [RoutePrefix("api/Categories")]
    [BasicAuthentication]
    public class CategoryController : ApiController
    {
        VirtualBazar context = new VirtualBazar();
        [Route("")]
        
        public IHttpActionResult Get()
        {
            var list = context.Categories.ToList();
            List<Category> categories = new List<Category>();
            foreach (var item in list)
            {
                Category cat = new Category();
                cat.Id = item.Id;
                cat.Name = item.Name;
                cat.links.Add(new Links() { HRef = "http://localhost:17193/api/Categories", Method = "GET", Rel = "Self" });
                cat.links.Add(new Links() { HRef = "http://localhost:17193/api/Categories/" + cat.Id, Method = "GET", Rel = "Specific Resource" });
                cat.links.Add(new Links() { HRef = "http://localhost:17193/api/Categories/" + cat.Id, Method = "PUT", Rel = "Resource Edit" });
                cat.links.Add(new Links() { HRef = "http://localhost:17193/api/Categories/" + cat.Id, Method = "DELETE", Rel = "Resource Delete" });
                cat.links.Add(new Links() { HRef = "http://localhost:17193/api/Categories", Method ="POST",Rel="Resource Create"});
                categories.Add(cat);
            }
            return Ok(categories);
        }
        [Route("{id}",Name="GetCategoryById")]
        public IHttpActionResult Get(int id)
        {
            var item = context.Categories.Where(p=>p.Id==id).FirstOrDefault();
            if (item == null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                Category cat = new Category();
                cat.Id = item.Id;
                cat.Name = item.Name;
                cat.links.Add(new Links() { HRef = "http://localhost:17193/api/Categories", Method = "GET", Rel = "All Resources" });
                cat.links.Add(new Links() { HRef = "http://localhost:17193/api/Categories/" + cat.Id, Method = "GET", Rel = "Self" });
                cat.links.Add(new Links() { HRef = "http://localhost:17193/api/Categories/" + cat.Id, Method = "PUT", Rel = "Resource Edit" });
                cat.links.Add(new Links() { HRef = "http://localhost:17193/api/Categories/" + cat.Id, Method = "DELETE", Rel = "Resource Delete" });
                cat.links.Add(new Links() { HRef = "http://localhost:17193/api/Categories", Method = "POST", Rel = "Resource Create" });
                cat.links.Add(new Links() { HRef = "http://localhost:17193/api/Categories" +cat.Id +"/Products", Method = "GET", Rel = "All Resource for Specific Category" });
                return Ok(cat);
            }
            
        }
        [Route("")]
        public IHttpActionResult Post(Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();
            return Created(Url.Link("GetCategoryById", new { id=category.Id}),category);
        }
        [Route("{id}")]
        public IHttpActionResult Put([FromUri]int id,[FromBody]Category category)
        {
            var item = context.Categories.Where(p => p.Id == id).FirstOrDefault();
            if (item == null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                item.Name = category.Name;
                item.Id = id;
                context.SaveChanges();
                return Ok(item);
            }
           
        }

        [Route("{id}")]
        public IHttpActionResult Delete([FromUri]int id)
        {
            var item = context.Categories.Where(p => p.Id == id).FirstOrDefault();
            if (item == null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                context.Categories.Remove(item);
                context.SaveChanges();
                return StatusCode(HttpStatusCode.NoContent);
            }

        }

        [Route("{id}/Products")]
        public IHttpActionResult GetProductsByCategory([FromUri]int id)
        {
            var list = context.Products.Where(p => p.Category_Id == id);
            if (list.FirstOrDefault() == null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                List<Product> products = new List<Product>();
                foreach (var item in list)
                {
                    Product pro = new Product();
                    pro.Id = item.Id;
                    pro.Name = item.Name;
                    pro.Price = item.Price;
                    pro.Quantity = item.Quantity;
                    pro.Category_Id = item.Category_Id;


                    products.Add(pro);
                }
                return Ok(products);
            }
            
        }

    }
}
