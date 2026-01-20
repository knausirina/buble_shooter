 using UnityEngine;

 public class GameLauncher : MonoBehaviour
 {
     [SerializeField] private Config _config;

     private Game _game;

     private void Awake()
     {
         var obj = FindObjectsByType<GameLauncher>(FindObjectsSortMode.None);
         if (obj.Length > 1)
         {
             Destroy(gameObject);
         }
         else
         {
             DontDestroyOnLoad(gameObject);
         }
     }

     public void StartGame()
     {
         _game ??= new Game(_config);

         if (_game.GameState == GameState.Stop)
         {
             _game.Start();
         }
     }

     public void StopGame()
     {
         _game?.Stop();
     }
 }