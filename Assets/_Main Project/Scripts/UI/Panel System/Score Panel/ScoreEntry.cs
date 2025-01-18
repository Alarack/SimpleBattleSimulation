using UnityEngine;
using TMPro;
using DG.Tweening;

public class ScoreEntry : MonoBehaviour
{
    public TextMeshProUGUI entryText;


    private ScoreManager.ScoreEntryData data;

    private CanvasGroup fader;

    private void Awake() {
        fader = GetComponent<CanvasGroup>();
    }

    public void Setup(ScoreManager.ScoreEntryData data, int rank) {
        this.data = data;

        SetupDisplay(rank);
    }


    private void SetupDisplay(int rank) {
        fader.alpha = 0f;
        fader.DOFade(1f, 0.5f);
        transform.localScale = Vector3.zero;
        transform.DOScale(1f, 0.3f).SetEase(Ease.OutBounce);

        entryText.text = rank.ToString() + ". " + data.name + " - Kills: " + data.kills.Count;
    }

}
