using Microsoft.AspNetCore.Mvc;
using Server.Services;
using SharedLibrary;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayerController  : ControllerBase{
    
    private readonly PlayerService _playerService;
    
    public PlayerController(PlayerService playerService) {
        _playerService = playerService;
    }
    
    [HttpGet("{id}")]
    public Player Get([FromRoute] int id) {
        var player = new Player() {
            Id = id
        };
        
        _playerService.DoSomething();
        return player;
    }


    [HttpPost]
    public Player Post(Player player) {
        Console.WriteLine("Player has beed added to database");
        return player;
    }
        
}