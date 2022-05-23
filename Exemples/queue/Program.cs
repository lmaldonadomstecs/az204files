﻿using System.Threading.Tasks; 
using Azure.Messaging.ServiceBus;



namespace Queue
{
    class Program
    {

        // connection string to your Service Bus namespace
        static string connectionString = "Endpoint=sb://notreentreprise.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=mHL4W4+m5TgZiRlufWB1mVaASCCfmrDKlB5wj1nKQxA=";
        // name of your Service Bus topic
        static string queueName = "messages";

        // the client that owns the connection and can be used to create senders and receivers
        static ServiceBusClient client;
        // the sender used to publish messages to the queue
        static ServiceBusSender sender;
        // number of messages to be sent to the queue
        private const int numOfMessages = 7;


        static async Task Main()
        {
            // Create the clients that we'll use for sending and processing messages.
            client = new ServiceBusClient(connectionString);
            sender = client.CreateSender(queueName);
            

            // // create a batch 
            // using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
            // for (int i = 1; i <= numOfMessages; i++)
            // {
            //     // try adding a message to the batch
            //     if (!messageBatch.TryAddMessage(new ServiceBusMessage($"Message {i}")))
            //     {
            //         // if an exception occurs
            //         throw new Exception($"Exception {i} has occurred.");
            //     }
            // }
            // try 
            // {
            //     // Use the producer client to send the batch of messages to the Service Bus queue
            //     await sender.SendMessagesAsync(messageBatch);
            //     Console.WriteLine($"A batch of {numOfMessages} messages has been published to the queue.");
            // }
            // finally
            // {
            //     // Calling DisposeAsync on client types is required to ensure that network
            //     // resources and other unmanaged objects are properly cleaned up.
            //     await sender.DisposeAsync();
            //     await client.DisposeAsync();
            // }
            // Console.WriteLine("Press any key to end the application");
            // Console.ReadKey();




            
            // create a processor that we can use to process the messages
            processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());
            try
            {
                // add handler to process messages
                processor.ProcessMessageAsync += MessageHandler;
                // add handler to process any errors
                processor.ProcessErrorAsync += ErrorHandler;
                // start processing 
                await processor.StartProcessingAsync();
                Console.WriteLine("Wait for a minute and then press any key to end the processing");
                Console.ReadKey();
                // stop processing 
                Console.WriteLine("\nStopping the receiver...");
                await processor.StopProcessingAsync();
                Console.WriteLine("Stopped receiving messages");
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await processor.DisposeAsync();
                await client.DisposeAsync();
            }
        }



        // the processor that reads and processes messages from the queue
        static ServiceBusProcessor processor;

        // handle received messages
        static async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            Console.WriteLine($"Received: {body}");
            // complete the message. messages is deleted from the queue. 
            await args.CompleteMessageAsync(args.Message);
        }
        // handle any errors when receiving messages
        static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}