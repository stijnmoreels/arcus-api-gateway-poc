﻿using System;
using Arcus.Observability.Correlation;
using Arcus.Shared.Correlation.WebApi;
using Arcus.WebApi.Logging.Correlation;
using GuardNet;
using Microsoft.AspNetCore.Http;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Adds operation and transaction correlation to the application.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static partial class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds operation and transaction correlation to the application.
        /// </summary>
        /// <param name="services">The services collection containing the dependency injection services.</param>
        /// <param name="configureOptions">The function to configure additional options how the correlation works.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="services"/> is <c>null</c>.</exception>
        public static IServiceCollection AddHttpCorrelationFromPoc(
            this IServiceCollection services,
            Action<CorrelationInfoOptions> configureOptions = null)
        {
            Guard.NotNull(services, nameof(services));

            services.AddHttpContextAccessor();
            services.AddCorrelation(
                serviceProvider =>
                {
                    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                    return new HttpCorrelationInfoAccessor(httpContextAccessor);
                },
                configureOptions);
            services.AddScoped<HttpWithUpstreamApiGatewayCorrelation>();

            return services;
        }
    }
}