using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitServerLibrary;
using RabbitServerLibrary.SendingModule;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(app =>
    {
        app.SetBasePath(Directory.GetCurrentDirectory());
        app.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        app.AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {
        Settings settings = new Settings();
        context.Configuration.GetSection(Settings.SettingsKey).Bind(settings);
        services.AddSingleton(settings);
        services.AddScoped<Producer>(
            x => new Producer(x.GetRequiredService<Settings>())
        );
        services.AddScoped<Consumer>(
            x => new Consumer(x.GetRequiredService<Settings>())
        );
        services.AddScoped<Mail>(
            x => new Mail()
            {
                From = "example@example.net",
                To = "example@example.net",
                Subject = "Test",
                Body = "Test",
                Method = EMethods.SmtpMethod
            }
        );
    }).Build();

Producer producer = host.Services.GetService<Producer>() ?? throw new ArgumentNullException(nameof(producer));
Consumer consumer = host.Services.GetService<Consumer>() ?? throw new ArgumentNullException(nameof(consumer));
Mail testMail = host.Services.GetService<Mail>() ?? throw new ArgumentNullException(nameof(testMail));

producer.OpenConnection();
producer.AddToQueue(testMail);
producer.CloseConnection();

consumer.OpenConnection();
consumer.StartConsuming();
consumer.Close();

host.StopAsync();
