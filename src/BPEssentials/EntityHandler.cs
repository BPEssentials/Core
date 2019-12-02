using BrokeProtocol.Entities;
using BrokeProtocol.Managers;
using System.Collections.Generic;

namespace BPEssentials
{
    public class EntityHandler
    {

        public Dictionary<string, ShItem> Items { get; set; } = new Dictionary<string, ShItem>();

        public Dictionary<string, ShTransport> Vehicles { get; set; } = new Dictionary<string, ShTransport>();

        public void LoadEntities()
        {
            foreach (var entity in SceneManager.Instance.entityCollection)
            {
                if (entity.Value == null)
                {
                    continue;
                }
                if (entity.Value is ShItem)
                {
                    Items.Add(entity.Value.name, entity.Value as ShItem);
                }
                if (entity.Value is ShTransport && !(entity.Value is ShParachute))
                {
                    Vehicles.Add(entity.Value.name, entity.Value as ShTransport);
                }
            }
        }
    }
}
