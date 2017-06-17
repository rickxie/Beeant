using Winner.Queue;

namespace Beeant.Distributed.Service.Host.Service
{
    public class QueueService : Winner.Queue.QueueService
    {
        public QueueService()
        {
            Queuer = new LocalQueue();
        }
    }
}
