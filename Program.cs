using chatApi.Setup;
using System.Configuration;

WebApplication app = DefaultWebApplication.Create(args, webappBuilder: webappBuilder =>
{
    webappBuilder.Services.AddApiToken(webappBuilder.Configuration);
    webappBuilder.Services.AddControllers();
    webappBuilder.Services.AddEndpointsApiExplorer();
    webappBuilder.Services.AddSwaggerGen();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseApiTokenMiddleware();

DefaultWebApplication.Run(app);
