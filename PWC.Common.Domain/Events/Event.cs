namespace PWC.Common.Domain.Events
{
    public abstract class Event
    {
        public Guid id { get; protected set; } = Guid.NewGuid();
        public DateTime Timestamp { get; protected set; }

        public Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}
