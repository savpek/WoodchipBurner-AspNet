namespace WCB.Web.Domain.Messages
{
    public class AirStateUpdatedMessage
    {
        public State State { get; }

        public AirStateUpdatedMessage(State state)
        {
            State = state;
        }
    }
}