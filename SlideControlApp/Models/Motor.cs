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
            while (Position != targetPosition)
            {
                if (Position < targetPosition)
                    Position += 1;
                else
                    Position -= 1;

                await Task.Delay(100/Speed); // késleltetés a szimulált mozgáshoz
            }
        }
    }
}

