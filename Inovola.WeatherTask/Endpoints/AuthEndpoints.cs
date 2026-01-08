using FluentValidation;
using Inovola.Application.DTOs.Auth;
using Inovola.Application.Services;

namespace Inovola.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/register",
            async (
                RegisterRequest request,
                IValidator<RegisterRequest> validator,
                AuthService authService) =>
            {
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                    return Results.ValidationProblem(validationResult.ToDictionary());

                var result = await authService.RegisterAsync(request);
                return Results.Ok(result);
            });

        app.MapPost("/api/auth/login",
            async (
                LoginRequest request,
                IValidator<LoginRequest> validator,
                AuthService authService) =>
            {
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                    return Results.ValidationProblem(validationResult.ToDictionary());

                var result = await authService.LoginAsync(request);
                return Results.Ok(result);
            });
    }
}
