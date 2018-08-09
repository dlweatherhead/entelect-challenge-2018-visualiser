using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EC2018 {
	public class AttackObjectPool : ExpandableObjectPool {

		public static AttackObjectPool current;

		void Awake() {
			current = this;
		}

		protected override string GetTag() {
			return Constants.Tags.Attack;
		}
	}

}
