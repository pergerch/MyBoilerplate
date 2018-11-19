// <copyright file="EntityToViewModelProfile.cs" company="pergerch">
// Copyright (c) pergerch. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MyBoilerplate.Core.Mapping
{
	using AutoMapper;
	using MyBoilerplate.Data.Entities;
	using MyBoilerplate.Models;

	public class EntityToViewModelProfile : Profile
	{
		public EntityToViewModelProfile()
		{
			CreateMap<DefaultItem, DefaultItemViewModel>();
		}
	}
}