using System.Threading.Tasks;

public interface IBlogCommunicationService
{
    Task StartConsuming();
    Task StopConsuming();
}
