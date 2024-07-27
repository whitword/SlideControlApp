using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using SlideControlApp.Models;
using System.Threading;
using System.Windows.Media.Media3D;

namespace SlideControlApp
{
    public partial class MainWindow : Window
    {
        private Stage _stage;

        // Logikai koordináta-rendszer (ablak)
        private const double WindowWidth = 1000;
        private const double WindowHeight = 1000;

        public MainWindow()
        {
            InitializeComponent();
            _stage = new Stage();
            InitializeTransform();
        }

        private void InitializeTransform()
        {
            // Számítsuk ki a skálázási tényezőket
            double scaleX = StageCanvas.Width / WindowWidth;
            double scaleY = StageCanvas.Height / WindowHeight;

            // Az Y tengely invertálása, hogy lefelé növekedjen
            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(new ScaleTransform(scaleX, -scaleY)); // Az Y tengely invertálása
            transformGroup.Children.Add(new TranslateTransform(0, StageCanvas.Height)); // Tolás az ablak bal alsó sarkába

            // Alkalmazzuk a transzformációt a Canvas-ra
            StageCanvas.RenderTransform = transformGroup;
        }
        
        async void StageCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var position = e.GetPosition(StageCanvas);

            // Logikai koordináták kiszámítása
            double x = position.X * WindowWidth / StageCanvas.ActualWidth;
            double y = position.Y * WindowHeight / StageCanvas.ActualHeight;

                int ix = (int)x;
                int iy = (int)y;
                int iz = 0; // Példa kedvéért Z-t 0-nak vesszük

                await _stage.MoveToAsync(ix, iy, iz);

                UpdateStagePosition();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetSpeeds_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(SpeedX.Text, out int speedX))
                _stage.MotorX.Speed = speedX;
            if (int.TryParse(SpeedY.Text, out int speedY))
                _stage.MotorY.Speed = speedY;
            if (int.TryParse(SpeedZ.Text, out int speedZ))
                _stage.MotorZ.Speed = speedZ;
        }

        private async void MoveX_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(MoveX.Text, out int x))
            {
                await _stage.MotorX.MoveToAsync(x);
                UpdateStagePosition();
            }
            else
            {
                MessageBox.Show("Please enter a valid integer for X position.");
            }
        }

        private async void MoveY_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(MoveY.Text, out int y))
            {
                await _stage.MotorY.MoveToAsync(y);
                UpdateStagePosition();
            }
            else
            {
                MessageBox.Show("Please enter a valid integer for Y position.");
            }
        }

        private async void MoveZ_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(MoveZ.Text, out int z))
            {
                await _stage.MotorZ.MoveToAsync(z);
                UpdateStagePosition();
            }
            else
            {
                MessageBox.Show("Please enter a valid integer for Z position.");
            }
        }

        private void UpdateStagePosition()
        {
            // Számítsuk ki a skálázási tényezőket az ablak méretei alapján
            double scaleX = StageCanvas.ActualWidth / WindowWidth;
            double scaleY = StageCanvas.ActualHeight / WindowHeight;

            // Asztal pozíciójának frissítése
            double stageX = _stage.MotorX.Position * scaleX;
            double stageY = (WindowHeight - _stage.MotorY.Position) * scaleY; // Invertálás figyelembe vétele

            StagePosition.SetValue(Canvas.LeftProperty, stageX - StagePosition.Width / 2);
            StagePosition.SetValue(Canvas.TopProperty, StageCanvas.ActualHeight - stageY - StagePosition.Height / 2);

            // Motorok pozíciójának frissítése
            double motorX = _stage.MotorX.Position * scaleX;
            double motorY = (WindowHeight - _stage.MotorY.Position) * scaleY; // Invertálás figyelembe vétele
            double motorZ = _stage.MotorZ.Position * scaleX; // Ha az Z tengely is skálázható az X tengelyhez hasonlóan

            MotorXPosition.SetValue(Canvas.LeftProperty, motorX - MotorXPosition.Width / 2);
            MotorXPosition.SetValue(Canvas.TopProperty, StageCanvas.ActualHeight - MotorXPosition.Height); // Motor X pozíció
            MotorYPosition.SetValue(Canvas.LeftProperty, StageCanvas.ActualWidth - MotorYPosition.Width); // Motor Y pozíció
            MotorYPosition.SetValue(Canvas.TopProperty, StageCanvas.ActualHeight - motorY - MotorYPosition.Height / 2); // Motor Y pozíció
            MotorZPosition.SetValue(Canvas.LeftProperty, motorZ - MotorZPosition.Width / 2); // Motor Z pozíció
            MotorZPosition.SetValue(Canvas.TopProperty, StageCanvas.ActualHeight - MotorZPosition.Height / 2); // Motor Z pozíció
        }
    }
}
