using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests.Integration
{
    public class TestWebApplicationFactory : WebApplicationFactory<Program>
    {
    }
}
