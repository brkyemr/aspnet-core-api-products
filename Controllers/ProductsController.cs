using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.DTO;
using ProductsAPI.Models;

namespace ProductsAPI.Controllers{


[ApiController]
[Route("api/[controller]")]  //controller = products' a denk gelir.
public class ProductsController:ControllerBase
{

    private readonly ProductsContext _context;

    public ProductsController(ProductsContext context) 
    { 
        _context = context;
    }


    //localhost:3000/api/products => get
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _context.Products.Where(i => i.IsActive).Select(p=> 
        new ProductDTO{
            ProductId = p.ProductId,
            ProductName = p.ProductName,
            Price = p.Price
        }).ToListAsync();
        return Ok(products);
    }

    //localhost:5000/api/products/1 => GET
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int? id)
    {
        //return _products?.FirstOrDefault(i=> i.ProductId == id) ?? new Product(); //null ise boş product nesnesi attık
        if (id == null)
        {
            return NotFound();
        }
        var p = await _context.Products
        /*.Select(p=> 
        new ProductDTO{
            ProductId = p.ProductId,
            ProductName = p.ProductName,
            Price = p.Price
        })*/
        .Where(i=> i.ProductId == id)
        .Select(p=> ProductToDTO(p))
        .FirstOrDefaultAsync();

        if(p == null)
        {
            return NotFound();
        }
        return Ok(p);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product entity)
    {
        _context.Products.Add(entity);
        await _context.SaveChangesAsync();

        //for response code 201 
        //CreatedAtAction oluşan ürünü sorgulama linki dönüyor response a     ====> localhost:5000/api/products/1 => GET
        return CreatedAtAction(nameof(GetProduct),new { id = entity.ProductId}, entity);
    }
    
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id,Product entity)
    {
        if (id != entity.ProductId)
        {
            return BadRequest();
        }

        var product = await _context.Products.FirstOrDefaultAsync(i => i.ProductId == id);
        if(product == null)
        {
            return NotFound();
        }
        product.ProductName = entity.ProductName;
        product.Price = entity.Price;
        product.IsActive = entity.IsActive;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            
            return NotFound();
        }
        return Ok(product);
    
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var product = await _context.Products.FirstOrDefaultAsync(i => i.ProductId == id);
        
        if (product == null)
        {
            return NotFound();
        }
        _context.Products.Remove(product);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            return NotFound();
        }
        return NoContent();
    }

    private static ProductDTO ProductToDTO(Product p){
        var entity = new ProductDTO();
        if (p != null)
        {
            entity.ProductId = p.ProductId;
            entity.ProductName = p.ProductName;
            entity.Price = p.Price;

        }
        return new ProductDTO 
        {
            ProductId = p.ProductId,
            ProductName = p.ProductName,
            Price = p.Price
        };
    }
}
}
