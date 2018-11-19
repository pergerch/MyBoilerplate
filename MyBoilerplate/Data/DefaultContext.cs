// <copyright file="DefaultContext.cs" company="pergerch">
// Copyright (c) pergerch. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MyBoilerplate.Data
{
	using Microsoft.EntityFrameworkCore;
	using MyBoilerplate.Data.Entities;
	using SpatialFocus.EntityFrameworkCore.Extensions;

	public class DefaultContext : DbContext
	{
		public DefaultContext(DbContextOptions<DefaultContext> options)
			: base(options)
		{
			Database.EnsureCreated();
		}

		public DbSet<DefaultItem> Items { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ConfigureNames(NamingOptions.Default);
		}
	}
}