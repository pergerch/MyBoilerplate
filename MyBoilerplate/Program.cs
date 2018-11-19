// <copyright file="Program.cs" company="pergerch">
// Copyright (c) pergerch. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MyBoilerplate
{
	using Microsoft.AspNetCore;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Configuration;
	using Serilog;

	public class Program
	{
		public static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();

		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args)
				.ConfigureAppConfiguration((hostingContext, config) =>
				{
					IHostingEnvironment env = hostingContext.HostingEnvironment;

					config.AddJsonFile("appsettings.json", false, true);
					config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
					config.AddEnvironmentVariables();
				})
				.ConfigureLogging((hostingContext, logging) =>
				{
					logging.AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(hostingContext.Configuration.GetSection("Logging"))
						.CreateLogger());
				})
				.Build()
				.Run();
		}
	}
}