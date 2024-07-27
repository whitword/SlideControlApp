using System.Reflection;
using System.Threading.Tasks;

namespace SlideControlApp.Models
{
    public class Motor
    {
        public int Position { get; private set; }
        public int Speed { get; set; }

        public Motor()
        {
            Position = 0;
            Speed = 1; // alapértelmezett sebesség
        }

        public async Task MoveToAsync(int targetPosition)
        {
            int minDelay = 10; // minimum késleltetés milliszekundumban
            int maxDelay = 100; // maximum késleltetés milliszekundumban

            var delay = Math.Max(minDelay, (int)(Speed * minDelay / maxDelay));

            while (Position != targetPosition)
            {
                int step = Math.Min(Speed, Math.Abs(targetPosition - Position));
                if (Position < targetPosition)
                    Position += step;
                else
                    Position -= step;

            await Task.Delay(delay); // késleltetés a szimulált mozgáshoz
            }
        }
    }
}

