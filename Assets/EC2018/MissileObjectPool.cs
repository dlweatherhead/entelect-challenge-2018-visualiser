﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EC2018 {
	public class MissileObjectPool : ExpandableObjectPool {

		public static MissileObjectPool current;

		void Awake() {
			current = this;
		}

		protected override string GetTag() {
			return Constants.Tags.Missile;
		}
	}
}
