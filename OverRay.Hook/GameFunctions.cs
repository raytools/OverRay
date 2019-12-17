namespace OverRay.Hook
{
    public partial class GameFunctions
    {
        public GameFunctions(RemoteInterface _interface)
        {
            Interface = _interface;
        }

        private RemoteInterface Interface { get; }
    }
}