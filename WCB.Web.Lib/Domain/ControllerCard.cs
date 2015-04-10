using System.Security.Policy;

namespace WCB.Web.Lib.Domain
{
    public class ControllerCard : IScrewIO, ISensor, IAir
    {
        public void SetScrew(State state)
        {
        }

        public State GetScrew()
        {
            return State.Enabled;
        }

        public void SetAir(State state)
        {
        }

        public State GetAir()
        {
            return State.Disabled;
        }

        public SensorValue GetSensor()
        {
            return new SensorValue(0);
        }
    }
}