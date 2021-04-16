using UnityEngine;

public class BestScore : ScoreCounter
{
    [SerializeField] private NowScore _nowScore;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("BestScore"))
        {
            PlayerPrefs.SetInt("BestScore", 0);
        }
        else
        {
            _text.text = PlayerPrefs.GetInt("BestScore").ToString();
        }
    }

    private void OnEnable()
    {
        _nowScore.UpdatedScore += OnUpdatedScore;
    }

    private void OnDisable()
    {
        _nowScore.UpdatedScore -= OnUpdatedScore;        
    }

    private void OnUpdatedScore(int score)
    {
        if(PlayerPrefs.GetInt("BestScore") < score)
        {
            PlayerPrefs.SetInt("BestScore", score);
            _text.text = score.ToString();
        }
    }
}
