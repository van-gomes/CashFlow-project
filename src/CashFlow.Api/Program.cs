var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços para Controllers e documentação Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ativar arquivos estáticos
app.UseStaticFiles();

// Gerar o Swagger
app.UseSwagger();

// Configurar corretamente o Swagger UI
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "CastFlow API v1");
    options.RoutePrefix = ""; // ou "" se quiser abrir direto no raiz
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); // Mapear Controllers

app.Run();