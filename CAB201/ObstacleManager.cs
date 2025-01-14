
namespace OOD_Assignment
{
    public class ObstacleManager
    {
        private Grid dotGrid;

        public ObstacleManager(Grid grid)
        {
            dotGrid = grid;
        }

        public void InsertGuard(Guard guard)
        {
            dotGrid.AddGuard(guard);
        }

        public void InsertFence(Fence fence)
        {
            dotGrid.AddFence(fence);
        }

        public void InsertSensor(Sensor sensor)
        {
            dotGrid.AddSensor(sensor);
        }
        public void InsertCamera(Camera camera)
        {
            dotGrid.AddCamera(camera);
        }

    }
}
