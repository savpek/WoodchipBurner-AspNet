namespace WCB.Web.Domain
{
    public interface IAir
    {
        void SetAir(State state);
        State GetAir();
    }
}