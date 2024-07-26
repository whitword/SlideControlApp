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
            Speed = 10; // alapértelmezett sebesség
        }

        public async Task MoveToAsync(int targetPosition)
        {
            while (Position != targetPosition)
            {
                if (Position < targetPosition)
                    Position += Speed;
                else
                    Position -= Speed;

                await Task.Delay(10); // késleltetés a szimulált mozgáshoz
            }
        }
    }
}

