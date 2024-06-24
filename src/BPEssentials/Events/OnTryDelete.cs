using System.Text.RegularExpressions;
using BrokeProtocol.API;
using BrokeProtocol.LiteDB;
using BrokeProtocol.Managers;
using BrokeProtocol.Utility;

namespace BPEssentials.Events
{
	public class OnDelete : IScript
	{
		[Target(GameSourceEvent.ManagerTryDelete, ExecutionMode.PreEvent)]
		public bool OnTryDelete(ConnectData connectionData)
		{
			if (SvManager.Instance.TryGetUserData(connectionData.username, out User user) && Core.Instance.Settings.General.DisableAccountOverwrite)
			{
				SvManager.Instance.RegisterFail(connectionData.connection, "This name has already been registered and this server has disabled overwriting accounts!");
				return false;
			}

			return true;
		}
	}
}
