﻿using Shuttle.Core.Infrastructure;

namespace Shuttle.Esb
{
    public class TransportMessagePipeline : Pipeline
    {
        public TransportMessagePipeline(IServiceBus bus)
        {
            Guard.AgainstNull(bus, "bus");

            State.SetServiceBus(bus);

            RegisterStage("Create")
                .WithEvent<OnAssembleMessage>()
                .WithEvent<OnAfterAssembleMessage>()
                .WithEvent<OnSerializeMessage>()
                .WithEvent<OnAfterSerializeMessage>()
                .WithEvent<OnEncryptMessage>()
                .WithEvent<OnAfterEncryptMessage>()
                .WithEvent<OnCompressMessage>()
                .WithEvent<OnAfterCompressMessage>();

            RegisterObserver(new AssembleMessageObserver());
            RegisterObserver(new SerializeMessageObserver());
            RegisterObserver(new CompressMessageObserver());
            RegisterObserver(new EncryptMessageObserver());
        }

        public bool Execute(TransportMessageConfigurator configurator)
        {
            Guard.AgainstNull(configurator, "options");

            State.SetTransportMessageContext(configurator);

            return base.Execute();
        }
    }
}