using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Ai
{
	public class SelectorNode : BehaviourNode
	{
		private List<BehaviourNode> _nodes;

		public SelectorNode(List<BehaviourNode> nodes)
		{
			_nodes = nodes;
		}

		// Evaluate the first non-failing node and then stop.
		public override NodeState Evaluate()
		{
			foreach (var node in _nodes)
			{
				switch (node.Evaluate())
				{
					case NodeState.SUCCESS:
					{
						return NodeState.SUCCESS;
					}
					case NodeState.RUNNING:
					{
						return NodeState.RUNNING;
					}
					case NodeState.FAILURE:
					{
						continue;
					}
				}
			}

			return NodeState.FAILURE;  // None of the evaluated nodes had status SUCCESS or RUNNING.
		}
	}
}
