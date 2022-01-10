using System;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceGame.Ui
{
	public class ScoreTalkToGM : TalkToGM
	{
		[SerializeField] private TextMeshProUGUI _loseWinText = default;
		[SerializeField] private TextMeshProUGUI _sumCreditsEarnedLabel = default;
		[SerializeField] private TextMeshProUGUI _sumCreditsEarnedValue = default;
		[SerializeField] private RectTransform _reasonsContainer = default;
		[SerializeField] private CreditsEarnedRow _creditsEarnedRowPrefab = default;

		private List<CreditsEarnedRow> _creditRows = default;

		protected override void Start()
		{
			base.Start();

			if (GameManager.State.HangarState == GameState.HangarArrivalState.SUCCESS)
			{
				_loseWinText.text = "SUCCESS";
			} else
			if (GameManager.State.HangarState == GameState.HangarArrivalState.FAILURE)
			{
				_loseWinText.text = "FAILURE";
				_sumCreditsEarnedLabel.gameObject.SetActive(false);
				_sumCreditsEarnedValue.gameObject.SetActive(false);
				_reasonsContainer.gameObject.SetActive(false);
			}
			else
			{
				Debug.LogError("Invalid state reached. Somehow came from the menu in the Score screen.");
			}

			_creditRows = new List<CreditsEarnedRow>();

			_creditRows.Add(Instantiate<CreditsEarnedRow>(_creditsEarnedRowPrefab, _reasonsContainer)
				.SetReason("Completed mission.")
				.SetAmount(2000));

			var playerHealth = GameManager.State.PlayerHealth;
			if (playerHealth >= GameManager.MaxPlayerHealth)
			{
				_creditRows.Add(Instantiate<CreditsEarnedRow>(_creditsEarnedRowPrefab, _reasonsContainer)
					.SetReason("Took no damage!")
					.SetAmount(1000));
			} else
			if (playerHealth > GameManager.MaxPlayerHealth * 0.75)
			{
				_creditRows.Add(Instantiate<CreditsEarnedRow>(_creditsEarnedRowPrefab, _reasonsContainer)
					.SetReason(">75% hull capacity.")
					.SetAmount(750));
			} else
			if (playerHealth > GameManager.MaxPlayerHealth * 0.50F)
			{
				_creditRows.Add(Instantiate<CreditsEarnedRow>(_creditsEarnedRowPrefab, _reasonsContainer)
					.SetReason(">50% hull capacity.")
					.SetAmount(500));
			}

			var allyDeathCount = GameManager.State.AllyDeathCount;
			if (allyDeathCount == 0)
			{
				_creditRows.Add(Instantiate<CreditsEarnedRow>(_creditsEarnedRowPrefab, _reasonsContainer)
					.SetReason("No ally died.")
					.SetAmount(2000));
			} 
			else
			{
				var text = allyDeathCount == 1
					? "An ally died."
					: $"{allyDeathCount} allies died.";
				_creditRows.Add(Instantiate<CreditsEarnedRow>(_creditsEarnedRowPrefab, _reasonsContainer)
					.SetReason(text)
					.SetAmount(-250 * allyDeathCount));
			}

			var teamKillCount = GameManager.State.PlayerAllyKillCount;
			if (teamKillCount > 0)
			{
				var text = teamKillCount == 1
					? "Killed an ally."
					: $"Killed {teamKillCount} allies.";
				_creditRows.Add(Instantiate<CreditsEarnedRow>(_creditsEarnedRowPrefab, _reasonsContainer)
					.SetReason(text)
					.SetAmount(-500 * teamKillCount));
			}

			var enemyKillCount = GameManager.State.PlayerEnemyKillCount;
			if (enemyKillCount > 0)
			{
				var text = enemyKillCount == 1
					? "Killed an alien."
					: $"Killed {enemyKillCount} aliens.";
				_creditRows.Add(Instantiate<CreditsEarnedRow>(_creditsEarnedRowPrefab, _reasonsContainer)
					.SetReason(text)
					.SetAmount(1000 * enemyKillCount));
			}

			var sum = _creditRows.Sum(x => x.GetAmount());

			GameManager.State.CreditsThisMission = sum;
			GameManager.State.TotalCredits += GameManager.State.CreditsThisMission;

			_sumCreditsEarnedValue.text = $"${GameManager.State.CreditsThisMission}";
		}
	}
}
