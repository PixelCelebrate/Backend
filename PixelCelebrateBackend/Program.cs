using MailKit;
using Newtonsoft.Json.Serialization;
using PixelCelebrateBackend.MailTrapConfig;
using PixelCelebrateBackend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<PixelCelebrateBackend.Services.IMailService, PixelCelebrateBackend.Services.MailService>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//Pentru endpoints:
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


//JSON Serialzer:
builder.Services.AddControllers().AddNewtonsoftJson(options =>
options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
.AddNewtonsoftJson(options=>options.SerializerSettings.ContractResolver=new DefaultContractResolver());

//Pentru timer: La fiecare x secunde:
builder.Services.AddHostedService<SendEmails>();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

var app = builder.Build();


//Enable CORS for frontend interaction; Fara nu ar merge frontend:
app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());


// Configure the HTTP request pipeline.
//No swagger:
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseAuthorization();

app.MapControllers();

app.Run();
