
namespace HeizungUeberwachung
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddScoped<Context>();
            builder.Services.AddScoped<Repository>();
            builder.Services.AddScoped<Manager>();

            builder.Services.AddHostedService<TimedServices>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            //app.UseAuthorization();
            app.UseMiddleware<ApiKeyAuthMiddleware>();

            app.AddEndpoints();

            app.Run();

        }
    }
}
