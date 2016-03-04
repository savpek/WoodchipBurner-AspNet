namespace WCB.Web.RC1.Domain.Messages
{
    public class ScrewStateUpdatedMessage
    {
        public State State { get; }

        public ScrewStateUpdatedMessage(State state)
        {
            State = state;
        }
    }
}