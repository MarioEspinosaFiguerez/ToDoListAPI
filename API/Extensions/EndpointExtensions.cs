namespace API.Extensions
{
    public static class EndpointExtensions
    {
        public static void MapAllEndpoints(this IEndpointRouteBuilder app)
        {
            var endpointType = typeof(IEndpoint);

            var endpoints = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => !t.IsAbstract && endpointType.IsAssignableFrom(t))
                .Select(Activator.CreateInstance)
                .Cast<IEndpoint>();

            foreach (var endpoint in endpoints)
            {
                endpoint.MapEndpoint(app);
            }
        }

    }
}