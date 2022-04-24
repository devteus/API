using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);   
var app = builder.Build();
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:SqlServer"]);

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
