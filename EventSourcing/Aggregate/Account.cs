namespace EventSourcing.Aggregate
{
    public class Account
    {
        private Guid Id { get; set; }
        private decimal CurrentAmount { get; set; }
        private decimal CurrentBalanced {  get; set; }
        public ICollection<Event> UnCommittedEvents { get; } = new List<Event>();
        private State CurrentState { get; set; } = State.NotSet;

        private enum State
        {
            NotSet,
            Opened,
            Closed,
        }
        public void Rehydrate(ICollection<Event> evts)
        {
            foreach (var evt in evts)
            {
                ((dynamic) this).Apply((dynamic) evt);
            }
        }


        public void Apply(AccountOpened evt) => CurrentState = State.Opened;
        public void Apply(MoneyTransafered evt) => CurrentBalanced -= evt.amount;
        public void Commit() => UnCommittedEvents.Clear();
        public void Open(string Owner, string BankNumber)
        {
            var evt = new AccountOpened(Id, Owner, BankNumber);
            UnCommittedEvents.Add(evt);
            Apply(evt);
        }

        public void Close()
        {
            if (CurrentState != State.Opened)
                throw new InvalidOperationException();
        }

        public void TransferMoney(decimal amount, string bankNumber)
        {
            if (CurrentAmount < amount)
                throw new InvalidOperationException("not enough money available");

            var evt = new MoneyTransafered(Id, amount, bankNumber);
            UnCommittedEvents.Add(evt);
            Apply(evt);
        }
    }



    //create class with default constructor in .net8
    public class MoneyTransafered(Guid id, decimal amount, string bankNumber) : Event
    {
        public Guid id { get; private set; } = id;
        public decimal amount { get; private set; } = amount;
        public string bankNumber { get; private set; } = bankNumber;


    }
    public class AccountOpened(Guid id, string owner, string bankNumber) : Event
    {
        public Guid Id { get; private set; } = id;
        public string Owner { get; private set; } = owner;
        public string BankNumber { get; private set; } = bankNumber;
    }

    public class Event
    {

    }
}
