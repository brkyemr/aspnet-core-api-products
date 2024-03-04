using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        var products = await _context.Products.ToListAsync();
        return Ok(products);
    }

    //localhost:5000/api/products/1 => GET
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int? id)
    {
        //return _products?.FirstOrDefault(i=> i.ProductId == id) ?? new Product(); //null ise boş product nesnesi attık
        if (id == null)
        {
            return NotFound();
        }
        var p = await _context.Products.FirstOrDefaultAsync(i => i.ProductId == id);

        if(p == null)
        {
            return NotFound();
        }
        return Ok(p);
    }


    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product entity)
    {
        _context.Products.Add(entity);
        await _context.SaveChangesAsync();

        //for response code 201 
        //CreatedAtAction oluşan ürünü sorgulama linki dönüyor response a     ====> localhost:5000/api/products/1 => GET
        return CreatedAtAction(nameof(GetProduct),new { id = entity.ProductId}, entity);
    }

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
        
        if(entity.ProductName != null )
        {
            product.ProductName = entity.ProductName;
        }
        if(entity.Price != 0 )
        {
            product.Price = entity.Price;
        }
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
}
}
