using OverRay.Hook.GameFunctions;

namespace OverRay.Hook.Mod
{
    public class GameManager
    {
        public GameManager()
        {
            InitHooks();

            Hud = new Hud(this);
        }

        public EngineFunctions Engine { get; } = new EngineFunctions();
        public GfxFunctions Graphics { get; } = new GfxFunctions();
        public TextFunctions Text { get; } = new TextFunctions();

        private Hud Hud { get; }

        private void InitHooks()
        {
            // Create all hooks here
            Engine.VEngine.CreateHook();
            Text.DrawsTexts.CreateHook();
        }
    }
}