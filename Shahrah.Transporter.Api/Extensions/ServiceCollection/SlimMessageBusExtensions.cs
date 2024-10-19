using Confluent.Kafka;
using Shahrah.Framework.Events;
using Shahrah.Framework.Requests;
using Shahrah.Framework.Responses;
using Shahrah.Transporter.Application.Drivers.EventHandlers;
using Shahrah.Transporter.Application.OrderItems.EventHandlers;
using Shahrah.Transporter.Application.Orders.EventHandlers;
using Shahrah.Transporter.Application.Transporters.RequestHandlers;
using SlimMessageBus.Host;
using SlimMessageBus.Host.Hybrid;
using SlimMessageBus.Host.Kafka;
using SlimMessageBus.Host.Outbox;
using SlimMessageBus.Host.Serialization.Json;

namespace Shahrah.Transporter.Api.Extensions.ServiceCollection
{
    public static class SlimMessageBusExtensions
    {
        public static void AddMessageBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<FindTransporterRequestHandler>();

            const string orderTopicName = "Order-Topic";
            var kafkaGroup = configuration["Kafka:Group"];

            services.AddSlimMessageBus(mbb =>
            {
                mbb = mbb.AddServicesFromAssembly(typeof(SenderOrderClosedEventHandler).Assembly);
                mbb = mbb.AddChildBus("kafka", mbb =>
                {
                     mbb
                        .Produce<OrderBaseEvent>(x => x.DefaultTopic(orderTopicName).KeyProvider((order, _) => order.GetKeyBytes()))
                        .Produce<TransportersVehicleUpdatedEvent>(x => x.DefaultTopic(orderTopicName))

                        .Produce<FindDriverRequest>(x => x.DefaultTopic(x.Settings.MessageType.Name))
                        .Produce<AssignVehicleToDriverRequest>(x => x.DefaultTopic(x.Settings.MessageType.Name))
                        .Produce<UnassignVehicleFromDriverRequest>(x => x.DefaultTopic(x.Settings.MessageType.Name))
                        .Produce<AllocateDriverRequest>(x => x.DefaultTopic(x.Settings.MessageType.Name))

                        .Consume<AuctionClosedEvent>(x => x.WithConsumer<AuctionClosedEventHandler>().Topic(orderTopicName).KafkaGroup(kafkaGroup))
                        .Consume<OrderRegisteredBySenderEvent>(x => x.WithConsumer<OrderRegisteredBySenderEventHandler>().Topic(orderTopicName).KafkaGroup(kafkaGroup))
                        .Consume<DriverAccountClosedEvent>(x => x.WithConsumer<DriverAccountClosedEventHandler>().Topic(orderTopicName).KafkaGroup(kafkaGroup))
                        .Consume<BidCanceledEvent>(x => x.WithConsumer<BidCanceledEventHandler>().Topic(orderTopicName).KafkaGroup(kafkaGroup))
                        .Consume<ConfirmTransporterOfferPriceEvent>(x => x.WithConsumer<ConfirmTransporterOfferPriceEventHandler>().Topic(orderTopicName).KafkaGroup(kafkaGroup))
                        .Consume<SenderConfirmedTripEndedEvent>(x => x.WithConsumer<SenderConfirmedTripEndedEventHandler>().Topic(orderTopicName).KafkaGroup(kafkaGroup))
                        .Consume<SenderConfirmedLoadingEvent>(x => x.WithConsumer<SenderConfirmedLoadingEventHandler>().Topic(orderTopicName).KafkaGroup(kafkaGroup))
                        .Consume<DriverConfirmedTripEndedEvent>(x => x.WithConsumer<DriverConfirmedTripEndedEventHandler>().Topic(orderTopicName).KafkaGroup(kafkaGroup))
                        .Consume<DriverConfirmedLoadingEvent>(x => x.WithConsumer<DriverConfirmedLoadingEventHandler>().Topic(orderTopicName).KafkaGroup(kafkaGroup))
                        .Consume<SenderOrderClosedEvent>(x => x.WithConsumer<SenderOrderClosedEventHandler>().Topic(orderTopicName).KafkaGroup(kafkaGroup))

                        .Consume<TestEvent>(x => x.WithConsumer<TransactionTestEventHandler>().Topic(orderTopicName).KafkaGroup(kafkaGroup))

                        .Handle<FindTransporterRequest, FindTransporterResponse>((HandlerBuilder<FindTransporterRequest, FindTransporterResponse> s) =>
                         {

                             s.Topic(s.ConsumerSettings.MessageType.Name, t =>
                             {
                                 t.WithHandler<FindTransporterRequestHandler>().KafkaGroup(configuration["Kafka:Group"]);
                             });
                         })
                        .ExpectRequestResponses(x =>
                        {
                            x.ReplyToTopic(configuration["Kafka:ResponsesTopic"]);
                            x.KafkaGroup(configuration["Kafka:Group"]);
                            x.DefaultTimeout(TimeSpan.FromSeconds(int.Parse(configuration["Kafka:TimeOut"])));
                        })
                        .PerMessageScopeEnabled(true)
                        .WithProviderKafka(x =>
                        {
                            x.BrokerList = configuration["Kafka:Brokers"];
                            x.ProducerConfig = config =>
                            {
                                config.LingerMs = 5;
                                config.SocketNagleDisable = true;
                            };
                            x.ConsumerConfig = config =>
                            {
                                config.FetchErrorBackoffMs = 1;
                                config.SocketNagleDisable = true;
                                config.AllowAutoCreateTopics = true;
                                config.SessionTimeoutMs = 7000;
                                config.AutoOffsetReset = AutoOffsetReset.Earliest;
                                config.Acks = Acks.All;
                            };
                        })
                        .UseOutbox()
                        .UseTransactionScope();
                })
                .WithSerializer<JsonMessageSerializer>()
                .AddJsonSerializer()
                .WithProviderHybrid();
            });

            //services.AddMessageBusOutboxUsingDbContext<ApplicationDbContext>(opts =>
            //{
            //    opts.PollBatchSize = 100;
            //    opts.MessageCleanup.Interval = TimeSpan.FromSeconds(10);
            //    opts.MessageCleanup.Age = TimeSpan.FromMinutes(1);
            //    //opts.TransactionIsolationLevel = System.Data.IsolationLevel.RepeatableRead;
            //    //opts.Dialect = SqlDialect.SqlServer;
            //});
        }
    }
}