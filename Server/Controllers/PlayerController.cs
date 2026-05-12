using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Services;
using SharedLibrary;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayerController  : ControllerBase{
    
    private readonly IPlayerService playerService;
    readonly GameDbContext dbContext;
    
    public PlayerController(IPlayerService playerService,GameDbContext dbContext) {
        this.playerService = playerService;
        this.dbContext = dbContext;
        
        var user = new User() {
            Username = "DEEP-32",
            Password = "pass",
            Salt = "salt"
        };

        dbContext.Add(user);
        dbContext.SaveChanges();
    }
    
    [HttpGet("{id}")]
    public Player Get([FromRoute] int id) {
        var player = new Player() {
            Id = id
        };

        
        
        
        
        playerService.DoSomething();
        return player;
    }


    [HttpPost]
    public Player Post(Player player) {
        Console.WriteLine("Player has beed added to database");
        return player;
    }
        
}