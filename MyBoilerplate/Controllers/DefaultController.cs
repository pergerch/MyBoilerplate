// <copyright file="DefaultController.cs" company="pergerch">
// Copyright (c) pergerch. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MyBoilerplate.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;
	using AutoMapper;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.Options;
	using MyBoilerplate.Core;
	using MyBoilerplate.Data.Entities;
	using MyBoilerplate.Data.Repositories;
	using MyBoilerplate.Models;

	[Route("/")]
	[ApiController]
	public class DefaultController : ControllerBase
	{
		public DefaultController(ILogger<DefaultController> logger, IOptions<ServiceOptions> serviceOptions, IMapper mapper,
			DefaultRepository defaultRepository)
		{
			Logger = logger;
			ServiceOptions = serviceOptions.Value;
			Mapper = mapper;
			DefaultRepository = defaultRepository;
		}

		public DefaultRepository DefaultRepository { get; private set; }

		public ILogger<DefaultController> Logger { get; private set; }

		public IMapper Mapper { get; private set; }

		public ServiceOptions ServiceOptions { get; private set; }

		[HttpGet]
		public ActionResult<string> Get()
		{
			var config = new
			{
				RootDir = ServiceOptions.DirectoryRoot,
				FilesPath = Url.Action(nameof(GetFiles)),
				ItemsPath = Url.Action(nameof(GetItems)),
				HealthChecks = "/health",
			};

			return Ok(config);
		}

		[HttpGet("Files")]
		public ActionResult<IEnumerable<string>> GetFiles()
		{
			string[] files = Directory.GetFiles(ServiceOptions.DirectoryRoot);

			return Ok(files);
		}

		[HttpGet("Items")]
		public async Task<ActionResult<IEnumerable<DefaultItemViewModel>>> GetItems()
		{
			List<DefaultItem> items = await DefaultRepository.GetItems();

			if (items == null || !items.Any())
			{
				items = new List<DefaultItem>
				{
					new DefaultItem
					{
						Id = Guid.NewGuid(),
						Name = "Demo Item 1",
					},
					new DefaultItem
					{
						Id = Guid.NewGuid(),
						Name = "Demo Item 2",
					},
				};
			}

			return Ok(Mapper.Map<IEnumerable<DefaultItemViewModel>>(items));
		}
	}
}