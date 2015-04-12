using System.Security.Cryptography.X509Certificates;

namespace WCB.Web.Domain
{
    public interface IScrewAndAirIO

    {
        State GetScrew();
        void SetScrew(State state);
        State GetAir();
        void SetAir(State state);
    }
}