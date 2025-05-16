using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    
    public class ProductsController(IGenericRepository<Product> repo): BaseApiController
    {   

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] ProductSpecParams specParams){

            var spec= new ProductSpecification(specParams);

            return await CreatePagedResult(repo, spec, specParams.PageIndex, specParams.PageSize);
        } 

        [HttpGet("{id:int}")] //api/products/id
        public async Task<ActionResult<Product>> GetProduct(int id){
            var product= await repo.GetByIdAsync(id);

            if(product==null) return NotFound();

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product){
            repo.Add(product);
            if(await repo.SaveAllAsync()){
                //adds a new location header, useful for the client to know where the created product is at.
                return CreatedAtAction("GetProduct", new {id = product.Id}, product);
            }

            return BadRequest("Problem creating product");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product){
            if(product.Id!= id || !ProductExists(id))
                return BadRequest("Cannot update product");

            repo.Update(product);
             if(await repo.SaveAllAsync()){
                return NoContent();
            }

            return BadRequest("Problem updating the product");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id){
            var product= await repo.GetByIdAsync(id);
            if(product==null) return NotFound();

            repo.Remove(product);

            if(await repo.SaveAllAsync()){
                return NoContent();
            }

            return BadRequest("Problem deleting the product");
        }


        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands(){
            var spec = new BrandListSpecification();
            return Ok(await repo.ListAsync(spec));
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes(){
            var spec = new TypeListSpecification();
            return Ok(await repo.ListAsync(spec));
        }
        private bool ProductExists(int id){
            return repo.Exists(id);
        }
        
    }
}