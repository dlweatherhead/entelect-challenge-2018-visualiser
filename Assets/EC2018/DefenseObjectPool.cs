using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EC2018 {
	public class DefenseObjectPool : ExpandableObjectPool {

		public static DefenseObjectPool current;

		void Awake() {
			current = this;
		}

		protected override string GetTag() {
			return Constants.Tags.Defense;
		}
	}
}