using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour {
	[Header("Health")]
	[SerializeField] public float maxHealth = 5f;
	[SerializeField] public float currentHealth = 5f;

	[Header("Heart UI")]
	[SerializeField] Image[] hearts;
	[SerializeField] Sprite fullHeart;
	[SerializeField] Sprite halfHeart;
	[SerializeField] Sprite emptyHeart;

	[Header("Game Over")]
	[SerializeField] string gameOverSceneName = "GameOver";

	public bool gameOverTriggered;

	void Start() {
		maxHealth = maxHealth;
		currentHealth = maxHealth;

		updateHearts();
	}

	public void takeDamage(float amount) {
		// Possible edgecase on multiple damage on one frame? Unlikely tbh 
		if (gameOverTriggered || amount <= 0f) {
			return;
		}

		// I dont want it to try and draw negative hearts
		currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);

		updateHearts();

		Debug.Log("Player health: " + currentHealth + "/" + maxHealth);

		if (currentHealth <= 0f) {
			gameOverTriggered = true;
			SceneManager.LoadScene(gameOverSceneName);
		}
	}

	void updateHearts() {
		if (hearts == null) {
			return;
		}

		for (int i = 0; i < hearts.Length; i++) {
			if (hearts[i] == null) {
				continue;
			}

			float healthForThisHeart = currentHealth - i;

			if (healthForThisHeart >= 1f) {
				hearts[i].sprite = fullHeart;
			}
			else if (healthForThisHeart >= 0.5f) {
				hearts[i].sprite = halfHeart;
			}
			else {
				hearts[i].sprite = emptyHeart;
			}
		}
	}
}