using UnityEngine;
using UnityEngine.UI;
using EC2018.Entities;
using EC2018.Enums;
using TMPro;

namespace EC2018
{
	public class UIManager : MonoBehaviour {
		public Text PlayerAName;
		public Text PlayerBName;
		public Text HealthA;
		public Text HealthB;
		public Text EnergyA;
		public Text EnergyB;
		public Text ScoreA;
		public Text ScoreB;
		public Text RoundText;

        public Image ironCurtainPlayerA;
        public Image ironCurtainPlayerB;

		public RectTransform healthBarA;
		public RectTransform healthBarB;

		public GameObject FinalGameHolder;
        public GameObject playerAWinHolder;
        public GameObject playerBWinHolder;

        public TextMeshProUGUI winningPlayerText;
        public TextMeshProUGUI winningPlayerStatsText;
        public TextMeshProUGUI losingPlayerStatsText;

        public BarrierHealth barrierHealthA;
        public BarrierHealth barrierHealthB;

        GameManager gameManager;

		bool pulseIronCurtainPlayerA;
		bool pulseIronCurtainPlayerB;

        PlayerType winningPlayer;

        void Awake() {
			gameManager = GameObject.FindGameObjectWithTag (Constants.Tags.GameManager).GetComponent<GameManager> ();
		}

		void Update() {
			if(Input.GetKey (KeyCode.Space)) {
				gameManager.OnPauseInteraction ();
			}
			if(Input.GetKey (KeyCode.R)) {
				gameManager.NavigateToReplayMenu ();
			}
			if(Input.GetKey (KeyCode.Q)) {
				Application.Quit ();
			}

			if(pulseIronCurtainPlayerA) {
				ironCurtainPlayerA.color = Color.Lerp (ColorUtil.playerA, ColorUtil.white, Mathf.PingPong (Time.time, 1f));
			}

			if(pulseIronCurtainPlayerB) {
				ironCurtainPlayerB.color = Color.Lerp (ColorUtil.playerB, ColorUtil.white, Mathf.PingPong (Time.time, 1f));
			}
		}

		public void SetPlayerNames(string playerAName, string playerBName) {
			PlayerAName.text = playerAName;
			PlayerBName.text = playerBName;
		}

		public void UpdateUI(GameDetails gameDetails, Player playerA, Player playerB) {
            barrierHealthA.SetHealth(playerA.Health);
            barrierHealthB.SetHealth(playerB.Health);

			HealthA.text = GetHealth(playerA);
			HealthB.text = GetHealth(playerB);

			float hNormA = playerA.Health / 100f;
			float hNormB = playerB.Health / 100f;

			healthBarA.localScale = new Vector3 (hNormA, 1f, 1f);
			healthBarB.localScale = new Vector3 (hNormB, 1f, 1f);

			EnergyA.text = GetEnergy (playerA);
			EnergyB.text = GetEnergy (playerB);

			ScoreA.text = GetScore (playerA);
			ScoreB.text = GetScore (playerB);

			if(playerA.IronCurtainAvailable) {
				pulseIronCurtainPlayerA = true;
			} else {
				pulseIronCurtainPlayerA = false;
				ironCurtainPlayerA.color = ColorUtil.lightGrey;
			}

			if(playerB.IronCurtainAvailable) {
				pulseIronCurtainPlayerB = true;
			} else {
				pulseIronCurtainPlayerB = false;
				ironCurtainPlayerB.color = ColorUtil.lightGrey;
			}

			RoundText.text = gameDetails.Round.ToString();

            if(playerA.Health > playerB.Health) {
                winningPlayer = PlayerType.A;
            } else {
                winningPlayer = PlayerType.B;
            }
		}

        public void DisplayFinalGameMessage(GameState finalState, string namePlayerA, string namePlayerB) {
            FinalGameHolder.SetActive(true);
            if(winningPlayer == PlayerType.A) {
                playerAWinHolder.SetActive(true);
                winningPlayerText.text = namePlayerA + " Wins!";
            } else {
                playerBWinHolder.SetActive(true);
                winningPlayerText.text = namePlayerB + " Wins!";
            }
		}

        string GetPlayerType(Player player) {
            return player.PlayerType.ToString();
        }

        string GetHealth(Player player) {
            return player.Health.ToString();
        }

        string GetEnergy(Player player) {
            return player.Energy.ToString();
        }

        string GetScore(Player player) {
            return player.Score.ToString();
        }

        string GetPrice(GameDetails gameDetails, BuildingType buildingType) {
            return gameDetails.BuildingPrices[buildingType].ToString();
        }
    }
}

