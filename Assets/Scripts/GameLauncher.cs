 using UnityEngine;

 public class GameLauncher : MonoBehaviour
 {
     [SerializeField] private Config _config;

     private Game _game;
     
     private void Awake()
     {
         DontDestroyOnLoad(gameObject);
     }

     public void StartGame()
     {
         _game ??= new Game(_config);

         if (_game.GameState == GameState.Stop)
         {
             _game.Start();
         }
     }
 }