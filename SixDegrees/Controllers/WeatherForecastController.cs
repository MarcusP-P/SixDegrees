// <copyright file="WeatherForecastController.cs" company="Marcus Pallinger">
// Copyright (c) 2019 Marcus Pallinger. All rights reserved.
// Licensed under the BSD 2-clause license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
// Contains the WeatherForecastController class, which gives us the weather API interface.
// </summary>

namespace SixDegrees.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Controller to return API Endpoints.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing",
            "Bracing",
            "Chilly",
            "Cool",
            "Mild",
            "Warm",
            "Balmy",
            "Hot",
            "Sweltering",
            "Scorching",
        };

#pragma warning disable IDE0052 // Remove unread private members
        private readonly ILogger<WeatherForecastController> logger;
#pragma warning restore IDE0052 // Remove unread private members

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherForecastController"/> class.
        /// </summary>
        /// <param name="logger">A Logger supplied by DI.</param>
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Return the fake weather for the next 5 days.
        /// </summary>
        /// <returns>Fake weather for the next 5 days.</returns>
        [HttpGet]
#pragma warning disable CA1822 // Mark members as static
        public IEnumerable<WeatherForecast> Get()
#pragma warning restore CA1822 // Mark members as static
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast(
                DateTime.Now.AddDays(index),
#pragma warning disable CA5394 // Do not use insecure randomness
                rng.Next(-20, 55),
                Summaries[rng.Next(Summaries.Length)]))
#pragma warning restore CA5394 // Do not use insecure randomness
            .ToArray();
        }
    }
}
