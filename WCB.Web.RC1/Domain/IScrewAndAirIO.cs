namespace WCB.Web.RC1.Domain
{
    public interface IScrewAndAirIO

    {
        State GetScrew();
        void SetScrew(State state);
        State GetAir();
        void SetAir(State state);
    }
}