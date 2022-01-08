using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Ai
{
	public class DoEachNode : BehaviourNode
	{
		private List<BehaviourNode> _nodes;

		public DoEachNode(List<BehaviourNode> nodes)
		{
			_nodes = nodes;
		}

		// Evaluate each node, stopping as soon as one of them fails.
		public override NodeState Evaluate()
		{
			var isChildRunning = false;

			foreach (var node in _nodes)
			{
				switch (node.Evaluate())
				{
					case NodeState.SUCCESS:
					{
						continue;
					}
					case NodeState.RUNNING:
					{
						isChildRunning = true;
						continue;
					}
					case NodeState.FAILURE:
					{
						return NodeState.FAILURE;
					}
				}
			}

			if (isChildRunning)
			{
				return NodeState.RUNNING;  // Some of the evaluated nodes are still running.
			}
			else
			{ 
				return NodeState.SUCCESS;  // All of the evaluated nodes had status SUCCESS.
			}
		}
	}
}
