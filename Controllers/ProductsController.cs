using Microsoft.AspNetCore.Mvc;
using ProductsAPI.Models;

namespace ProductsAPI.Controllers{


[ApiController]
[Route("api/[controller]")]  //controller = products' a denk gelir.
public class ProductsController:ControllerBase
{
    private static List<Product>? _products;

    public ProductsController() 
    { 
        _products = new List<Product>
        {
            new() {ProductId = 1, ProductName = "Iphone 11",Price = 15000, IsActive = true },
            new() {ProductId = 2, ProductName = "Iphone 12",Price = 25000, IsActive = true },
            new() {ProductId = 3, ProductName = "Iphone 13",Price = 35000, IsActive = true },
            new() {ProductId = 4, ProductName = "Iphone 14",Price = 45000, IsActive = true }

        };
    }


    //localhost:3000/api/products => get
    [HttpGet]
    public IActionResult GetProducts()
    {
        //return _products ?? new List<Product>();
        if (_products == null)
        {
            return NotFound();
        }

        return Ok(_products);
    }

    //localhost:5000/api/products/1 => GET
    [HttpGet("{id}")]
    public IActionResult GetProduct(int? id)
    {
        //return _products?.FirstOrDefault(i=> i.ProductId == id) ?? new Product(); //null ise boş product nesnesi attık
        if (id == null)
        {
            return NotFound();
        }
        var p = _products?.FirstOrDefault(i => i.ProductId == id);

        if(p == null)
        {
            return NotFound();
        }
        return Ok(p);
    }

}
}
