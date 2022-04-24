using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
builder.Services.AddDbContext<ApplicationDbContext>();

//Passando informação pelo Body
app.MapPost("/products", (Product product) => {
    ProductRepository.Add(product);
    return Results.Created($"/products/product.Code", product.Code);
});
//Passando parametro pela Rota
app.MapGet("/products/{code}" , ([FromRoute] string code) => {
    var product = ProductRepository.GetBy(code);
    if(product != null)
        return Results.Ok(product);
    return Results.NotFound();
});

app.MapPut("/products" , (Product product) => {
    var productSave = ProductRepository.GetBy(product.Code);
    productSave.Name = product.Name;
    return Results.Ok();
});

app.MapDelete("/products/{code}" , ([FromRoute] string code) => {
    var removeProduct = ProductRepository.GetBy(code);
    ProductRepository.Remove(removeProduct);
    return Results.Ok();
});

app.Run();

public static class ProductRepository{
    public static List<Product> Products {get; set;}

    public static void Add(Product product){
        if(Products == null)
        Products = new List<Product>();

        Products.Add(product);
    }

    public static Product GetBy(string code){
        return Products.FirstOrDefault(p => p.Code == code);
    }

    public static void Remove(Product product) {
        Products.Remove(product);
    }

}
public class Product{
    
    public int Id {get; set;}
    public string Code {get; set;}
    public string Name {get; set;}
    public string Description {get; set;}
}

public class ApplicationDbContext : DbContext {
    
    public DbSet<Product> Products {get; set;}

    protected override void OnConfiguring(DbContextOptionsBuilder options)=>
    options.UseSqlServer("Server=DESKTOP-1IH9IFP;Database=Products;MultipleActiveResultSets=true;Encrypt=Yes;TrustServerCertificate=Yes");

}