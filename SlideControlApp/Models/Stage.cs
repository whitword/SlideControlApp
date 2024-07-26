using System.Threading.Tasks;

namespace SlideControlApp.Models
{
    public class Stage
    {
        public Motor MotorX { get; private set; }
        public Motor MotorY { get; private set; }
        public Motor MotorZ { get; private set; }

        public Stage()
        {
            MotorX = new Motor();
            MotorY = new Motor();
            MotorZ = new Motor();
        }

        public async Task MoveToAsync(int x, int y, int z)
        {
            await Task.WhenAll(
                MotorX.MoveToAsync(x),
                MotorY.MoveToAsync(y),
                MotorZ.MoveToAsync(z)
            );
        }
    }
}
