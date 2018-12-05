// <copyright file="Startup.cs" company="pergerch">
// Copyright (c) pergerch. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MyBoilerplate
{
	using System.Linq;
	using AutoMapper;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Diagnostics.HealthChecks;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using MyBoilerplate.Core;
	using MyBoilerplate.Core.HealthChecks;
	using MyBoilerplate.Data;
	using MyBoilerplate.Data.Repositories;
	using Newtonsoft.Json;

	public class Startup
	{
		public Startup(IConfiguration configuration, IHostingEnvironment env)
		{
			Configuration = configuration;
			Environment = env;
		}

		public IConfiguration Configuration { get; }

		public IHostingEnvironment Environment { get; }

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMapper mapper)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			mapper.ConfigurationProvider.AssertConfigurationIsValid();

			HealthCheckOptions options = new HealthCheckOptions
			{
				ResponseWriter = async (c, r) =>
				{
					string result = JsonConvert.SerializeObject(new
					{
						status = r.Status.ToString(),
						checks = r.Entries.Select(e => new
						{
							key = e.Key,
							value = e.Value.Status.ToString(),
						}),
					});

					c.Response.ContentType = "application/json";
					await c.Response.WriteAsync(result);
				},
			};
			app.UseHealthChecks("/health", options);

			app.UseMvc();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			string rootDir = Configuration.GetValue<string>("RootDir");
			if (string.IsNullOrEmpty(rootDir))
			{
				rootDir = Environment.WebRootPath;
			}

			services.Configure<ServiceOptions>(options => { options.DirectoryRoot = rootDir; });

			services.AddDbContext<DefaultContext>(options =>
				options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")).UseLazyLoadingProxies());

			services.AddTransient<DefaultRepository>();

			services.AddAutoMapper();

			services.AddHealthChecks().AddCheck<DbContextHealthCheck>("DbContext").AddCheck<FilesystemHealthCheck>("FileSystem");

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
		}
	}
}