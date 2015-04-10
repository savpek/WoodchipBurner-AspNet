using System.Reactive.Subjects;

namespace WCB.Web.Lib.Domain
{
    public interface IScrewIO
    {
        State GetScrew();
        void SetScrew(State state);
    }

    public class ScrewIO : IScrewIO
    {
        private State _state;

        public State GetScrew()
        {
            return _state;
        }

        public void SetScrew(State state)
        {
            _state = state;
        }
    }
}