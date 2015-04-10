namespace WCB.Web.Lib.Domain
{
    public interface IAir
    {
        void SetAir(State state);
        State GetAir();
    }
}