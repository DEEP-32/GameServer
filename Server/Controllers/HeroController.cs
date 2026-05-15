using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Services;
using SharedLibrary;
using SharedLibrary.Requests;

namespace Server.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class HeroController  : ControllerBase{
    
    readonly IHeroService heroService;
    readonly GameDbContext dbContext;
    
    public HeroController(IHeroService heroService,GameDbContext dbContext) {
        this.heroService = heroService;
        this.dbContext = dbContext;
    }
    
    [HttpGet("{id}")]
    public Hero Get([FromRoute] int id) {
        var player = new Hero() {
            Id = id
        };

        
        
        
        
        heroService.DoSomething();
        return player;
    }


    [HttpPost]
    public Hero Post(CreateHeroRequest heroRequest) {
        var userId = int.Parse(User.FindFirst("id").Value);
        var user = dbContext.Users.Include(u => u.Heroes).First(u => u.Id == userId);
        
        var hero = new Hero() {
            Name = heroRequest.Name,
            User = user
        };
        
        dbContext.Add(hero);
        dbContext.SaveChanges();
        return hero;

    }
        
}