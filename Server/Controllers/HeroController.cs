using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
    
    [HttpPost("{id}")]
    public IActionResult Edit([FromRoute] int id,[FromBody] CreateHeroRequest req) {
        var heroesId = JsonConvert.DeserializeObject<List<int>>(User.FindFirst("heroes").Value);

        if (!heroesId.Contains(id)) return Unauthorized(); 
        
        var hero = dbContext.Heroes.First(h => h.Id == id);
        hero.Name = req.Name;
        dbContext.SaveChanges();
        
        return Ok();
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
        
        //right now doing this so that our user dont get sent to client
        hero.User = null;
        
        return hero;

    }
        
}