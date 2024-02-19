using EventSourcing.Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventSourcing.Repository
{
    public class AccountRespository
    {
        public void Save(Account account)
        {
            var evts = account.UnCommittedEvents.ToList();
            evts.Select(ev => new EventDescriptor(account.Id, 0, JsonSerializer.Serialize(ev), Environment.UserName, Guid.NewGuid().ToString());
            //:TODO save in database
            // _context.Events.AddRange(evts)
            account.Commit();
        }
        public Account Load(Guid aggregateId)
        {   //:TODO load from database
            // _context.Events.Where(e=>e.AggragateId=aggregateId).OrderBy(x=>x.CreatedDate)
            var account = new Account();
            return account;
        }

        private record EventDescriptor(Guid AggregateId,long EventId,string EventData,string User,string CorrelationId);
    }
}
