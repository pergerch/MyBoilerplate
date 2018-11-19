// <copyright file="DefaultRepository.cs" company="pergerch">
// Copyright (c) pergerch. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MyBoilerplate.Data.Repositories
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;
	using MyBoilerplate.Data.Entities;

	public class DefaultRepository
	{
		public DefaultRepository(DefaultContext context)
		{
			Context = context;
		}

		private DefaultContext Context { get; }

		public async Task<List<DefaultItem>> GetItems()
		{
			return await Context.Items.ToListAsync();
		}
	}
}