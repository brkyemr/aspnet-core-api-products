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

}
}
