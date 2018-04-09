using System;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace StorageQueue
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var queue = GetQueueReference();

            CreateQueue(queue);

            QueueMessages(queue);

            DequeueSingleMessage(queue);

            DequeueMultipleMessages(queue);
        }

        private static CloudQueue GetQueueReference()
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            var queueClient = storageAccount.CreateCloudQueueClient();

            return queueClient.GetQueueReference("queue");
        }

        private static void CreateQueue(CloudQueue queue)
        {
            queue.CreateIfNotExists();
        }

        private static void QueueMessages(CloudQueue queue)
        {
            queue.AddMessage(new CloudQueueMessage("Queued message 1"));
            queue.AddMessage(new CloudQueueMessage("Queued message 2"));
            queue.AddMessage(new CloudQueueMessage("Queued message 3"));
        }

        private static void DequeueSingleMessage(CloudQueue queue)
        {
            var message = queue.GetMessage(new TimeSpan(0, 5, 0));

            if (message != null)
            {
                Console.WriteLine(message.AsString);
            }
        }

        private static void DequeueMultipleMessages(CloudQueue queue)
        {
            var messages = queue.GetMessages(3, new TimeSpan(0, 5, 0));

            foreach (var item in messages)
            {
                Console.WriteLine(item.AsString);
            }
        }
    }
}