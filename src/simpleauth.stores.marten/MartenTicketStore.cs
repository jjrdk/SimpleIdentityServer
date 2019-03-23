﻿namespace SimpleAuth.Stores.Marten
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using global::Marten;
    using SimpleAuth.Shared.Models;
    using SimpleAuth.Shared.Repositories;

    /// <summary>
    /// Defines the marten based ticket store.
    /// </summary>
    /// <seealso cref="SimpleAuth.Shared.Repositories.ITicketStore" />
    public class MartenTicketStore : ITicketStore
    {
        private readonly Func<IDocumentSession> _sessionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MartenScopeRepository"/> class.
        /// </summary>
        /// <param name="sessionFactory">The session factory.</param>
        public MartenTicketStore(Func<IDocumentSession> sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        /// <inheritdoc />
        public async Task<bool> Add(Ticket ticket, CancellationToken cancellationToken)
        {
            using (var session = _sessionFactory())
            {
                session.Store(ticket);
                await session.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return true;
            }
        }

        /// <inheritdoc />
        public async Task<bool> Remove(string ticketId, CancellationToken cancellationToken)
        {
            using (var session = _sessionFactory())
            {
                session.Delete<Ticket>(ticketId);
                await session.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return true;
            }
        }

        /// <inheritdoc />
        public async Task<Ticket> Get(string ticketId, CancellationToken cancellationToken)
        {
            using (var session = _sessionFactory())
            {
                var ticket = await session.LoadAsync<Ticket>(ticketId, cancellationToken).ConfigureAwait(false);

                return ticket;
            }
        }
    }
}