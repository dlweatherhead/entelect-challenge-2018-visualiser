using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EC2018.Entities;

public class Round {

	public Round(GameState statePlayerA, GameState statePlayerB) {
		this.statePlayerA = statePlayerA;
		this.statePlayerB = statePlayerB;
	}

	public GameState statePlayerA {
		get;
		set;
	}

	public GameState statePlayerB {
		get;
		set;
	}
}
