using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Ai
{
	public class SequenceNode : BehaviourNode
	{
		private List<BehaviourNode> _nodes;

		public SequenceNode(List<BehaviourNode> nodes)
		{
			_nodes = nodes;
		}

		// Evaluate each node, stopping as soon as one of them fails.
		public override NodeState Evaluate()
		{
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
						return NodeState.RUNNING;
					}
					case NodeState.FAILURE:
					{
						return NodeState.FAILURE;
					}
				}
			}

			return NodeState.SUCCESS;  // All of the evaluated nodes had status SUCCESS.
		}
	}
}
