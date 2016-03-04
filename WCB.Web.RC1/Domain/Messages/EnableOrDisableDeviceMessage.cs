namespace WCB.Web.RC1.Domain.Messages
{
    public class EnableOrDisableDeviceMessage
    {
        public State DesiredState { get; }

        public EnableOrDisableDeviceMessage(State desiredState)
        {
            DesiredState = desiredState;
        }
    }
}