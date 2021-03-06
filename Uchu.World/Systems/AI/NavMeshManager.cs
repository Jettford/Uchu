using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Uchu.Navigation;

namespace Uchu.World.Systems.AI
{
    public class NavMeshManager
    {
        public bool Enabled { get; }
        
        public Zone Zone { get; }
        
        public Solver Solver { get; }

        public NavMeshManager(Zone zone, bool enabled)
        {
            Enabled = enabled;
            
            Zone = zone;
            
            Solver = new Solver();
        }

        public async Task GeneratePointsAsync()
        {
            const float scale = 3.125f;

            var terrain = Zone.ZoneInfo.TerrainFile;

            var heightMap = terrain.GenerateHeightMap();

            var inGameValues = new Dictionary<int, Dictionary<int, Vector3>>();

            var centerX = (heightMap.GetLength(0) - 1) / 2;
            var centerY = (heightMap.GetLength(1) - 1) / 2;

            var min = float.MaxValue;
            
            for (var x = 0; x < heightMap.GetLength(0); x++)
            {
                for (var y = 0; y < heightMap.GetLength(1); y++)
                {
                    var value = heightMap[x, y];

                    if (value < min)
                        min = value;

                    var realX = x - centerX;
                    var realY = y - centerY;
                    
                    var inGame = new Vector3(realX, 0, realY);

                    inGame *= scale;

                    inGame.Y = value;

                    if (inGameValues.TryGetValue(x, out var dict))
                    {
                        dict[y] = inGame;
                    }
                    else
                    {
                        inGameValues[x] = new Dictionary<int, Vector3>
                        {
                            [y] = inGame
                        };
                    }
                }
            }

            await Solver.GenerateAsync(inGameValues, heightMap.GetLength(0), heightMap.GetLength(1), min);
        }

        public Vector3[] GeneratePath(Vector3 start, Vector3 end)
        {
            return Solver.GeneratePath(start, end);
        }

        public Vector3 FindClosestNode(Vector3 position)
        {
            var node = Solver.GetClosest(position);

            return node.Position.ToVector3();
        }
    }
}