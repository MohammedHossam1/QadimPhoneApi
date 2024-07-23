using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.RepositoriesInterfaces;
using Talabat.Repositories.Repositories;

namespace Talabat.APIs.Extentions
{
    public static class AddApplicationServicesExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepositories<>));//allow dependency inj product ,pbrand,pcategory....

            services.Configure<ApiBehaviorOptions>(option =>
            {
                option.InvalidModelStateResponseFactory = (actionContext =>
                {
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                   .SelectMany(p => p.Value.Errors)
                                   .Select(e => e.ErrorMessage).ToArray();
                    var apivalidateresponse = new ApiValidationResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(apivalidateresponse);
                });

            });
        return services;
        }


    }
}
