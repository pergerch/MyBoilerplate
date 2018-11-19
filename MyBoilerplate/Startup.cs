// <copyright file="Startup.cs" company="pergerch">
// Copyright (c) pergerch. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MyBoilerplate
{
	using AutoMapper;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using MyBoilerplate.Core;
	using MyBoilerplate.Data;
	using MyBoilerplate.Data.Repositories;

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

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
		}
	}
}