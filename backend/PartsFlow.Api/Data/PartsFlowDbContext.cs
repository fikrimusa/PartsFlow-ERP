using Microsoft.EntityFrameworkCore;

namespace PartsFlow.Api.Data;

public class PartsFlowDbContext(DbContextOptions<PartsFlowDbContext> options) : DbContext(options)
{
}
