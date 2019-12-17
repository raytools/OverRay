namespace OverRay.Hook.Utils
{
    public static class Log
    {
        public static int ScrollbackSize { get; set; } = 1000;
        public static bool IsPaused { get; set; }
        public static ObservableQueue<string> Messages { get; } = new ObservableQueue<string>();

        public static void Add(string message)
        {
            if (IsPaused) return;

            while (Messages.Count > ScrollbackSize) Messages.Dequeue();
            Messages.Enqueue(message);
        }
    }
}