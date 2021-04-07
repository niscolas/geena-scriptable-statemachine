using System.Collections.Generic;
using System.Linq;
using XNode;

namespace ScriptableStateMachine.Utilities
{
	public static class XNodeExtensions
	{
		public static TNode TryGetNoInputNode<TNode>(this NodeGraph nodeGraph) where TNode : Node
		{
			return (TNode) nodeGraph.nodes.Find(
				node =>
					node is TNode &&
					node.Inputs.All(port => !port.IsConnected)
			);
		}

		public static NodePort[] GetDynamicOutputs(this Node node, string basePortName)
		{
			List<NodePort> nodePorts = new List<NodePort>();

			int i = 0;
			while (true)
			{
				NodePort nodePort = node.GetOutputPort($"{basePortName} {i}");

				if (nodePort != null)
				{
					nodePorts.Add(nodePort);
				}
				else
				{
					break;
				}

				i++;
			}

			return nodePorts.ToArray();
		}
	}
}