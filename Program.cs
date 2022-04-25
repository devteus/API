using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);   
var app = builder.Build();
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:SqlServer"]);

//Passando informação pelo Body
app.MapPost("/products", (ProductDTO productDto, ApplicationDbContext context) => {

    var category = context.Category.Where(c => c.Id == productDto.CategoryId).First();
    var product = new Product
    {
        Code = productDto.Code,
        Name = productDto.Name,
        Description = productDto.Description,
        Category = category
    };  
    context.Add(product);
    return Results.Created($"/products/{product.Id}", product.Id);
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
