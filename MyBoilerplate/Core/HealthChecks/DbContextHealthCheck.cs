// <copyright file="DbContextHealthCheck.cs" company="pergerch">
// Copyright (c) pergerch. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MyBoilerplate.Core.HealthChecks
{
	using System;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Diagnostics.HealthChecks;
	using MyBoilerplate.Data;

	public class DbContextHealthCheck : IHealthCheck
	{
		public DbContextHealthCheck(DefaultContext context)
		{
			Context = context;
		}

		private DefaultContext Context { get; }

		public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			try
			{
				int i = Context.Items.Count();

				return Task.FromResult(HealthCheckResult.Healthy());
			}
			catch (Exception e)
			{
				return Task.FromResult(HealthCheckResult.Unhealthy());
			}
		}
	}
}