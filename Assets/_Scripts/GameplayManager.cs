using System;
using TMPro;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] Ball _ball;
    [SerializeField] CoinPrefabsHandler _coinPrefabsHandler;
    [SerializeField] TextMeshProUGUI _coinsText;
    [SerializeField] TextMeshProUGUI _timerText;
    [SerializeField] AudioSource _gameplayAudioSource;
    [SerializeField] AudioClip _winAudio;
    [SerializeField] AudioClip _loseAudio;

    [SerializeField] private float _targetCompletionTime;

    private KeyCode _restartKey = KeyCode.R;

    private bool _isGamePlaying = true;
    private bool _isOneTimePlaying;

    private bool CheckLoseCondition() => _currentTime <= 0;
    private bool CheckWinCondition() => _ball.GetBalance() == _totalCoins;

    private int _currentCoins;
    private int _totalCoins;
    private float _currentTime;

    private void Start()
    {
        _totalCoins = _coinPrefabsHandler.TotalCoins();

        _coinPrefabsHandler.ResetCoinsState();
        ResetGameplayTimer();
    }

    private void Update()
    {
        ShowCoins(); //independent balance update - to remove same-frame gameplay stop corner cases

        if (CheckLoseCondition())
            LoseGame(); 
        else if (CheckWinCondition()) 
            WinGame(); 

        if (_isGamePlaying)
        {
            _currentTime -= Time.deltaTime;
            ShowTimer(); 
        }
        else
        {
            if (Input.GetKeyDown(_restartKey))
                RestartGame();
        }
    }

    private void ShowCoins()
    {
        _coinsText.text = "Монеток " + _ball.GetBalance() + " / " + _totalCoins;
    }

    private void ShowTimer()
    {
        _timerText.text = "Осталось " + _currentTime.ToString("0.00");
    }

    private void ResetGameplayTimer()
    {
        _currentTime = _targetCompletionTime;
    }

    private void ResetOneTimeAudioPlayer()
    {
        _isOneTimePlaying = false;
    }

    private void WinGame()
    {
        StopGameplay(_winAudio, $"Отличная работа! Жмите {_restartKey} если хотите повторить!");
    }

    private void LoseGame()
    {
        StopGameplay(_loseAudio, $"Вы не успели! Жмите {_restartKey} и погнали снова!");
    }

    private void RestartGame()
    {
        _ball.MoveToStartPosition();

        ResetGameplayTimer();

        ResetOneTimeAudioPlayer();

        _coinPrefabsHandler.ResetCoinsState();
        _ball.ResetBalance();

        if (_ball.PhysicsEnabled() == false)
            _ball.EnablePhysics();

        if (_ball.MovementEnabled() == false)
            _ball.EnableMovement();

        _isGamePlaying = true;

        Time.timeScale = 1.0f;
    }

    private void StopGameplay(AudioClip clipToPlay, String textToShow)
    {
        _isGamePlaying = false;

        Time.timeScale = 0.0f;

        if (_ball.PhysicsEnabled())
            _ball.DisablePhysics();

        if (_ball.MovementEnabled())
            _ball.DisableMovement();

        ShowEffects(clipToPlay, textToShow);
    }

    private void ShowEffects(AudioClip clipToPlay, String textToShow) 
    {
        Debug.Log(textToShow);
        PlaySingleTime(clipToPlay);
    }

    private void PlaySingleTime(AudioClip audioClip)
    {
        if (_isOneTimePlaying == false)
        {
            _gameplayAudioSource.PlayOneShot(audioClip);
            _isOneTimePlaying = true;
        }
    }

}
