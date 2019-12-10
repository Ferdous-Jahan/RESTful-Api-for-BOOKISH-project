using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    [RoutePrefix("api/Products")]
    public class ProductController : ApiController
    {
        VirtualBazar context = new VirtualBazar();
        [Route("")]
        public IHttpActionResult Get()
        {
            var list = context.Products.ToList();
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
        [Route("{id}", Name = "GetProductById")]
        public IHttpActionResult Get(int id)
        {
            var item = context.Products.Where(p => p.Id == id).FirstOrDefault();
            if (item == null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                Product pro = new Product();
                pro.Id = item.Id;
                pro.Name = item.Name;
                pro.Price = item.Price;
                pro.Quantity = item.Quantity;
                pro.Category_Id = item.Category_Id;
                return Ok(pro);
            }

        }
        [Route("")]
        public IHttpActionResult Post(Product product)
        {
            context.Products.Add(product);
            context.SaveChanges();
            return Created(Url.Link("GetProductById", new { id = product.Id }), product);
        }
        [Route("{id}")]
        public IHttpActionResult Put([FromUri]int id, [FromBody]Product product)
        {
            var item = context.Products.Where(p => p.Id == id).FirstOrDefault();
            if (item == null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                item.Id = id;
                item.Name = product.Name;
                item.Price = product.Price;
                item.Quantity = product.Quantity;
                item.Category_Id = product.Category_Id;
                item.Category = null;

                Product pro = new Product();
                pro.Id = item.Id;
                pro.Name = item.Name;
                pro.Price = item.Price;
                pro.Quantity = item.Quantity;
                pro.Category_Id = item.Category_Id;
                context.SaveChanges();
                return Ok(pro);
            }

        }

        [Route("{id}")]
        public IHttpActionResult Delete([FromUri]int id)
        {
            var item = context.Products.Where(p => p.Id == id).FirstOrDefault();
            if (item == null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                context.Products.Remove(item);
                context.SaveChanges();
                return StatusCode(HttpStatusCode.NoContent);
            }

        }
    }
}
