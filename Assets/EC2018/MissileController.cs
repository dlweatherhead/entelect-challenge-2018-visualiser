using UnityEngine;
using EC2018;
using EC2018.Entities;

[RequireComponent(typeof(AudioSource))]
public class MissileController : MonoBehaviour {

    public int StartX;
    public int TargetX;
    public float speed = 1f;
    public GameObject explosion;

    public Missile missile;
    float nextStatePosition;

    public AudioSource audioSource;

    public void Setup(Missile missile, int direction, float rate) {
        this.missile = missile;
        transform.GetChild(0).gameObject.SetActive(true);
        speed = missile.Speed * direction / rate;
        nextStatePosition = transform.position.x + missile.Speed * direction;
    }

    void Update() {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (speed > 0) {
            if (transform.position.x > nextStatePosition) {
                HaltAndSetToNextStatePosition();
            }
        } else {
            if (transform.position.x < nextStatePosition) {
                HaltAndSetToNextStatePosition();
            }
        }
    }

    public void PlaySound(AudioClip clip) {
        audioSource.clip = clip;
        audioSource.pitch = Random.Range(0.7f, 1f);
        audioSource.Play();
    }

    public void HaltAndSetToNextStatePosition() {
        Halt();
        transform.position = new Vector3(nextStatePosition, transform.position.y, transform.position.z);
    }

    public void Halt() {
        speed = 0;
    }

    void OnTriggerEnter(Collider other) {
        switch (other.tag) {
            case Constants.Tags.Defense:
            case Constants.Tags.Attack:
            case Constants.Tags.Tesla:
            case Constants.Tags.Energy:
            case Constants.Tags.MissileCollider:
                var buildingCtrl = other.gameObject.GetComponentInParent<BuildingController>();
				if (!buildingCtrl.isUnderConstruction) {
					if (missile.PlayerType != buildingCtrl.building.PlayerType) {
                        Instantiate(explosion, other.gameObject.transform.position, Quaternion.identity);
						gameObject.SetActive(false);
					}
				}
                break;
            case Constants.Tags.Barrier:
                Instantiate(explosion, transform.position, Quaternion.identity);
                gameObject.SetActive(false);
                break; 
            case Constants.Tags.IronCurtainA:
                if(missile.PlayerType == EC2018.Enums.PlayerType.B) {
                    Instantiate(explosion, transform.position, Quaternion.identity);
                    gameObject.SetActive(false);
                }
                break;   
            case Constants.Tags.IronCurtainB:
                if (missile.PlayerType == EC2018.Enums.PlayerType.A) {
                    Instantiate(explosion, transform.position, Quaternion.identity);
                    gameObject.SetActive(false);
                }
                break;
        }
    }
}
