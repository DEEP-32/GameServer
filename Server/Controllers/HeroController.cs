using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Services;
using SharedLibrary;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class HeroController  : ControllerBase{
    
    private readonly IPlayerService playerService;
    readonly GameDbContext dbContext;
    
    public HeroController(IPlayerService playerService,GameDbContext dbContext) {
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
    public Hero Get([FromRoute] int id) {
        var player = new Hero() {
            Id = id
        };

        
        
        
        
        playerService.DoSomething();
        return player;
    }


    [HttpPost]
    public Hero Post(Hero hero) {
        Console.WriteLine("Player has beed added to database");
        return hero;
    }
        
}