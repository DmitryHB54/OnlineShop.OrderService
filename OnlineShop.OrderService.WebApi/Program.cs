using OnlineShop.OrderService.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddSaga(builder.Configuration);

builder.Services.AddAuthentication(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

app.UseSwaggerDocumentaion(app.Environment);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();