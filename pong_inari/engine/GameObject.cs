using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace pong_inari.engine
{
    public class GameObject : IDisposable
    {
        private bool _isCollided;
        public Shape? _collidedObj;
        private DependencyPropertyDescriptor _topDesc;
        private DependencyPropertyDescriptor _leftDesc;
        public GameWindow GWin { get; set; }
        public string Name { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public Brush View { get; set; }
        public Shape GameShape { get; set; }
        public bool IsMoving { get; set; }
        public bool IsCollided
        {
            get { return _isCollided; }
            set
            {
                _isCollided = value;
                if (OnHit is null) { return; }
                OnHit.Invoke(this, _collidedObj);
                _collidedObj = null;
            }
        }
        public event EventHandler<object?>? OnHit;
        public GameObject(string name, Shape? obj, bool isCollided = false)
        {
            Name = name;
            if (obj is null) { throw new NullReferenceException(); }
            View = obj.Fill;
            GameShape = obj;
            GameShape.Name = name;
            IsCollided = isCollided;
            IsMoving = false;
            _topDesc = DependencyPropertyDescriptor.FromProperty(Canvas.TopProperty, Canvas.TopProperty.OwnerType);
            _leftDesc = DependencyPropertyDescriptor.FromProperty(Canvas.LeftProperty, Canvas.LeftProperty.OwnerType);
            _topDesc.AddValueChanged(this.GameShape, PositionChangedHandler);
            _leftDesc.AddValueChanged(this.GameShape, PositionChangedHandler);
        }
        public async void PositionChangedHandler(object? sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                if (sender is null) { return; }
                UpdateCenter((Shape)sender);
            });

        }
        public Task UpdateCenter(Shape obj)
        {
            GameShape.Dispatcher.Invoke(() =>
            {
                CenterX = Canvas.GetLeft(obj) + (obj.ActualWidth / 2);
                CenterY = Canvas.GetTop(obj) + (obj.ActualHeight / 2);
            });
            return Task.CompletedTask;
        }
        public Task SetByCenter(double x, double y)
        {
            this.GameShape.Dispatcher.Invoke(() =>
            {
                Canvas.SetLeft(this.GameShape, x + (this.GameShape.ActualWidth / 2));
                Canvas.SetTop(this.GameShape, y + (this.GameShape.ActualHeight / 2));
            });
            return Task.CompletedTask;
        }
        public Task<bool> GotIntersected(Shape obj)
        {
            var isIntersected = GameShape.RenderedGeometry.FillContainsWithDetail(obj.RenderedGeometry);
            var isCollided = (isIntersected == IntersectionDetail.FullyContains ||
                          isIntersected == IntersectionDetail.Intersects ||
                          isIntersected == IntersectionDetail.FullyInside);
            if (isCollided)
            {
                IsCollided = true;
                _collidedObj = obj;
            }
            return Task.FromResult(IsCollided);
        }
        public void Dispose()
        {
            _topDesc.RemoveValueChanged(GameShape, PositionChangedHandler);
            _leftDesc.RemoveValueChanged(GameShape, PositionChangedHandler);
            GC.Collect();
        }
    }
}
