﻿namespace SimpleAuth.Shared.Events
{
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the composite event publisher.
    /// </summary>
    public class CompositeEventPublisher : IEventPublisher
    {
        private readonly IEventPublisher[] _publishers;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeEventPublisher"/> class.
        /// </summary>
        public CompositeEventPublisher(params IEventPublisher[] publishers)
        {
            _publishers = publishers.ToArray();
        }

        public Task Publish<T>(T evt)
            where T : Event
        {
            return Task.WhenAll(_publishers.Select(_ => _.Publish(evt)).ToArray());
        }
    }
}