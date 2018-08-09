using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EC2018 {
	public class EnergyObjectPool : ExpandableObjectPool {

		public static EnergyObjectPool current;

		void Awake() {
			current = this;
		}

		protected override string GetTag() {
			return Constants.Tags.Energy;
		}
	}

}
