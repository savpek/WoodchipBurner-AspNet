namespace WCB.Web.Lib.Domain.Messages
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