using Microsoft.Extensions.Options;

namespace spgnn.Models.Configurations
{
    public interface IApplicationConfiguration
    {
        IOptions<FileConfiguration> fileConfiguration { get; }
    }
}